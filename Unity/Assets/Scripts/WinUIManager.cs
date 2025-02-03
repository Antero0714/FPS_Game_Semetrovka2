using UnityEngine;
using TMPro;

public class WinUIManager : MonoBehaviour
{
    public static WinUIManager instance;

    [SerializeField] private GameObject winPanel;         // Панель победы (выключена по умолчанию)
    [SerializeField] private TextMeshProUGUI winnerText;    // Текст для отображения имени победителя

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Optionally: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowWinner(string winnerName)
    {
        if (winPanel != null && winnerText != null)
        {
            winnerText.text = $"Победитель: {winnerName}";
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("WinUIManager: winPanel или winnerText не назначены в инспекторе!");
        }
    }

    public static void ShowWinnerStatic(string winnerName)
    {
        if (instance != null)
        {
            instance.ShowWinner(winnerName);
        }
        else
        {
            Debug.LogError("WinUIManager: instance не установлена!");
        }
    }
}
