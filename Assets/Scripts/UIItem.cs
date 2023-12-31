using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItem : MonoBehaviour
{
    public Item item;
    private Image spriteImage;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
    }

    public void UpdateItem(Item item)
    {
        this.item = item;
        if (this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon; 
            spriteImage.transform.GetChild(0).GetComponent<TMP_Text>().text = this.item.icon.name;
        }
        else
        {
            spriteImage.color = Color.clear;
            spriteImage.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        }
    }
}
