using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ClientManager : NetworkHost {

    public Player player;
    private Camera _mainCamera;
    private int connectionToServer;
    //private Message message = new Message(MessageType.SetUp, "blank");    

    void Awake()
    {
        base.Setup(Random.Range(9002, 65000), 1);
        connectionToServer = base.Connect("104.33.20.133", 9001);
        _mainCamera = Camera.main;
    }
    void Update()
    {
        PollMovement();
        UpdateGrid();
        base.Send(connectionToServer, System.Text.Encoding.UTF8.GetBytes("Hello Server"));

        ReceiveEvent eventData = base.Receive();
        if (eventData.type == NetworkEventType.DataEvent)
        {
            var message = JsonUtility.FromJson<Message>(System.Text.Encoding.UTF8.GetString(eventData.data));
            if (message.type == MessageType.SetUp)
            {
                var subMessage = JsonUtility.FromJson<CellID>(message.subJson);
                Debug.Log(subMessage);
            }            
        }        

        Render();
    }

    void PollMovement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {            
            //player.SetDirection(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            //player.SetDirection(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            //player.SetDirection(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            //player.SetDirection(Direction.Down);
        }        
    }

    void UpdateGrid()
    {        
        //GameObject meh = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        //if (GridManager.GetCell(player.headLocation) == CellID.None)
        //{
        //    Instantiate(meh, new Vector2(player.headLocation.x, -player.headLocation.y), Quaternion.identity);
        //}
    }

    void Render()
    {
        if (player != null)
            _mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
