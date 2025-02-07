using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerListUI : MonoBehaviour
{
    public static PlayerListUI instance;
    public TextMeshProUGUI playerListText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerListText.gameObject.SetActive(false); // Скрываем список игроков в меню
    }

    public void UpdatePlayerList()
    {
        if (GameManager.players == null || GameManager.players.Count == 0)
        {
            Debug.Log("Нет игроков для отображения.");
            playerListText.gameObject.SetActive(false); // Если никого нет, скрываем
            return;
        }

        string playerList = "Игроки онлайн:\n";
        foreach (var player in GameManager.players.Values)
        {
            playerList += $"{player.username}\n";
        }

        Debug.Log($"Список игроков обновлён:\n{playerList}");
        playerListText.text = playerList;
        playerListText.gameObject.SetActive(true);
        Debug.Log("Список игроков отображён.");
    }
}
