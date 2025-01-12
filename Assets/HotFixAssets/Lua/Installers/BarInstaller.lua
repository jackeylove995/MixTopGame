--- author:author
--- create:2025/1/12 20:21:09
--- desc: 

---@class BarInstaller
IOC.BindMonoClass(Bar_lua)

IOC.BindClass(BarModel_lua).FromFactory()

IOC.BindStartMethod(function()
    Log("BarInstaller Start")
end)