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
                new Item(0, "Snare"),
                new Item(1, "Hihat"),
                new Item(2, "Bassdrum"),
                new Item(3, "Kick"),
                new Item(4, "Crash"),
                new Item(5, "RhythmGuitarMetal1"),
                new Item(6, "RhythmGuitarMetal2"),
                new Item(7, "LeadGuitarMetal1"),
                new Item(8, "LeadGuitarMetal2"),
                new Item(9, "BassGuitarMetal"),
                new Item(10, "Keys"),
                new Item(11, "Vox"),
                new Item(10, "Toms")
                };
    }
}
