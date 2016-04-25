using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct PlayerData
{
    public GameObject playerObject, playerTrail, playerImage;
    public PlayerData(GameObject playerObject, GameObject playerTrail, GameObject playerImage)
    {
        this.playerObject = playerObject;
        this.playerTrail = playerTrail;
        this.playerImage = playerImage;
    }
}

public class ClientManager : NetworkHost {       

    //private string _ip = "192.168.0.4";
    //private int _portNum = 9001;
    //public string IP { get { return this._ip; } set { this._ip = value; } }
    //public int Port { get { return this._portNum; } set { this._portNum = value; } }
    

    private bool _isGameStarted = false;
    private CellID _myPlayer;
    //private Camera _mainCamera;
    private int _server;    

    ///In-Game Variables    
    //private Dictionary<CellID, GameObject> _playerTrails = new Dictionary<CellID, GameObject>();
    //private Dictionary<CellID, GameObject> _players = new Dictionary<CellID, GameObject>();
    private Dictionary<CellID, PlayerData> _players = new Dictionary<CellID, PlayerData>();
    private GameObject _hud;
    private RectTransform _hudArrow;    

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        base.Setup(Random.Range(9002, 65000), 1);
        _server = base.Connect(NetworkHost.ServerIP, NetworkHost.Port);

        
        //_mainCamera = Camera.main;
    }
    void Update()
    {
        if (_isGameStarted)
        {
            PollMovement();
            ManageHud();
        }        

        ReceiveEvent recEvent = base.Receive();
        if (recEvent.type == NetworkEventType.DataEvent)
        {
            Message message = recEvent.message;
            if (message.type == MessageType.SetUp)
            {
                CellID playerNum = (CellID)message.GetData();
                _myPlayer = playerNum;
                Debug.Log(playerNum);
                StartGame();
            }
            else if (message.type == MessageType.StateUpdate)
            {
                ChangeLog changes = (ChangeLog)message.GetData();
                foreach (var kvp in _players)
                {
                    if (!changes.PlayerLocations.ContainsKey(kvp.Key))
                    {
                        kvp.Value.playerObject.SetActive(false);
                        kvp.Value.playerImage.SetActive(false);
                        //_players.Remove(kvp.Key);
                    }
                }
                
                foreach (var playerTransform in changes.PlayerLocations)
                {
                    CellID playerNum = playerTransform.Key;
                    Vector2 position = (Vector2)playerTransform.Value;
                    float rotation = playerTransform.Value.z;
                    //_players[playerNum].transform.position = position;
                    //_players[playerNum].transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                    _players[playerNum].playerObject.transform.position = position;
                    _players[playerNum].playerObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                    if (playerTransform.Key == _myPlayer)                        
                        Camera.main.transform.position = new Vector3(position.x, position.y, -10);                        
                }
                foreach (var kvp in changes.ChangedCells)
                {
                    //Instantiate(_playerTrails[kvp.Value], new Vector2(kvp.Key.x, -kvp.Key.y), Quaternion.identity);
                    Instantiate(_players[kvp.Value].playerTrail, new Vector2(kvp.Key.x, -kvp.Key.y), Quaternion.identity);
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

    void ManageHud()
    {
        _hudArrow.localPosition = _players[_myPlayer].playerImage.transform.localPosition - new Vector3(50, 0, 0);
    }

    void StartGame()
    {
        //_playerTrails[CellID.Player1] = Resources.Load("Prefabs/OrangeTrail") as GameObject;
        //_playerTrails[CellID.Player2] = Resources.Load("Prefabs/GreenTrail") as GameObject;
        //_playerTrails[CellID.Player3] = Resources.Load("Prefabs/BlueTrail") as GameObject;
        //_playerTrails[CellID.Player4] = Resources.Load("Prefabs/YellowTrail") as GameObject;
        //_playerTrails[CellID.Player5] = Resources.Load("Prefabs/PurpleTrail") as GameObject;
        //_playerTrails[CellID.Player6] = Resources.Load("Prefabs/RedTrail") as GameObject;
        //_playerTrails[CellID.Player7] = Resources.Load("Prefabs/PinkTrail") as GameObject;
        //_playerTrails[CellID.Player8] = Resources.Load("Prefabs/BrownTrail") as GameObject;

        //_players[CellID.Player1] = GameObject.Find("Player1");
        //_players[CellID.Player2] = GameObject.Find("Player2");
        //_players[CellID.Player3] = GameObject.Find("Player3");
        //_players[CellID.Player4] = GameObject.Find("Player4");
        //_players[CellID.Player5] = GameObject.Find("Player5");
        //_players[CellID.Player6] = GameObject.Find("Player6");
        //_players[CellID.Player7] = GameObject.Find("Player7");
        //_players[CellID.Player8] = GameObject.Find("Player8");

        _players[CellID.Player1] = new PlayerData(GameObject.Find("Player1"), Resources.Load("Prefabs/OrangeTrail") as GameObject, GameObject.Find("Player1Image"));
        _players[CellID.Player2] = new PlayerData(GameObject.Find("Player2"), Resources.Load("Prefabs/GreenTrail") as GameObject, GameObject.Find("Player2Image"));
        _players[CellID.Player3] = new PlayerData(GameObject.Find("Player3"), Resources.Load("Prefabs/BlueTrail") as GameObject, GameObject.Find("Player3Image"));
        _players[CellID.Player4] = new PlayerData(GameObject.Find("Player4"), Resources.Load("Prefabs/YellowTrail") as GameObject, GameObject.Find("Player4Image"));
        _players[CellID.Player5] = new PlayerData(GameObject.Find("Player5"), Resources.Load("Prefabs/PurpleTrail") as GameObject, GameObject.Find("Player5Image"));
        _players[CellID.Player6] = new PlayerData(GameObject.Find("Player6"), Resources.Load("Prefabs/RedTrail") as GameObject, GameObject.Find("Player6Image"));
        _players[CellID.Player7] = new PlayerData(GameObject.Find("Player7"), Resources.Load("Prefabs/PinkTrail") as GameObject, GameObject.Find("Player7Image"));
        _players[CellID.Player8] = new PlayerData(GameObject.Find("Player8"), Resources.Load("Prefabs/BrownTrail") as GameObject, GameObject.Find("Player8Image"));

        _hud = GameObject.Find("HUD");
        _hudArrow = _hud.transform.FindChild("Arrow").GetComponent<RectTransform>();
        _isGameStarted = true;
    }
}
