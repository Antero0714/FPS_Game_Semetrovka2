using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speen : MonoBehaviour
{
    private bool isCoroutine = true;
    private int totalAngle;
    [SerializeField] private string[] prizeText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private int section;
    [SerializeField] private int angleSpeed;
    [SerializeField] private int minTime;
    [SerializeField] private int maxTime;
    [SerializeField] private Button spinButton;

    private void Start()
    {
        if (prizeText == null || winText == null || section == 0 || angleSpeed == 0 || spinButton == null)
        {
            Debug.LogError("�� ��� ���� ��������� � ����������!");
            return;
        }

        totalAngle = 360 / section;

        if (prizeText.Length < section)
        {
            Debug.LogError("����� ������� prizeText ������, ��� ���������� ������!");
            return;
        }

        spinButton.onClick.AddListener(StartSpin);
    }

    public void StartSpin()
    {
        if (isCoroutine)
        {
            StartCoroutine(Spin());
        }
    }

    private IEnumerator Spin()
    {
        isCoroutine = false;
        int randTime = Random.Range(minTime, maxTime);
        float currentSpeed = angleSpeed;
        float totalRotation = 0;

        while (currentSpeed > 0.5f)
        {
            transform.Rotate(0, 0, currentSpeed);
            totalRotation += currentSpeed;
            currentSpeed *= 0.98f;
            yield return new WaitForSeconds(0.01f);
        }

        float randomOffset = Random.Range(-totalAngle / 2, totalAngle / 2);
        transform.Rotate(0, 0, randomOffset);

        float finalAngle = transform.eulerAngles.z;
        finalAngle = (finalAngle + 360) % 360;

        int closestSectionIndex = Mathf.RoundToInt(finalAngle / totalAngle) % section;
        if (closestSectionIndex < 0) closestSectionIndex += section;

        float targetAngle = closestSectionIndex * totalAngle;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        winText.text = prizeText[closestSectionIndex];

        Debug.Log($"��������� ����: {finalAngle}, ������: {closestSectionIndex}, ����: {prizeText[closestSectionIndex]}");

        // ���������� ��������� �� ������
        int points = int.Parse(prizeText[closestSectionIndex]); // �����������, ��� prizeText �������� ����
        SendDrumResultToServer(closestSectionIndex, points);

        isCoroutine = true;
    }

    public void SendDrumResultToServer(int sectorNumber, int points)
    {
        if (Client.instance == null)
        {
            Debug.LogError("Client.instance is null!");
            return;
        }

        if (Client.instance.tcp == null)
        {
            Debug.LogError("Client.instance.tcp is null!");
            return;
        }

        using (Packet packet = new Packet((int)ClientPackets.drumSpinResult)) // ������ ������ �� ������ ����������
        {
            packet.Write(Client.instance.myId); // ID ������
            packet.Write(sectorNumber); // ����� �������
            packet.Write(points); // ����
            Client.instance.tcp.SendData(packet);
        }
    }

}