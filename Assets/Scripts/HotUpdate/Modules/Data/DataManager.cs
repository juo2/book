using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XModules.Data
{
    public static class DataManager
    {
        public static Dictionary<string,ChatResponse> chatResponseDic = new Dictionary<string, ChatResponse>();

        public static NPCResponse npcResponse = null;
        public static PlayerResponse playerResponse = null;
        public static SessionResponse sessionResponse = null;
        public static StoryResponse storyResponse = null;
        public static StoryResponse storyNoPlayResponse = null;
        public static OneShotChatResponse oneShotChatResponse = null;
        
        public static void setNoPlayResponse(StoryResponse _storyNoPlayResponse)
        {
            DataManager.storyNoPlayResponse = _storyNoPlayResponse;
#if  UNITY_EDITOR
            string[] fileEntries = Directory.GetFiles(Application.streamingAssetsPath);
            foreach (string fileName in fileEntries)
            {
                // ����ļ���չ���Ƿ�Ϊ .xml
                if (fileName.EndsWith(".xml"))
                {
                    string tmp = fileName.Replace(Application.streamingAssetsPath + "\\","");
                    // ��ȡ����ӡ XML �ļ�������
                    //string content = File.ReadAllText(fileName);
                    Debug.Log($"��ȡ�� XML �ļ�: {fileName}");
                    //Debug.Log($"����: {content}");

                    StoryData storyData = new StoryData();
                    storyData.id = tmp;
                    storyData.title = tmp;
                    storyData.isEditor = true;
                    DataManager.storyNoPlayResponse.data.Insert(0, storyData);
                }
            }
#endif
        }

        public static string getPlayerId()
        {
            //��ʱ��token��ס
            string id = PlayerPrefs.GetString("TEMP_ID");
            if (string.IsNullOrEmpty(id))
            {
                if (playerResponse == null)
                {
                    Debug.Log("getPlayerId playerResponse == null");
                    return "";
                }
                else
                {
                    return playerResponse.data.id;
                }
            }

            return id;
        }

        public static string getToken()
        {
            //��ʱ��token��ס
            string token = PlayerPrefs.GetString("TEMP_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                return playerResponse.data.token;
            }

            return token;
        }

        public static ChatData createChatData(string npcId,string role,string content)
        {
            ChatData chatdata = new ChatData();
            chatdata.userId = getPlayerId();
            chatdata.content = content;
            chatdata.npcId = npcId;
            chatdata.role = role;

            if (chatResponseDic.TryGetValue(npcId, out ChatResponse chatResponse))
            {
                chatResponse.data.Add(chatdata);
            }

            return chatdata;
        }

        public static List<NPCData> getNpcList()
        {
            if (npcResponse == null)
            {
                return new List<NPCData>();
            }
            else
            {
                return npcResponse.data;
            }
        }

        public static NPCData getNpcById(string npcId)
        {
            foreach(var npc in getNpcList())
            {
                if (npcId == npc.id)
                {
                    return npc;
                }
            }

            return null;
        }

        public static List<SessionData> getSessionList()
        {
            return sessionResponse.data;
        }

        public static void detelteChatResponse(string npcId)
        {
            List<ChatData> chatDataList = getChatDatabyNpcId(npcId);

            if(chatDataList != null)
            {
                chatDataList.Clear();
            }
            else
            {
                Debug.LogError($"û�ҵ�npcId:{npcId}");
            }
        }

        public static void addChatResponse(string npcId, ChatResponse chatResponse)
        {
            if (!chatResponseDic.ContainsKey(npcId))
            {
                chatResponseDic[npcId] = chatResponse;
            }
        }

        public static bool IsHasChatResponse(string npcId)
        {
            return chatResponseDic.ContainsKey(npcId);
        }

        public static List<ChatData> getChatDatabyNpcId(string npcId)
        {
            if(chatResponseDic.TryGetValue(npcId,out ChatResponse chatResponse))
            {
                return chatResponse.data;
            }

            return null;
        }

        public static string getNpcResponse()
        {
            if (oneShotChatResponse == null)
            {
                return "����ʧ��";
            }
            else
            {
                return oneShotChatResponse.data.npcResponse;
            }
        }

        public static string getWebStreamSocketRequest(string textContent, string question, string options)
        {
            WebStreamSocketRequest webStreamSocketRequest = new WebStreamSocketRequest();
            webStreamSocketRequest.userId = getPlayerId();
            webStreamSocketRequest.npcId = ConversationData.TempNpcCharacterInfo.characterID;
            webStreamSocketRequest.textContent = textContent;
            webStreamSocketRequest.question = question;
            webStreamSocketRequest.options = options;
            webStreamSocketRequest.storyId = ConversationData.currentStory;

            return JsonUtility.ToJson(webStreamSocketRequest);
        }

        public static string getWebStreamSocketLoopRequest(string question)
        {
            WebStreamSocketLoopRequest webStreamSocketRequest = new WebStreamSocketLoopRequest();
            webStreamSocketRequest.userId = getPlayerId();
            webStreamSocketRequest.npcId = ConversationData.TempNpcCharacterInfo.characterID;
            webStreamSocketRequest.question = question;
            webStreamSocketRequest.storyId = ConversationData.currentStory;

            return JsonUtility.ToJson(webStreamSocketRequest);
        }

        public static List<StoryData> getStoryList()
        {
            return storyResponse.data;
        }

        public static List<StoryData> getStoryNoPlayList()
        {
            return storyNoPlayResponse.data;
        }

    }
}


