--[[
    author:author
    create:2024/12/12 14:55:08
    desc: 
]] 
Factory = {}

Factory.AssemblyLineMap = {} -- 流水线

function Factory.AddScriptAssemblyLine(scriptKey, getter)
    if Factory.AssemblyLineMap[scriptKey] ~= nil then
        return
    end

    local factoryLine = {}
    factoryLine.getter = getter
    factoryLine.pKey = 0
    factoryLine.inUse = {}
    factoryLine.noUse = {}
    Factory.AssemblyLineMap[scriptKey] = factoryLine
end

function Factory.Get(scriptKey, ...)
    local factoryLine = Factory.AssemblyLineMap[scriptKey]
    local hasCache = #factoryLine.noUse ~= 0
    local obj = hasCache and table.remove(factoryLine.noUse) or factoryLine.getter(...)
    local pKey = factoryLine.pKey + 1
    factoryLine.pKey = pKey
    table.insert(factoryLine.inUse, pKey, obj)
    obj.pKey = pKey
    obj.scriptKey = scriptKey
    if rawget(obj, "OnUse") then
        Debug.Log("get from factory")
        obj:OnUse(...)
    end
    return obj
end

function Factory.Take(obj)
    if rawget(obj, "OnUnUse") then
        obj:OnUnUse()
    end
    local factoryLine = Factory.AssemblyLineMap[obj.scriptKey]
    table.insert(factoryLine.noUse, table.remove(factoryLine.inUse, obj.pKey))
end

