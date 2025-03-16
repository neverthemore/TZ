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
        rb.isKinematic = true; // Отключаем физику при подборе
    }

    public void Drop()
    {
        rb.isKinematic = false; // Включаем физику при выбрасывании
    }
}