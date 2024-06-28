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
    private Transform dropPosition;  // 드롭 위치를 위한 트랜스폼 변수

    void Start()
    {
        slots = inventory_Slots.GetComponentsInChildren<Slot>();
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
        
        // 플레이어의 자식 오브젝트 위치에 드롭
        if (dropPosition != null)
        {
            Instantiate(itemToDrop.itemPrefab, dropPosition.position, Quaternion.identity);
        }
    }
}
