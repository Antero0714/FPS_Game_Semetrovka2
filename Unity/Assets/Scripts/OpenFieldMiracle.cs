using UnityEngine;

public class OpenShow : MonoBehaviour
{
    public GameObject FieldMiracle;
    public GameObject Circle;
    public GameObject Arrow;
    public GameObject btnArrow;
    public GameObject Points;
    public GameObject Assistant;
    public GameObject Yakubovich;

    public GameObject[] InputLetters;
    public GameObject[] AnswerLetters;
    public GameObject[] BlueScreen;
    void Start()
    {
        if (FieldMiracle != null)
        {
            FieldMiracle.SetActive(false); // Скрываем объект при старте
        }
        if (Circle != null)
        {
            Circle.SetActive(false); // Скрываем объект при старте
        }
        if (Arrow != null)
        {
            Arrow.SetActive(false); // Скрываем объект при старте
        }
        if (btnArrow != null)
        {
            btnArrow.SetActive(false); // Скрываем объект при старте
        }
        if (Points != null)
        {
            Points.SetActive(false); // Скрываем объект при старте
        }
        if (Points != null)
        {
            Assistant.SetActive(false); // Скрываем объект при старте
        }
        if (Points != null)
        {
            Yakubovich.SetActive(false); // Скрываем объект при старте
        }

        for (int i = 0; i < InputLetters.Length; i++)
        {
            InputLetters[i].SetActive(false);
        }
        for (int i = 0; i < AnswerLetters.Length; i++)
        {
            AnswerLetters[i].SetActive(false);
        }
        for (int i = 0; i < BlueScreen.Length; i++)
        {
            BlueScreen[i].SetActive(false);
        }

    }

    public void OpenFieldMiracle()
    {
        if (FieldMiracle != null)
        {
            bool isActive = FieldMiracle.activeSelf;
            FieldMiracle.SetActive(!isActive); // Переключаем видимость
        }
        if (Circle != null)
        {
            bool isActive = Circle.activeSelf;
            Circle.SetActive(!isActive); // Переключаем видимость
        }
        if (Arrow != null)
        {
            bool isActive = Arrow.activeSelf;
            Arrow.SetActive(!isActive); // Переключаем видимость
        }
        if (btnArrow != null)
        {
            bool isActive = btnArrow.activeSelf;
            btnArrow.SetActive(!isActive); // Переключаем видимость
        }
        if (Points != null)
        {
            bool isActive = Points.activeSelf;
            Points.SetActive(!isActive); // Переключаем видимость
        }
        if (Assistant != null)
        {
            bool isActive = Assistant.activeSelf;
            Assistant.SetActive(!isActive); // Переключаем видимость
        }
        if (Yakubovich != null)
        {
            bool isActive = Yakubovich.activeSelf;
            Yakubovich.SetActive(!isActive); // Переключаем видимость
        }

        for (int i = 0; i < InputLetters.Length; i++)
        {
            bool isActive = InputLetters[i].activeSelf;
            InputLetters[i].SetActive(!isActive); // Переключаем видимость
        }

        for (int i = 0; i < AnswerLetters.Length; i++)
        {
            bool isActive = AnswerLetters[i].activeSelf;
            AnswerLetters[i].SetActive(!isActive); // Переключаем видимость
        }

        for (int i = 0; i < BlueScreen.Length; i++)
        {
            bool isActive = BlueScreen[i].activeSelf;
            BlueScreen[i].SetActive(!isActive); // Переключаем видимость
        }
    }

    public void ShowLetter(int index)
    {
        if (index >= 0 && index < InputLetters.Length && InputLetters[index] != null)
        {
            InputLetters[index].SetActive(false); // Скрываем квадрат
        }
    }
}
