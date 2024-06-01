using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XModules.Data
{

    [Serializable]
    public class NPCResponse
    {
        public string code; // ������
        public string msg; // ������Ϣ
        public List<NPCData> data; // ����
    }

    [Serializable]
    public class NPCData
    {
        public string id; // NPCid
        public string content; // Ԥ������
        public string description; // ������
        public string createTime; // ����ʱ��
    }
    
}


