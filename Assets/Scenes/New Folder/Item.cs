// Item.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    public ItemConfig config;
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Pickup()
    {
        rb.isKinematic = true; // ��������� ������ ��� �������
    }

    public void Drop()
    {
        rb.isKinematic = false; // �������� ������ ��� ������������
    }
}