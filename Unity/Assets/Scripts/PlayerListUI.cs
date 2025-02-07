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
            playerListText.gameObject.SetActive(false); // Если никого нет, скрываем
            return;
        }

        string playerList = "Игроки онлайн:\n";
        foreach (var player in GameManager.players.Values)
        {
            if (player.id != Client.instance.myId) // Не показываем свой ник
            {
                playerList += $"{player.username}\n";
            }
        }

        if (playerList == "Игроки онлайн:\n")
        {
            playerListText.gameObject.SetActive(false); // Если никого кроме тебя, скрываем
        }
        else
        {
            playerListText.text = playerList;
            playerListText.gameObject.SetActive(true);
        }
    }
}
