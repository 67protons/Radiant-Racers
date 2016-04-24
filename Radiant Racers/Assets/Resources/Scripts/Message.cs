using UnityEngine;
using System.Collections;

public enum MessageType
{    
    SetUp,
    StateUpdate,    
    Move,
    None
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
                return JsonUtility.FromJson<CellID>(subJson);
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
