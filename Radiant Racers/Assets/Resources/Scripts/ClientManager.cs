using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ClientManager : NetworkHost {

    //public Player player;
    private CellID myPlayer;
    //private Camera _mainCamera;
    private int _server;
    private Dictionary<CellID, GameObject> _playerTrails = new Dictionary<CellID, GameObject>();

    void Awake()
    {
        base.Setup(Random.Range(9002, 65000), 1);
        _server = base.Connect("104.33.20.133", 9001);

        _playerTrails[CellID.Player1] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player2] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player3] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player4] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player5] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player6] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player7] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        _playerTrails[CellID.Player8] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        //_mainCamera = Camera.main;
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
            else if (message.type == MessageType.StateUpdate)
            {
                ChangeLog changes = (ChangeLog)message.GetData();
                foreach (CellID playerNum in changes.PlayerLocations.Keys)
                {
                    if (playerNum == myPlayer)
                        //This needs to be cleaned up after I set each player's position
                        Camera.main.transform.position = new Vector3(changes.PlayerLocations[playerNum].x, changes.PlayerLocations[playerNum].y, -10);
                }
                foreach (var kvp in changes.ChangedCells)
                {                    
                    Instantiate(_playerTrails[kvp.Value], new Vector2(kvp.Key.x, -kvp.Key.y), Quaternion.identity);
                }
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
        //if (player != null)
        //    _mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
