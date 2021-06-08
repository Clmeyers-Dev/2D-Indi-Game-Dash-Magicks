using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_iTEM;
    public int NUMBER_OF_COLUM;
    public int Y_SPACE_BETWEEN_ITEMS;
    public GameObject inventoryPrefab;
    public override void createSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { onEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { onExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { onDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { onDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { onDrag(obj); });

            inventory.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.Container.Slots[i]);

        }
    }
    private void Update()
    {
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            inventory.Container.Slots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + X_SPACE_BETWEEN_iTEM * (i % NUMBER_OF_COLUM), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUM)), 0f);
    }
}
