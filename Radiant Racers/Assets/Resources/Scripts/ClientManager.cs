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
    private Dictionary<CellID, GameObject> _players = new Dictionary<CellID, GameObject>();

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
                StartGame();
            }
            else if (message.type == MessageType.StateUpdate)
            {
                ChangeLog changes = (ChangeLog)message.GetData();
                //foreach (CellID playerNum in changes.PlayerLocations.Keys)
                foreach (var playerData in changes.PlayerLocations)
                {
                    CellID playerNum = playerData.Key;
                    Vector2 position = (Vector2)playerData.Value;
                    float rotation = playerData.Value.z;
                    _players[playerNum].transform.position = position;
                    _players[playerNum].transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                    if (playerData.Key == _myPlayer)
                        //This needs to be cleaned up after I set each player's position
                        Camera.main.transform.position = new Vector3(position.x, position.y, -10);                        
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

    void StartGame()
    {
        _players[CellID.Player1] = GameObject.Find("Player1");
        _players[CellID.Player2] = GameObject.Find("Player2");
        _players[CellID.Player3] = GameObject.Find("Player3");
        _players[CellID.Player4] = GameObject.Find("Player4");
        _players[CellID.Player5] = GameObject.Find("Player5");
        _players[CellID.Player6] = GameObject.Find("Player6");
        _players[CellID.Player7] = GameObject.Find("Player7");
        _players[CellID.Player8] = GameObject.Find("Player8");
        _isGameStarted = true;
    }
}
