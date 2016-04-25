using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    [HideInInspector]
    public Dictionary<int, Player> Players = new Dictionary<int, Player>();
    private List<CellID> availableNums = new List<CellID>() { 
        CellID.Player1, CellID.Player2, CellID.Player3, CellID.Player4, CellID.Player5, CellID.Player6, CellID.Player7, CellID.Player8 };
    
    private GridManager _grid;
    private ServerManager _server;    

    void Awake()
    {
        _grid = this.GetComponent<GridManager>();
        _server = GameObject.Find("Server").GetComponent<ServerManager>();
        _server._gameManager = this;
    }

    void Start()
    {
        StartGame();        
    }

    void LateUpdate()
    {
        ChangeLog changes = new ChangeLog();        
        //Dictionary<string, int> changeLog = new Dictionary<string, int>();
        foreach (Player player in Players.Values){
            if (player.isAlive)
            {
                //Vector2 oldLoc = GridManager.GridPosition(player.transform.position);
                Vector2 oldLoc = GridManager.GridPosition(player.position);
                player.Move();                
                //changes.PlayerLocations.Add(player.playerNum, player.transform.position);
                //Vector2 newLoc = GridManager.GridPosition(player.transform.position);
                changes.PlayerLocations.Add(player.playerNum, new Vector3(player.position.x, player.position.y, player.rotation));
                Vector2 newLoc = GridManager.GridPosition(player.position);
                if (newLoc != oldLoc)
                {
                    _grid.SetCell(oldLoc, player.playerNum);
                    changes.ChangedCells.Add(oldLoc, player.playerNum);

                    if (_grid.GetCell(newLoc) != CellID.None)
                    {
                        player.isAlive = false;
                        Debug.Log("Player " + player.playerNum + " died");
                    }
                }
            }            
        }

        _server.SendAll(MessageType.StateUpdate, changes);
    }

    void StartGame()
    {        
        foreach (int i in _server.clientList)
        {
            if (Players.ContainsKey(i))
            {
                Debug.Log("Player " + i + " already exists.");
                return;
            }
            else
            {
                CreatePlayer(i);                             
            }
        }
    }

    void CreatePlayer(int connectionID)
    {
        ///Choose random playerNum (that isn't already taken)
        CellID randPlayerNum;
        randPlayerNum = availableNums[Random.Range(0, availableNums.Count)];        
        availableNums.Remove(randPlayerNum);

        ///Grab the player associated with the random playerNum and assign it that number
        GameObject player = GameObject.Find(randPlayerNum.ToString());
        //Player playerScript = player.AddComponent<Player>();
        Player playerScript = new Player();
        playerScript.playerNum = randPlayerNum;

        ///Choose random position
        List<Vector2> emptyCells = _grid.EmptyCells();
        Vector2 randomCell = emptyCells[Random.Range(0, emptyCells.Count)];
        playerScript.SetPosition(randomCell);

        ///Set direction (also avoid defaulting into a wall)        
        if (randomCell.x < _grid.gridSize.x / 2){            
            playerScript.SetDirection(Direction.Right, true);
        }
        else if (randomCell.x >= _grid.gridSize.x / 2)
        {
            playerScript.SetDirection(Direction.Left, true);
        }

        ///Add new Player to dictionary
        Players.Add(connectionID, playerScript);
        
        ///Send over the playerNum to that player - might take this out later
        _server.Send(connectionID, MessageType.SetUp, randPlayerNum);        
    }
}