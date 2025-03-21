--- author:author
--- create:2024/12/9 20:51:32
--- desc: 飞行物

--- Fly
local Fly = IOC.InjectClass(Fly_lua)

function Fly:OnGet(param)
    self.transform:SetParent(param.parent)
    self.gameObject:SetActive(true)
    self.role = param.role
    self.enter = param.enter
    self.tag = "fly"
end

function Fly:OnOtherTriggerEnter(other)
    self.enter(self, other)
end

function Fly:OnRecycle()
    self.gameObject:SetActive(false)
end

return Fly