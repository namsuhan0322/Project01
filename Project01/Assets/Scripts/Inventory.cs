using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;  // 인벤토리가 활성화되어 있는지 여부를 나타내는 정적 변수

    // 인벤토리 슬롯들을 담고 있는 게임 오브젝트
    [SerializeField]
    private GameObject inventory_Slots;
    
    private Slot[] slots;   // 슬롯 배열
    
    private int selectedIndex = -1; // 현재 선택된 슬롯의 인덱스

    // 아이템을 드롭할 위치를 나타내는 트랜스폼 변수
    [SerializeField]
    private Transform dropPosition;

    // 상점 드롭 위치를 나타내는 트랜스폼 배열 변수
    [SerializeField]
    private Transform[] shopDropPositions;
    private DropZone dropZone;  // 드롭 존 스크립트의 참조 변수
    
    private int currentShopDropIndex = 0;   // 현재 상점 드롭 위치의 인덱스
    private int totalDroppedItems = 0;  // 총 드롭된 아이템의 갯수
    private int totalPrice = 0; // 총 드롭된 아이템의 가격

    // 이미 드롭된 아이템을 추적하기 위한 해시 집합
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

    // 인벤토리에 아이템을 추가하는 함수
    public void AddItemToInventory(Item item)
    {
        // 모든 슬롯을 순회하면서 비어 있는 슬롯에 아이템을 추가
        foreach (Slot slot in slots)
        {
            if (slot.item == null)
            {
                slot.AddItem(item);
                break;
            }
        }
    }

    // 키 입력에 따라 슬롯을 선택하는 함수
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

    // 특정 인덱스의 슬롯을 강조 표시하는 함수
    private void HighlightSlot(int index)
    {
        selectedIndex = index;  // 선택된 슬롯의 인덱스를 저장
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetHighlight(i == index);  // 선택된 슬롯을 강조 표시
        }
    }

    // 아이템을 드롭하는 동작을 처리하는 함수
    private void HandleItemDrop()
    {
        // F 키가 눌리고 선택된 슬롯이 유효한 경우에 실행
        if (Input.GetKeyDown(KeyCode.F) && selectedIndex != -1)
        {
            Slot selectedSlot = slots[selectedIndex];  // 선택된 슬롯 가져오기
            if (selectedSlot.item != null)  // 선택된 슬롯에 아이템이 존재하는 경우
            {
                DropItem(selectedSlot);  // 아이템을 드롭하는 함수 호출
            }
        }
    }

    // 실제 아이템을 드롭하는 함수
    private void DropItem(Slot slot)
    {
        Item itemToDrop = slot.item;  // 드롭할 아이템 가져오기
        slot.ClearSlot();  // 슬롯 비우기

        bool isNewDrop = !droppedItems.Contains(itemToDrop);  // 아이템이 처음으로 드롭되는지 확인

        // 플레이어가 드롭 존에 있고 존재하는 경우
        if (dropZone != null && dropZone.IsPlayerInZone())
        {
            Transform dropPos = shopDropPositions[currentShopDropIndex];  // 현재 상점 드롭 위치 가져오기
            Instantiate(itemToDrop.itemPrefab, dropPos.position, Quaternion.identity);  // 아이템의 프리팹을 해당 위치에 생성
            currentShopDropIndex = (currentShopDropIndex + 1) % shopDropPositions.Length;  // 다음 상점 드롭 위치 인덱스로 업데이트

            if (isNewDrop)
            {
                totalPrice += itemToDrop.Value;  // 아이템 가격을 총 가격에 추가
                droppedItems.Add(itemToDrop);  // 드롭된 아이템을 집합에 추가
            }
        }
        // 드롭 위치가 설정되어 있고 존재하지 않는 경우 (일반적인 드롭)
        else if (dropPosition != null)
        {
            Instantiate(itemToDrop.itemPrefab, dropPosition.position, Quaternion.identity);  // 아이템을 설정된 드롭 위치에 생성
        }

        if (isNewDrop)
        {
            totalDroppedItems++;  // 드롭된 아이템 갯수 증가
        }

        // 디버그 로그로 총 갯수와 총 가격 출력
        Debug.Log($"총 갯수: {totalDroppedItems}");
        Debug.Log($"총 가격: {totalPrice}");
    }
}
