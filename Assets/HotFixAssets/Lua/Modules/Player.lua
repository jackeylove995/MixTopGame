--[[
    author:author
    create:2024/12/7 20:35:32
    desc: 玩家控制
]]

--- Player

local speed = 5

function Move(dir)
    Debug.Log("玩家向"..dir.x .." " .. dir.y .. "方向移动")
    UIUtil.LocalMove(self.transform, dir.x * speed , dir.y * speed)
end

--- Use this for initialization
function Start()
    
end

--- Update is called once per frame
function Update()
    
end

function OnDestroy()
    
end
