using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ClientManager : NetworkHost {       

    //private string _ip = "192.168.0.4";
    //private int _portNum = 9001;
    //public string IP { get { return this._ip; } set { this._ip = value; } }
    //public int Port { get { return this._portNum; } set { this._portNum = value; } }
    

    private bool _isGameStarted = false;
    private CellID _myPlayer;
    //private Camera _mainCamera;
    private int _server;
    private Dictionary<CellID, GameObject> _playerTrails = new Dictionary<CellID, GameObject>();    

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        base.Setup(Random.Range(9002, 65000), 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);

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
        if (_isGameStarted)
        {
            PollMovement();
        }        

        ReceiveEvent recEvent = base.Receive();
        if (recEvent.type == NetworkEventType.DataEvent)
        {
            Message message = recEvent.message;
            if (message.type == MessageType.SetUp)
            {
                CellID playerNum = (CellID)message.GetData();
                _myPlayer = playerNum;
                _isGameStarted = true;
            }
            else if (message.type == MessageType.StateUpdate)
            {
                ChangeLog changes = (ChangeLog)message.GetData();
                foreach (CellID playerNum in changes.PlayerLocations.Keys)
                {
                    if (playerNum == _myPlayer)
                        //This needs to be cleaned up after I set each player's position
                        Camera.main.transform.position = new Vector3(changes.PlayerLocations[playerNum].x, changes.PlayerLocations[playerNum].y, -10);
                }
                foreach (var kvp in changes.ChangedCells)
                {
                    Instantiate(_playerTrails[kvp.Value], new Vector2(kvp.Key.x, -kvp.Key.y), Quaternion.identity);
                }
            }
        }
    }

    void PollMovement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            base.Send(_server, MessageType.Move, Direction.Right);            
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            base.Send(_server, MessageType.Move, Direction.Left);            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            base.Send(_server, MessageType.Move, Direction.Up);            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            base.Send(_server, MessageType.Move, Direction.Down);            
        }        
    }
}
