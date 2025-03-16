--- author:author
--- create:2024/12/19 10:43:04
--- desc: 

---@class WaveModel
local WaveModel = IOC.InjectClass(WaveModel_lua)

function WaveModel:Init(config, increaseConfig)
    self.config = config
    self.increaseConfig = increaseConfig
end

function WaveModel:GetTimeToNext()
    return self.config.timeToNext
end

function WaveModel:GetEnemyIdAndCount()
    return self.config.roleId, self.config.roleCount
end

function WaveModel:GetIncreaseConfig()
    return self.increaseConfig
end

function WaveModel:IsElite()
    return self.config.isElite
end

function WaveModel:IsBoss()
    return self.config.isBoss
end

return WaveModel