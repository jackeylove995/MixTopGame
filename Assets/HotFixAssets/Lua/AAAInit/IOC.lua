--[[
    author:author
    create:2024/12/11 15:43:13
    desc: 依赖注入（DI：Dependency Injection）
]]
    
IOC = {}

IOC.Containor = {}

--[[
bindItem:
    classAddress    绑定类address
    className       类名
    implement       继承的类
    getter          获取方式,除非是常量，否则使用function 返回值，因为如果使用new直接赋值会调用里面的Inject方法
    fromFactory     从工厂获取
    fromInstance
    fromNewPrefab
    async
    sync
]]

IOC.FromType = {
    FromInstance = 1,
    FromNewPrefab = 2,
    FromFactory = 3
}

IOC.TypeGetter = {
    [IOC.FromType.FromInstance] = function(...)
        IOC.InstanceGetter(...)
    end,
    [IOC.FromType.FromNewPrefab] = function(...)
        IOC.NewPrefabGetter(...)
    end,
    [IOC.FromType.FromFactory] = function(...)
        IOC.FactoryGetter(...)
    end
}

--- 绑定Class到Containor
---@param classAddress 绑定classAddress
---@param . 继承类, 可传入多个参数实现多重继承
function IOC.BindClass(classAddress, ...)
    IOC.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.classAddress = classAddress
    bindItem.className = GetClassNameByAddress(classAddress)
    bindItem.implement = ...
    IOC.Containor[classAddress] = bindItem
    return IOC
end

--- 绑定Class到Containor, 自动继承Mono
---@param key 绑定的classAddress
function IOC.BindMonoClass(classAddress)
    IOC.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.classAddress = classAddress
    bindItem.className = GetClassNameByAddress(classAddress)
    bindItem.implement = MonoBehaviour_lua
    IOC.Containor[classAddress] = bindItem
    return IOC
end

--- Inject时总获取绑定的唯一对象
function IOC.FromInstance()
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromInstance
    return IOC
end

--- Inject时从新的Prefab身上获取luaScript
---@param prefabAddress 初始化的预制体
function IOC.FromNewPrefab(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromNewPrefab
    bindItem.prefabAddress = prefabAddress
    return IOC
end


--- Inject时从工厂生成的Prefab获取luaScript, 默认为异步操作
---@param prefabAddress 预制体
function IOC.FromFactory(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getType = IOC.FromType.FromFactory
    bindItem.prefabAddress = prefabAddress
    return IOC
end

--- 加载prefab过程标记为同步
function IOC.Sync()
    IOC.Containor[IOC.lockClassAddressKey].sync = true
end

--- 加载prefab过程标记为异步
function IOC.Async()
    IOC.Containor[IOC.lockClassAddressKey].async = true
end

function IOC.InitGetters()
    for k, v in pairs(IOC.Containor) do
        if IOC.TypeGetter[v.getType] then
            IOC.TypeGetter[v.getType](v)
        end
    end
end

function IOC.InstanceGetter(bindItem)
    bindItem.getter = function()
        if bindItem.instance == nil then
            bindItem.instance = new(bindItem.classAddress)
        end
        return bindItem.instance
    end
end

function IOC.NewPrefabGetter(bindItem)
    if bindItem.sync then
        bindItem.getter = function(parentTransform)
            return AssetLoader.LoadGameObjectSync(bindItem.prefabAddress, parentTransform)
        end
    else
        bindItem.getter = function(parentTransform, onCreate)
            AssetLoader.LoadGameObjectAsync(bindItem.prefabAddress, parentTransform, onCreate)
        end
    end
end

function IOC.FactoryGetter(bindItem)
    -- 没绑定Class, 工厂内不可以加入不带脚本的流水线
    if bindItem.classAddress == nil then
        Debug.LogError("Factory item must be a lua table, which item is " .. IOC.lockClassAddressKey)
        return
    end

    if bindItem.prefabAddress then
        if bindItem.sync then
            -- 添加流水线的getter从GameObject获取新script
            Factory.AddAssemblyLineSync(bindItem.className, function()
                return AssetLoader.LoadGameObjectSync(bindItem.prefabAddress)
            end)

            bindItem.getter = function(param)
                return Factory.GetSync(bindItem.className, param)
            end
        else
            -- 添加流水线的getter从GameObject获取新script
            Factory.AddAssemblyLineAsync(bindItem.className, function(onCreate)
                AssetLoader.LoadGameObjectAsync(bindItem.prefabAddress, nil, onCreate)
            end)

            bindItem.getter = function(param, onCreate)
                return Factory.GetAsync(bindItem.className, param, onCreate)
            end
        end
    else
        -- 如果是普通Class，从工厂获取
        -- 添加流水线的getter从新类获取script
        Factory.AddAssemblyLineSync(bindItem.className, function()
            return new(bindItem.classAddress)
        end)

        bindItem.getter = function(param)
            return Factory.GetSync(bindItem.className, param)
        end
    end
end

--- 通用绑定方法
---@param key 绑定的key
---@param getter 方式，function或常量都可以传
function IOC.Bind(key, getter)
    local bindItem = {}
    bindItem.getter = getter
    IOC.Containor[key] = bindItem
end

function IOC.InjectClass(classAddress)
    local bindItem = IOC.Containor[classAddress]
    if bindItem.fromFactory then
        if bindItem.implement then
            return Class(bindItem.className, bindItem.implement, IFactory_lua)
        else
            return Class(bindItem.className, IFactory_lua)
        end
    end
    return Class(bindItem.className, bindItem.implement)
end

--- 注入
---
--- FromInstance
--- 可变参数为空，返回Instance
---
--- FromNewPrefab
--- 同步：参数为父物体，返回lua脚本
--- 异步：第一个参数为父物体，第二个参数传入带lua脚本的回调
---
--- FromFactory
--- 同步：参数为单个对象param(推荐使用table格式)，返回lua脚本
--- 异步：第一个参数为param，第二个参数为带lua脚本的回调
--- param会传递到生成lua的OnGetOrCreate方法中
---@param key 通过BindXXX()方法绑定的key
function IOC.Inject(key, ...)
    local bindItem = IOC.Containor[key]
    if bindItem.getter then
        if type(bindItem.getter) == "function" then
            return bindItem.getter(...)
        else
            return bindItem.getter
        end
    else
        -- 最低优先级，什么都没From，只有classAddress，那么new一个Class
        return new(bindItem.classAddress, ...)
    end
end

function IOC.BindStartMethod(startAction)
    IOC.Start = startAction
end

function IOC.LoadInstaller(address)
    IOC.Start = nil
    IOC.Containor = {}
    require(address)
    -- 调用完释放lockClassAddressKey
    IOC.lockClassAddressKey = nil
    IOC.InitGetters()

    if IOC.Start then
        IOC.Start()
    else
        Debug.LogError(
            "Installer must have BindStartByMethod, which is the start action of the scope! The installer address:" ..
                address)
    end
end

return IOC
