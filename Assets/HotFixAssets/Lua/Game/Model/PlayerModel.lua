--- author:author
--- create:2024/12/12 21:51:20
--- desc: 一个玩家的数据（包含bot）

local PlayerModel = IOC.InjectClass(PlayerModel_lua)

local moveSpeedExtra = 0.1
local weaponSpeedExtra = 100

local BallsConfig = IOC.Inject(BallsConfig_lua)

function PlayerModel:OnGet(param)
    self.team = param.team
    self.pos = param.pos
    self.roleConfig = param.roleConfig
    self.weaponConfig = param.weaponConfig
    self:InitBallsModel()
end

function PlayerModel:InitBallsModel()
    local balls = string.Split(self.roleConfig.ballsId, '-')
    self.ballsModel = {}
    for i, v in ipairs(balls) do
        local ballConfig = BallsConfig[tonumber(v)]
        local ballModel = IOC.Inject(BallModel_lua, {config = ballConfig})
        table.insert(self.ballsModel, ballModel)
    end
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

function PlayerModel:GetRandomBall()
    return self.ballsModel[math.random(#self.ballsModel)]
end

return PlayerModel