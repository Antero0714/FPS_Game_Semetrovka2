using GameServer;
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
        Debug.Log($"������� welcome-�����! ��� ID: {Client.instance.myId}");
        Debug.Log($"�������� myId �� �������: {_myId}");


    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector2();
        Quaternion _rotation = _packet.ReadQuaternion();

        Debug.Log($"[Client] ������� ����� ID {_id}, Username: {_username}, Pos: {_position}");

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }






    public static void PlayerData(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _rating = _packet.ReadInt();

        Debug.Log($"[Client] �������� ������: ID {_id}, ������� {_rating}");

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
            Debug.LogWarning($"������� ������� ������ {_id}, �� ��� ��� � GameManager.players");
        }
    }
    public static void LetterResult(Packet _packet)
    {
        int playerId = _packet.ReadInt();
        int pointsAwarded = _packet.ReadInt();

        Debug.Log($"[Client] ����� {playerId} ������� {pointsAwarded} ����� �� �����");

        if (GameManager.players.TryGetValue(playerId, out PlayerManager player))
        {
            player.SetRating(player.GetRating() + pointsAwarded);
        }
    }

    public static void WinAnnouncement(Packet _packet)
    {
        int winnerId = _packet.ReadInt();
        string winnerName = _packet.ReadString();

        Debug.Log($"[Client] ����������: {winnerName} (ID: {winnerId})");
        WinUIManager.ShowWinnerStatic(winnerName);
    }
    public static void DrumSpinResult(Packet _packet)
    {
        int playerId = _packet.ReadInt();
        int sectorNumber = _packet.ReadInt();
        int points = _packet.ReadInt();

        Debug.Log($"[Client] drumSpinResult: playerId={playerId}, sector={sectorNumber}, points={points}");

        foreach (var player in GameManager.players.Values)
        {
            Speen speen = player.GetComponent<Speen>();
            if (speen != null)
            {
                speen.SpinToSector(sectorNumber);
            }
        }
    }

    public static void DrumSpinRequest(Packet _packet)
    {
        int playerId = _packet.ReadInt(); // ������ ID ������
        Debug.Log($"[Client] ������� ������ �� ���� �� ������ {playerId}");

        // ����� ����� ��������� �������� �� �������
        if (GameManager.players.TryGetValue(playerId, out PlayerManager player))
        {
            player.GetComponent<Speen>().StartSpin();
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
            packet.Write(Client.instance.myId); // ID ������
            Client.instance.tcp.SendData(packet);
        }
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
