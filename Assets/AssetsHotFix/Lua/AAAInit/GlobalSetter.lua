--- author:author
--- create:2025/1/18 15:32:07
--- desc: 
---@class GlobalSetter
local GlobalSetter = {}

function GlobalSetter.BanSetGlobalValueDirectly()
    -- 设置全局环境表的元表
    setmetatable(_G, {
        __newindex = function(tbl, key, value)
            CS.UnityEngine.Debug.LogError("全局变量" .. key .. "需要在main的GlobalSetter设定")
        end
    })
end

function GlobalSetter.LogGlobalInfos()
    -- 设置全局环境表的元表
    CS.UnityEngine.Debug.Log("[GlobalSetter]全局属性信息\n\n" ..  GlobalSetter.GlobalPropertyInfo .. GlobalSetter.GlobalPropertiesFromTableInfo)
end

GlobalSetter.GlobalPropertyInfo = ""
GlobalSetter.GlobalPropertiesFromTableInfo = ""
local metatable_global_property = {
    __newindex = function(tbl, key, value)
        GlobalSetter.GlobalPropertyInfo = GlobalSetter.GlobalPropertyInfo .. "[" .. key .. "] type: ".. type(value) .. "\n"
        rawset(_G, key, value)
    end
}

local metatable_global_properties_in_table = {
    __newindex = function(tbl, key, value)
        GlobalSetter.GlobalPropertiesFromTableInfo = GlobalSetter.GlobalPropertiesFromTableInfo .. "\ntable name: " .. key ..
                                              "\n values in the table set as global:\n"
        for k, v in pairs(value) do
            GlobalSetter.GlobalPropertiesFromTableInfo = GlobalSetter.GlobalPropertiesFromTableInfo .. "        [" .. k .. "] type: ".. type(v) .. "\n"
            rawset(_G, k, v)
        end
    end
}

GlobalSetter.GlobalProperty = {}
GlobalSetter.GlobalPropertiesFromTable = {}

setmetatable(GlobalSetter.GlobalProperty, metatable_global_property)
setmetatable(GlobalSetter.GlobalPropertiesFromTable, metatable_global_properties_in_table)

return GlobalSetter
