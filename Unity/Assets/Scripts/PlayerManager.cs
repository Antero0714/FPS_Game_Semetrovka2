using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    private int rating;

    public void SetRating(int rating)
    {
        this.rating = rating; // ��������� ������� (�� ������ ������� ���������� rating � ������!)
        Debug.Log($"����� {username} ������ ����� ������� {rating}");
    }




}
