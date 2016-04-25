using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerManager : NetworkHost {    
    public List<int> clientList = new List<int>();

    [HideInInspector]
    public GameManager _gameManager;

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        //base.Setup(NetworkHost.Port, 8, NetworkHost.ServerIP);
        base.Setup(NetworkHost.Port, 8);        
    }

    void Update()
    {
        ReceiveEvent recEvent = base.Receive();
        switch (recEvent.type)
        {
            case NetworkEventType.ConnectEvent:
                clientList.Add(recEvent.sender);
                break;
            case NetworkEventType.DataEvent:
                Message message = recEvent.message;
                if (message.type == MessageType.Move)
                {                    
                    Direction moveDirection = (Direction)message.GetData();
                    _gameManager.Players[recEvent.sender].SetDirection(moveDirection);
                }                      
                break;
            case NetworkEventType.DisconnectEvent:
                break;
        }   
    }

    public void SendAll(MessageType messageType, object data)
    {        
        foreach(int i in clientList)
        {            
            base.Send(i, messageType, data);
        }            
    }
}
