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
        try
        {
            int playerId = _packet.ReadInt();
            int pointsAwarded = _packet.ReadInt();
            Debug.Log($"[Client] ��������� ������� ����� ��� ������ {playerId}: {pointsAwarded} �����.");
            // ��������� ������� � UI, ���� ����������
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� ������� ������ letterResult: {ex}");
        }
    }

    public static void WinAnnouncement(Packet _packet)
    {
        try
        {
            int winnerId = _packet.ReadInt();
            string winnerName = _packet.ReadString();
            Debug.Log($"[Client] ����������: {winnerName} (ID: {winnerId})");
            WinUIManager.ShowWinnerStatic(winnerName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ��� ������� ������ winAnnouncement: {ex}");
        }
    }


    public static void DrumSpinResult(Packet _packet)
    {
        Debug.Log("[CLIENT] ������� ����� drumSpinResult!");
        Debug.Log($"����� ������: {_packet.Length()}");

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
