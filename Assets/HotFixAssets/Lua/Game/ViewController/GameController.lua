--- author:author
--- create:2024/12/8 20:06:24
--- desc: 游戏管理器
local GameController = IOC.InjectClass(GameController_lua)

local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:FixedUpdate()
    if self.enemys and self.mainPlayer then
        for i, v in ipairs(self.enemys) do
            v:MoveTo(self.mainPlayer)
        end
    end
end

function GameController:OpenGame()
    MonoUtil.AddFixedUpdate("GameController", function()
        self:FixedUpdate()
    end)
    self:InitJoyStick()
    self:InitPanel()
    self:InitPlayer()
    self:StartWaveLoop()
end

--- 初始化摇杆
function GameController:InitJoyStick()
    CS.JoyStick.OnMove("+", function(x, y)
        if self.mainPlayer then
            self.mainPlayer:Move(x, y)
        end
    end)

    CS.JoyStick.OnBeginMove("+", function()
        if self.mainPlayer then
            self.mainPlayer:BeginMove()
        end
    end)

    CS.JoyStick.OnEndMove("+", function()
        if self.mainPlayer then
            self.mainPlayer:EndMove()
        end
    end)
end

--- 初始化游戏Panel
function GameController:InitPanel()
    self.gamePanel = IOC.Inject(GamePanel_lua, FullScreenPanelContainor)

    -- 每过几秒生成一个球.Name("CreateBall")
    Clock.FixTimeCall(3, true, function(count)
        print("asd")
        IOC.Inject(Ball_lua, {
            parent = self.gamePanel.BallContent,
            color = CS.UnityEngine.Color.red,
            clickFunc = function(ball)
                self:OnBallClick(ball)
            end
        })
    end)
end

function GameController:OnBallClick(ball)
    Factory.Take(ball)
end

--- 初始化玩家
function GameController:InitPlayer()
    local mainPlayerModel = GameDataManager:GetPlayerModel()
    IOC.Inject(Player_lua, {
        parent = Sprite3DContainor,
        data = mainPlayerModel
    }, function(mainPlayer)
        self.mainPlayer = mainPlayer
        FollowUtil.FollowTargetXY(TMainCamera, mainPlayer.transform)
    end)

end

--- 开始关卡波次轮替
function GameController:StartWaveLoop()
    if not GameDataManager:ToNextWave() then
        print("Game Over")
        return
    end

    -- 生成enemy
    local enemyModels = GameDataManager:GetCurrentWaveEnemyModels()
    --[[IOC.Inject(Enemy_lua, {
        parent = Sprite3DContainor,
        model = enemyModels[1]
    }, function(enemy)
        self:OnEnemyCreate(enemy, #enemyModels)
    end)]]

    for i, v in ipairs(enemyModels) do
        IOC.Inject(Enemy_lua, {
            parent = Sprite3DContainor,
            model = v
        }, function(enemy)
            self:OnEnemyCreate(enemy, #enemyModels)
        end)
    end

    local timeToNext = GameDataManager:GetTimeToNext()
    Clock.StartTimer(timeToNext, 0, -1, function(t)
        self.time = t
        self.gamePanel:SetTime(tostring(t))
        if self.time and self.time == 0 then
            self:StartWaveLoop()
        end
    end)
end

function GameController:CheckWaveOver()

end

function GameController:OnEnemyCreate(enemy, enemyCount)
    self.enemys = self.enemys or {}
    table.insert(self.enemys, enemy)
    if #self.enemys == enemyCount then
        Log("all enemy created")
    end
end

return GameController
