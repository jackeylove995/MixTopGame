--- author:author
--- create:2024/12/5 21:12:38
--- desc: 全局方法

--- 推送消息
---@param name 消息名称
---@param tableParam 参数，table格式
function Push(name, tableParam)
    EventUtil.Push(name, tableParam)
end

--- 接收消息
---@param receiver 接收者
---@param name 消息名称
---@param andDo 接收到后做什么 ，andDo方法带参数，table格式
function Receive(receiver, name, andDo)
    EventUtil.Receive(receiver, name, andDo)
end

--- 函数用于深拷贝一个 table
function DeepCopy(original)
    local copy
    if type(original) == 'table' then
        copy = {}
        for k, v in next, original, nil do
            copy[DeepCopy(k)] = DeepCopy(v)
        end
        setmetatable(copy, DeepCopy(getmetatable(original)))
    else
        copy = original
    end
    return copy
end

--- 安全调用方法，如果为nil，则不调用
---@param func 传入方法
function SafeInvoke(func, ...)
    if func then
        func(...)
    end
end

function Log(mes)
    Debug.Log("LUA:" .. mes)
end

function LogFormat(mes, ...)
    Debug.Log("LUA:" .. string.format(mes, ...))
end

function print(mes)
    Debug.Log("LUA:" .. mes)
end

function printFormat(mes, ...)
    Debug.Log("LUA:" .. string.format(mes, ...))
end

function LogError(mes)
    Debug.LogError("LUA:" .. mes)
end

function LogTable(table, tableName)
    print(table.ToString(table, tableName))
end

--- 函数
---@param table 哪个table
---@param func table的方法
function PackFunction(table, func)
    local af = function(...)
        func(table, ...)
    end
    return af
end

--- obj is number check is nil or obj == 0
--- obj is string check is nil or obj == ""
--- obj is table  check is nil or #obj == 0
---@param obj 传入对象
function IsNilOrEmpty(obj)
    if obj == nil then
        return true
    end

    local type = type(obj)
    if type == "number" then
        return obj == 0
    elseif type == "string" then
        return obj == ""
    elseif type == "table" then
        return #obj == 0 
    else
        LogError("判断了一个没定义的类型，请添加此类型 ：" .. type)
    end
end

--- obj is number check is not nil and obj ~= 0
--- obj is string check is not nil and obj ~= ""
--- obj is table  check is not nil and #obj ~= 0
---@param obj 传入对象
function IsNotEmpty(obj)
    if obj == nil then
        return false
    end

    local type = type(obj)
    if type == "number" then
        return obj ~= 0
    elseif type == "string" then
        return obj ~= ""
    elseif type == "table" then
        return #obj ~= 0 
    else
        LogError("判断了一个没定义的类型，请添加此类型 ：" .. type)
    end
end

--- 添加点击事件
---@param button Unity Button
---@param func click event
---@param dotOverride 不重写之前的
function AddListener(button, func, dotOverride)
    if not dotOverride then
        button.onClick:RemoveAllListeners()
    end
    button.onClick:AddListener(func)
end