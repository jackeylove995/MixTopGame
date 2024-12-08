--[[
    author:author
    create:2024/12/7 20:35:32
    desc: 玩家控制
]]

--- Player

local speed = 1
local speedExtra = 0.1
function Move(dir)
    UnityUtil.LocalMove(self.transform, dir.x * speed * speedExtra , dir.y * speed * speedExtra)
end

