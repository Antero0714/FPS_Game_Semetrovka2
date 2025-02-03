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
    public static void SendDrumSpinResult(int pointsAwarded)
    {
        using (Packet _packet = new Packet((int)ClientPackets.drumSpinResult))
        {
            // Отправляем ID игрока и начисленные очки
            _packet.Write(Client.instance.myId);
            _packet.Write(pointsAwarded);
            Client.instance.tcp.SendData(_packet);
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
            // Отправляем ID клиента, чтобы сервер убедился в корректности
            _packet.Write(Client.instance.myId);
            // Можно добавить имя, если требуется:
            // _packet.Write(Client.instance.username);
            Client.instance.tcp.SendData(_packet);
        }
    }


}
