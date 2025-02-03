using System;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();
        Debug.Log($"Получен welcome-пакет! Наш ID: {Client.instance.myId}");
        Debug.Log($"Принятый myId от сервера: {_myId}");


    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector2();
        Quaternion _rotation = _packet.ReadQuaternion();

        Debug.Log($"[Client] Получен игрок ID {_id}, Username: {_username}, Pos: {_position}");

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }






    public static void PlayerData(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _rating = _packet.ReadInt();

        Debug.Log($"[Client] Получены данные: ID {_id}, Рейтинг {_rating}");

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].SetRating(_rating);
        }
    }



    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        if (GameManager.players.ContainsKey(_id))
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                UnityEngine.Object.Destroy(GameManager.players[_id].gameObject);
                GameManager.players.Remove(_id);
            });
        }
        else
        {
            Debug.LogWarning($"Попытка удалить игрока {_id}, но его нет в GameManager.players");
        }
    }
    public static void LetterResult(Packet _packet)
    {
        try
        {
            int playerId = _packet.ReadInt();
            int pointsAwarded = _packet.ReadInt();
            Debug.Log($"[Client] Результат нажатия буквы для игрока {playerId}: {pointsAwarded} очков.");
            // Обновляем рейтинг в UI, если необходимо
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при разборе пакета letterResult: {ex}");
        }
    }

    public static void WinAnnouncement(Packet _packet)
    {
        try
        {
            int winnerId = _packet.ReadInt();
            string winnerName = _packet.ReadString();
            Debug.Log($"[Client] Победитель: {winnerName} (ID: {winnerId})");
            WinUIManager.ShowWinnerStatic(winnerName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при разборе пакета winAnnouncement: {ex}");
        }
    }


    public static void DrumSpinResult(Packet _packet)
    {
        Debug.Log("[CLIENT] Получен пакет drumSpinResult!");
        Debug.Log($"Длина пакета: {_packet.Length()}");

        int playerId = _packet.ReadInt();
        int sectorNumber = _packet.ReadInt();
        int points = _packet.ReadInt();

        Debug.Log($"[CLIENT] drumSpinResult: {playerId}, {sectorNumber}, {points}");
    }


    public static void PlayerPosition(Packet packet)
    {
        int _id = packet.ReadInt();
        Vector3 position = packet.ReadVector2();

        GameManager.players[_id].transform.position = position;
    }

    public static void PlayerRotation(Packet packet)
    {
        int _id = packet.ReadInt();
        Quaternion rotation = packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = rotation;
    }
}
