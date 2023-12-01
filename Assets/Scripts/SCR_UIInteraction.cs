using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SCR_UIInteraction : MonoBehaviour
{
    public Shelf shelf;

    public GameObject ui_canvas;
    GraphicRaycaster ui_raycaster;

    PointerEventData click_data;
    List<RaycastResult> click_results;

    void Start()
    {
        ui_raycaster = ui_canvas.GetComponent<GraphicRaycaster>();
        click_data = new PointerEventData(EventSystem.current);
        click_results = new List<RaycastResult>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetUiElementsClicked();
        }
    }

    void GetUiElementsClicked()
    {
        click_data.position = Mouse.current.position.ReadValue();
        click_results.Clear();

        ui_raycaster.Raycast(click_data, click_results);

        foreach (RaycastResult result in click_results)
        {
            GameObject ui_element = result.gameObject;
            if (ui_element.name == "Item")
            {
                string nameItem = ui_element.GetComponent<Image>().sprite.name;
                shelf.RemoveItem(nameItem);
                Rigidbody objBody = GameObject.Find(nameItem).transform.gameObject.GetComponent<Rigidbody>();
                AudioSource objAudio = GameObject.Find(nameItem).transform.gameObject.GetComponentInChildren<AudioSource>();
                objBody.transform.position = new Vector3(objBody.transform.position.x, objBody.transform.position.y, objBody.transform.position.z + 300);
                objAudio.mute = false;
            }
        }
    }
}
