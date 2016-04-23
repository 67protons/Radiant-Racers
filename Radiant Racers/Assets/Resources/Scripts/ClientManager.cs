using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : NetworkHost {

    int connectionToServer;

    void Awake()
    {
        base.Setup(Random.Range(9002, 65000), 1);
        connectionToServer = base.Connect("104.33.20.133", 9001);        
    }

    void Update()
    {        
        ReceiveEvent eventData = base.Receive();
        if (eventData.type == NetworkEventType.DataEvent)
        {
            //Debug.Log(this.gameObject.name + ": " + System.Text.Encoding.UTF8.GetString(eventData.data));
        }

        base.Send(connectionToServer, System.Text.Encoding.UTF8.GetBytes("Hello Server"));
    }
}
