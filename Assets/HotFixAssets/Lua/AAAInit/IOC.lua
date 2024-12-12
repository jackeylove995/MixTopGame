--[[
    author:author
    create:2024/12/11 15:43:13
    desc: 依赖注入（DI：Dependency Injection）
]]

IOC = {}

IOC.Containor = {}

--[[
bindItem:
    bindClass
    implement
    getter
]]

--- 绑定Class到Containor
---@param classAddress 绑定classAddress
---@param implement 继承类
function IOC.BindClass(classAddress, implement)
    IOC.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.bindClass = classAddress
    bindItem.implement = implement
    IOC.Containor[classAddress] = bindItem
    return IOC
end

--- 绑定Class到Containor, 自动继承Mono
---@param key 绑定的classAddress
function IOC.BindMonoClass(classAddress)
    IOC.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.bindClass = classAddress
    bindItem.implement = MonoBehaviour_lua
    IOC.Containor[classAddress] = bindItem
    return IOC
end

function IOC.FromInstance()
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getter = new(bindItem.bindClass)
end

function IOC.FromNewPrefab(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getter = function(prefabAddress, ...)
        AssetLoader.LoadGameObjectSync(prefabAddress, ({...})[1])
    end
end

function IOC.FromFactory(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    
    --三种情况
    --绑定了MonoClass，那么创建预制体并获取物体
    --绑定了Class，那么newClass
    --绑定了预制体, 工厂内不可以加入不带脚本的流水线
    if bindItem.bindClass == nil then
        Debug.LogError("Factory item must be a lua table, which item is " .. IOC.lockClassAddressKey)
        return
    end

    if IsMono(bindItem.bindClass) then
        Factory.AddScriptAssemblyLine(IOC.lockClassAddressKey, function(...)
            return AssetLoader.LoadGameObjectSync(prefabAddress, ({...})[1])
        end)
    else
        Factory.AddScriptAssemblyLine(IOC.lockClassAddressKey, function(...)
            return new(bindItem.bindClass, ...)
        end)
    end

    bindItem.getter = function()
        return Factory.Get(IOC.lockClassAddressKey)
    end 
end

function IOC.InjectClass(classAddress)
    local bindItem = IOC.Containor[classAddress]
    return Class(bindItem.bindClass, bindItem.implement)
end

function IOC.Inject(address, ...)
    local bindItem = IOC.Containor[address]
    if bindItem.getter then
        if type(bindItem.getter) == "function" then
            return bindItem.getter(...)
        else
            return bindItem.getter
        end
    else
        --最低优先级，什么都没绑定，只有BindClass，那么new一个Class
        return new(bindItem.bindClass, ...)
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