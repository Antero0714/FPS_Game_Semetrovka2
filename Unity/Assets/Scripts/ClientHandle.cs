using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Сообщение от сервера: {_msg}");
        Client.instance.myId = _myId;
        //TODO: send welcome received packet
    }
}
