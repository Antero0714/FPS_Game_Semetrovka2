using UnityEngine;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    public static PlayerNameUI instance;

    public TextMeshProUGUI playerNameText; // UI-������� � ����� ������

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

        playerNameText.gameObject.SetActive(false); // �������� ��� � ����
    }

    public void ShowPlayerName(string playerName)
    {
        playerNameText.text = playerName;
        playerNameText.gameObject.SetActive(true); // ���������� ��� ����� �����������
    }
}
