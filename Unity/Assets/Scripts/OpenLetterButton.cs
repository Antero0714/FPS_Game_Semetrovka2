using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenLetterButton : MonoBehaviour
{
    public GameObject BlueScreen;
    private Button button; // ������ �� ������

    private void Start()
    {
        button = GetComponent<Button>(); // �������� ��������� ������
    }

    public void ShowLetter()
    {
        if (BlueScreen != null && button != null)
        {
            StartCoroutine(ShowAndDisableButton(0.3f));
        }
    }

    private IEnumerator ShowAndDisableButton(float delay)
    {
        // ���������� �����
        BlueScreen.SetActive(false);

        yield return new WaitForSeconds(delay);

        // ������ ������ ���������� ����� ��������
        button.interactable = false;
        button.gameObject.SetActive(false);
    }
}