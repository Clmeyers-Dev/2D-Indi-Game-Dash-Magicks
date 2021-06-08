using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Food,
    Helment,
    Weapon,
    Utility,
    Boots,
    Chest,
    Accessory, 
    Potion,
    Modifier,
    ModifiedWeapon,
    Consumable,
    Default
}
public enum Attributes
{
    Agility,
    Intelect,
    Stamina,
    Strength,
    Constituion,
    speed
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{
    public GameObject Model;
    public float TimeTillDecay;
    public bool canDecay;
    public GameObject prefab;
    public Sprite uiDisplay;
    public bool stackable;
    public int value;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();
    public bool isModfiable;
    public ItemObject Replacement;
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

    public virtual void Useitem()
    {

    }
}

[System.Serializable]
public class Item
{
    public int value=0;
    public bool canDie;
    public float timeRemaining = 10;
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
   
    public Item(ItemObject item)
    {
        value = item.value;
        timeRemaining = item.TimeTillDecay;
        canDie = item.canDecay;
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}