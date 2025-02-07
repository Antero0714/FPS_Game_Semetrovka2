using UnityEngine;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    public static PlayerNameUI instance;
    public TextMeshProUGUI playerNameText;

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

        playerNameText.gameObject.SetActive(false); // Скрываем ник в меню
    }

    public void ShowPlayerName(string playerName)
    {
        Debug.Log($"Устанавливаем ник: {playerName}");
        if (playerNameText == null)
        {
            Debug.LogError("playerNameText is null!");
            return;
        }
        playerNameText.text = playerName;
        playerNameText.gameObject.SetActive(true); // Показываем ник после подключения
        Debug.Log("Ник установлен и отображён.");
    }
}
