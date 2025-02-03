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
    public static void SendDrumSpinResult(int pointsAwarded)
    {
        using (Packet _packet = new Packet((int)ClientPackets.drumSpinResult))
        {
            // ���������� ID ������ � ����������� ����
            _packet.Write(Client.instance.myId);
            _packet.Write(pointsAwarded);
            Client.instance.tcp.SendData(_packet);
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
            // ���������� ID �������, ����� ������ �������� � ������������
            _packet.Write(Client.instance.myId);
            // ����� �������� ���, ���� ���������:
            // _packet.Write(Client.instance.username);
            Client.instance.tcp.SendData(_packet);
        }
    }


}
