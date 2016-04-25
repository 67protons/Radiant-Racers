using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

public class NetworkHost : MonoBehaviour {
    public static string ServerIP = Network.player.ipAddress;
    //public static string ServerIP = "104.33.20.133";
    public static int Port = 9001;

    private ConnectionConfig _config;
    private int _myReliableChannelID, _myUnreliableChannelID;
    private HostTopology _topology;
    private int _hostID;

    public struct ReceiveEvent{
        public NetworkEventType type;
        public int sender;
        public Message message;

        public ReceiveEvent(NetworkEventType type, int connectionID, byte[] message)
        {
            this.type = type;
            this.sender = connectionID;
            if (type != NetworkEventType.DataEvent)
                this.message = new Message();
            else
                this.message = JsonUtility.FromJson<Message>(System.Text.Encoding.UTF8.GetString(message));
        }
    }

    public void Setup(int port, int maxConnections)
    {
        NetworkTransport.Init();
        _config = new ConnectionConfig();
        _myReliableChannelID = _config.AddChannel(QosType.Reliable);
        //_myUnreliableChannelID = _config.AddChannel(QosType.Unreliable);
        _topology = new HostTopology(_config, maxConnections);
        _hostID = NetworkTransport.AddHost(_topology, port);
    }

    public void Setup(int port, int maxConnections, string ipAdress)
    {
        NetworkTransport.Init();
        _config = new ConnectionConfig();
        _myReliableChannelID = _config.AddChannel(QosType.Reliable);
        //_myUnreliableChannelID = _config.AddChannel(QosType.Unreliable);
        _topology = new HostTopology(_config, maxConnections);
        _hostID = NetworkTransport.AddHost(_topology, port, ipAdress);
    }

    public int Connect(string ipAddress, int port)
    {
        byte error;
        int connectionID = NetworkTransport.Connect(_hostID, "192.168.0.4", 9001, 0, out error);
        if (error == 0)
        {
            return connectionID;
        }
        else
        {
            return -1;
        }
    }

    public ReceiveEvent Receive()
    {
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;        
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(_hostID, out connectionID, out channelID, recBuffer, bufferSize, out dataSize, out error);
        return new ReceiveEvent(recData, connectionID, recBuffer);
    }

    public void Send(int connectionID, MessageType messageType, object data)
    {
        string message = JsonUtility.ToJson(new Message(messageType, data));
        byte error;
        NetworkTransport.Send(_hostID, connectionID, _myReliableChannelID, System.Text.Encoding.UTF8.GetBytes(message), 1024, out error);
        //return error;
    }
}
