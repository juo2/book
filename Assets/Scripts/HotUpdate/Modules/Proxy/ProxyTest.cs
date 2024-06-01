using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking; // ������������
using UnityEngine.UI;
using XModules.Data;

public class ProxyTest : MonoBehaviour
{
    public string email = "849616969@qq.com";
    string url = "http://119.91.133.26";

    public Button sendCodeBtn;
    public Button loginBtn;
    public InputField inputField;

    public Button npcAllListBtn;
    public Button getChatRecordBtn;
    public Button getUserSessionBtn;

    // Start is called before the first frame update
    void Start()
    {
        sendCodeBtn.onClick.AddListener(() => 
        { 
            StartCoroutine(SendCodeRequest($"{url}/chat/user/sendCode"));
        });

        loginBtn.onClick.AddListener(() => 
        {
            StartCoroutine(PostRequest($"{url}/chat/user/login", inputField.text));
        });

        npcAllListBtn.onClick.AddListener(() => 
        {
            StartCoroutine(GetNPCAllList($"{url}/chat/npc/npcAllList", DataManager.playerResponse.data.token));
        });

        getChatRecordBtn.onClick.AddListener(() => 
        {
            StartCoroutine(GetChatRecord($"{url}/chat/chatRecord/getChatRecord", DataManager.playerResponse.data.id, DataManager.playerResponse.data.token));
        });

        getUserSessionBtn.onClick.AddListener(() => 
        { 
            StartCoroutine(GetUserSessionList($"{url}/chat/chatRecord/getUserSessionList"));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SendCodeRequest(string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // ����User-Agent����Ȼ��Unity���ⲻ�Ǳ���ģ���Ϊ�˱���һ���ԣ�������Ȼ�����������
        webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    IEnumerator PostRequest(string url, string code)
    {

        Debug.Log($"code:{code}");

        // ʹ��WWWForm������������
        WWWForm form = new WWWForm();
        form.AddField("loginType", "1");
        form.AddField("email", email);
        form.AddField("code", code);
        form.AddField("accessToken", "");

        // ����UnityWebRequest������URL�ͷ���
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // ����User-Agent����ѡ��
        webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");

        // �������󲢵ȴ���Ӧ
        yield return webRequest.SendWebRequest();

        // ����Ƿ��д�����
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            // ��ӡ������Ϣ
            Debug.LogError(webRequest.error);
        }
        else
        {
            // ����ɹ���ʹ��webRequest.downloadHandler.text��ȡ��Ӧ����

            DataManager.playerResponse = JsonUtility.FromJson<PlayerResponse>(webRequest.downloadHandler.text);

            Debug.Log(webRequest.downloadHandler.text);

        }
    }

    IEnumerator GetNPCAllList(string url,string token)
    {
        Debug.Log($"token:{token}");

        // �������û�б����ݣ���������Ȼ����һ���յ�WWWForm�����Է���UnityWebRequest.Post�Ĳ���Ҫ��
        WWWForm form = new WWWForm();
        //form.AddField("token", token);
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // ����User-Agent����Ȼ��Unity���ⲻ�Ǳ���ģ���Ϊ�˱���һ���ԣ�������Ȼ�����������
        webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
        webRequest.SetRequestHeader("token", token);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            DataManager.npcResponse = JsonUtility.FromJson<NPCResponse>(webRequest.downloadHandler.text);
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    IEnumerator GetChatRecord(string url,string playerid,string token)
    {

        Debug.Log($"playerResponse.data.id:{DataManager.playerResponse.data.id}");

        WWWForm form = new WWWForm();
        form.AddField("userId", playerid);
        form.AddField("npcId", "1");

        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // ����User-Agent
        webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
        webRequest.SetRequestHeader("token", token);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            //DataManager.chatResponse = JsonUtility.FromJson<ChatResponse>(webRequest.downloadHandler.text);
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    static IEnumerator GetUserSessionList(string url)
    {

        Debug.Log($"playerResponse.data.id:{DataManager.getPlayerId()}");

        WWWForm form = new WWWForm();
        form.AddField("userId", DataManager.getPlayerId());

        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // ����User-Agent
        webRequest.SetRequestHeader("User-Agent", "Apifox/1.0.0 (https://apifox.com)");
        webRequest.SetRequestHeader("token", DataManager.getToken());

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {

            Debug.Log(webRequest.downloadHandler.text);

            SessionResponse sessionResponse = JsonUtility.FromJson<SessionResponse>(webRequest.downloadHandler.text);

            if (sessionResponse.code == "0")
            {
                DataManager.sessionResponse = sessionResponse;
                Debug.Log("<color=#4aff11>GetUserSessionList ����ɹ�!!!</color>");
            }
            else
            {
            }
        }
    }

}
