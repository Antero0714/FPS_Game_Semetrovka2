using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    public static PlayerListUI instance;

    public TextMeshProUGUI playerListText; // UI-����� ��� ������ �������

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

        playerListText.gameObject.SetActive(false); // �������� ������ � ����
    }

    public void UpdatePlayerList()
    {
        if (GameManager.players == null || GameManager.players.Count == 0) return;

        string playerList = "������ ������:\n";
        foreach (var player in GameManager.players.Values)
        {
            if (player.id != Client.instance.myId) // �� ���������� ���� � ������
            {
                playerList += $"{player.username}\n";
            }
        }

        playerListText.text = playerList;
        playerListText.gameObject.SetActive(true); // ���������� ������ �������
    }
}
