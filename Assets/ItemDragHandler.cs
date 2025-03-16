// Inventory/ItemDragHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryItem))]
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera _mainCamera;
    private InventoryItem _item;
    private Vector3 _offset;
    private float _zDistance;

    private void Awake()
    {
        _item = GetComponent<InventoryItem>();
        _mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CalculateOffset();
        _item.Pickup(_mainCamera.transform); // Берем предмет
    }

    private void CalculateOffset()
    {
        _zDistance = Vector3.Distance(_mainCamera.transform.position, transform.position);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistance);
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
        _offset = transform.position - worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = GetMouseWorldPosition() + _offset;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 15f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleDrop();
    }

    private void HandleDrop()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out Backpack backpack))
            {
                backpack.TryStoreItem(_item);
                return;
            }
        }
        _item.Drop(); // Если не попали в рюкзак - бросаем
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _zDistance;
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
}