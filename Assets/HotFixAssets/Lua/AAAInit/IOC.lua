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
]]

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
    bindItem.getter = function()
        if bindItem.instance == nil then
            bindItem.instance = new(bindItem.classAddress)
        end
        return bindItem.instance
    end 
end

--- Inject时从新的Prefab身上获取luaScript
---@param prefabAddress 初始化的预制体
function IOC.FromNewPrefab(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getter = function(parentTransform)
        return AssetLoader.LoadGameObjectSync(prefabAddress, parentTransform)
    end
end

--- Inject时从工厂生成的Prefab获取luaScript
---@param prefabAddress 预制体
function IOC.FromFactory(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.fromFactory = true
    --三种情况
    --没绑定Class, 工厂内不可以加入不带脚本的流水线
    if bindItem.classAddress == nil then
        Debug.LogError("Factory item must be a lua table, which item is " .. IOC.lockClassAddressKey)
        return
    end

    --如果是Mono，那么从工厂获取物体bind各道
    if prefabAddress then
        --添加流水线的getter从GameObject获取新script
        Factory.AddScriptAssemblyLine(bindItem.className, function()
            return AssetLoader.LoadGameObjectSync(prefabAddress)
        end)
    else
        --如果是普通Class，从工厂获取
        --添加流水线的getter从新类获取script
        Factory.AddScriptAssemblyLine(bindItem.className, function()
            return new(bindItem.classAddress)
        end)
    end

    bindItem.getter = function(...)
        return Factory.Get(bindItem.className, ...)
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
        --最低优先级，什么都没绑定，只有classAddress，那么new一个Class
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
    --调用完释放lockClassAddressKey
    IOC.lockClassAddressKey = nil
    if IOC.Start then
        IOC.Start()
    else
        Debug.LogError("Installer must have BindStartByMethod, which is the start action of the scope! The installer address:" .. address)
    end
end

return IOC