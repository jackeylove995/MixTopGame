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
    self.maxFlyCount = param.roleConfig.maxFlyCount

    self.hp = self.roleConfig.hp
    self.configMoveSpeed = self.roleConfig.moveSpeed * moveSpeedExtra *
                               (self.increaseConfig and self.increaseConfig.moveSpeedIncrease or 1)

    -- 角色武器速度 = 角色武器速度 * 增益百分比
    self.configWeaponSpeed = self.roleConfig.weaponSpeed * weaponSpeedExtra *
                                 (self.increaseConfig and self.increaseConfig.weaponSpeedIncrease or 1)

    -- 角色攻击力 = （角色基础攻击 + 角色武器攻击）* 增益百分比
    self.configAttack = (self.roleConfig.attack + self.weaponConfig.attack) *
                            (self.increaseConfig and self.increaseConfig.attackIncrease or 1)
end

function RoleModel:InitPublicProperties(param)
    self.Team = param.team
    self.Pos = param.pos
    self.RoleId = self.roleConfig.id
    self.FlyDistance = self.roleConfig.flyDistance
    self.AnimLocalPosY = self.roleConfig.animPosY
    self.BaseAnimScale = self.roleConfig.animScale
    self.AttackPriority = param.attackPriority or 0
end

function RoleModel:InitBallsModel()
    if IsNilOrEmpty(self.roleConfig.ballsId) then
        return
    end
    local balls = string.Split(self.roleConfig.ballsId, '-')
    self.ballsModel = {}
    for i, v in ipairs(balls) do
        local ballConfig = BallsConfig[tonumber(v)]
        local ballModel = IOC.Inject(BallModel_lua, {
            config = ballConfig
        })
        table.insert(self.ballsModel, ballModel)
    end
end

--- 设置加速因子
---@param roleBaseInfoType 枚举 RoleBaseInfoType
---@param name 名称 string
---@param num 值 number
function RoleModel:SetIncreaseFactor(roleBaseInfoType, name, num)
    self.increaseFactors = self.increaseFactors or {}
    self.increaseFactors[roleBaseInfoType] = self.increaseFactors[roleBaseInfoType] or {}
    self.increaseFactors[roleBaseInfoType].name = num
end

function RoleModel:GetIncrease(roleBaseInfoType)
    local ret = 1
    if self.increaseFactors and self.increaseFactors[roleBaseInfoType] then
        for k, v in pairs(self.increaseFactors[roleBaseInfoType]) do
            ret = ret * v
        end
    end
    return ret
end

function RoleModel:GetMoveSpeed()
    return self.configMoveSpeed * self:GetIncrease(RoleBaseInfoType.Speed)
end

function RoleModel:GetWeaponSpeed()
    return self.configWeaponSpeed * self:GetIncrease(RoleBaseInfoType.Speed)
end

function RoleModel:GetAttack()
    return self.configAttack * self:GetIncrease(RoleBaseInfoType.Attack)
end

function RoleModel:GetMaxFlyCount()
    return self.maxFlyCount
end

function RoleModel:GetRandomBall()
    return self.ballsModel[math.random(#self.ballsModel)]
end

function RoleModel:RegistHpChangeEvent(event)
    self.onHpChange = self.onHpChange or {}
    table.insert(self.onHpChange, event)
end

function RoleModel:UnregisEvents()
    self.onHpChange = nil
end

function RoleModel:ChangeHp(changeValue)
    self.hp = self.hp + changeValue
    self:ExcuteHpChangeEvents()
end

function RoleModel:SetHp(newValue)
    self.hp = newValue
    self:ExcuteHpChangeEvents()
end

function RoleModel:GetHp()
    return self.hp
end

function RoleModel:ExcuteHpChangeEvents()
    if self.onHpChange then
        for _, event in pairs(self.onHpChange) do
            event(self.hp)
        end
    end
end

return RoleModel
