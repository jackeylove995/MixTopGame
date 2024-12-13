--[[
    author:author
    create:2024/12/12 21:51:20
    desc: 一个玩家的数据（包含bot）
]]

local PlayerData = IOC.InjectClass(PlayerData_lua)

function PlayerData:OnUse(pos, speed, flyCount)
    self.pos = pos
    self.speed = speed
    self.flyCount = flyCount
end

return PlayerData