using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenLetterButton : MonoBehaviour
{
    public GameObject BlueScreen;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void ShowLetter()
    {
        if (BlueScreen != null && button != null)
        {
            StartCoroutine(ShowAndDisableButton(0.3f));

            // ���������� �� ������ ���������� � ������� �����
            char letter = button.GetComponentInChildren<Text>().text[0]; // �������� �����
            SendLetterToServer(letter);
        }
    }

    private void SendLetterToServer(char letter)
    {
        using (Packet packet = new Packet((int)ClientPackets.letterPressed))
        {
            packet.Write(Client.instance.myId); // ID ������
            packet.Write(letter); // �����
            Client.instance.tcp.SendData(packet);
        }
    }

    private IEnumerator ShowAndDisableButton(float delay)
    {
        yield return new WaitForSeconds(delay);
        BlueScreen.SetActive(false);
        button.interactable = false;
        button.gameObject.SetActive(false);
    }
}