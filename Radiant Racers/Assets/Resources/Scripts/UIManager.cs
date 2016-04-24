using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("ServerGame");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("ClientGame");
        }
	}
}
