
using System;
using System.Collections;
using System.Collections.Generic;
using MTG;
using Unity.VisualScripting;
using UnityEngine;
using XLua;

public class ColliderCheck : MonoBehaviour
{
    public Action<LuaTable,LuaTable> OnOtherFlyEnter;
    public LuaBehaviour lua;
    void Start()
    {
        OnOtherFlyEnter = lua.scriptTable.Get<Action<LuaTable,LuaTable>>("OnOtherFlyEnter");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        ColliderCheck colliderCheck = other.GetComponent<ColliderCheck>();
        if (colliderCheck != null)
        {           
            OnOtherFlyEnter?.Invoke(lua.scriptTable, colliderCheck.lua.scriptTable);
        }        
    }
}
