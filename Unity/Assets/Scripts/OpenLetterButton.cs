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

            // Отправляем на сервер информацию о нажатой букве
            char letter = button.GetComponentInChildren<Text>().text[0]; // Получаем букву
            SendLetterToServer(letter);
        }
    }

    private void SendLetterToServer(char letter)
    {
        using (Packet packet = new Packet((int)ClientPackets.letterPressed))
        {
            packet.Write(Client.instance.myId); // ID игрока
            packet.Write(letter); // Буква
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