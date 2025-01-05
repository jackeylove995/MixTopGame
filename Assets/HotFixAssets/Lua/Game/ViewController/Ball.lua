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
    self.gameObject:SetActive(true)
    self.transform:SetParent(param.parent)
    self.transform:SetAsFirstSibling()
    self.model = param.model
    
    if self.model.AttackIncrease ~=nil then
        self.Icon.color = CS.UnityEngine.Color.red
    elseif self.model.DefenceIncrease ~= nil then
        self.Icon.color = CS.UnityEngine.Color.yellow
    else
        self.Icon.color = CS.UnityEngine.Color.blue
    end
end

function Ball:OnClick()
    self.param.clickFunc(self)
end

function Ball:OnRecycle()
    self.gameObject:SetActive(false)
end

return Ball
