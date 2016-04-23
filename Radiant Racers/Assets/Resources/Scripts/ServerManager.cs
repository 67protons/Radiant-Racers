using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerManager : NetworkHost {

    void Awake(){
        base.Setup(9001, 8);
    }

    void Update()
    {
        ReceiveEvent eventData = base.Receive();
        switch (eventData.type)
        {
            case NetworkEventType.ConnectEvent:
                Debug.Log("New connect from client: " + eventData.connectionID);
                break;
            case NetworkEventType.DataEvent:
                Debug.Log(System.Text.Encoding.UTF8.GetString(eventData.data));                
                break;
            case NetworkEventType.DisconnectEvent:
                break;
        }
    }
}
