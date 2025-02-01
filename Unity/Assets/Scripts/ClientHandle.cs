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

}
