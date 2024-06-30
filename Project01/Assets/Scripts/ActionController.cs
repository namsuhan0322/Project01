using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 플레이어가 아이템을 감지할 수 있는 최대 거리
    private bool pickupActivated = false; // 아이템을 줍기 위한 상태를 나타내는 변수
    private RaycastHit hitInfo; // 레이캐스트에 의해 얻어진 충돌 정보를 저장하는 변수
    [SerializeField]
    private LayerMask layerMask; // 레이캐스트가 충돌을 검사할 레이어 마스크
    [SerializeField]
    private LayerMask ignorePickupLayer; // 아이템을 무시할 레이어 마스크
    [SerializeField]
    private Text actionText; // 화면에 표시될 텍스트 UI
    private Text dontActionText; // 사용되지 않는 변수
    [SerializeField]
    private Inventory inventory; // 플레이어의 인벤토리 클래스

    void Update()
    {
        CheckItem(); // 주변에 아이템이 있는지 체크하는 메서드 호출
        TryAction(); // 플레이어 입력에 따라 아이템을 줍기 시도하는 메서드 호출
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E)) // E 키 입력을 받으면
        {
            CheckItem(); // 주변에 아이템이 있는지 다시 체크
            CanPickUp(); // 아이템을 줍기 시도
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask)) // 정면 방향으로 레이를 쏘아 아이템을 감지
        {
            if (hitInfo.transform != null && hitInfo.transform.CompareTag("Item")) // 충돌한 객체가 아이템 태그를 가지고 있으면
            {
                ItemInfoAppear(); // 아이템 정보를 화면에 표시
            }
        }
        else // 아이템이 감지되지 않으면
        {
            ItemInfoDisappear(); // 아이템 정보 화면을 숨김
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true; // 아이템 줍기 활성화 상태로 설정
        actionText.gameObject.SetActive(true); // 텍스트 UI 활성화
        if (hitInfo.transform != null)
        {
            ItemPickUp itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>(); // 충돌한 객체에서 ItemPickUp 컴포넌트 가져오기
            if (itemPickUp != null)
            {
                actionText.text = itemPickUp.item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>"; // 아이템 정보 텍스트 설정
            }
        }
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false; // 아이템 줍기 비활성화 상태로 설정
        actionText.gameObject.SetActive(false); // 텍스트 UI 비활성화
    }

    private void CanPickUp()
    {
        if(pickupActivated && hitInfo.transform != null) // 아이템 줍기 활성화 상태이고 충돌 정보가 있으면
        {
            if (((1 << hitInfo.transform.gameObject.layer) & ignorePickupLayer) != 0) // 아이템을 무시할 레이어 마스크와 충돌한 객체의 레이어가 일치하는지 확인
            {
                return; // 무시할 레이어에 속하면 아이템을 줍지 않음
            }

            ItemPickUp itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>(); // 충돌한 객체에서 ItemPickUp 컴포넌트 가져오기
            if (itemPickUp != null)
            {
                Debug.Log(itemPickUp.item.itemName + " 획득 했습니다."); // 아이템을 플레이어가 획득했음을 콘솔에 로그로 출력
                if (inventory != null)
                {
                    bool itemAdded = inventory.AddItemToInventory(itemPickUp.item); // 인벤토리에 아이템 추가 시도
                    if (itemAdded)
                    {
                        Destroy(hitInfo.transform.gameObject); // 아이템을 성공적으로 추가했으면 게임에서 제거
                    }
                    else
                    {
                        Debug.Log("인벤토리가 가득 찼습니다!"); // 인벤토리가 가득 차서 아이템을 추가할 수 없음을 콘솔에 로그로 출력
                    }
                }
                ItemInfoDisappear(); // 아이템 정보 화면 숨김
            }
        }
    }
}
