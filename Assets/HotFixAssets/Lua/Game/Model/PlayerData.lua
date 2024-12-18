--- author:author
--- create:2024/12/12 21:51:20
--- desc: 一个玩家的数据（包含bot）
local PlayerData = IOC.InjectClass(PlayerData_lua)

function PlayerData:OnGetOrCreate(param)
    self.team = param.team
    self.pos = param.pos
    self.flyCount = param.flyCount
    self.roleConfig = param.roleConfig
    self.weaponConfig = param.weaponConfig
end

return PlayerData