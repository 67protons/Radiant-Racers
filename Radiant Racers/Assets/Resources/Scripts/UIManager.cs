using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    ///Main Menu UI Variabes
    private GameObject _networkPanel;
    private Text _actionLabel;
    private InputField _ipInput, _portInput;
    private bool _isCreating = false;
    private bool _isJoining = false;

    ///Server Loby UI Variabes
    private GameObject _statusPanel;
    private Text _ipText, _portText, _playersText;
    private ServerManager _server;

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

        _statusPanel = GameObject.Find("StatusPanel");
        if (_statusPanel != null)
        {
            _ipText = _statusPanel.transform.FindChild("IPText").GetComponent<Text>();
            _ipText.text = "IP Adress: " + NetworkHost.ServerIP;
            _portText =_statusPanel.transform.FindChild("PortText").GetComponent<Text>();
            _portText.text = "Port Number: " + NetworkHost.Port.ToString();
            _playersText = _statusPanel.transform.FindChild("PlayersText").GetComponent<Text>();
            _server = GameObject.Find("Server").GetComponent<ServerManager>();
        }        
    }

    void LateUpdate()
    {
        if (_playersText != null && _server != null)
        {
            _playersText.text = "Players: " + _server.clientList.Count + "/8";
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

    public void PressPlay()
    {
        SceneManager.LoadScene("ServerGame");
    }

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
