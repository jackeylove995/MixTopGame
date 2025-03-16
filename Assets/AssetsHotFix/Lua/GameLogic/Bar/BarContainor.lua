--- author:author
--- create:2025/1/12 20:21:09
--- desc: 

---@class BarContainor
ContainorBuilder.NewContainorWithAddress(BarContainor_lua)

ContainorBuilder.BindMonoClass(Bars_lua).FromFactory(Bars_prefab)
ContainorBuilder.BindMonoClass(Bar_lua)

ContainorBuilder.BindClass(BarModel_lua).FromFactory()

ContainorBuilder.BindStartMethod(function()
    Log("BarContainor Start")
end)

ContainorBuilder.Build()