using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MessageType
{    
    SetUp,
    StateUpdate,
    Move,
    None
}

[System.Serializable]
public struct SetUpMessage
{
    public List<CellID> activePlayers;
    public SetUpMessage(List<CellID> activePlayers)
    {
        this.activePlayers = activePlayers;
    }
}

[System.Serializable]
public class Message {
    public MessageType type;
    public string subJson;

    public Message(MessageType type = MessageType.None, object data = null)
    {       
        this.type = type;
        this.subJson = JsonUtility.ToJson(data);
    }

    public object GetData()
    {
        switch (this.type)
        {
            case MessageType.SetUp:
                return JsonUtility.FromJson<SetUpMessage>(subJson);
            case MessageType.Move:
                return JsonUtility.FromJson<Direction>(subJson);
            case MessageType.StateUpdate:
                return JsonUtility.FromJson<ChangeLog>(subJson);
            default:
                Debug.Log("Nothing written");
                return null;
        }
    }
}
