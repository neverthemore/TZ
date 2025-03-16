// Items/InventoryItem.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class InventoryItem : MonoBehaviour
{
    [Header("Item Configuration")]
    [SerializeField] private ItemData _itemData;

    private Rigidbody _rb;
    private Collider _collider;
    private Transform _originalParent;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    public Vector3 OriginalLocalPosition => _originalPosition;
    public Quaternion OriginalLocalRotation => _originalRotation;
    public Transform OriginalParent => _originalParent;


    public ItemData Data => _itemData;

    private void Awake()
    {
        InitializeComponents();
        StoreOriginalTransform();
    }

    private void InitializeComponents()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rb.mass = _itemData.weight; // Устанавливаем массу из данных
    }

    private void StoreOriginalTransform()
    {
        _originalParent = transform.parent;
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    public void Pickup(Transform newParent)
    {
        _rb.isKinematic = true; // Отключаем физику при подборе
        _collider.enabled = false;
        transform.SetParent(newParent);
    }

    public void Drop()
    {
        _rb.isKinematic = false;
        _collider.enabled = true;
        transform.SetParent(_originalParent);
        ResetPosition();
    }

    private void ResetPosition()
    {
        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;
    }
}