using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeZone : MonoBehaviour
{
    [Header("아이템 정보 UI")]
    // 드랍된 아이템들을 저장할 리스트
    public List<Item> droppedItems = new List<Item>();
    public GameObject itemInSafeZone;
    public Text itemName;
    public Text itemValue;
    public Image itemImage;
    
    private void OnTriggerEnter(Collider other)
    {
        // 아이템인지 확인 (태그를 이용)
        if (other.CompareTag("Item"))
        {
            ItemPickUp itemPickUp = other.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                // 리스트에 추가
                droppedItems.Add(itemPickUp.item);
                UpdateUI(itemPickUp.item);
                itemInSafeZone.gameObject.SetActive(true);  
                Invoke("StopShowUI", 3f);
            }
        }
    }
    
    void UpdateUI(Item item)
    {
        itemName.text = $"{item.itemName}을 수집함!";
        itemValue.text = $"가격 : ${item.Value}";
        itemImage.sprite = item.itemImage;
    }

    void StopShowUI()
    {
        itemInSafeZone.gameObject.SetActive(false); 
    }
}
