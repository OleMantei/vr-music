using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public List<Item> shelfItems = new List<Item>();
    public ItemDatabase itemDatabase;

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
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItem(int id)
    {
        return shelfItems.Find(item => item.id == id);
    }

    public void RemoveItem(int id)
    {
        Item item = CheckForItem(id);
        if (item != null)
        {
            shelfItems.Remove(item);
            Debug.Log("Item removed: " + item.title);
        }
    }
}
