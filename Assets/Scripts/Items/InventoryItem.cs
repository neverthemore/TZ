using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InventoryItem : MonoBehaviour
{
    public ItemData Data; // Ссылка на данные предмета

    private Rigidbody _rb;
    private Collider _collider;  
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public Vector3 OriginalLocalPosition => originalPosition;
    public Quaternion OriginalLocalRotation => originalRotation;
  
 
    public bool IsInBackpack { get; private set; }


    private void Awake()
    {
        
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rb.mass = Data.weight;
        IsInBackpack = false; // По умолчанию предмет не в рюкзаке
        StoreOriginalTransform();
    }
    

    private void StoreOriginalTransform()
    {

        originalPosition = transform.localPosition; // Начальная позиция
        originalRotation = transform.localRotation; // Начальная ориентация
    }
    public void Pickup()
    {
        IsInBackpack = false;
        _rb.isKinematic = true;
        _rb.useGravity = false; // Отключаем гравитацию при поднятии
    }

    public void Drop()
    {
        _rb.isKinematic = false; // Включаем физику
        _rb.useGravity = true; // Включаем гравитацию
        IsInBackpack = false;
  
    }
    public void ResetPosition() // Метод для возвращения на начальную позицию
    {
            StartCoroutine(SnapToPosition(originalPosition, originalRotation));   
            _rb.isKinematic = false;
            _rb.useGravity = false;
            IsInBackpack = false;
        

    }
    public void SetInBackpack()
    {
        IsInBackpack = true; // Корректировка состояния предмета
        _rb.isKinematic = false;
        
    }

  
    private IEnumerator SnapToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        float duration = 0.3f;
        float elapsed = 0.0f;
        Vector3 initialPosition = transform.localPosition;
        Quaternion initialRotation = transform.localRotation;

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, (elapsed / duration));
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, (elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        

        transform.localPosition = targetPosition;
        transform.localRotation = targetRotation;
    }
}