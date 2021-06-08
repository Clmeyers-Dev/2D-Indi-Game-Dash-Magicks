using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;
using TMPro;

public abstract class UserInterface : MonoBehaviour
{
    public Transform player;
    public GameObject itemPrefab;
    public AttributesAndInventory playerRef;
    public InventoryObject inventory;
    
   public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    // Start is called before the first frame update

    void Start()
    {
       
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            inventory.Container.Slots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        createSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { onEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { onExitInterface(gameObject); });
    }
    private void Update()
    {
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            inventory.Container.Slots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
    }
    public void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    slotsOnInterface.UpdateSlotDisplay();
    //}
   
    public abstract void createSlots();


    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public void onEnter(GameObject obj)
    {
       MouseData.slotHoveredOver = obj;
       
    }
    public void onExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void onDragStart(GameObject obj)
    {
      
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
       
    }
    private void DropItem(ItemObject itemObject)
    {
        itemPrefab = itemObject.prefab;
        player = FindObjectOfType<AttributesAndInventory>().transform;
        var groundItem = Instantiate(itemPrefab).GetComponent<GroundItem>();
        groundItem.item = itemObject;
        groundItem.GetComponentInChildren<SpriteRenderer>().sprite = groundItem.item.uiDisplay;

        var angle = Random.Range(0f, 360f);
        var dist = Random.Range(1f, 2f);
        Vector3 PlayerPlus = new Vector3(0, 1, 1);
        groundItem.transform.position = player.transform.position+PlayerPlus;
        groundItem.transform.Translate(new Vector3(dist, 0, dist));
        groundItem.transform.Rotate(new Vector3(0, angle, 0));
    }
    public void onDragEnd(GameObject obj)
    {

        
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            DropItem(slotsOnInterface[obj].ItemObject);
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
       
       
    }
    public Consumables tempConsumable;
    IEnumerator Fade()
    {
       
        InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
        mouseHoverSlotData.RemoveAmount(1);
        yield return new WaitForSeconds(5f);
        StopCoroutine("Fade");

    }
    public void RightClick()
    {
        if (MouseData.slotHoveredOver)
        {

            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            if(mouseHoverSlotData.ItemObject!=null&&mouseHoverSlotData.ItemObject.type == ItemType.Consumable)
            {
                tempConsumable = (Consumables)mouseHoverSlotData.ItemObject;
                tempConsumable.UseItem();
                if (mouseHoverSlotData.amount > 1)
                {
                    StartCoroutine("Fade");
                    
                  
                }
                else
                {
                    mouseHoverSlotData.RemoveItem();
                    
                }
                
               
            }
           
        }
        
    }
    public void onDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged !=null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
        }
    }
    public void onEnterInterface(GameObject obj)
    {
       MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public void onExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
  

}
public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;

}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}

