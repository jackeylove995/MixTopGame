--- author:author
--- create:2025/1/26 20:45:31
--- desc: 

---@class Bars
local Bars = IOC.InjectClass(Bars_lua)

function Bars:OnGet(param)
    self.player = param.player
    self.player.model:RegistHpChangeEvent(PackFunction(self,self.OnHpChange))
    self.barModel = param.barModel
    self.transform:SetParent(param.player.transform)
    self.transform:Standard()
    self.Hp:Init(self.barModel)
end

function Bars:OnHpChange(newValue)
    self.Hp:SetValue(newValue)
end

function Bars:Release()
    self.player.model:UnregisEvents()
    Factory:Take(self)
end

return Bars