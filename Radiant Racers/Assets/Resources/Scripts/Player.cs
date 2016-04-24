using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
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

    void Update()
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

    public void SetDirection(Direction direction)
    {
        this._currentDirection = direction;
    }
}
