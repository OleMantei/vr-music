using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    private void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string title)
    {
        return items.Find(item => item.title == title);
    }

    void BuildDatabase()
    {
        items = new List<Item>() {
                new Item(0, "kick"),
                new Item(1, "snare"),
                new Item(2, "hihat"),
                new Item(3, "toms"),
                new Item(4, "crash"),
                new Item(5, "ride"),
                new Item(6, "keys"),
                new Item(7, "rh_guitar_metal_1"),
                new Item(8, "rh_guitar_metal_2"),
                new Item(9, "lead_guitar_metal_1"),
                new Item(10, "lead_guitar_metal_2"),
                new Item(11, "rh_guitar_rock_1"),
                new Item(12, "rh_guitar_rock_2"),
                new Item(13, "lead_guitar_rock_1"),
                new Item(14, "lead_guitar_rock_2"),
                new Item(15, "bass"),
                new Item(16, "microphone"),
                };
    }
}
