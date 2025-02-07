using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    public static PlayerListUI instance;

    public TextMeshProUGUI playerListText; // UI-текст для списка игроков

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }

        playerListText.gameObject.SetActive(false); // Скрываем список в меню
    }

    public void UpdatePlayerList()
    {
        if (GameManager.players == null || GameManager.players.Count == 0) return;

        string playerList = "Игроки онлайн:\n";
        foreach (var player in GameManager.players.Values)
        {
            if (player.id != Client.instance.myId) // Не показываем себя в списке
            {
                playerList += $"{player.username}\n";
            }
        }

        playerListText.text = playerList;
        playerListText.gameObject.SetActive(true); // Показываем список игроков
    }
}
