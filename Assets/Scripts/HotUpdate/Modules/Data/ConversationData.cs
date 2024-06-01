using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace XModules.Data
{
    public static class ConversationData
    {
        /// <summary>
        /// �洢�����籾��XML�ĵ�
        /// </summary>

        [Serializable]
        public class Struct_PlotData
        {
            public List<XElement> ListMainPlot = new();
            public class Struct_Choice
            {
                public string Title;
                public int JumpID;
            }
            public class Struct_CharacterInfo
            {
                public string characterID;
                public string name;
                public string image;
                public bool isSelf = false;
            }
            public List<Struct_CharacterInfo> CharacterInfoList = new();
            public List<Struct_Choice> ChoiceTextList = new();
            /// <summary>
            /// ��ǰ�ľ���ڵ�
            /// </summary>
            public XElement NowPlotDataNode;

            /// <summary>
            /// ��ǰ�Ƿ�Ϊ��֧����ڵ�
            /// </summary>
            public int NextJumpID;

            public class HistoryContent
            {
                public string id;
                public string speaker;
                public string content;
                public string optContent;
            }
            public List<HistoryContent> historyContentList = new List<HistoryContent>();
        }

        public static Struct_PlotData PlotData = new();

        public static Struct_PlotData.Struct_CharacterInfo SelfCharacterInfo = null;

        public static Struct_PlotData.Struct_CharacterInfo TempNpcCharacterInfo = null;

        public static List<string> cacheOutMessageList = new List<string>();
        public static int cacheIndex = 0;
        public static int currentCacheIndex = 0;

        public static string tempInputMessage = null;
        public static string webSocketSteamContent = "";
        public static string currentWebSocketSteamContent = "";

        public static string currentStory = null;


        public static Struct_PlotData.Struct_CharacterInfo AddCharacter()
        {
            var characterInfo = new Struct_PlotData.Struct_CharacterInfo();
            var _CharacterId = PlotData.NowPlotDataNode.Attribute("CharacterID").Value;
            characterInfo.name = PlotData.NowPlotDataNode.Attribute("CharacterName").Value;
            characterInfo.image = PlotData.NowPlotDataNode.Attribute("CharacterImage").Value;
            characterInfo.characterID = $"{currentStory}_{ _CharacterId}";
            characterInfo.isSelf = PlotData.NowPlotDataNode.Attribute("IsSelf").Value == "True";

            if (DataManager.getNpcById(characterInfo.characterID) == null)
            {
                Debug.LogError($"ǰ������ݲ���Ӧ characterInfo.characterID:{characterInfo.characterID}");
            }

            if (characterInfo.isSelf)
            {
                SelfCharacterInfo = characterInfo;
            }
            else
            {
                TempNpcCharacterInfo = characterInfo;
            }

            return characterInfo;
        }

        public static Struct_PlotData.Struct_CharacterInfo GetCharacterObjectByName(string _ID)
        {
            string ID = $"{currentStory}_{ _ID}";
            return PlotData.CharacterInfoList.Find(t => t.characterID == ID);
        }

        /// <summary>
        /// ����һ����ɫ
        /// </summary>
        /// <param name="ID"></param>
        public static void DestroyCharacterByID(string _ID)
        {
            string ID = $"{currentStory}_{ _ID}";

            var _ = PlotData.CharacterInfoList.Find(t => t.characterID == ID);
            //SendCharMessage(ID, "Quit");
            PlotData.CharacterInfoList.Remove(_);
        }

        /// <summary>
        /// ����
        /// </summary>
        public static void ResetPlotData()
        {
            PlotData = new Struct_PlotData();
            IsCanJump = true;
            IsSpeak = false;
            SelfCharacterInfo = null;
            TempNpcCharacterInfo = null;
        }

        /// <summary>
        ///�Ƿ�������� 
        /// </summary>
        public static bool IsCanJump = true;

        /// <summary>
        /// ��ǰ�Ƿ����ڷ���
        /// ���Ϊ������Կ�ʼ��һ��
        /// ������ı��������ʱ��ҲΪTrue
        /// </summary>
        public static bool IsSpeak;

        public static void JumpNext(int jumpID,string title)
        {
            PlotData.NextJumpID = jumpID;
            IsCanJump = true;
            if (jumpID == -1)
            {
                return;
            }
            PlotData.NextJumpID = jumpID;

            int count = PlotData.historyContentList.Count;
            if (count > 0)
            {
                var historyContent = PlotData.historyContentList[count - 1];
                historyContent.optContent = title;
            }
        }

        public static void AddHistoryContent(string id, string speaker, string content = "",string optContent = "")
        {
            Struct_PlotData.HistoryContent historyContent = new Struct_PlotData.HistoryContent();
            historyContent.id = id;
            historyContent.speaker = speaker;
            historyContent.content = content;
            historyContent.optContent = optContent;

            PlotData.historyContentList.Add(historyContent);
        }

        public static void ClearHistoryContent()
        {
            PlotData.historyContentList.Clear();
        }

        public static List<Struct_PlotData.HistoryContent> GetHistoryContentList()
        {
            return PlotData.historyContentList;
        }

        public static void ClearCacheOneChar()
        {
            webSocketSteamContent = "";
            currentWebSocketSteamContent = "";
            cacheIndex = 0;
            currentCacheIndex = 0;
            cacheOutMessageList.Clear();
        }

        public static string getCacheOneChar()
        {
            //Debug.Log($"getCacheOneChar cacheOutMessageList.Count:{cacheOutMessageList.Count}");

            for (int i = cacheIndex; i < cacheOutMessageList.Count; i++)
            {
                webSocketSteamContent += cacheOutMessageList[i];
            }

            cacheIndex = cacheOutMessageList.Count;

            //Debug.Log($"getCacheOneChar webSocketSteamContent.Length:{webSocketSteamContent.Length}");

            if (currentCacheIndex < webSocketSteamContent.Length)
            {
                // ��⵱ǰ�ַ��Ƿ�Ϊ '['
                if (webSocketSteamContent[currentCacheIndex] == '[')
                {
                    // ���ʣ����ַ����Ƿ�ʼΪ "[Done]"
                    string remainingContent = webSocketSteamContent.Substring(currentCacheIndex);
                    if (remainingContent.StartsWith("[DONE]"))
                    {
                        currentCacheIndex += "[DONE]".Length; // ������������ "[Done]"
                        currentWebSocketSteamContent += "[DONE]"; // �� "[Done]" ��ӵ���ǰWebSocket������
                        return "[DONE]"; // ֱ�ӷ��� "[Done]"
                    }
                }

                // ����ԭ�����߼���������һ���ַ�
                char targetOut = webSocketSteamContent[currentCacheIndex];
                currentCacheIndex++;
                //Debug.Log($"getCacheOneChar currentCacheIndex:{currentCacheIndex}");
                currentWebSocketSteamContent += targetOut;
                //Debug.Log($"getCacheOneChar currentWebSocketSteamContent:{currentWebSocketSteamContent}");
                return targetOut.ToString();
            }

            // ���û�и�����ַ����Է��أ��������Ҫ�����������
            return string.Empty; // ���߷���null���������ʵ�ֵ
        }

        public static void completeCacheOneChar()
        {
            //cacheOutMessageList.Clear();
        }

        public static int getOneShotChatSelect()
        {
            var strArray = webSocketSteamContent.Split("|");

            if (strArray.Length == 2)
            {
                string temp = strArray[1].Replace(" ", "");
                return (int)Char.GetNumericValue(temp[0]);;
            }
            else
            {
                Debug.LogError("webSocketSteamContent û�ҵ� �ָ�� |");
                return 0;
            }
        }
    }
}