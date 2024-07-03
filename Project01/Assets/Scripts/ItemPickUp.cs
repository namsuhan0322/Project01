using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public GameObject itemInfoObj;

    public Text itemName;
    public Text itemValue;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.RegisterItemPickUp(this);
    }
    
    void Update()
    {
        // UI가 항상 카메라를 바라보도록 설정
        itemInfoObj.transform.LookAt(Camera.main.transform);
        itemInfoObj.transform.Rotate(0, 180, 0); // LookAt이 반대로 설정되어 있으면 180도 회전
    }
    
    void OnDestroy()
    {
        player.UnregisterItemPickUp(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                bool itemAdded = inventory.AddItemToInventory(item);
                if (itemAdded)
                {
                    Destroy(gameObject); // 아이템을 성공적으로 인벤토리에 추가한 경우에만 아이템 오브젝트를 파괴
                }
                else
                {
                    Debug.Log("인벤토리가 가득 찼습니다!"); // 인벤토리가 가득 차서 아이템을 추가할 수 없는 경우 메시지 출력
                }
            }
        }
    }

    public void ScanningItemInfo()
    {
        if (player.isScanning)
        {
            itemInfoObj.SetActive(true);
            itemName.text = $"이름 : {item.itemName}";
            itemValue.text = $"가격 : {item.Value}";
        }
    }

    public void FinishScanning()
    {
        itemInfoObj.SetActive(false);
    }
}