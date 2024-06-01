using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XModules.Data
{
    [Serializable]
    public class ChatResponse
    {
        public string code; // ������
        public string msg; // ������Ϣ
        public List<ChatData> data; // The array of objects

        
    }

    [Serializable]
    public class ChatData
    {
        public string id; // �����¼id
        public string userId; // �û�id
        public string npcId; // npcid
        public string role; // ��ɫ
        public string content; // �ظ�����
        public string createTime; // ����ʱ��
    }

    

}


