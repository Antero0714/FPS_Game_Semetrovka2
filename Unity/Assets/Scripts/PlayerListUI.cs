using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerListUI : MonoBehaviour
{
    public static PlayerListUI instance;
    public TextMeshProUGUI playerListText; // Текст для списка игроков

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

        playerListText.gameObject.SetActive(false); // Скрываем список в меню
    }

    public void UpdatePlayerList()
    {
        if (GameManager.players == null || GameManager.players.Count == 0)
        {
            playerListText.gameObject.SetActive(false);
            return;
        }

        string playerList = "Игроки онлайн:\n";
        foreach (var player in GameManager.players.Values)
        {
            if (player.id != Client.instance.myId) // Не показываем себя в списке
            {
                playerList += $"{player.username} [{player.health} HP]\n"; // Добавляем HP
            }
        }

        playerListText.text = playerList;
        playerListText.gameObject.SetActive(true);
    }
}
