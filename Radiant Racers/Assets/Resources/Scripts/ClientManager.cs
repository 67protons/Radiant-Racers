using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ClientManager : NetworkHost {

    public Player player;
    private CellID myPlayer;
    private Camera _mainCamera;
    private int _server;    

    void Awake()
    {
        base.Setup(Random.Range(9002, 65000), 1);
        _server = base.Connect("104.33.20.133", 9001);
        _mainCamera = Camera.main;
    }
    void Update()
    {
        PollMovement();        
        //base.Send(connectionToServer, System.Text.Encoding.UTF8.GetBytes("Hello Server"));

        ReceiveEvent recEvent = base.Receive();
        if (recEvent.type == NetworkEventType.DataEvent)
        {
            Message message = recEvent.message;
            if (message.type == MessageType.SetUp)
            {                
                CellID playerNum = (CellID)message.GetData();
                myPlayer = playerNum;
                Debug.Log(myPlayer);
            }
        }        

        Render();
    }

    void PollMovement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            base.Send(_server, MessageType.Move, Direction.Right);
            //player.SetDirection(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            base.Send(_server, MessageType.Move, Direction.Left);
            //player.SetDirection(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            base.Send(_server, MessageType.Move, Direction.Up);
            //player.SetDirection(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            base.Send(_server, MessageType.Move, Direction.Down);
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
