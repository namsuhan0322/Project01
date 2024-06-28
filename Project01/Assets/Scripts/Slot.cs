using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemImage;
    [SerializeField]
    private GameObject highlight;

    void Start()
    {
        SetColor(0);
        SetHighlight(false);
    }

    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImage;
        SetColor(1);
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        SetColor(0);
    }

    public void SetHighlight(bool isActive)
    {
        if (highlight != null)
        {
            highlight.SetActive(isActive);
        }
    }
}