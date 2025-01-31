using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    private int rating;

    public void SetRating(int rating)
    {
        this.rating = rating; // Сохраняем рейтинг (не забудь создать переменную rating в классе!)
        Debug.Log($"Игрок {username} теперь имеет рейтинг {rating}");
    }




}
