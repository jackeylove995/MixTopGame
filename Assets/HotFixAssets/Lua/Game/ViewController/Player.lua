--- author:author
--- create:2024/12/7 20:35:32
--- desc: 玩家控制

---@class Player
local Player = IOC.InjectClass(Player_lua)
Player.tag = "Player"
local flys = {}
local canFly

function Player:OnGet(param)
    self.transform:SetParent(param.parent)
    self.data = param.data
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)
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

function Player:CreateFly()
    IOC.Inject(Fly_lua, {
        parent = self.FlyContainer,
        player = self,
        enter = PackFunction(self, self.OnMyFlyAttackOther)
    }, PackFunction(self, self.flyEulerChange))
end

function Player:flyEulerChange(fly)
    table.insert(flys, fly)
    local flyCount = #flys
    local everyAddEuler = 360 / flyCount
    local distance = self.data:GetFlyDistance()
    canFly = false
    self.FlyContainer.eulerAngles = Vector3.zero
    for i = 1, flyCount, 1 do
        local hudu = (i * everyAddEuler * math.pi) / 180
        local x = math.sin(hudu) * distance
        local y = math.cos(hudu) * distance
        local fly = flys[i]
        -- 使物体的Y轴指向指定方向
        fly.transform.localRotation = Quaternion.Euler(0, 0, -i * everyAddEuler)
        UnityUtil.SetLocalPosition(fly.transform, x, y, FlyZDepth)       
    end
    canFly = true
end

function Player:FixedUpdate()
    if canFly then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * self.data:GetWeaponSpeed())
    end
end

function Player:OnMyFlyAttackOther(fly, other)
    --[[if other.tag == "Player" then
        self:OnFlyWithPlayerCollider(fly, other.player)
    end]]

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
    enemy:BeAttack(fly)
end

function Player:DestroyFly(fly)
    Factory.Take(fly)
    table.RemoveByObj(fly)
end

return Player
