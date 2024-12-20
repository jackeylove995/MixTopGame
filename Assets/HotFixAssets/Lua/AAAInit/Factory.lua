--- author:author
--- create:2024/12/12 14:55:08
--- desc: 工厂
--- 工厂包含产品流水线，根据产品名key来定位具体流水线，然后操作流水线中的inuse和nouse数据
 
---@class Factory
Factory = {}

Factory.AssemblyLineMap = {} -- 流水线

--- 添加流水线
---@param keyInFactory 工厂中的key，产品名来定位具体流水线
---@param getter 获取方法
function Factory.AddAssemblyLineSync(keyInFactory, getter)
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

function Factory.AddAssemblyLineAsync(keyInFactory, getterWithOnCreate)
    if Factory.AssemblyLineMap[keyInFactory] ~= nil then
        return
    end

    local factoryLine = {}
    factoryLine.getter = getterWithOnCreate
    factoryLine.pKey = 0
    factoryLine.inUse = {}
    factoryLine.noUse = {}
    Factory.AssemblyLineMap[keyInFactory] = factoryLine
end

--- 从流水线中获取一个数据
---@param keyInFactory 产品名key，用来获取它的流水线
function Factory.GetSync(keyInFactory, param)
    local factoryLine = Factory.AssemblyLineMap[keyInFactory]
    local hasCache = #factoryLine.noUse ~= 0
    local obj = hasCache and table.remove(factoryLine.noUse) or factoryLine.getter()
    factoryLine.pKey = factoryLine.pKey + 1
    factoryLine.inUse[factoryLine.pKey] = obj
    obj.pKeyInFactory = factoryLine.pKey
    obj.keyInFactory = keyInFactory
    obj:OnGetOrCreate(param)
    return obj
end

--- 从流水线中获取一个数据
---@param keyInFactory 产品名key，用来获取它的流水线
function Factory.GetAsync(keyInFactory, param, onCreate)
    local factoryLine = Factory.AssemblyLineMap[keyInFactory]
    local hasCache = #factoryLine.noUse ~= 0

    if hasCache then
        local obj = table.remove(factoryLine.noUse)
        factoryLine.pKey = factoryLine.pKey + 1
        factoryLine.inUse[factoryLine.pKey] = obj
        obj.pKeyInFactory = factoryLine.pKey
        obj.keyInFactory = keyInFactory
        SafeInvoke(onCreate, obj)
        obj:OnGetOrCreate(param)        
    else
        local obj = {}
        factoryLine.pKey = factoryLine.pKey + 1
        factoryLine.inUse[factoryLine.pKey] = obj       
        local pKey = factoryLine.pKey
        factoryLine.getter(function(obj)
            obj.pKeyInFactory = pKey
            obj.keyInFactory = keyInFactory
            SafeInvoke(onCreate, obj)
            obj:OnGetOrCreate(param)           
        end)
    end
end

function Factory.Take(obj)
    local factoryLine = Factory.AssemblyLineMap[obj.keyInFactory]
    local nouse = factoryLine.inUse[obj.pKeyInFactory]
    factoryLine.inUse[obj.pKeyInFactory] = nil
    table.insert(factoryLine.noUse, nouse)
    obj:OnRecycle()
end

