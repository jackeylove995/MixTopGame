--- author:author
--- create:2024/12/9 20:51:32
--- desc: 飞行物

--- Fly
local Fly = IOC.InjectClass(Fly_lua)

function Fly:OnGet(param)
    self.transform:SetParent(param.parent)
    self.gameObject:SetActive(true)
    self.player = param.player
    self.enter = param.enter
end

function Fly:OnOtherColliderEnter(other)
    self.enter(self, other)
end

function Fly:OnRecycle()
    self.gameObject:SetActive(false)
end

return Fly