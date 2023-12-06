using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShelf : MonoBehaviour
{
    public List<UIItem> uIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numbersOfSlots = 12;
    public LookAtObject lookAt;

    private void Awake()
    {
        Camera m_MainCamera;
        m_MainCamera = Camera.main;
        for (int i = 0; i < numbersOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab, slotPanel);
            uIItems.Add(instance.GetComponentInChildren<UIItem>());
            instance.AddComponent<LookAtObject>().mainCamera = m_MainCamera;
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        uIItems[slot].UpdateItem(item);
    }
    public void AddNewItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.item == null), item);
    }
    public void RemoveItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.item == item), null);
    }
}
