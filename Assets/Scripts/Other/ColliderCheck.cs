using System;
using MTG;
using UnityEngine;
using XLua;

public class ColliderCheck : MonoBehaviour
{
    public Action<LuaTable, LuaTable> OnOtherColliderEnter;
    public Action<LuaTable, LuaTable> OnOtherTriggerEnter;
    public LuaBehaviour lua;

    void Start()
    {
        OnOtherColliderEnter = lua.scriptTable.Get<Action<LuaTable, LuaTable>>("OnOtherColliderEnter");
        OnOtherTriggerEnter = lua.scriptTable.Get<Action<LuaTable, LuaTable>>("OnOtherTriggerEnter");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DeliverLua deliverLua = other.GetComponent<DeliverLua>();
        if (deliverLua != null)
        {
            OnOtherTriggerEnter?.Invoke(lua.scriptTable, deliverLua.holder.scriptTable);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        DeliverLua deliverLua = other.gameObject.GetComponent<DeliverLua>();
        if (deliverLua != null)
        {
            OnOtherColliderEnter?.Invoke(lua.scriptTable, deliverLua.holder.scriptTable);
        }
    }
}
