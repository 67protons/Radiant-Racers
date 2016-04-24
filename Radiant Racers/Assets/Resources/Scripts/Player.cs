using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public bool isAlive = true;
    public CellID playerNum;

    private Direction _currentDirection;
    private float speed = 10f;

    public Vector2 headLocation
    {
        get { return GridManager.GridPosition(this.transform.position); }
    }
    public Vector2 tailLocation
    {
        get { return GridManager.GridPosition(
            new Vector2(this.transform.position.x + Mathf.Sin(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad),
                        this.transform.position.y - Mathf.Cos(this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad)));
        }
    }

    void Awake()
    {        
        _currentDirection = Direction.Right;          
    }

    public void Move()
    {       
        switch (_currentDirection)
        {
            case Direction.Up:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));                
                break;
            case Direction.Left:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));                
                break;
            case Direction.Down:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));                
                break;
            case Direction.Right:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));                
                break;
        }
        
        this.transform.Translate(Vector2.up * speed * Time.deltaTime);                
    }

    public void SetPosition(Vector2 gridPosition)
    {
        if (this.isAlive)
        {
            gridPosition.x += 0.5f;
            gridPosition.y = -gridPosition.y - 0.5f;
            this.transform.position = gridPosition;
        }       
    }

    public void SetDirection(Direction direction, bool byPassCheck = false)
    {
        if (this.isAlive)
        {
            if (byPassCheck || this._currentDirection != OppositeDirection(direction))
            {
                SetPosition(GridManager.GridPosition(this.transform.position));
                this._currentDirection = direction;
            }            
        }
    }

    private Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Left:
                return Direction.Right;
            case Direction.Down:
                return Direction.Up;
            case Direction.Right:
                return Direction.Left;
            default:
                return Direction.Up;
        }
    }
}
