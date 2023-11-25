using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item
{
    public int id;
    public string title;
    public Sprite icon;
    public Item(int id, string title)
    {
        this.id = id;
        this.title = title;
        this.icon = Resources.Load<Sprite>("Sprites/" + title);
    }
    public Item(Item item)
    {
        this.id = item.id;
        this.title = item.title;
        this.icon = Resources.Load<Sprite>("Sprites/" + item.title);
    }
}
