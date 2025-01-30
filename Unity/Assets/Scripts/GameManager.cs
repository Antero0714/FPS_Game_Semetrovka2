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
        GameObject _player;
        if(_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            GameObject[] prefabs = { playerPrefab, playerPrefab1, playerPrefab2 };
            _player = Instantiate(prefabs[_id % prefabs.Length], _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());

    }
}
