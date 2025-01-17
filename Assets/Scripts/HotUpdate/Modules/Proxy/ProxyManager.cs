using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking; // 用于网络请求
using XModules.Data;

namespace XModules.Proxy
{
    public class ProxyManager
    {
        static string url = "http://ai.sorachat.site";
        public static string upload_url = $"{url}/chat/user/upload";

        public static void SendCodeRequest(string email, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(SendCodeRequest($"{url}/chat/user/sendCode", email, callBack, errorBack));
        }

        public static void LoginRequest(string email, string code, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(LoginRequest($"{url}/chat/user/login", email, code, callBack, errorBack));
        }

        public static void GetNPCAllList(Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(GetNPCAllList($"{url}/chat/npc/npcAllList", callBack, errorBack));
        }

        public static void GetUserSessionList(Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(GetUserSessionList($"{url}/chat/chatRecord/getUserSessionList", callBack, errorBack));
        }

        public static void GetChatRecord(string npcId,Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(GetChatRecord($"{url}/chat/chatRecord/getChatRecord", npcId,callBack, errorBack));
        }

        public static void DeleteUserSession(string userSessionId,string npcId, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(DeleteUserSession($"{url}/chat/chatRecord/deleteUserSession", userSessionId, npcId, callBack, errorBack));
        }

        public static void StreamOneShotChat(string npcId,string textContent,string question,string options, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(StreamOneShotChat($"{url}/chat/chatRecord/streamOneShotChat", npcId, textContent, question, options, callBack, errorBack));
        }

        public static void SaveUserSession(string npcId, Action callBack = null,Action errorBack = null)
        {
            TimerManager.AddCoroutine(SaveUserSession($"{url}/chat/chatRecord/saveUserSession",npcId, callBack, errorBack));
        }

        public static void GetStoryList(int type, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(GetStoryList($"{url}/chat/userStory/getStoryList", type, callBack, errorBack));
        }

        public static void SaveStoryRecord(string storyId,Action callBack = null,Action errorBack = null)
        {
            TimerManager.AddCoroutine(SaveStoryRecord($"{url}/chat/userStory/saveStoryRecord", storyId, callBack, errorBack));
        }

        public static void Upload(string fileName,byte[] fileData, Action callBack = null, Action errorBack = null)
        {
            TimerManager.AddCoroutine(Upload(upload_url, fileName,fileData, callBack, errorBack));
        }

        static IEnumerator SendCodeRequest(string url, string email, Action callBack, Action errorBack)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent，虽然在Unity中这不是必需的，但为了保持一致性，我们仍然包含这个步骤
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);

