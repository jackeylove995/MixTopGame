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
---@param name 消息名称
---@param andDo 接收到后做什么 ，andDo方法带参数，table格式
function Receive(name, andDo)
    EventUtil.Receive(name, andDo)
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

function print(mes)
    Debug.Log("LUA:" .. mes)
end

function LogError(mes)
    Debug.LogError("LUA:" .. mes)
end

function LogTable(table, tableName)
    print(table.ToString(table, tableName))
end