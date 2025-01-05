--- author:author
--- create:2025/1/5 13:36:23
--- desc: 

---@class BallModel
local BallModel = IOC.InjectClass(BallModel_lua)

function BallModel:OnGetOrCreate(param)
    local config = param.config
    self.config = config
    self.Type = config.type
    self.Icon = config.icon
    self.AttackIncrease = config.attackIncrease
    self.DefenceIncrease = config.defenceIncrease
    self.SpeedIncrease = config.speedIncrease
    self.Effect = config.effect
    self.Time = config.time
end

return BallModel