using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private Camera _mainCamera;
    private Direction _currentDirection;
    private float speed = 10f;

    void Awake()
    {
        _currentDirection = Direction.Right;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        //Debug.Log(_currentDirection);
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
        _mainCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }
}
