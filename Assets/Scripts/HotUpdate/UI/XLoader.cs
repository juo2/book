﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XGUI
{
    public class XLoader : XView
    {
        public class LoaderEvent : UnityEvent<LoaderItemRenderer> { }

        private LoaderEvent m_OnCreateRenderer = new LoaderEvent();
        private UnityEvent m_OnUpdateRendererLua = new UnityEvent();

        public LoaderEvent onCreateRenderer { get { return m_OnCreateRenderer; } }
        public UnityEvent onUpdateRendererLua { get { return m_OnUpdateRendererLua; } }

        public LoaderItemRenderer render = null;

        private bool isdispose = false;
        [SerializeField]
        protected GameObject m_Template;
        [SerializeField]
        private string m_TemplateAsset;
        public string templateAsset
        {
            get { return m_TemplateAsset; }
            set
            {
                if (m_TemplateAsset == value)
                    return;
                m_TemplateAsset = value;
            }
        }

        public virtual void StartLoad()
        {
            LoadTemplateAsset();
        }

        private AssetManagement.AssetInternalLoader loader;
        void LoadTemplateAsset()
        {
            if (string.IsNullOrEmpty(m_TemplateAsset))
                return;

            if (m_Template != null || loader != null)
                return;
#if UNITY_EDITOR
            if (AssetManagement.AssetManager.Instance.AssetLoaderOptions == null)
                AssetManagement.AssetManager.Instance.Initialize(new GameLoaderOptions());
#endif
            loader = AssetManagement.AssetUtility.LoadAsset<GameObject>(this.m_TemplateAsset);
            loader.onComplete += LoadDone;
        }
        private void LoadDone(AssetManagement.AssetInternalLoader load)
        {
            if (string.IsNullOrEmpty(load.Error))
                this.m_Template = load.GetRawObject<GameObject>();

            OnLoadComplete();
        }


        protected virtual void OnLoadComplete()
        {
            loader = null;
            CreateItemRenderer();
        }

        private void CreateItemRenderer()
        {
            if (m_Template == null)
            {
                return;
            }
            render = new LoaderItemRenderer();

            //全局缓存池
            //GameObject go = PreCreateCachePool.TryGetGameObject(m_TemplateAsset);

            //if (go)
            //{
            //    RectTransform rect=  go.GetComponent<RectTransform>();
            //    rect.SetParent(this.transform);
            //    rect.anchoredPosition3D = Vector3.zero;
            //    rect.localScale = Vector3.one;
            //    rect.localRotation = Quaternion.identity;
            //    render.SetData(go);
            //}
            //else
            render.SetData(Instantiate<GameObject>(this.m_Template, this.transform, false));

            //try
            //{
                m_OnCreateRenderer.Invoke(render);
            //}
            //catch (Exception e)
            //{
            //    XLogger.ERROR(e.ToString());
            //}

            ForceRefresh();
        }

        public void ForceRefresh()
        {
            if (render != null)
            {
                try
                {
                    m_OnUpdateRendererLua.Invoke();
                }
                catch (Exception e)
                {
                    XLogger.ERROR(e.ToString());
                }
            }
        }

        public override void ClearEvent()
        {
            base.ClearEvent();
            if (m_OnCreateRenderer != null)
            {
                m_OnCreateRenderer.RemoveAllListeners();
                m_OnCreateRenderer = null;
            }

            if (m_OnUpdateRendererLua != null)
            {
                m_OnUpdateRendererLua.RemoveAllListeners();
                m_OnUpdateRendererLua = null;
            }
        }

        public override void OnDestroy()
        {
            if (loader != null)
            {
                loader.onComplete -= LoadDone;
                loader = null;
            }

            ClearEvent();

            render = null;

            if (m_Template != null && AssetManagement.AssetCache.ContainsRawObject(m_Template))
            {
                AssetManagement.AssetUtility.DestroyAsset(m_Template);
                m_Template = null;
                m_TemplateAsset = null;
            }

            base.OnDestroy();


        }

        public class LoaderItemRenderer
        {
            private int m_InstanceID;
            public int instanceID { get { return m_InstanceID; } }

            private GameObject m_GameObject;
            public UnityEngine.GameObject gameObject { get { return m_GameObject; } }
            private RectTransform m_Transform;
            public UnityEngine.RectTransform transform { get { return m_Transform; } }

            public void SetData(GameObject gameObject)
            {
                this.m_GameObject = gameObject;
                this.gameObject.SetActive(true);
                this.m_Transform = gameObject.GetComponent<RectTransform>();
                this.m_InstanceID = gameObject.GetInstanceID();
            }

            public void Destroy()
            {
                UnityEngine.Object.Destroy(this.m_GameObject);
            }
        }
    }
}

