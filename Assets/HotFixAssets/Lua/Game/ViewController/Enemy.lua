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
    UnityUtil.MoveToTargetBySpeed(self.transform, target.transform, self.data:GetMoveSpeed())
end

return Enemy