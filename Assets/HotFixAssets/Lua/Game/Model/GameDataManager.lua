--[[
    author:author
    create:2024/12/12 21:06:07
    desc: 
]]

local GameDataManager = IOC.InjectClass(GameDataManager_lua)

function GameDataManager:GetPlayerData()
    return IOC.Inject(PlayerData_lua, Vector3(0,0,0), 5, 5)
end

function GameDataManager:GetBotsData()
    local data = {}    
    for i = 1, IOC.Inject("BotCount"), 1 do
        data[i] = IOC.Inject(PlayerData_lua, Vector3(i,0,0), 5,3)
    end
    return data
end

return GameDataManager