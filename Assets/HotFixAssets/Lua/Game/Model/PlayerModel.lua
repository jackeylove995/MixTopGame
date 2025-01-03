--- author:author
--- create:2024/12/12 21:51:20
--- desc: 一个玩家的数据（包含bot）

local PlayerModel = IOC.InjectClass(PlayerModel_lua)

local moveSpeedExtra = 0.1
local weaponSpeedExtra = 100
function PlayerModel:OnGetOrCreate(param)
    self.team = param.team
    self.pos = param.pos
    self.flyCount = param.flyCount
    self.roleConfig = param.roleConfig
    self.weaponConfig = param.weaponConfig
end

function PlayerModel:GetMoveSpeed()
    return self.roleConfig.moveSpeed * moveSpeedExtra
end

function PlayerModel:GetWeaponSpeed()
    return self.roleConfig.weaponSpeed * weaponSpeedExtra
end

function PlayerModel:GetAttack()
    return self.weaponConfig.attack
end

function PlayerModel:GetFlyDistance()
    return self.roleConfig.flyDistance
end

return PlayerModel