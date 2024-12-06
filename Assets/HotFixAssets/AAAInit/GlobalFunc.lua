--[[
    author:author
    create:2024/12/5 21:12:38
    desc: 全局方法
]]

--- 创建GameObject
---@param address 资源在HotFixAssets文件夹下地址
---@param parent 父物体
---@param onCreate 创建成功携带GameObject和Lua脚本的回调
function LoadGameObject(address, parent, onCreate)
    AssetLoader.LoadGameObject(address, parent, onCreate)
end

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

function newTable(tableAddess)
    return require(tableAddess)
end

function print(mes)
    Debug.Log(mes)
end