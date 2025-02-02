using UnityEngine;

public class OpenShow : MonoBehaviour
{
    public GameObject FieldMiracle;

    void Start()
    {
        if (FieldMiracle != null)
        {
            FieldMiracle.SetActive(false); // Скрываем объект при старте
        }
    }

    public void OpenFieldMiracle()
    {
        if (FieldMiracle != null)
        {
            bool isActive = FieldMiracle.activeSelf;
            FieldMiracle.SetActive(!isActive); // Переключаем видимость
        }
    }
}
