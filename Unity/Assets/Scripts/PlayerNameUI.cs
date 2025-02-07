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

        playerNameText.gameObject.SetActive(false); // �������� ��� � ����
    }

    public void ShowPlayerName(string playerName)
    {
        Debug.Log($"������������� ���: {playerName}");
        if (playerNameText == null)
        {
            Debug.LogError("playerNameText is null!");
            return;
        }
        playerNameText.text = playerName;
        playerNameText.gameObject.SetActive(true); // ���������� ��� ����� �����������
        Debug.Log("��� ���������� � ��������.");
    }
}
