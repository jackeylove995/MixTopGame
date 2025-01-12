--- author:author
--- create:2024/12/21 10:13:52
--- desc: 

---@class Enemy
local Enemy = IOC.InjectClass(Enemy_lua)

Enemy.tag = "Enemy"
local flys = {}
local canFly

function Enemy:OnGet(param)
    self.gameObject:SetActive(true)
    self.transform:SetParent(param.parent)
    self.data = param.model
    self.die = false
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)   
end

function Enemy:Move(x, y)
    UnityUtil.LocalMove(self.transform, x * self.data:GetMoveSpeed(), y * self.data:GetMoveSpeed())
end

function Enemy:MoveTo(target)
    if self.die then
        return
    end
    local distance = Vector3.Distance(target.transform.position, self.transform.position)
    if distance < 0.2 then
        self.FrameAnimation:PlayLoop("idle")
        return
    end
    UnityUtil.MoveToTargetBySpeed(self.transform, target.transform, self.data:GetMoveSpeed())
    self.FrameAnimation:PlayLoop("run")
    local x = target.transform.position.x > self.transform.position.x and 1 or -1
    UnityUtil.SetRotation(self.FrameAnimation.transform, 0 , x > 0 and 0 or 180, 0)
end

function Enemy:BeAttack(fly)
    if self.attack then
        return
    end
    self.attack = true
    Clock.DelayCall(0.2,function()
        self.attack = false
    end)
    
    self.FrameAnimation:PlayOnce("hurt")
    self.data.hp = self.data.hp - fly.player.data:GetAttack()
    if self.data.hp <= 0 then
        self:Die()
    end
    UnityUtil.BackMove(self.transform, fly.player.transform)
end

function Enemy:OnRecycle()
    self.die = true
    self.gameObject:SetActive(false)
end

function Enemy:Die()
    Factory.Take(self)
end

return Enemy