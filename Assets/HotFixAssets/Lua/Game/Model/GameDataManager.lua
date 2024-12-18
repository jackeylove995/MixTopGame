--- author:author
--- create:2024/12/12 21:06:07
--- desc: 

local GameDataManager = IOC.InjectClass(GameDataManager_lua)

local RolesConfig = IOC.Inject(RolesConfig_lua)
local LevelConfig = IOC.Inject(LevelConfig_lua)
local WeaponsConfig = IOC.Inject(WeaponsConfig_lua)

local playerData 
function GameDataManager:GetPlayerData()

    local playerRole = RolesConfig[1]
    local playerWeapon = WeaponsConfig[1]
    playerData = IOC.Inject(PlayerData_lua, {
        team = 1,
        pos = Vector3(0, 0, -1),
        flyCount = 5,
        roleConfig = playerRole,
        weaponConfig = playerWeapon
    })
    return playerData
    
end

function GameDataManager:GetBotsData()
    local data = {}
    for i = 1, 5, 1 do
        local role = RolesConfig[1]
        local weapon = WeaponsConfig[1]
        return IOC.Inject(PlayerData_lua, {
            team = 1,
            pos = Vector3(0, 0, -1),
            flyCount = 5,
            roleConfig = role,
            weaponConfig = weapon
        })
    end
    return data
end

function GameDataManager:GetLevelConfig()
    local levelConfig = IOC.Inject(LevelConfig_lua)[1]
    return levelConfig
end

return GameDataManager
