--[[
    author:author
    create:2024/12/12 14:55:08
    desc: 工厂
    工厂包含产品流水线，根据产品名key来定位具体流水线，然后操作流水线中的inuse和unuse数据
]]
Factory = {}

Factory.AssemblyLineMap = {} -- 流水线


--- 添加流水线
---@param keyInFactory 工厂中的key，产品名来定位具体流水线
---@param getter 获取方法
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

--- 从流水线中获取一个数据
---@param keyInFactory 产品名key，用来获取它的流水线
function Factory.Get(keyInFactory, ...)
    local factoryLine = Factory.AssemblyLineMap[keyInFactory]
    local hasCache = #factoryLine.noUse ~= 0
    local obj = hasCache and table.remove(factoryLine.noUse) or factoryLine.getter()
    factoryLine.pKey = factoryLine.pKey + 1
    table.insert(factoryLine.inUse, factoryLine.pKey, obj)
    obj.pKeyInFactory = factoryLine.pKey
    obj.keyInFactory = keyInFactory
    obj:OnUse(...)
    return obj
end

function Factory.Take(obj)
    local factoryLine = Factory.AssemblyLineMap[obj.keyInFactory]
    table.insert(factoryLine.noUse, table.remove(factoryLine.inUse, obj.pKeyInFactory))
    obj:OnUnUse()
end

