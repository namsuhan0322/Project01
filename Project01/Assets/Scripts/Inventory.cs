using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;
    [SerializeField]
    private GameObject inventory_Slots;
    private Slot[] slots;
    private int selectedIndex = -1;

    [SerializeField]
    private Transform dropPosition;
    [SerializeField]
    private Transform[] shopDropPositions;

    private DropZone dropZone;
    private int currentShopDropIndex = 0;
    private int totalDroppedItems = 0;
    private int totalPrice = 0;
    
    private HashSet<Item> droppedItems = new HashSet<Item>();

    void Start()
    {
        slots = inventory_Slots.GetComponentsInChildren<Slot>();
        dropZone = FindObjectOfType<DropZone>();
    }

    void Update()
    {
        HandleSlotSelection();
        HandleItemDrop();
    }

    public void AddItemToInventory(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item == null)
            {
                slot.AddItem(item);
                break;
            }
        }
    }

    private void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HighlightSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HighlightSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HighlightSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HighlightSlot(3);
        }
    }

    private void HighlightSlot(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetHighlight(i == index);
        }
    }

    private void HandleItemDrop()
    {
        if (Input.GetKeyDown(KeyCode.F) && selectedIndex != -1)
        {
            Slot selectedSlot = slots[selectedIndex];
            if (selectedSlot.item != null)
            {
                DropItem(selectedSlot);
            }
        }
    }

    private void DropItem(Slot slot)
    {
        Item itemToDrop = slot.item;
        slot.ClearSlot();

        bool isNewDrop = !droppedItems.Contains(itemToDrop);

        if (dropZone != null && dropZone.IsPlayerInZone())
        {
            Transform dropPos = shopDropPositions[currentShopDropIndex];
            Instantiate(itemToDrop.itemPrefab, dropPos.position, Quaternion.identity);
            currentShopDropIndex = (currentShopDropIndex + 1) % shopDropPositions.Length;

            if (isNewDrop)
            {
                totalPrice += itemToDrop.Value;
                droppedItems.Add(itemToDrop);
            }
        }
        else if (dropPosition != null)
        {
            Instantiate(itemToDrop.itemPrefab, dropPosition.position, Quaternion.identity);
        }

        if (isNewDrop)
        {
            totalDroppedItems++;
        }

        Debug.Log($"총 갯수: {totalDroppedItems}");
        Debug.Log($"총 가격: {totalPrice}");
    }
}