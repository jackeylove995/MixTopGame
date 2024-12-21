--- author:author
--- create:2024/12/21 10:13:52
--- desc: 

---@class Enemy
local Enemy = IOC.InjectClass(Enemy_lua)

local flys = {}
local canFly

function Enemy:OnGetOrCreate(param)
    self.transform:SetParent(param.parent)
    self.data = param.model
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)
    self:GenerateFlys()
end

function Enemy:Move(x, y)
    UnityUtil.LocalMove(self.transform, x * self.data:GetMoveSpeed(), y * self.data:GetMoveSpeed())
end

--- 生成飞行物
---@param count 数量
function Enemy:GenerateFlys()
    local everyAddEuler = 360 / self.data.flyCount
    local distance = 1

    for i = 1, self.data.flyCount, 1 do
        IOC.Inject(Fly_lua, {
            parent = self.FlyContainer,
            Enemy = self,
            enter = function(...)
                self:OnOtherFlyEnter(...)
            end
        }, function(fly)
            table.insert(flys, fly)
            if #flys == self.data.flyCount then
                self:FlyOver()
            end
        end)

    end

    canFly = true
end

function Enemy:FlyOver()
    local everyAddEuler = 360 / self.data.flyCount
    local distance = 1

    for i = 1, self.data.flyCount, 1 do
        local hudu = (i * everyAddEuler * Math.PI) / 180
        local x = Math.Sin(hudu) * distance
        local y = Math.Cos(hudu) * distance
        local fly = flys[i]
        UnityUtil.SetLocalPosition(fly.transform, x, y, -1)
        -- 使物体的Y轴指向指定方向
        fly.transform.localRotation = Quaternion.Euler(0, 0, -i * everyAddEuler)
    end
end

function Enemy:FixedUpdate()
    if canFly then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * self.data:GetWeaponSpeed())
    end
end

function Enemy:OnOtherFlyEnter(fly, other)
    local otherEnemy = other.Enemy
    if self.data.team == otherEnemy.data.team then
        return
    end

    -- 1.比较攻击力
    -- 2.攻击力相同，触发回震系数
    local thisFlyAttack = self.data:GetAttack()
    local otherFlyAttack = otherEnemy.data:GetAttack()
    if thisFlyAttack > otherFlyAttack then
        otherEnemy:DestroyFly(other)
    elseif otherFlyAttack > thisFlyAttack then
        self:DestroyFly(fly)
    else
        otherEnemy:DestroyFly(other)
        self:DestroyFly(fly)
    end
end

function Enemy:DestroyFly(fly)
    Factory.Take(fly)
    self.data.flyCount = self.data.flyCount - 1
end

return Enemy