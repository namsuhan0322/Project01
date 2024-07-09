using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
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

    [SerializeField]
    private Text currentBalanceText;

    private List<GameObject> droppedItems = new List<GameObject>();

    void Start()
    {
        slots = inventory_Slots.GetComponentsInChildren<Slot>();
        dropZone = FindObjectOfType<DropZone>();
        UpdateBalanceText();
    }

    void Update()
    {
        HandleSlotSelection();
        HandleItemDrop();
    }

    public bool AddItemToInventory(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item == null)
            {
                slot.AddItem(item);
                return true;
            }
        }
        Debug.Log("아이템이 가득 찼습니다!");
        return false;
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

        if (dropZone != null && dropZone.IsPlayerInZone())
        {
            Transform dropPos = shopDropPositions[currentShopDropIndex];
            GameObject droppedItem = Instantiate(itemToDrop.itemPrefab, dropPos.position, Quaternion.identity);
            droppedItem.layer = LayerMask.NameToLayer("DroppedInShop");
            currentShopDropIndex = (currentShopDropIndex + 1) % shopDropPositions.Length;

            totalPrice += itemToDrop.Value; // 판매 가격 추가

            droppedItems.Add(droppedItem);
        }
        else if (dropPosition != null)
        {
            Instantiate(itemToDrop.itemPrefab, dropPosition.position, Quaternion.identity);
        }

        totalDroppedItems++;
    }

    public IEnumerator ProcessSellItems()
    {
        float deleteDelay = Random.Range(3f, 5f);
        yield return new WaitForSeconds(deleteDelay);

        foreach (GameObject droppedItem in droppedItems)
        {
            Destroy(droppedItem);
        }
        droppedItems.Clear();

        float settleDelay = Random.Range(5f, 10f);
        yield return new WaitForSeconds(settleDelay);

        UpdateBalanceText();
        Debug.Log("정산 완료!");
    }

    private void UpdateBalanceText()
    {
        currentBalanceText.text = $"현재 잔고: {totalPrice}$";
    }
}
