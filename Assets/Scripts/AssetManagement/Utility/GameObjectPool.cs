﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour
{
    public class GameObjectInfo
    {
        public GameObject p_GameObject;
        public float p_ReleaseTime;
        public bool p_IsClear;
    }


    private static ObjectPool<GameObjectInfo> s_InfoPool = new ObjectPool<GameObjectInfo>(null, x => x.p_GameObject = null);
    private static ObjectPool<Queue<GameObjectInfo>> s_QueuePool = new ObjectPool<Queue<GameObjectInfo>>(null, x => x.Clear());
    private Dictionary<string, Queue<GameObjectInfo>> m_PoolMap = new Dictionary<string, Queue<GameObjectInfo>>();
    public Dictionary<string, Queue<GameObjectInfo>> poolMap { get { return m_PoolMap; } }
    private Transform m_Transform;
    private bool m_IsDestroy;
    //生命时长秒
    [SerializeField]
    private float m_LifeTimeLength = 20;
    public float lifeTimeLength { get { return m_LifeTimeLength; } set { m_LifeTimeLength = value; } }
    [SerializeField]
    private int m_totalObjects = 0;
    static List<string> s_tempList = new List<string>();
    int m_FrameCount = 0;


    void Awake() { m_Transform = transform; }

    public int typeCount { get { return m_PoolMap.Count; } }

    public bool ContainsKey(string name)
    {
        return m_PoolMap.ContainsKey(name);
    }

    public GameObject Get(string name, bool isCreate = true)
    {
        Queue<GameObjectInfo> queue;
        if (m_PoolMap.TryGetValue(name, out queue) && queue.Count > 0)
        {
            GameObjectInfo info = queue.Dequeue();
            GameObject go = info.p_GameObject;
            s_InfoPool.Release(info);

            if (queue.Count < 1)
            {
                s_QueuePool.Release(queue);
                m_PoolMap.Remove(name);
            }

            return go;
        }
        else
        {
            if (isCreate)
            {
                GameObject newGO = new GameObject(name);
                return newGO;
            }
            else
                return null;
        }
    }

    public void Release(GameObject gameObject, string name,bool isClear = true)
    {
        if (gameObject == null || gameObject.IsNull()) return;

        if (m_IsDestroy)
        {
            //应该立即销毁
            if (AssetManagement.AssetCache.ContainsInstanceObject(gameObject))
                AssetManagement.AssetCache.DestroyAsset(gameObject, 0);
            else  
                GameObject.Destroy(gameObject);

            return;
        }

        if (string.IsNullOrEmpty(name)) return;

        Queue<GameObjectInfo> queue;
        if (!m_PoolMap.TryGetValue(name, out queue))
        {
            queue = s_QueuePool.Get();
            m_PoolMap.Add(name, queue);
        }

        GameObjectInfo info = s_InfoPool.Get();
        info.p_GameObject = gameObject;
        info.p_ReleaseTime = Time.time;
        info.p_GameObject.SetActive(false);
        info.p_GameObject.transform.SetParent(m_Transform);
        info.p_IsClear = isClear;
        queue.Enqueue(info);
    }



    public void Update()
    {
        if (!(++m_FrameCount % 60 == 0))
            return;
        m_totalObjects = m_PoolMap.Count;
        if (m_totalObjects < 1)
            return;

        float now = Time.time;
        foreach (var pool in m_PoolMap)
        {
            if (pool.Value.Count > 0)
            {
                GameObjectInfo info = pool.Value.Peek();
                while (info.p_IsClear && now - info.p_ReleaseTime > m_LifeTimeLength)
                {
                    pool.Value.Dequeue();

                    if (AssetManagement.AssetCache.ContainsInstanceObject(info.p_GameObject))
                        AssetManagement.AssetCache.DestroyAsset(info.p_GameObject, 0);
                    else
                        Object.Destroy(info.p_GameObject);

                    info.p_GameObject = null;
                    info.p_ReleaseTime = -1;
                    s_InfoPool.Release(info);

                    if (pool.Value.Count > 0)
                        info = pool.Value.Peek();
                    else
                        break;
                }

                if (pool.Value.Count < 1)
                    s_tempList.Add(pool.Key);
            }
        }


        if (s_tempList.Count > 0)
        {
            foreach (var item in s_tempList)
            {
                Queue<GameObjectInfo> pool = m_PoolMap[item];
                m_PoolMap.Remove(item);
                s_QueuePool.Release(pool);
            }
            s_tempList.Clear();
        }

    }




    public void UnloadAll()
    {
        foreach (var pool in m_PoolMap)
        {
            foreach (var info in pool.Value)
            {
                if (AssetManagement.AssetCache.ContainsInstanceObject(info.p_GameObject))
                    AssetManagement.AssetCache.DestroyAsset(info.p_GameObject, 0);
                else
                    Object.Destroy(info.p_GameObject);

                info.p_GameObject = null;
                s_InfoPool.Release(info);
            }
            s_QueuePool.Release(pool.Value);
        }
        m_PoolMap.Clear();
    }

    public void OnDestroy()
    {
        this.m_IsDestroy = true;
        UnloadAll();
    }
}
