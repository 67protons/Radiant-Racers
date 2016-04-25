using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private GameObject _networkPanel;
    private Text _actionLabel;
    private InputField _ipInput, _portInput;
    private bool _isCreating = false;
    private bool _isJoining = false;

    void Awake()
    {
        _networkPanel = GameObject.Find("NetworkPanel");
        if (_networkPanel != null)
        {
            _actionLabel = _networkPanel.transform.FindChild("ActionLabel").GetComponent<Text>();
            _ipInput = _networkPanel.transform.FindChild("IPInput").GetComponent<InputField>();
            _ipInput.text = NetworkHost.ServerIP;
            _portInput = _networkPanel.transform.FindChild("PortInput").GetComponent<InputField>();
            _portInput.text = NetworkHost.Port.ToString();
            _networkPanel.SetActive(false);
        }
    }

    public void PressCreate()
    {
        _isJoining = false;
        _isCreating = true;
        _networkPanel.SetActive(true);
        _actionLabel.text = "Creating Game";
    }

    public void PressJoin()
    {
        _isCreating = false;
        _isJoining = true;
        _networkPanel.SetActive(true);
        _actionLabel.text = "Joining Game";
    }
    void Update () {        
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        SceneManager.LoadScene("ServerGame");
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SceneManager.LoadScene("ClientGame");
    //    }
    }

    //public void PressPlay()
    //{    
    //}

    public void PressQuit()
    {

    }

    

    public void PressStart()
    {
        NetworkHost.ServerIP = _ipInput.text;
        NetworkHost.Port = int.Parse(_portInput.text);

        if (_isCreating)
        {
            SceneManager.LoadScene("ServerLobby");
        }
        else if (_isJoining)
        {
            SceneManager.LoadScene("ClientGame");
        }
    }
}
