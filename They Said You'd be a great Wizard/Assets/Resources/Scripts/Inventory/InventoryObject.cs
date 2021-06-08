using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}
[CreateAssetMenu(fileName ="New Inventory", menuName ="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public InterfaceType type;
    public string savePath;
    public ItemDataBaseObject database;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }
   
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.ItemObjects[_item.Id].stackable|| slot == null)
        {
            setEmptySlot(_item, _amount);
            return true;
        }
       
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }
    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
            
        }
        return null;
    }
    public InventorySlot setEmptySlot(Item _item , int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot( _item, _amount);
                return GetSlots[i];
            }
        }
        //set functionality for full invnentory 
        return null;
    }
    public void SwapItems(InventorySlot item1,InventorySlot item2)
    {
        if(item2.CanPlaceInSlot(item1.ItemObject)&& item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot( item2.item, item2.amount);
            item2.UpdateSlot( item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }
    public void removeItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot( null, 0);
            }
        }
    }
  
    [ContextMenu("Save")]
    public void Save()
    {
        /*  string saveData = JsonUtility.ToJson(this, true);
          BinaryFormatter bf = new BinaryFormatter();
          FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
          bf.Serialize(file, saveData);
          file.Close();
          */
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))){
            /*  BinaryFormatter bf = new BinaryFormatter();
              FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
              JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
              file.Close();*/
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
           Inventory  newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                Container.Slots[i].UpdateSlot( newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void clear()
    {
        Container.Clear();
    }
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[48];
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
    
}
public delegate void slotUpdated(InventorySlot _slot);
[System.Serializable]
public class InventorySlot
{
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public slotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public slotUpdated OnBeforeUpdate;
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public ItemObject ItemObject {
        get
        {
            if (item.Id >= 0)
            {

                return parent.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }
 

    public void UpdateSlot( Item _item, int _amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if (OnAfterUpdate!=null)
            OnAfterUpdate.Invoke(this);
    }
    public void RemoveItem()
    {
        
        UpdateSlot(new Item(), 0);
    }
    public void ReplaceItem(Item repacementItem,int amount)
    {
        UpdateSlot(new Item(), 0);
        UpdateSlot(repacementItem, amount);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
       
    }
 
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
        
    }
    public void RemoveAmount(int value)
    {
        
            UpdateSlot(item, amount -= value);
           
        
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0|| _itemObject ==null ||_itemObject.data.Id<0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}


