--- author:author
--- create:2024/12/12 21:06:07
--- desc: 

local GameDataManager = IOC.InjectClass(GameDataManager_lua)

local RolesConfig = IOC.Inject(RolesConfig_lua)
local LevelConfig = IOC.Inject(LevelConfig_lua)
local WeaponsConfig = IOC.Inject(WeaponsConfig_lua)

local PlayerModel 
function GameDataManager:GetPlayerModel()

    local playerRole = RolesConfig[1]
    local playerWeapon = WeaponsConfig[1]
    PlayerModel = IOC.Inject(PlayerModel_lua, {
        team = 1,
        pos = Vector3(0, 0, -1),
        flyCount = 5,
        roleConfig = playerRole,
        weaponConfig = playerWeapon
    })
    return PlayerModel
    
end

function GameDataManager:GetLevelModel()
    local config = LevelConfig[1].config
    local levelModel = IOC.Inject(LevelModel_lua)
    levelModel:SetAndResolveConfig(config)
    return levelModel
end

function GameDataManager:GetBotsData()
    local data = {}
    for i = 1, 5, 1 do
        local role = RolesConfig[1]
        local weapon = WeaponsConfig[1]
        return IOC.Inject(PlayerModel_lua, {
            team = 1,
            pos = Vector3(0, 0, -1),
            flyCount = 5,
            roleConfig = role,
            weaponConfig = weapon
        })
    end
    return data
end

return GameDataManager
