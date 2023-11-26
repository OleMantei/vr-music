using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public List<Item> shelfItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIShelf shelfUI;

    public void Start()
    {
        //GiveItem("Snare");
        //GiveItem("Hihat");
        //GiveItem("Bassdrum");
    }
    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        shelfItems.Add(itemToAdd);
        shelfUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public void GiveItem(string title)
    {
        Item itemToAdd = itemDatabase.GetItem(title);
        shelfItems.Add(itemToAdd);
        shelfUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItem(int id)
    {
        return shelfItems.Find(item => item.id == id);
    }
    public Item CheckForItem(string title)
    {
        return shelfItems.Find(item => item.title == title);
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            shelfItems.Remove(itemToRemove);
            shelfUI.RemoveItem(itemToRemove);
            Debug.Log("Item removed: " + itemToRemove.title);
        }
    }
    public void RemoveItem(string title)
    {
        Item itemToRemove = CheckForItem(title);
        if (itemToRemove != null)
        {
            shelfItems.Remove(itemToRemove);
            shelfUI.RemoveItem(itemToRemove);
            Debug.Log("Item removed: " + itemToRemove.title);
        }
    }
}
