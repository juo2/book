using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.UI;
using XModules.Data;

public class WebSocketTest : MonoBehaviour
{
    WebSocket websocket = null;

    public Button send;

    public InputField inputField;

    public bool isConnecting = false;
    void Awake()
    {

        send.onClick.AddListener(() => 
        {
            SendMessageWebSocket(inputField.text);
        });
       
    }

    async void OnEnable()
    {
        if (DataManager.playerResponse == null)
            return;

        websocket = new WebSocket($"ws://119.91.133.26/chat/websocket/1/{DataManager.playerResponse.data.id}");

        websocket.OnOpen += () =>
        {
            isConnecting = true;
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            isConnecting = false;
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            isConnecting = false;
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! " + message);

            DataManager.createChatData("1", "user", inputField.text);
            DataManager.createChatData("1", "assistant", message);
        };
        Debug.Log("������websocket.Connect");
        await websocket.Connect();
    }

    async void OnDisable()
    {
        if (websocket == null)
            return;

        Debug.Log("������websocket.Close");
        websocket.CancelConnection();
        await websocket.Close();

        websocket = null;
    }

    // �������������������Ϣ
    public async void SendMessageWebSocket(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            Debug.Log($"SendMessageWebSocket:{message}");
            // �����ı���Ϣ
            await websocket.SendText(message);
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (isConnecting)
            websocket.DispatchMessageQueue();
#endif
    }

    private void OnDestroy()
    {
        send.onClick.RemoveAllListeners();
    }
}
