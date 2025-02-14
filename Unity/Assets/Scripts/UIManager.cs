﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }
    public void ShowErrorMessage(string _message)
    {
        startMenu.SetActive(true); // Показываем стартовое меню
        usernameField.interactable = true; // Разрешаем ввод ника
        Debug.Log($"Error: {_message}");
        // Здесь можно добавить отображение сообщения об ошибке на экране
        // Например, через TextMeshPro или другой UI-элемент
    }
}
