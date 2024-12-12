--[[
    author:author
    create:2024/12/11 15:43:13
    desc: 依赖注入（DI：Dependency Injection）
]]

IOC = {}

IOC.Containor = {}

--[[
bindItem:
    classAddress 绑定类address
    className 类名
    implement 继承的类
    getter    获取方式,除非是常量，否则使用function 返回值，因为如果使用new直接赋值会调用里面的Inject方法
]]

--- 绑定Class到Containor
---@param classAddress 绑定classAddress
---@param implement 继承类
function IOC.BindClass(classAddress, implement)
    IOC.lockClassAddressKey = classAddress
    local bindItem = {}
    bindItem.classAddress = classAddress
    bindItem.className = GetClassNameByAddress(classAddress)
    bindItem.implement = implement
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

function IOC.FromInstance()
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getter = function()
        if bindItem.instance == nil then
            bindItem.instance = new(bindItem.classAddress)
        end
        return bindItem.instance
    end 
end

function IOC.FromNewPrefab(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    bindItem.getter = function(...)
        return AssetLoader.LoadGameObjectSync(prefabAddress, ({...})[1])
    end
end

function IOC.FromFactory(prefabAddress)
    local bindItem = IOC.Containor[IOC.lockClassAddressKey]
    
    --三种情况
    --绑定了MonoClass，那么创建预制体并获取物体
    --绑定了Class，那么newClass
    --绑定了预制体, 工厂内不可以加入不带脚本的流水线
    if bindItem.classAddress == nil then
        Debug.LogError("Factory item must be a lua table, which item is " .. IOC.lockClassAddressKey)
        return
    end

    --如果是Mono
    if IsMono(bindItem.classAddress) then
        --添加流水线的getter从GameObject获取新script
        Factory.AddScriptAssemblyLine(bindItem.className, function(...)
            return AssetLoader.LoadGameObjectSync(prefabAddress, ({...})[1])
        end)
    else
        --添加流水线的getter从新类获取script
        Factory.AddScriptAssemblyLine(bindItem.className, function(...)
            return new(bindItem.classAddress, ...)
        end)
    end

    bindItem.getter = function(...)
        return Factory.Get(bindItem.className, ...)
    end 
end

function IOC.Bind(key, getter)
    local bindItem = {}
    bindItem.getter = getter
    IOC.Containor[key] = bindItem
end

function IOC.InjectClass(classAddress)
    local bindItem = IOC.Containor[classAddress]
    return Class(bindItem.className, bindItem.implement)
end

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