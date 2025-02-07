using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI instance;

    public GameObject victoryPanel;
    public TextMeshProUGUI winnerText;

    private void Awake()
    {
        instance = this;
        victoryPanel.SetActive(false);
    }

    public void ShowWinner(string winnerName)
    {
        victoryPanel.SetActive(true);
        winnerText.text = $"{winnerName} WINNER!";
        StartCoroutine(DisconnectAfterDelay());
    }

    private IEnumerator DisconnectAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Ждём 5 сек.
        Client.instance.Disconnect(); // Отключаем клиента
    }
}