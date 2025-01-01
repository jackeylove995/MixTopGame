--- author:author
--- create:2024/12/7 20:35:32
--- desc: 玩家控制

---@class Player
local Player = IOC.InjectClass(Player_lua)
Player.tag = "Player"
local flys = {}
local canFly

function Player:OnGetOrCreate(param)
    self.transform:SetParent(param.parent)
    self.data = param.data
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)
    self:GenerateFlys()
end

function Player:BeginMove() 
    self.FrameAnimation:PlayLoop("walk")
end

function Player:EndMove() 
    self.FrameAnimation:PlayLoop("idle")
end

function Player:Move(x, y)
    UnityUtil.SetRotation(self.FrameAnimation.transform, 0 , x > 0 and 0 or 180, 0) 
    UnityUtil.LocalMove(self.transform, x * self.data:GetMoveSpeed(), y * self.data:GetMoveSpeed())
end

--- 生成飞行物
---@param count 数量
function Player:GenerateFlys()
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
end

function Player:FlyOver()
    local everyAddEuler = 360 / self.data.flyCount
    local distance = 1

    for i = 1, self.data.flyCount, 1 do
        local hudu = (i * everyAddEuler * math.pi) / 180
        local x = math.sin(hudu) * distance
        local y = math.cos(hudu) * distance
        local fly = flys[i]
        UnityUtil.SetPosition(fly.transform, x, y, FlyZDepth)
        -- 使物体的Y轴指向指定方向
        fly.transform.localRotation = Quaternion.Euler(0, 0, -i * everyAddEuler)
    end
    canFly = true
end

function Player:FixedUpdate()
    if canFly then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * self.data:GetWeaponSpeed())
    end
end

function Player:OnOtherFlyEnter(fly, other)
    if other.tag == "Player" then
        self:OnFlyWithPlayerCollider(fly, other.player)
    end

    if other.tag == "Enemy" then
        self:OnFlyWithEnemyCollider(fly, other)
    end
end

function Player:OnFlyWithPlayerCollider(fly, otherPlayer)
    local otherPlayer = otherPlayer.player
    if self.data.team == otherPlayer.data.team then
        return
    end

    -- 1.比较攻击力
    -- 2.攻击力相同，触发回震系数
    local thisFlyAttack = self.data:GetAttack()
    local otherFlyAttack = otherPlayer.data:GetAttack()
    if thisFlyAttack > otherFlyAttack then
        otherPlayer:DestroyFly(otherPlayer)
    elseif otherFlyAttack > thisFlyAttack then
        self:DestroyFly(fly)
    else
        otherPlayer:DestroyFly(other)
        self:DestroyFly(fly)
    end
end

function Player:OnFlyWithEnemyCollider(fly, enemy)
    print("飞行物撞击敌人")
end

function Player:DestroyFly(fly)
    Factory.Take(fly)
    self.data.flyCount = self.data.flyCount - 1
end

return Player
