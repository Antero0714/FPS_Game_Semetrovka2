﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
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

        // Устанавливаем ник игрока
        PlayerNameUI.instance.ShowPlayerName(UIManager.instance.usernameField.text);

        // Теперь, когда у нас есть ID клиента, подключаем UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        // Обновляем список игроков
        PlayerListUI.instance.UpdatePlayerList();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);

        // Обновляем список игроков
        PlayerListUI.instance.UpdatePlayerList();
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
        PlayerListUI.instance.UpdatePlayerList();
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _health = _packet.ReadInt(); // Читаем значение здоровья

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].SetHealth(_health); // Обновляем здоровье игрока
        }
    }



    /*    public static void PlayerRespawned(Packet _packet)
        {
            int _id = _packet.ReadInt();

            GameManager.players[_id].Respawn();
        }*/

    public static void CreateItemSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();

        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].ItemSpawned();
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].ItemPickedUp();
    }

    public static void SpawnProjectile(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManager.instance.SpawnProjectile(_projectileId, _position);
    }

    public static void ProjectilePosition(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.projectiles[_projectileId].transform.position = _position;
    }

    public static void ProjectileExploded(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.projectiles[_projectileId].Explode(_position);
    }

    public static void PlayerWin(Packet _packet)
    {
        int _winnerId = _packet.ReadInt();
        string _winnerName = GameManager.players[_winnerId].username;
        VictoryUI.instance.ShowWinner(_winnerName);
    }
    public static void ServerFull(Packet _packet)
    {
        string _message = _packet.ReadString();
        Debug.Log($"Server is full: {_message}");
        UIManager.instance.ShowErrorMessage(_message); // Показываем сообщение об ошибке
        Client.instance.Disconnect(); // Отключаем клиента
    }
}
