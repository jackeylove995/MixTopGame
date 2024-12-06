--[[
    author:author
    create:2024/12/6 16:31:02
    desc: 游戏数据
]]

--- GameData
local GameData = {}

function GameData:InitData(level, coin)
    self.level = level
    self.coin = coin
end

return GameData