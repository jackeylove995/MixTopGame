using System;
using MTG;
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
        HoldLua holdLua = other.GetComponent<HoldLua>();
        if (holdLua != null)
        {           
            OnOtherFlyEnter?.Invoke(lua.scriptTable, holdLua.holder.scriptTable);
        }        
    }
}
