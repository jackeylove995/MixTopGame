--[[
    author:author
    create:2024/12/12 14:55:08
    desc: 
]] 
Factory = {}

Factory.AssemblyLineMap = {} -- 流水线

function Factory.AddScriptAssemblyLine(keyInFactory, getter)
    if Factory.AssemblyLineMap[keyInFactory] ~= nil then
        return
    end

    local factoryLine = {}
    factoryLine.getter = getter
    factoryLine.pKey = 0
    factoryLine.inUse = {}
    factoryLine.noUse = {}
    Factory.AssemblyLineMap[keyInFactory] = factoryLine
end

function Factory.Get(keyInFactory, ...)
    local factoryLine = Factory.AssemblyLineMap[keyInFactory]
    local hasCache = #factoryLine.noUse ~= 0
    local obj = hasCache and table.remove(factoryLine.noUse) or factoryLine.getter(...)
    factoryLine.pKey = factoryLine.pKey + 1
    table.insert(factoryLine.inUse, factoryLine.pKey, obj)
    obj.pKeyInFactory = factoryLine.pKey
    obj.keyInFactory = keyInFactory
    return obj
end

function Factory.Take(obj)
    local factoryLine = Factory.AssemblyLineMap[obj.keyInFactory]
    table.insert(factoryLine.noUse, table.remove(factoryLine.inUse, obj.pKeyInFactory))
end

