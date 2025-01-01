using System;
using MTG;
using UnityEngine;
using XLua;

public class ColliderCheck : MonoBehaviour
{
    public Action<LuaTable, LuaTable> OnOtherColliderEnter;
    public LuaBehaviour lua;

    void Start()
    {
        OnOtherColliderEnter = lua.scriptTable.Get<Action<LuaTable,LuaTable>>("OnOtherColliderEnter");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DeliverLua deliverLua = other.GetComponent<DeliverLua>();
        if (deliverLua != null)
        {           
            OnOtherColliderEnter?.Invoke(lua.scriptTable, deliverLua.holder.scriptTable);
        }        
    }
}
