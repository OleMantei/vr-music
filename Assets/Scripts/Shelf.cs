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
        GiveItem(0);
        GiveItem(1);
        GiveItem(2);
        RemoveItem(1);
    }
    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        shelfItems.Add(itemToAdd);
        shelfUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItem(int id)
    {
        return shelfItems.Find(item => item.id == id);
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
}
