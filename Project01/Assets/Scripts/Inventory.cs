using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory_Slots; // 슬롯을 포함하는 게임 오브젝트

    private Slot[] slots; // 슬롯 배열

    private int selectedIndex = -1; // 선택된 슬롯 인덱스

    [SerializeField]
    private Transform dropPosition; // 아이템을 드롭할 위치

    [SerializeField]
    private Transform[] shopDropPositions; // 상점에 아이템을 드롭할 위치 배열
    private DropZone dropZone; // 드랍 존 스크립트

    private int currentShopDropIndex = 0; // 현재 상점 드롭 위치 인덱스
    private int totalDroppedItems = 0; // 총 드롭된 아이템 수
    private int totalPrice = 0; // 총 판매 가격

    [SerializeField]
    private Text currentBalanceText; // 현재 잔고를 표시하는 텍스트 UI

    void Start()
    {
        slots = inventory_Slots.GetComponentsInChildren<Slot>(); // 슬롯 배열 초기화
        dropZone = FindObjectOfType<DropZone>(); // 드랍 존 스크립트 초기화
        UpdateBalanceText(); // 잔고 텍스트 업데이트
    }

    void Update()
    {
        HandleSlotSelection(); // 슬롯 선택 처리
        HandleItemDrop(); // 아이템 드롭 처리
    }

    public bool AddItemToInventory(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item == null) // 비어있는 슬롯을 찾으면
            {
                slot.AddItem(item); // 아이템 추가
                return true;
            }
        }
        Debug.Log("아이템이 가득 찼습니다!"); // 모든 슬롯이 찼을 때 메시지 출력
        return false;
    }

    private void HandleSlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 키 입력에 따라 슬롯을 선택
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
        selectedIndex = index; // 선택된 슬롯 인덱스 설정
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetHighlight(i == index); // 선택된 슬롯을 하이라이트 표시
        }
    }

    private void HandleItemDrop()
    {
        if (Input.GetKeyDown(KeyCode.F) && selectedIndex != -1) // F 키 입력에 따라 아이템을 드롭
        {
            Slot selectedSlot = slots[selectedIndex];
            if (selectedSlot.item != null) // 선택된 슬롯이 비어있지 않으면
            {
                DropItem(selectedSlot); // 아이템 드롭
            }
        }
    }

    private void DropItem(Slot slot)
    {
        Item itemToDrop = slot.item; // 드롭할 아이템 가져오기
        slot.ClearSlot(); // 슬롯 비우기

        if (dropZone != null && dropZone.IsPlayerInZone()) // 플레이어가 드롭 존에 있는 경우
        {
            Transform dropPos = shopDropPositions[currentShopDropIndex]; // 현재 상점 드롭 위치 가져오기
            GameObject droppedItem = Instantiate(itemToDrop.itemPrefab, dropPos.position, Quaternion.identity); // 아이템 드롭
            droppedItem.layer = LayerMask.NameToLayer("DroppedInShop"); // 상점 드롭 레이어로 설정
            currentShopDropIndex = (currentShopDropIndex + 1) % shopDropPositions.Length; // 다음 상점 드롭 위치 인덱스로 변경

            totalPrice += itemToDrop.Value; // 판매 가격 추가
            
            Destroy(droppedItem, 3f); // 일정 시간 후 아이템 삭제
            
            Invoke("UpdateBalanceText", 5f); // 일성 시간 후 정산 (이건 이제 벨을 눌렀을때 아이템을 3 ~ 5초에 전체 삭제 시키고 랜덤으로 5 ~ 10초 사이에 정산으로 바뀔예정)
        }
        else if (dropPosition != null) // 드롭 존에 없는 경우
        {
            Instantiate(itemToDrop.itemPrefab, dropPosition.position, Quaternion.identity); // 기본 위치에 아이템 드롭
        }
        
        totalDroppedItems++; // 총 드롭된 아이템 수 증가

        Debug.Log($"총 갯수: {totalDroppedItems}"); // 드롭된 아이템 수 로그 출력
        Debug.Log($"총 가격: {totalPrice}"); // 총 판매 가격 로그 출력
        //UpdateBalanceText(); // 잔고 텍스트 업데이트
    }

    private void UpdateBalanceText()
    {
        currentBalanceText.text = $"현재 잔고: {totalPrice}$"; // 잔고 텍스트 업데이트
    }
}
