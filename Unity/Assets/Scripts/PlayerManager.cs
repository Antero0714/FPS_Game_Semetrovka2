using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public int health = 100; // Начальное здоровье
    public MeshRenderer model;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = 100; // Устанавливаем начальное здоровье
    }

    public void SetColor(Material material)
    {
        model.material = material;
    }

    public void SetHealth(int _health)
    {
        health = _health;
        Debug.Log($"{username} здоровье обновлено: {health}");
        PlayerListUI.instance.UpdatePlayerList(); // Обновляем список HP

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false; // Скрываем игрока
    }
}
