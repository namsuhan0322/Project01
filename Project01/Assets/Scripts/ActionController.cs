using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;
    private bool pickupActivated = false;
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory inventory;

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null && hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        if (hitInfo.transform != null)
        {
            ItemPickUp itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                actionText.text = itemPickUp.item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
            }
        }
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if(pickupActivated && hitInfo.transform != null)
        {
            ItemPickUp itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                Debug.Log(itemPickUp.item.itemName + " 획득 했습니다.");
                if (inventory != null)
                {
                    inventory.AddItemToInventory(itemPickUp.item);
                }
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
}
