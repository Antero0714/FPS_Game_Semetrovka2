using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>
    /// ���������� ��������� ����� �������� �� ������.
    /// </summary>
    /// <param name="pointsAwarded">����������� ���� (��������, 500).</param>
    public static void SendDrumSpinResult(int sectorNumber, int points)
    {
        using (Packet packet = new Packet((int)ClientPackets.drumSpinResult))
        {
            packet.Write(Client.instance.myId); // ���������� ���� ID
            packet.Write(sectorNumber);
            packet.Write(points);
            Client.instance.tcp.SendData(packet);
        }

        Debug.Log($"[Client] ��������� drumSpinResult: ID={Client.instance.myId}, sector={sectorNumber}, points={points}");
    }


    public void SendDrumSpinRequest()
    {
        if (Client.instance == null)
        {
            Debug.LogError("Client.instance is null!");
            return;
        }

        if (Client.instance.tcp == null)
        {
            Debug.LogError("Client.instance.tcp is null!");
            return;
        }

        using (Packet packet = new Packet((int)ClientPackets.drumSpinRequest))
        {
            packet.Write(Client.instance.myId); // ID ������
            Client.instance.tcp.SendData(packet);
        }
    }

    /// <summary>
    /// ���������� ��������� ����� �� ������.
    /// </summary>
    /// <param name="letter">��������� �����, ��������, "�".</param>
    public static void SendLetterPressed(string letter)
    {
        using (Packet _packet = new Packet((int)ClientPackets.letterPressed))
        {
            // ���������� ����� (���� �����, ����� �������� ID, �� _fromClient � ����������� ������� ������� �� IAsyncResult)
            _packet.Write(letter);
            Client.instance.tcp.SendData(_packet);
        }
    }

    /// <summary>
    /// �������� ������ welcomeReceived ����� ��������� Welcome.
    /// </summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("PlayerName"); // ������ ����� ������
            Client.instance.tcp.SendData(_packet);
        }
    }


}
