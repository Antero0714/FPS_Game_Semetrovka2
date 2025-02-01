
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        Debug.Log($"[GameManager] Попытка создать игрока с ID: {_id}, Username: {_username}");

        if (players.ContainsKey(_id))
        {
            Debug.LogWarning($"[Ошибка] Игрок с ID {_id} уже существует! Отмена создания.");
            return;
        }

        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else if (_id == 1)
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        else if (_id == 2)
        {
            _player = Instantiate(playerPrefab2, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab3, _position, _rotation);
        }

        PlayerManager pm = _player.GetComponent<PlayerManager>();
        pm.id = _id;
        pm.username = _username;
        players.Add(_id, pm);

        Debug.Log($"[GameManager] Игрок {_username} (ID: {_id}) успешно создан.");
    }


}