                BasicResponse basicResponse = JsonUtility.FromJson<BasicResponse>(webRequest.downloadHandler.text);
                if(basicResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>SendCodeRequest 请求成功!!!</color>");
                    callBack?.Invoke();
                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator LoginRequest(string url, string email, string code, Action callBack, Action errorBack)
        {

            Debug.Log($"code:{code}");

            // 使用WWWForm来构建表单数据
            WWWForm form = new WWWForm();
            form.AddField("loginType", "1");
            form.AddField("email", email);
            form.AddField("code", code);
            form.AddField("accessToken", "");

            // 创建UnityWebRequest，设置URL和方法
            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent（可选）
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");

            // 发送请求并等待响应
            yield return webRequest.SendWebRequest();

            // 检查是否有错误发生
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // 打印错误信息
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {
                // 请求成功，使用webRequest.downloadHandler.text获取响应内容

                Debug.Log(webRequest.downloadHandler.text);

                PlayerResponse playerResponse = JsonUtility.FromJson<PlayerResponse>(webRequest.downloadHandler.text);
                if (playerResponse.code == "0")
                {
                    DataManager.playerResponse = playerResponse;

                    //临时把token记住
                    PlayerPrefs.SetString("TEMP_TOKEN", DataManager.playerResponse.data.token);
                    PlayerPrefs.SetString("TEMP_ID", DataManager.playerResponse.data.id);

                    Debug.Log("<color=#4aff11>LoginRequest 请求成功!!!</color>");
                    callBack?.Invoke();
                }
                else
                {
                    errorBack?.Invoke();
                }
            }
            
        }

        static IEnumerator GetNPCAllList(string url, Action callBack, Action errorBack)
        {
            Debug.Log($"token:{DataManager.getToken()}");

            // 这个请求没有表单数据，但我们依然创建一个空的WWWForm对象，以符合UnityWebRequest.Post的参数要求
            WWWForm form = new WWWForm();
            //form.AddField("token", token);

            Debug.Log($"url:{url}");

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent，虽然在Unity中这不是必需的，但为了保持一致性，我们仍然包含这个步骤
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                
                NPCResponse npcResponse = JsonUtility.FromJson<NPCResponse>(webRequest.downloadHandler.text);

                if (npcResponse.code == "0")
                {
                    DataManager.npcResponse = npcResponse;
                    Debug.Log("<color=#4aff11>GetNPCAllList 请求成功!!!</color>");
                    callBack?.Invoke();
                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator GetUserSessionList(string url, Action callBack, Action errorBack)
        {

            Debug.Log($"playerResponse.data.id:{DataManager.getPlayerId()}");

            WWWForm form = new WWWForm();
            form.AddField("userId", DataManager.getPlayerId());

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                SessionResponse sessionResponse = JsonUtility.FromJson<SessionResponse>(webRequest.downloadHandler.text);

                if (sessionResponse.code == "0")
                {
                    DataManager.sessionResponse = sessionResponse;
                    Debug.Log("<color=#4aff11>GetUserSessionList 请求成功!!!</color>");
                    callBack?.Invoke();
                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator GetChatRecord(string url, string npcId, Action callBack, Action errorBack)
        {

            Debug.Log($"playerResponse.data.id:{DataManager.getPlayerId()}");

            WWWForm form = new WWWForm();
            form.AddField("userId", DataManager.getPlayerId());
            form.AddField("npcId", npcId);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(webRequest.downloadHandler.text);

                if (chatResponse.code == "0")
                {
                    DataManager.addChatResponse(npcId, chatResponse);
                    Debug.Log("<color=#4aff11>GetChatRecord 请求成功!!!</color>");
                    callBack?.Invoke();
                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator DeleteUserSession(string url, string userSessionId,string npcId, Action callBack, Action errorBack)
        {

            Debug.Log($"userSessionId:{userSessionId}");

            WWWForm form = new WWWForm();
            form.AddField("userSessionId", userSessionId);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                BasicResponse basicResponse = JsonUtility.FromJson<BasicResponse>(webRequest.downloadHandler.text);
                if (basicResponse.code == "0")
                {
                    DataManager.detelteChatResponse(npcId);
                    Debug.Log("<color=#4aff11>DeleteUserSession 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator StreamOneShotChat(string url,string npcId, string textContent, string question, string options, Action callBack, Action errorBack)
        {

            Debug.Log($"npcId:{npcId}");

            WWWForm form = new WWWForm();
            form.AddField("userId",DataManager.getPlayerId());
            form.AddField("npcId", npcId);
            form.AddField("textContent", textContent);
            form.AddField("question", question);
            form.AddField("options", options);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                DataManager.oneShotChatResponse = JsonUtility.FromJson<OneShotChatResponse>(webRequest.downloadHandler.text);
                if (DataManager.oneShotChatResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>StreamOneShotChat 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }

                //ConversationData.isRequestChating = false;
            }
        }

        static IEnumerator SaveUserSession(string url, string npcId, Action callBack, Action errorBack)
        {

            Debug.Log($"npcId:{npcId}");

            WWWForm form = new WWWForm();
            form.AddField("userId", DataManager.getPlayerId());
            form.AddField("npcId", npcId);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);

                BasicResponse basicResponse = JsonUtility.FromJson<BasicResponse>(webRequest.downloadHandler.text);
                if (basicResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>SaveUserSession 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator GetStoryList(string url,int type, Action callBack, Action errorBack)
        {
            Debug.Log($"type:{type}");

            WWWForm form = new WWWForm();

            if (!string.IsNullOrEmpty(DataManager.getPlayerId()))
            {
                form.AddField("userId", DataManager.getPlayerId());
            }
            form.AddField("type", type);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            //webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                StoryResponse storyResponse = JsonUtility.FromJson<StoryResponse>(webRequest.downloadHandler.text);

                if(type == 0)
                {
                    DataManager.setNoPlayResponse(storyResponse);
                }
                else
                {
                    DataManager.storyResponse = storyResponse;
                }
                
                if (storyResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>GetStoryList 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        static IEnumerator SaveStoryRecord(string url, string storyId, Action callBack, Action errorBack)
        {
            Debug.Log($"storyId:{storyId}");

            WWWForm form = new WWWForm();
            form.AddField("userId", DataManager.getPlayerId());
            form.AddField("storyId", storyId);

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
            webRequest.SetRequestHeader("token", DataManager.getToken());

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                BasicResponse basicResponse = JsonUtility.FromJson<BasicResponse>(webRequest.downloadHandler.text);
                if (basicResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>SaveStoryRecord 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }

        public static IEnumerator Upload(string url, string fileName,byte[] fileData, Action callBack = null, Action errorBack = null)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", fileData, fileName, "video/mp4");

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            // 设置User-Agent
            webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                errorBack?.Invoke();
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);

                UploadResponse uploadResponse = JsonUtility.FromJson<UploadResponse>(webRequest.downloadHandler.text);
                if (uploadResponse.code == "0")
                {
                    Debug.Log("<color=#4aff11>SaveStoryRecord 请求成功!!!</color>");
                    callBack?.Invoke();

                }
                else
                {
                    errorBack?.Invoke();
                }
            }
        }
    }

}

