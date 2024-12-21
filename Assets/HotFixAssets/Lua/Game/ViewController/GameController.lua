--- author:author
--- create:2024/12/8 20:06:24
--- desc: 游戏管理器
local GameController = IOC.InjectClass(GameController_lua)

local GameDataManager = IOC.Inject(GameDataManager_lua)

function GameController:Update()
    print("Asd")
end

function GameController:OpenGame()
    MonoUtil.AddUpdate("GameController", function()
        self:Update()
    end)
    self:InitJoyStick()
    self:InitPanel()
    self:InitPlayer()
    self:StartWaveLoop()
end

--- 初始化摇杆
function GameController:InitJoyStick()
    CS.JoyStick.OnJoyStickMove("+", function(x, y)
        if self.mainPlayer then
            self.mainPlayer:Move(x, y)
        end
    end)
end

--- 初始化游戏Panel
function GameController:InitPanel()
    IOC.Inject(GamePanel_lua, FullScreenPanelContainor)
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
    self.levelModel = GameDataManager:GetLevelModel()
    self.waveModel = self.levelModel:GetNextWave()

    local enemyId, enemyCount = self.waveModel:GetEnemyIdAndCount()

    for i = 1, enemyCount, 1 do
        local enemyModel = IOC.Inject(EnemyModel_lua, {
            config = GameDataManager.GetEnemyConfigById(enemyId),
            increaseConfig = self.waveModel:GetIncreaseConfig()
        })
        IOC.Inject(Enemy_lua, {
            parent = Sprite3DContainor,
            model = enemyModel
        }, function(enemy)
            self:OnEnemyCreate(enemy, enemyCount)
        end)
    end

    -- 生成enemy
    local timeToNext = self.waveModel:GetTimeToNext()
    Clock.StartTimer(timeToNext, 0, -1, function(t)
        Log("Time to " .. tostring(t))
    end)
end

function GameController:OnEnemyCreate(enemy, enemyCount)
    self.enemys = self.enemys or {}
    table.insert(self.enemys, enemy)
    if #self.enemys == enemyCount then
        Log("all enemy created")
    end
end

return GameController
