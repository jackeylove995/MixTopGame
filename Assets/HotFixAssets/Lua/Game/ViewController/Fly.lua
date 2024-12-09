--[[
    author:author
    create:2024/12/9 20:51:32
    desc: 飞行物
]]

--- Fly

fly = false

--- 设置哪个玩家所拥有
---@param owner 玩家
function SetOwner(owner)
    
end

--- 设置飞行物数据
---@param flyModel 飞行物数据据
function SetFlyModel(flyModel)
    self.flyModel = flyModel
end

function StartFly()
    fly = true
end

function FixedUpdate()
    if fly then
        
    end
end

function OnDestroy()
    
end