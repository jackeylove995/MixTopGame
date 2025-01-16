--- author:author
--- create:2024/12/12 21:51:20
--- desc: 一个玩家的数据（包含bot）

local RoleModel = IOC.InjectClass(RoleModel_lua)

local moveSpeedExtra = 0.1
local weaponSpeedExtra = 100

local BallsConfig = IOC.Inject(BallsConfig_lua)

function RoleModel:OnGet(param)
    self:InitPrivateProperties(param)
    self:InitPublicProperties(param)
    self:InitBallsModel()
end

function RoleModel:InitPrivateProperties(param)
    self.roleConfig = param.roleConfig
    self.weaponConfig = param.weaponConfig
    self.increaseConfig = param.increaseConfig
    self.configMoveSpeed = self.roleConfig.moveSpeed * moveSpeedExtra * (self.increaseConfig and self.increaseConfig.moveSpeedIncrease or 1)
    self.configWeaponSpeed = self.roleConfig.weaponSpeed * weaponSpeedExtra * (self.increaseConfig and self.increaseConfig.weaponSpeedIncrease or 1)
    self.configAttack = self.weaponConfig.attack * (self.increaseConfig and self.increaseConfig.attackIncrease or 1)
end

function RoleModel:InitPublicProperties(param)
    self.Team = param.team
    self.Pos = param.pos
    self.RoleId = self.roleConfig.id
    self.FlyDistance = self.roleConfig.flyDistance
    self.AnimLocalPosY = self.roleConfig.animPosY
    self.BaseAnimScale = self.roleConfig.animScale
    self.Hp = self.roleConfig.hp
end

function RoleModel:InitBallsModel()
    if IsNilOrEmpty(self.roleConfig.ballsId) then
        return
    end
    local balls = string.Split(self.roleConfig.ballsId, '-')
    self.ballsModel = {}
    for i, v in ipairs(balls) do
        local ballConfig = BallsConfig[tonumber(v)]
        local ballModel = IOC.Inject(BallModel_lua, {config = ballConfig})
        table.insert(self.ballsModel, ballModel)
    end
end

function RoleModel:GetMoveSpeed()
    return self.configMoveSpeed
end

function RoleModel:GetWeaponSpeed()
    return self.configWeaponSpeed
end

function RoleModel:GetAttack()
    return self.configAttack
end

function RoleModel:GetRandomBall()
    return self.ballsModel[math.random(#self.ballsModel)]
end


return RoleModel