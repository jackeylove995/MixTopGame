--- author:author
--- create:2024/12/12 21:06:07
--- desc: 

local GameDataManager = IOC.InjectClass(GameDataManager_lua)

local RolesConfig = IOC.Inject(RolesConfig_lua)
local LevelConfig = IOC.Inject(LevelConfig_Level_lua)
local WeaponsConfig = IOC.Inject(WeaponsConfig_lua)
local EnemyConfig = IOC.Inject(EnemyConfig_lua)

local PlayerModel 
function GameDataManager:GetPlayerModel()

    local playerRole = RolesConfig[1]
    local playerWeapon = WeaponsConfig[1]
    PlayerModel = IOC.Inject(PlayerModel_lua, {
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
        local enemyModel = IOC.Inject(EnemyModel_lua, {
            config = self:GetEnemyConfigById(enemyId),
            increaseConfig = self.waveModel:GetIncreaseConfig(),
            pos = Vector3(math.random( -10,10),math.random( -10,10),PlayerZDepth)
        })
        table.insert(ret, enemyModel)
    end
    return ret
end

function GameDataManager:GetTimeToNext()
    return self.waveModel:GetTimeToNext()
end

function GameDataManager:GetEnemyConfigById(id)
    return EnemyConfig[id]
end

function GameDataManager:GetBotsData()
    local data = {}
    for i = 1, 5, 1 do
        local role = RolesConfig[1]
        local weapon = WeaponsConfig[1]
        return IOC.Inject(PlayerModel_lua, {
            team = 1,
            pos = Vector3(0, 0, -1),
            roleConfig = role,
            weaponConfig = weapon
        })
    end
    return data
end

return GameDataManager
