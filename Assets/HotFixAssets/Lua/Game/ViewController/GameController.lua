--[[
    author:author
    create:2024/12/8 20:06:24
    desc: 游戏管理器
]]

local GameController = IOC.InjectClass(GameController_lua)


local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:OpenGame()
    CS.JoyStick.OnJoyStickMove("+", function(x, y)
        self:OnPlayMove(x, y)
    end)
    IOC.Inject(GamePanel_lua,  FullScreenPanelContainor)


    local mainPlayerData = GameDataManager.GetPlayerData()
    local mainPlayer = IOC.Inject(Player_lua, Sprite3DContainor)
    self.playerlua = mainPlayer
    Debug.Log(TMainCamera.name .. mainPlayer.transform.name)
    FollowUtil.FollowTargetXY(TMainCamera, mainPlayer.transform)
    mainPlayer:SetData(mainPlayerData)

    local botsData = GameDataManager.GetBotsData()
    for i, v in ipairs(botsData) do
        local botPlayer = IOC.Inject(Player_lua, Sprite3DContainor)
        botPlayer:SetData(v)
    end

    local s = IOC
    local ss = Factory
end

function GameController:OnPlayMove(x, y)    
    if self.playerlua then
        self.playerlua:Move(x, y)
    end
end

return GameController
