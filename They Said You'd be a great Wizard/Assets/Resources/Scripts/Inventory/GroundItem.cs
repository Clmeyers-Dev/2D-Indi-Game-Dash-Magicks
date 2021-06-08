using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GroundItem : MonoBehaviour , ISerializationCallbackReceiver
{

    public ItemObject item;
    
    public void OnAfterDeserialize()
    {
       
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (GetComponentInChildren<SpriteRenderer>().sprite != null)
        {
            if (item.uiDisplay != null)
            {


                GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
            }
        }
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }
}
