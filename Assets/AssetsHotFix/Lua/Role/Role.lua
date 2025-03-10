--- author:author
--- create:2025/1/14 17:24:02
--- desc: 
---@class Role
local Role = IOC.InjectClass(Role_lua)

local SpriteAtlasFormat = "SpriteAtlas_PackSeparate/role_%s.spriteatlas";
--- 角色控制，控制一切玩家/怪物/Boss
--[[
    应包含
    方向：控制移动
    数据：可以获取速度，血量，技能，飞行道具等信息
    生成飞行物：根据数据生成
    被攻击：被攻击的效果

    基础动画：idle，run，skill，beAttack
]] --

--@region factory implementation
function Role:OnGet(param)
    self:ChangeState(RoleState.Virsual)
    self.transform:SetParent(param.parent)
    self.model = param.model
    self.transform:LocalPosition(self.model.Pos)
    self:UpdateAnimation()
    self.tag = "role"
end

function Role:OnRecycle()
    self:ChangeState(RoleState.Disable)
end
--@endregion

--@region State Control
function Role:ChangeState(state)
    self.State = state
    if state == RoleState.Alive then
        self.gameObject:SetActive(true)
    end

    if state == RoleState.Disable then
        self.gameObject:SetActive(false)
        if self.bars then 
            self.bars:Release()
        end
    end 
end

function Role:IsAlive()
    return self.State == RoleState.Alive
end

function Role:NotAlive()
    return self.State ~= RoleState.Alive
end
--@endregion

function Role:BindBars()
    if self.bars ~= nil then
        self.bars:OnGet({
            player = self,
            barModel = IOC.Inject(BarModel_lua, {
                color = Color.red,
                maxValue = self.model:GetHp()
            })
        })
        return
    end

    IOC.Inject(Bars_lua, {
        player = self,
        barModel = IOC.Inject(BarModel_lua, {
            color = Color.red,
            maxValue = self.model:GetHp()
        }, function(bars)
            self.bars = bars
        end)
    })
end

--@region init animation
function Role:UpdateAnimation()
    local fa = self.FrameAnimation
    fa:ClearAllAnimation()
    fa.transform:LocalPosition(0, self.model.AnimLocalPosY, 0)
    fa.transform:Scale(self.model.BaseAnimScale, self.model.BaseAnimScale, 1)
    AssetLoader.LoadSpriteAtlasAsync(string.format(SpriteAtlasFormat, self.model.RoleId),
        PackFunction(self, self.OnAnimSpritesLoad))
end

function Role:OnAnimSpritesLoad(spriteAltas)
    local sprites = spriteAltas:GetAllSprites()
    local anims = {}
    for i = 0, spriteAltas.spriteCount - 1, 1 do
        -- spirte名称是animName_animIndex(Clone)格式，例如idle_1(Clone)
        local sprite = sprites[i]
        local animInfo = string.Split(sprite.name, '_')
        local animName = animInfo[1]
        local animIndex = tonumber(string.RemoveLastChar(animInfo[2], 7))
        if anims[animName] == nil then
            anims[animName] = {}
        end
        anims[animName][animIndex] = sprite
    end

    for k, v in pairs(anims) do
        self.FrameAnimation:AddAnimation(k, v)
    end

    self:ChangeState(RoleState.Alive)
    
end

function Role:PlayAnim(animName, once)
    if once then
        self.FrameAnimation:PlayOnce(animName)
    else
        self.FrameAnimation:PlayLoop(animName)
    end
end
--@endregion

--@region mono
function Role:FixedUpdate()
    if IsNotEmpty(self.flys) then
        self.FlyContainer:Rotate(0, 0, Time.fixedDeltaTime * self.model:GetWeaponSpeed())
    end
end
--@endregion

--@region move by speed
function Role:Direction(x, y)
    if self:NotAlive() then return end
    if x == 0 and y == 0 then
        self:PlayAnim("idle")
        return
    end

    self:PlayAnim("run")
    self.FrameAnimation.transform:Euler(0, x > 0 and 0 or 180, 0)
    self.transform:LocalMove(x * self.model:GetMoveSpeed(), y * self.model:GetMoveSpeed(), 0)
