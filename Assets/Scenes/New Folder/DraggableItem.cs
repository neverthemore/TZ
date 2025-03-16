// DraggableItem.cs
using UnityEngine.EventSystems;
using UnityEngine;

public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Camera mainCamera;
    private Vector3 offset;

    private void Awake() => mainCamera = Camera.main;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)
        );
        transform.position = mousePos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Проверка коллизии с рюкзаком
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("Backpack"))
            {
                Backpack backpack = hit.collider.GetComponent<Backpack>();
                backpack.AddItem(GetComponent<Item>());
            }
        }
    }
}