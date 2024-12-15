--[[
    author:author
    create:2024/12/7 20:35:32
    desc: 玩家控制
]] --- Player
local Player = IOC.InjectClass(Player_lua)

local speedExtra = 0.1
local flys = {}
local canFly

function Player:OnGetOrCreate(param)
    self.transform:SetParent(param.parent)
    self.data = param.data
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)
    self:GenerateFlys()
end

function Player:Move(x, y)
    UnityUtil.LocalMove(self.transform, x * self.data.speed * speedExtra, y * self.data.speed * speedExtra)
end

--- 生成飞行物
---@param count 数量
function Player:GenerateFlys()
    local everyAddEuler = 360 / self.data.flyCount
    local distance = 1

    for i = 1, self.data.flyCount, 1 do
        IOC.Inject(Fly_lua, {
            parent = self.FlyContainer,
            player = self,
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

function Player:FlyOver()
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

function Player:FixedUpdate()
    if canFly then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * 10 * self.data.flyModel.speed)
    end
end

function Player:OnOtherFlyEnter(fly, other)
    local otherPlayer = other.player
    if self.data.team == otherPlayer.data.team then
        return
    end

    -- 1.比较攻击力
    -- 2.攻击力相同，触发回震系数
    local thisFlyAttack = self.data.flyModel.attack
    local otherFlyAttack = otherPlayer.data.flyModel.attack
    if thisFlyAttack > otherFlyAttack then
        otherPlayer:DestroyFly(other)
    elseif otherFlyAttack > thisFlyAttack then
        self:DestroyFly(fly)
    else
        otherPlayer:DestroyFly(other)
        self:DestroyFly(fly)
    end
end

function Player:DestroyFly(fly)
    Factory.Take(fly)
    self.data.flyCount = self.data.flyCount - 1
end

return Player
