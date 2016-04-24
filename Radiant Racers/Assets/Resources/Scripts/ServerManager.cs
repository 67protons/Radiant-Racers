using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerManager : NetworkHost {

    [HideInInspector]
    public GameManager _gameManager;
    // Might change this to a list of Player classes in the future
    public List<int> clientList = new List<int>();

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        base.Setup(9001, 8);
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

        //SendAll(System.Text.Encoding.UTF8.GetBytes("Hello"));        
    }

    public void SendAll(MessageType messageType, object data)
    {        
        foreach(int i in clientList)
        {            
            base.Send(i, messageType, data);
        }            
    }
}
