--[[
    author:author
    create:2024/12/9 20:43:30
    desc: 飞行物数据类
]]

local FlyModel = IOC.InjectClass(FlyModel_lua)

--- 数据模型
---@param echo: 回震系数
function FlyModel:OnGetOrCreate(param)
    self.flyType = param.flyType
    self.attack = param.attack
    self.speed = param.speed
    self.echo = param.echo
end

return FlyModel