using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;  // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    private GameObject inventory_Slots;

    private Slot[] slots;  // 슬롯들 배열

    void Start()
    {
        slots = inventory_Slots.GetComponentsInChildren<Slot>();
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
}