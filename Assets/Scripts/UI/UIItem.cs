using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI countText;
    public int itemNumber; // 0 for Slot 1, 1 for Slot 2
    
    private GameController gc;

    private void OnEnable()
    {
        GameController.OnInventoryUpdated += UpdateSlotUI;
    }

    private void OnDisable()
    {
        GameController.OnInventoryUpdated -= UpdateSlotUI;
    }

    private void Awake()
    {
        // Fetch reference in Awake to guarantee 'gc' exists before OnEnable/Start events
        gc = FindFirstObjectByType<GameController>();
    }

    private void Start()
    {
        UpdateSlotUI(); // Initial check
    }

    public void UpdateSlotUI()
    {
        // Fallback if gc wasn't caught yet
        if (gc == null)
        {
            gc = FindFirstObjectByType<GameController>();
            if (gc == null) return;
        }

        // Check if this slot index exists in inventory list
        if (itemNumber < gc.items.Count)
        {
            ItemEntry currentItem = gc.items[itemNumber];

            // CRITICAL: Ensure item exists, count > 0 AND icon sprite is NOT null!
            if (currentItem != null && currentItem.amount > 0 && currentItem.icon != null)
            {
                itemIcon.sprite = currentItem.icon;
                itemIcon.enabled = true; // Only enable if we actually have a valid Sprite!
                
                if (countText != null)
                {
                    countText.text = currentItem.amount.ToString();
                }
                return;
            }
        }

        // If no item, clear sprite AND disable Image to prevent solid white box
        ClearSlot();
    }

    private void ClearSlot()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false; // Hides the component completely
        }

        if (countText != null)
        {
            countText.text = "";
        }
    }
}