using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenLetterButton : MonoBehaviour
{
    public GameObject BlueScreen;
    private Button button; // Ссылка на кнопку

    private void Start()
    {
        button = GetComponent<Button>(); // Получаем компонент кнопки
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
        // Показываем букву
        

        yield return new WaitForSeconds(delay);
        BlueScreen.SetActive(false);
        // Делаем кнопку неактивной после задержки
        button.interactable = false;
        button.gameObject.SetActive(false);
    }
}