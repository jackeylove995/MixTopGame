--- author:author
--- create:2024/12/11 15:43:13
--- desc: 依赖注入（DI：Dependency Injection）

---@class IOC
IOC = {}
IOC.Containors = {}

function IOC.AddContainor(containor)
    IOC.Containors[containor.name] = containor
end

function IOC.GetOrCreateContainorByAddress(address)
    local name = GetClassNameByAddress(address)
    if IOC.Containors[name] == nil then
        require(address)  
        if IOC.Containors[name] == nil then
            LogError("The containor you get can not find, addresss is " .. address)
            return
        end
        IOC.InitGetters(IOC.Containors[name])
    end
    return IOC.Containors[name]
end

function IOC.LoadContainorWithScope(address, ...)
    local containor = IOC.GetOrCreateContainorByAddress(address)
    IOC.NowScopeContainor = containor
    if containor.start then
        containor.start(...)
    else
        LogError(
            "Containor must have BindStartByMethod, which is the start action of the scope! The containor address:" ..
            containor.address)
    end
end

function IOC.InjectClass(classAddress)
    local bindItem = IOC.GetItemFromNowScope(classAddress)
    if bindItem.getType == IOC.FromType.FromFactory then
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
--- 同步：可变参数为单个对象param(推荐使用table格式)，param会传递到生成lua的OnGet方法中
--- 返回lua脚本
---
--- 异步：可变参数第一个为param，param会传递到生成lua的OnGet方法中,
--- 第二个为带lua脚本的回调
--- 
---@param key 通过BindXXX()方法绑定的key
function IOC.Inject(key, ...)
    local bindItem = IOC.GetItemFromNowScope(key)
    if not bindItem then
        Debug.LogError("[IOC] the key is not binded in now scope，key is " .. key .. " ,current scope is " .. IOC.NowScopeContainor.name)
        return nil
    end
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

--- 从当前范围获取item
function IOC.GetItemFromNowScope(addressKey)
    local bindItem = IOC.NowScopeContainor.content[addressKey]
    if bindItem == nil then
        for containorName, containorAddress in pairs(IOC.NowScopeContainor.otherContainorReferences) do
            local containor = IOC.GetOrCreateContainorByAddress(containorAddress)
            bindItem = containor.content[addressKey]
            if bindItem then
                return bindItem
            end
        end
    end
    return bindItem
end

--@region set getter
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
    -- 没绑定Class, 工厂内不可以加入不是Class的流水线
    if bindItem.classAddress == nil then
        LogError("[IOC] Factory item must be a lua table, which item is " .. bindItem.classAddress)
        return
    end

    --是预制体获取的class
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

function IOC.InitGetters(containor)
    for k, v in pairs(containor.content) do
        if IOC.TypeGetter[v.getType] then
            IOC.TypeGetter[v.getType](v)
        end
    end
end
--@endregion

return IOC
