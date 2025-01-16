--- author:author
--- create:2024/12/12 21:06:07
--- desc: 
local GameDataManager = IOC.InjectClass(GameDataManager_lua)

local RolesConfig = IOC.Inject(RolesConfig_lua)
local LevelConfig = IOC.Inject(LevelConfig_Level_lua)
local WeaponsConfig = IOC.Inject(WeaponsConfig_lua)
local IncreaseConfig = IOC.Inject(IncreaseConfig_lua)

local PlayerModel
function GameDataManager:GetPlayerModel()
    local playerRole = RolesConfig[10001]
    local playerWeapon = WeaponsConfig[50001]
    PlayerModel = IOC.Inject(RoleModel_lua, {
        team = 1,
        pos = Vector3(0, 0, -1),
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

function GameDataManager:ToNextWave()
    self.levelModel = self.levelModel or self:GetLevelModel()
    self.waveModel = self.levelModel:GetNextWave()
    return self.waveModel ~= nil
end

function GameDataManager:GetCurrentWaveEnemyModels()
    local enemyId, enemyCount = self.waveModel:GetEnemyIdAndCount()

    local ret = {}
    for i = 1, enemyCount, 1 do
        local enemyModel = IOC.Inject(RoleModel_lua, {
            team = 2,
            pos = Vector3(math.random(-10, 10), math.random(-10, 10), PlayerZDepth),
            roleConfig = RolesConfig[enemyId],
            increaseConfig = self.waveModel:GetIncreaseConfig()
        })
        table.insert(ret, enemyModel)
    end
    return ret
end

function GameDataManager:GetTimeToNext()
    return self.waveModel:GetTimeToNext()
end

return GameDataManager
