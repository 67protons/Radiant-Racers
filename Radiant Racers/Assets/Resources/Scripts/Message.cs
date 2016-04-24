using UnityEngine;
using System.Collections;

public enum MessageType
{
    SetUp,
    StateUpdate
}

[System.Serializable]
public class Message {
    public MessageType type;    
    public string subJson;

    public Message(MessageType type, string subJson)
    {       
        this.type = type;
        this.subJson = subJson;
    }
}
