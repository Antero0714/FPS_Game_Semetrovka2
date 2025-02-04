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
    public static void SendDrumSpinResult(int playerId, int sectorNumber, int points)
    {
        using (Packet packet = new Packet((int)ClientPackets.drumSpinResult))
        {
            packet.Write(playerId);
            packet.Write(sectorNumber);
            packet.Write(points);
            SendTCPData(packet);
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
