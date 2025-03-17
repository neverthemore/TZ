using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryItem))]
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera _mainCamera;
    private InventoryItem _inventoryItem;
    private Vector3 _offset;
    private float _zDistance;

    private void Awake()
    {
        _inventoryItem = GetComponent<InventoryItem>();
        _mainCamera = Camera.main;
    }
    public void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _inventoryItem.Pickup(); // Поднимаем предмет
        CalculateOffset();
    }

    private void CalculateOffset()
    {
        _zDistance = Vector3.Distance(_mainCamera.transform.position, transform.position);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistance);
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        _offset = transform.position - worldPosition; // Вычисляем смещение
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = GetMouseWorldPosition() + _offset;
        transform.position = newPosition; // Двигаем предмет вместе с мышью
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleDrop();
    }

    private void HandleDrop()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<Backpack>(out Backpack backpack))
            {
                Debug.Log("Trying to store item in backpack.");
                backpack.TryStoreItem(_inventoryItem);
            }
            else
            {
                Debug.Log("Dropped outside the backpack. Item will fall.");
                _inventoryItem.Drop(); // Если отпустили предмет вне рюкзака
            }
        }
        else
        {
            Debug.Log("Dropped outside the backpack. Item will fall.");
            _inventoryItem.Drop(); // Если отпустили предмет вне рюкзака
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _zDistance;
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
}