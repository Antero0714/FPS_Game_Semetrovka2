using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>
    /// Отправляет результат спина барабана на сервер.
    /// </summary>
    /// <param name="pointsAwarded">Начисленные очки (например, 500).</param>
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
            packet.Write(Client.instance.myId); // ID игрока
            Client.instance.tcp.SendData(packet);
        }
    }

    /// <summary>
    /// Отправляет выбранную букву на сервер.
    /// </summary>
    /// <param name="letter">Выбранная буква, например, "С".</param>
    public static void SendLetterPressed(string letter)
    {
        using (Packet _packet = new Packet((int)ClientPackets.letterPressed))
        {
            // Отправляем букву (если нужно, можно добавить ID, но _fromClient в обработчике сервера берется из IAsyncResult)
            _packet.Write(letter);
            Client.instance.tcp.SendData(_packet);
        }
    }

    /// <summary>
    /// Отправка пакета welcomeReceived после получения Welcome.
    /// </summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("PlayerName"); // Пример имени игрока
            Client.instance.tcp.SendData(_packet);
        }
    }


}
