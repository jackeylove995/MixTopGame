--- author:author
--- create:2025/1/12 21:20:18
--- desc: 
--[[
    containor = 
    {
        name = name,                    --containor name
        address = address,              --containor address
        content = {},                   --containor bindItems in content, bindItem struct is below
        otherContainorReferences = {},  --other containors ref
        start = nil                    --containor start func
    }

    bindItem =
    {
        classAddress = str,         --绑定类address
        className = str,            --类名
        implement = str,            --继承的类
        getter = func,              --获取方式,除非是常量，否则使用function 返回值，因为如果使用new直接赋值会调用里面的Inject方法
        getType = type,             --获取方式
        async = true,
        sync = false
    } 
]] --
---@class ContainorBuilder
ContainorBuilder = {}

function ContainorBuilder.NewContainorWithAddress(address)
    local name = GetClassNameByAddress(address)

    local containor = 
    {
        name = name, 
        address = address, 
        content = {}, 
        otherContainorReferences = {}, 
        start = nil
    }
    ContainorBuilder.containor = containor
end

--- 通用绑定方法
---@param key 绑定的key
---@param getter 方式，function或常量都可以传
function ContainorBuilder.Bind(key, getter)
    local bindItem = {}
    bindItem.getter = getter
    ContainorBuilder.containor.content[key] = bindItem
end

--- 绑定Class到ContainorBuilder
---@param classAddress 绑定classAddress
---@param . 继承类, 可传入多个参数实现多重继承
function ContainorBuilder.BindClass(classAddress, ...)
    ContainorBuilder.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.classAddress = classAddress
    bindItem.className = GetClassNameByAddress(classAddress)
    bindItem.implement = ...
    ContainorBuilder.containor.content[classAddress] = bindItem
    return ContainorBuilder
end

--- 绑定Class到ContainorBuilder, 自动继承Mono
---@param key 绑定的classAddress
function ContainorBuilder.BindMonoClass(classAddress)
    ContainorBuilder.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.classAddress = classAddress
    bindItem.className = GetClassNameByAddress(classAddress)
    bindItem.implement = MonoBehaviour_lua
    ContainorBuilder.containor.content[classAddress] = bindItem
    return ContainorBuilder
end

--- 通过Require绑定lua为一个Instance，通常为一个table
---@param addressKey any
function ContainorBuilder.BindInstanceByRequire(addressKey)
    local bindItem = {}
    bindItem.getter = function()
        if bindItem.instance == nil then
            bindItem.instance = require(addressKey)
        end
        return bindItem.instance
    end
    ContainorBuilder.containor.content[addressKey] = bindItem
end

--- 绑定一个Config表数据，这个表格内数据是只读的
---@param addressKey any
function ContainorBuilder.BindReadOnlyConfig(addressKey)
    local bindItem = {}
    bindItem.getter = function()
        if bindItem.instance == nil then
            bindItem.instance = require(addressKey)
            local meta = { __newIndex = function(table, key, value)
                LogError(key .. "设置新值失败，配置表格为只读")
            end}
            setmetatable(bindItem.instance, meta)
        end
        return bindItem.instance
    end
    ContainorBuilder.containor.content[addressKey] = bindItem
end

--- Inject时总获取绑定的唯一对象
function ContainorBuilder.FromInstance()
    local bindItem = ContainorBuilder.containor.content[ContainorBuilder.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromInstance
    return ContainorBuilder
end

--- Inject时从新的Prefab身上获取luaScript
---@param prefabAddress 初始化的预制体
function ContainorBuilder.FromNewPrefab(prefabAddress)
    local bindItem = ContainorBuilder.containor.content[ContainorBuilder.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromNewPrefab
    bindItem.prefabAddress = prefabAddress
    return ContainorBuilder
end

--- Inject时从工厂生成的Prefab获取luaScript, 默认为异步操作
---@param prefabAddress 预制体
function ContainorBuilder.FromFactory(prefabAddress)
    local bindItem = ContainorBuilder.containor.content[ContainorBuilder.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromFactory
    bindItem.prefabAddress = prefabAddress
    return ContainorBuilder
end

--- 加载prefab过程标记为同步
function ContainorBuilder.Sync()
    ContainorBuilder.containor.content[ContainorBuilder.lockClassAddressKey].sync = true
end

--- 加载prefab过程标记为异步
function ContainorBuilder.Async()
    ContainorBuilder.containor.content[ContainorBuilder.lockClassAddressKey].async = true
end

--- 如果自己的containor找不到，从其他containor的content获取item
function ContainorBuilder.BindItemsFromOtherContainorContent(containorAddress)
    local name = GetClassNameByAddress(containorAddress)
    ContainorBuilder.containor.otherContainorReferences[name] = containorAddress
end

function ContainorBuilder.BindStartMethod(startAction)
    ContainorBuilder.containor.start = startAction
end

function ContainorBuilder.Build()
    IOC.AddContainor(ContainorBuilder.containor)
    ContainorBuilder.lockClassAddressKey = nil
    ContainorBuilder.containor = nil
end

return ContainorBuilder
