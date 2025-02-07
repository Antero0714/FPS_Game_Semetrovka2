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
            Debug.Log("��� ������� ��� �����������.");
            playerListText.gameObject.SetActive(false); // ���� ������ ���, ��������
            return;
        }

        string playerList = "������ ������:\n";
        foreach (var player in GameManager.players.Values)
        {
            playerList += $"{player.username}\n";
        }

        Debug.Log($"������ ������� �������:\n{playerList}");
        playerListText.text = playerList;
        playerListText.gameObject.SetActive(true);
        Debug.Log("������ ������� ��������.");
    }
}
