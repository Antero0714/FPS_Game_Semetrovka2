using UnityEngine;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    public static PlayerNameUI instance;

    public TextMeshProUGUI playerNameText; // UI-элемент с ником игрока

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

        playerNameText.gameObject.SetActive(false); // Скрываем ник в меню
    }

    public void ShowPlayerName(string playerName)
    {
        playerNameText.text = playerName;
        playerNameText.gameObject.SetActive(true); // Показываем ник после подключения
    }
}
