--- author:author
--- create:2025/2/11 15:00:06
--- desc: 

---@class GlobalEnum
RoleState = 
{
    Virsual = 1, --get but not ready to play 
    Alive = 2,   --get and ready to play
    Disable = 3  --recycle or die
}

--- 角色基础属性类型
RoleBaseInfoType = 
{
    Attack = "Attack",
    Defense = "Defense",
    Speed = "Speed"
}