--- author:author
--- create:2024/12/21 11:05:54
--- desc: 

---@class EnemyModel
local EnemyModel = IOC.InjectClass(EnemyModel_lua)

function EnemyModel:OnGetOrCreate(param)
    self.config = param.config
    self.increaseConfig = param.increaseConfig
end

function EnemyModel:GetAttack()
    return self.config.attack * self.increaseConfig.attackIncrease
end

return EnemyModel