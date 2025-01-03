--- author:author
--- create:2025/1/3 16:06:38
--- desc: 
---@class Ball
local Ball = IOC.InjectClass(Ball_lua)

function Ball:Start()
    self.Btn.onClick:AddListener(function()
        self:OnClick()
    end)
end

function Ball:OnGetOrCreate(param)
    if not self.hasSetParent then
        self.transform:SetParent(param.parent)
    end
    self.param = param
    self.transform:SetParent(param.parent)
    self.transform:SetAsFirstSibling()
    self.Icon.color = param.color
end

function Ball:OnClick()
    self.param.clickFunc(self)
end

function Ball:OnRecycle()

end

return Ball
