using System;
using System.Collections.Generic;

[Serializable]
public class StoryResponse
{
    public string code; // ������
    public string msg; // ������Ϣ
    public List<StoryData> data; // �籾�����б�
}

[Serializable]
public class StoryData
{
    public string id; // �籾id
    public string title; // ����
    public string description; // ����
    public string createTime; // ����ʱ��
#if UNITY_EDITOR
    public bool isEditor = false; //�Ƿ��������
#endif
}