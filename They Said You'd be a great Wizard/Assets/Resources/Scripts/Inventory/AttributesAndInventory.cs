using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesAndInventory : MonoBehaviour
{
    public InventoryObject deleteBox;
    public HealthManager healthManager;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject Modifiers;
    //public UseItemEffects itemEffects;
    public Attribute[] attributes;
    public PlayerManager playerManager;
   // public UIManager uManager;
    //public InputHandler input;
    
    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
/*        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }*/
        //itemEffects = GetComponent<UseItemEffects>();
        playerManager = FindObjectOfType<PlayerManager>();
       // uManager = FindObjectOfType<UIManager>();
    }

   
    
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
               // playerManager.checkForItem();
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
               // playerManager.updateMaxResistances();
              
                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.addModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
               
        }
    }


   // public GroundItem groundItem;
    public Collider temp;
    public void OnTriggerEnter(Collider other)
    {
        temp = other;
        // groundItem = other.GetComponent<GroundItem>();
     /*   if (groundItem)
        {
            uManager.InteractBar.SetActive(true);
           
        }*/
    }
    public void OnTriggerExit(Collider other)
    {
        temp = other;
       // groundItem = other.GetComponent<GroundItem>();
        /*if (groundItem)
        {
            uManager.InteractBar.SetActive(false);

        }*/
    }
/*
    public  void Interact()
    {
      
           Item _item; = new Item(groundItem.item);
      
        if (inventory.AddItem(_item, 1))
        {
            if (temp != null && temp.gameObject != null)
            {
                Destroy(temp.gameObject);
               // playerManager.InteractableItemInRange = false;
                //uManager.InteractBar.SetActive(false);
                return;
            }
            return;
               
        }
        
    }*/
    private void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Alpha1))
         {
             inventory.Save();
             equipment.Save();
         }
         if (Input.GetKeyDown(KeyCode.Alpha2))
         {
             inventory.Load();
             equipment.Load();
         }
         if (Input.GetKeyDown(KeyCode.Alpha4))
         {
             itemEffects.checkAcessories();
         }*/
       /* if (deleteBox.Container.Slots[0].item != null)
        {

            deleteBox.Container.Slots[0].RemoveItem();
        }*/
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
        healthManager.updateMaxStats();
    }
    public ModifiableInt getAttribute(Attribute attribute)
    {
        return attribute.value;
    }

    private void OnApplicationQuit()
    {
        inventory.clear();
       // equipment.clear();
        //Modifiers.clear();
        //deleteBox.clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public AttributesAndInventory parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(AttributesAndInventory _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}