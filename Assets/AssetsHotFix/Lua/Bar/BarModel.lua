--- author:author
--- create:2025/1/12 20:19:50
--- desc: 

---@class BarModel
local BarModel = IOC.InjectClass(BarModel_lua)

function BarModel:OnGet(param)
    self.Color = param.color
    self.MinValue = param.minValue or 0
    self.MaxValue = param.maxValue or 100
    self.Value = param.value or self.MaxValue
end

return BarModel