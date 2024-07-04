using System.Collections;
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
    private LayerMask ignorePickupLayer;
    [SerializeField]
    private Text actionText;
    private Text dontActionText;
    [SerializeField]
    private Inventory inventory;

    void Update()
    {
        CheckItem();
        TryAction();
        CheckMouseClick();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        if (pickupActivated && hitInfo.transform != null)
        {
            if (((1 << hitInfo.transform.gameObject.layer) & ignorePickupLayer) != 0)
            {
                return;
            }

            ItemPickUp itemPickUp = hitInfo.transform.GetComponent<ItemPickUp>();
            if (itemPickUp != null)
            {
                Debug.Log(itemPickUp.item.itemName + " 획득 했습니다.");
                if (inventory != null)
                {
                    bool itemAdded = inventory.AddItemToInventory(itemPickUp.item);
                    if (itemAdded)
                    {
                        Destroy(hitInfo.transform.gameObject);
                    }
                    else
                    {
                        Debug.Log("인벤토리가 가득 찼습니다!");
                    }
                }
                ItemInfoDisappear();
            }
        }
    }

    private void CheckMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
            {
                if (hitInfo.transform != null && hitInfo.transform.CompareTag("Bell"))
                {
                    Debug.Log("벨을 눌렀습니다!");
                    StartCoroutine(inventory.ProcessSellItems());
                }
            } 
        }
    }
}
