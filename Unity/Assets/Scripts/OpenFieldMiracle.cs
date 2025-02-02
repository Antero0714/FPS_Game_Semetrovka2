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
            FieldMiracle.SetActive(false); // �������� ������ ��� ������
        }
        if (Circle != null)
        {
            Circle.SetActive(false); // �������� ������ ��� ������
        }
        if (Arrow != null)
        {
            Arrow.SetActive(false); // �������� ������ ��� ������
        }
        if (btnArrow != null)
        {
            btnArrow.SetActive(false); // �������� ������ ��� ������
        }
        if (Points != null)
        {
            Points.SetActive(false); // �������� ������ ��� ������
        }
    }

    

    public void OpenFieldMiracle()
    {
        if (FieldMiracle != null)
        {
            bool isActive = FieldMiracle.activeSelf;
            FieldMiracle.SetActive(!isActive); // ����������� ���������
        }
        if (Circle != null)
        {
            bool isActive = Circle.activeSelf;
            Circle.SetActive(!isActive); // ����������� ���������
        }
        if (Arrow != null)
        {
            bool isActive = Arrow.activeSelf;
            Arrow.SetActive(!isActive); // ����������� ���������
        }
        if (btnArrow != null)
        {
            bool isActive = btnArrow.activeSelf;
            btnArrow.SetActive(!isActive); // ����������� ���������
        }
        if (Points != null)
        {
            bool isActive = Points.activeSelf;
            Points.SetActive(!isActive); // ����������� ���������
        }
    }
}
