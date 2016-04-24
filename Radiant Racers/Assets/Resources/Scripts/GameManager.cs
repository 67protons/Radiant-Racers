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
        ///Choose random position
        List<Vector2> emptyCells = _grid.EmptyCells();
        Vector2 randomCell = emptyCells[Random.Range(0, emptyCells.Count)];
        randomCell.x += 0.5f;
        randomCell.y = -randomCell.y - 0.5f;

        GameObject newPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/Player") as GameObject, randomCell, Quaternion.identity);
        Player playerScript = newPlayer.GetComponent<Player>();

        ///Set direction (also avoid defaulting into a wall)
        if (randomCell.x < _grid.gridSize.x / 2)
            playerScript.SetDirection(Direction.Right);
        else
        {
            playerScript.SetDirection(Direction.Left);
        }

        ///Choose random playerNum (that isn't already taken)
        CellID randomNum;        
        randomNum = availableNums[Random.Range(0, availableNums.Count)];
        playerScript.playerNum = randomNum;
        availableNums.Remove(randomNum);       

        ///Add new Player to dictionary
        Players.Add(connectionID, playerScript);
        
        ///Send over the playerNum to that player - might take this out later
        _server.Send(connectionID, MessageType.SetUp, randomNum);        
    }
}