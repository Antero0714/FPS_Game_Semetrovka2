using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Соединение уже сущесвтеует, чужой объект уничтожен");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string  _username, Vector2 _position, Quaternion _rotation)
    {
        Debug.Log($"Spawning player with ID: {_id}, Username: {_username}");

        GameObject _player;
        if(_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else if (_id == 1)
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        else if (_id == 2)
        {
            _player = Instantiate(playerPrefab1, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab2, _position, _rotation);
        }
        

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }
}
