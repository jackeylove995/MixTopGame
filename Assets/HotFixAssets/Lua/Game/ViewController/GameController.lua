--[[
    author:author
    create:2024/12/8 20:06:24
    desc: 游戏管理器
]]

local GameController = IOC.InjectClass(GameController_lua)

function GameController:OpenGame()
    CS.JoyStick.OnJoyStickMove("+", function(x, y)
        self:OnPlayMove(x, y)
    end)

    IOC.InjectNew(GamePanel_lua,  FullScreenPanelContainor)

    local player = IOC.InjectNew(Player_lua, Sprite3DContainor)
    self:OnPlayerCreate(player)
end


function GameController:OnPlayerCreate(player)
    self.playerlua = player
    UnityUtil.SetZ(player.transform, PlayerZDepth)
    FollowUtil.FollowTargetXY(TMainCamera, player.transform)
    player:GenerateFlys(3)
end

function GameController:OnPlayMove(x, y)    
    if self.playerlua then
        self.playerlua:Move(x, y)
    end
end

return GameController
