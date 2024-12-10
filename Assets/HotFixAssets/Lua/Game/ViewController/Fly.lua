--[[
    author:author
    create:2024/12/9 20:51:32
    desc: 飞行物
]]

--- Fly
local Fly = Class("Fly", MonoBehaviour_lua)

--- 设置哪个玩家所拥有
---@param owner 玩家
function Fly:SetOwner(owner)
    self.owner = owner 
    self.transform:SetParent(owner.FlyContainer)
    self.transform.localPosition = Vector3(1,0,0)
end

--- 设置飞行物数据
---@param flyModel 飞行物数据据
function Fly:SetFlyModel(flyModel)
    self.flyModel = flyModel
end

return Fly