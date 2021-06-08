using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

   
    public override void createSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Slots.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { onEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { onExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { onDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { onDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { onDrag(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.Container.Slots[i]);
        }
    }
}