end

function Role:MoveToPosition(v3)
    if self:NotAlive() then return end
    local pos = self.transform.position
    local distance = Vector3.Distance(v3, pos)
    if distance < 0.2 then
        self:PlayAnim("idle")
        return
    end
    self:PlayAnim("run")

    local moveLength = (v3 - pos).normalized * self.model:GetMoveSpeed()
    self.transform:WorldMove(moveLength)
    self.FrameAnimation.transform:Euler(0, moveLength.x > 0 and 0 or 180, 0)
end
--@endregion

--@region fly control
function Role:OnBallsClick(ballModels)
    self.flys = self.flys or {}

    local canFlyCount = self.model:GetMaxFlyCount() - #self.flys
    local flyCount 
    if #ballModels > canFlyCount then
        flyCount = canFlyCount
    else
        flyCount = #ballModels
    end

    local buffCount = #ballModels - flyCount
    
    local model = self.model
    model:SetIncreaseFactor(RoleBaseInfoType.Attack, "ball", ballModels[1].AttackIncrease or 1)
    model:SetIncreaseFactor(RoleBaseInfoType.Defense, "ball", ballModels[1].DefenseIncrease or 1)
    model:SetIncreaseFactor(RoleBaseInfoType.Speed, "ball", ballModels[1].SpeedIncrease or 1)
    
    self:CreateFly(flyCount)
end

function Role:CreateFly(flyCount) 
    for i = 1, flyCount, 1 do
        IOC.Inject(Fly_lua, {
            parent = self.FlyContainer,
            role = self,
            enter = PackFunction(self, self.OnMyFlyAttackOther)
        }, PackFunction(self, self.FlyEulerChange))
    end
end

function Role:FlyEulerChange(fly)
    table.insert(self.flys, fly)
    local flyCount = #self.flys
    local everyAddEuler = 360 / flyCount
    local distance = self.model.FlyDistance
    self.FlyContainer.eulerAngles = Vector3.zero
    for i = 1, flyCount, 1 do
        local hudu = (i * everyAddEuler * math.pi) / 180
        local x = math.sin(hudu) * distance
        local y = math.cos(hudu) * distance
        local fly = self.flys[i]
        -- 使物体的Y轴指向指定方向
        fly.transform.localRotation = Quaternion.Euler(0, 0, -i * everyAddEuler)
        fly.transform:LocalPosition(x, y, 0)
    end
end

function Role:GetTeamId()
    return self.model.Team
end

function Role:OnMyFlyAttackOther(myFly, other)
    if other:GetTeamId() ~= self:GetTeamId() then
        self:OnFlyWithOtherRole(myFly, other)
    end
end

function Role:OnFlyWithOtherRole(myFly, role)
    role:BeAttack(self.model:GetAttack(), self.transform.position)
end

function Role:DestroyFly(fly)
    Factory.Take(fly)
    table.RemoveByObj(self.flys, fly)
end
--@endregion

--@region 碰撞
function Role:OnOtherColliderEnter(other)
    if self:NotAlive() then return end

    if other.tag == "role" then
        if other:GetTeamId() ~= self:GetTeamId() and other.model.AttackPriority > self.model.AttackPriority then
            self:BeAttack(other.model:GetAttack(), other.transform.position)
        end
    end

    if other.tag == "fly" and other.role ~= self then
        other.role:BeAttack(self.model:GetAttack(), self.transform.position)
    end
end
--@endregion
function Role:BeAttack(attackNum, attackerPos)
    if self:NotAlive() or self.hurting then return end

    self.hurting = true
    Clock.DelayCall(0.2, function()
        self.hurting = false
    end)

    self:PlayAnim("hurt", true)
    self.model:ChangeHp(-attackNum)
    if self.model:GetHp() <= 0 then
        self:Die()
    end

    self.transform:WorldMove((self.transform.position - attackerPos).normalized)
end

function Role:Die()
    Factory.Take(self)
end

function Role:CurrentAnimName()
    return self.FrameAnimation.currentAnimation.name
end
return Role
