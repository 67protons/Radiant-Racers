using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    [HideInInspector]
    public Dictionary<int, Player> Players = new Dictionary<int, Player>();

    private GridManager _grid;
    private ServerManager _server;    

    void Awake()
    {
        _grid = this.GetComponent<GridManager>();
        _server = GameObject.Find("Server").GetComponent<ServerManager>();
    }

    void Start()
    {
        StartGame();
        //byte[] meh = System.BitConverter.GetBytes((char)CellID.None);
        //Debug.Log(System.BitConverter.ToChar(meh, 0));
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
                //_server.Send(i, System.BitConverter.GetBytes(PlayerID.A));                
            }
        }
    }

    void CreatePlayer(int playerNum)
    {
        List<Vector2> emptyCells = _grid.EmptyCells();
        Vector2 randomCell = emptyCells[Random.Range(0, emptyCells.Count)];
        randomCell.x += 0.5f;
        randomCell.y = -randomCell.y - 0.5f;
        GameObject newPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/Player") as GameObject, randomCell, Quaternion.identity);
        Player playerScript = newPlayer.GetComponent<Player>();
        if (randomCell.x < _grid.gridSize.x / 2)
            playerScript.SetDirection(Direction.Right);
        else
        {
            playerScript.SetDirection(Direction.Left);
        }

        Players.Add(playerNum, playerScript);
    }
}
