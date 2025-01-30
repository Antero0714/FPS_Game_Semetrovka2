using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        /*Client.instance.tcp.SendData(_packet);*/
    }
}
