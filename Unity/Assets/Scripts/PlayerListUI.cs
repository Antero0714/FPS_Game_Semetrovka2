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

        playerListText.gameObject.SetActive(false); // �������� ������ ������� � ����
    }

    public void UpdatePlayerList()
    {
        if (GameManager.players == null || GameManager.players.Count == 0)
        {
            playerListText.gameObject.SetActive(false); // ���� ������ ���, ��������
            return;
        }

        string playerList = "������ ������:\n";
        foreach (var player in GameManager.players.Values)
        {
            if (player.id != Client.instance.myId) // �� ���������� ���� ���
            {
                playerList += $"{player.username}\n";
            }
        }

        if (playerList == "������ ������:\n")
        {
            playerListText.gameObject.SetActive(false); // ���� ������ ����� ����, ��������
        }
        else
        {
            playerListText.text = playerList;
            playerListText.gameObject.SetActive(true);
        }
    }
}
