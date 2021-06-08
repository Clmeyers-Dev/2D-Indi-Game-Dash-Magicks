using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt 
{
    [SerializeField]
    private int baseValue;
    private int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [SerializeField]
    private int modifiedValue;
    public int ModifiedValue { get { return modifiedValue; }private set { modifiedValue = value; } }
    public List<IModifiers> modifiers = new List<IModifiers>();
    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = BaseValue;
        if(method != null)
        {
            ValueModified += method;
        }
    }
    public void RegisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }
    public void UnregisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }
    public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);
        }
        modifiedValue = BaseValue + valueToAdd;
        if(ValueModified!= null)
        {
            ValueModified.Invoke();
        }
    }
    public void addModifier(IModifiers _modifer)
    {
        modifiers.Add(_modifer);
        UpdateModifiedValue();
    }
    public void RemoveModifier(IModifiers _modifer)
    {
        modifiers.Remove(_modifer);
        UpdateModifiedValue();
    }
}
