using UnityEngine;

public class OpenShow : MonoBehaviour
{
    public GameObject FieldMiracle;
    public GameObject Circle;
    public GameObject Arrow;
    public GameObject btnArrow;
    public GameObject Points;

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
    }
}
