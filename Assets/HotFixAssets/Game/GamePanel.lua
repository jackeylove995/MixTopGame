

function Start()
    Debug.ShowDebugMes("enter game")
    --UIUtil.Size(bg, 200, 200)
    DOTweenUtil.SetLoop(self:GetComponent("Image"):DOFade(0,1), -1)
    DOTweenUtil.SetLoop(bg:DOFade(0,1), -1)

    TestData()
end

function TestData()
    local data = newTable(GameData_lua)
    data:InitData(1,1)
    print(data.level .. data.coin)
end