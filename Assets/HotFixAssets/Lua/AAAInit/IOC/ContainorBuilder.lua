--- author:author
--- create:2025/1/12 21:20:18
--- desc: 

---@class ContainorBuilder
local ContainorBuilder = {}

function ContainorBuilder.SetContainoCorReference(containor)
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

function ContainorBuilder.BindStartMethod(startAction)
    ContainorBuilder.containor.start = startAction
end

function ContainorBuilder.Build()
    IOC.AddContainor(ContainorBuilder.containor)
    ContainorBuilder = nil
end

return ContainorBuilder