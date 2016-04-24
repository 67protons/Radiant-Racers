using UnityEngine;
using System.Collections;

public enum MessageType
{
    SetUp,
    StateUpdate,
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
}
