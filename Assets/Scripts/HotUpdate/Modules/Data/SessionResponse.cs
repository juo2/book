using System;
using System.Collections.Generic;


namespace XModules.Data
{

    [Serializable]
    public class SessionResponse
    {
        public string code; // ������
        public string msg; // ������Ϣ
        public List<SessionData> data; // ��������
    }

    [Serializable]
    public class SessionData
    {
        public string id; // �Ựid����ѡ
        public string userId; // �û�id����ѡ
        public string npcId; // NPCID����ѡ
        public string createTime; // ����ʱ�䣬��ѡ
        public string updateTime; // ����ʱ�䣬��ѡ
    }
}
