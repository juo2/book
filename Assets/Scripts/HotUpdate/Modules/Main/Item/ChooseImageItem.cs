using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XGUI;

namespace XModules.Main.Item
{
    public class ChooseImageItem : MonoBehaviour
    {
        [SerializeField]
        Image pic;

        [SerializeField]
        XButton upload;

        [SerializeField]
        XButton select;

        [SerializeField]
        XButton change;

        int index = -1;
        string uri = string.Empty;

        public void Refresh(int _index,string _uri)
        {
            Debug.Log("Refresh RefreshRefresh Refresh Refresh");

            index = _index;
            uri = _uri;

            if (index == 0)
            {
                upload.SetActive(true);
                select.SetActive(false);
                change.SetActive(false);
                pic.SetActive(false);
            }
            else
            {
                upload.SetActive(false);
                select.SetActive(true);
                change.SetActive(true);
                pic.SetActive(true);

#if UNITY_EDITOR
                StartCoroutine(LoadImageUri(uri));
#else
                StartCoroutine(LoadImageUri($"file://{uri}"));
#endif
            }
        }

        IEnumerator LoadImageUri(string uri)
        {
            // ʹ�� UnityWebRequestTexture ��ȡ����
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
            yield return request.SendWebRequest();

            // ����Ƿ��д�����
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading image: " + request.error);
            }
            else
            {
                // ��ȡ���غõ�����
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                pic.sprite = sprite;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            upload.onClick.AddListener(() => 
            {
#if UNITY_EDITOR
                SDK.PhotoData photoData = new SDK.PhotoData();
                photoData.path = Path.Combine(Application.dataPath, $"Art/Scenes/Game/Texture2D/bg1.jpg");
                photoData.exData = index.ToString();

                string json = JsonUtility.ToJson(photoData);
                XEvent.EventDispatcher.DispatchEvent("LOAD_IMAGE", json);
#else
                SDK.SDKManager.Instance.Photo(index.ToString());
#endif

            });

            select.onClick.AddListener(() => 
            {
                Debug.Log($"index : {index}    uri : {uri}");
            });

            change.onClick.AddListener(() => 
            {
#if UNITY_EDITOR
                SDK.PhotoData photoData = new SDK.PhotoData();
                photoData.path = Path.Combine(Application.dataPath, $"Art/Scenes/Game/Texture2D/bg2.jpg");
                photoData.exData = index.ToString();

                string json = JsonUtility.ToJson(photoData);
                XEvent.EventDispatcher.DispatchEvent("LOAD_IMAGE", json);
#else
                SDK.SDKManager.Instance.Photo(index.ToString());
#endif
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}



