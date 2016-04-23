using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : NetworkHost {

    int connectionToServer;

    void Awake()
    {
        base.Setup(Random.Range(9002, 65000), 1);
        connectionToServer = base.Connect("192.168.0.4", 9001);        
    }

    void Update()
    {        
        //base.Send(connectionToServer, System.Text.Encoding.UTF8.GetBytes("Hello"));
    }
}
