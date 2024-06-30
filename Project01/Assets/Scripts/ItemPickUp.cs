using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

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
}
