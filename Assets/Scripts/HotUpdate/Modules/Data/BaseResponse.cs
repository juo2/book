using System;

namespace XModules.Data
{
    [Serializable]
    public class BasicResponse
    {
        public string code; // ������
        public string msg; // ������Ϣ
        public object data; // ���ݣ�����Ϊnull
    }
}