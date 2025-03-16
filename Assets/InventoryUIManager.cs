// UI/InventoryUIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup _inventoryPanel;
    [SerializeField] private RectTransform[] _uiSlots;
    [SerializeField] private Image[] _slotIcons;

    [Header("Settings")]
    [SerializeField] private float _fadeSpeed = 5f;

    private bool _shouldShowUI;
    private Backpack _backpack;

    private void Awake()
    {
        _backpack = FindObjectOfType<Backpack>();
        _backpack.OnInventoryChanged.AddListener(UpdateUI);
        InitializeUI();
    }

    private void InitializeUI()
    {
        _inventoryPanel.alpha = 0f;
        foreach (Image icon in _slotIcons) icon.enabled = false;
    }

    private void Update()
    {
        HandleInventoryVisibility();
        UpdateUIVisibility();
    }

    private void HandleInventoryVisibility()
    {
        _shouldShowUI = Input.GetMouseButton(0) &&
                        IsPointerOverBackpack();
    }

    private bool IsPointerOverBackpack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) &&
               hit.collider.CompareTag("Backpack");
    }

    private void UpdateUIVisibility()
    {
        float targetAlpha = _shouldShowUI ? 1f : 0f;
        _inventoryPanel.alpha = Mathf.Lerp(
            _inventoryPanel.alpha,
            targetAlpha,
            Time.deltaTime * _fadeSpeed
        );
    }

    private void UpdateUI(ItemData item, string action)
    {
        int slotIndex = (int)item.itemType;

        if (action == "added")
        {
            _slotIcons[slotIndex].sprite = item.uiIcon;
            _slotIcons[slotIndex].enabled = true;
        }
        else
        {
            _slotIcons[slotIndex].enabled = false;
        }
    }
}