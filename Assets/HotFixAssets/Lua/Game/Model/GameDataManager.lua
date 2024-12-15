--[[
    author:author
    create:2024/12/12 21:06:07
    desc: 
]] local GameDataManager = IOC.InjectClass(GameDataManager_lua)

function GameDataManager:GetPlayerData()
    local flyModel = IOC.Inject(FlyModel_lua, {
        flyType = 0,
        attack = 10,
        speed = 5,
        cho = 1
    })
    return IOC.Inject(PlayerData_lua, {
        team = 1,
        pos = Vector3(0, 0, -1),
        speed = 2,
        flyCount = 5,
        flyModel = flyModel
    })
end

function GameDataManager:GetBotsData()
    local data = {}
    for i = 1, IOC.Inject("BotCount"), 1 do
        local flyModel = IOC.Inject(FlyModel_lua, {
            flyType = 0,
            attack = 5,
            speed = 5,
            cho = 1
        })
        data[i] = IOC.Inject(PlayerData_lua, {
            team = 2,
            pos = Vector3(i, 0, -1),
            speed = 2,
            flyCount = 3,
            flyModel = flyModel
        })
    end
    return data
end

return GameDataManager
