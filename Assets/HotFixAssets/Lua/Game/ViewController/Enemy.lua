--- author:author
--- create:2024/12/21 10:13:52
--- desc: 

---@class Enemy
local Enemy = IOC.InjectClass(Enemy_lua)

Enemy.tag = "Enemy"
local flys = {}
local canFly

function Enemy:OnGetOrCreate(param)
    self.transform:SetParent(param.parent)
    self.data = param.model
    UnityUtil.SetLocalPosition(self.transform, self.data.pos)   
end

function Enemy:Move(x, y)
    UnityUtil.LocalMove(self.transform, x * self.data:GetMoveSpeed(), y * self.data:GetMoveSpeed())
end

function Enemy:MoveTo(target)
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

return Enemy