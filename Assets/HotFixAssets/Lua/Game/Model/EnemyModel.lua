--- author:author
--- create:2024/12/21 11:05:54
--- desc: 

---@class EnemyModel
local EnemyModel = IOC.InjectClass(EnemyModel_lua)
local moveSpeedExtra = 0.1
function EnemyModel:OnGetOrCreate(param)
    self.config = param.config
    self.increaseConfig = param.increaseConfig
    self.pos = param.pos
end

function EnemyModel:GetAttack()
    return self.config.attack * self.increaseConfig.attackIncrease
end

function EnemyModel:GetMoveSpeed()
    return self.config.moveSpeed * moveSpeedExtra
end

return EnemyModel