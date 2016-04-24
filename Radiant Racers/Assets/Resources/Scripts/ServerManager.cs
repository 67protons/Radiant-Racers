using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerManager : NetworkHost {

    // Might change this to a list of Player classes in the future
    public List<int> clientList = new List<int>();

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        base.Setup(9001, 8);
    }

    void Update()
    {
        ReceiveEvent eventData = base.Receive();
        switch (eventData.type)
        {
            case NetworkEventType.ConnectEvent:
                //Debug.Log("New connect from client: " + eventData.connectionID);

                clientList.Add(eventData.connectionID);
                break;
            case NetworkEventType.DataEvent:
                //Debug.Log(eventData.connectionID + ": " + System.Text.Encoding.UTF8.GetString(eventData.data));              
                break;
            case NetworkEventType.DisconnectEvent:
                break;
        }

        SendAll(System.Text.Encoding.UTF8.GetBytes("Hello"));        
    }

    void SendAll(byte[] data)
    {
        //for (int i = 0; i < clientList.Count; i++)
        foreach(int i in clientList)
        {            
            base.Send(i, data);
        }            
    }
}
