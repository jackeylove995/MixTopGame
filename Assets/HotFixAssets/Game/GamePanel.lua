function Start()
    Debug.ShowDebugMes("enter game")
    -- UIUtil.Size(bg, 200, 200)
    DOTweenUtil.SetLoop(self:GetComponent("Image"):DOFade(0, 1), -1)
    DOTweenUtil.SetLoop(bg:DOFade(0, 1), -1)

    TestData()
end

function TestData()
    local myData = {}
    for i = 1, 10 do
        local data = newClass(GameData_lua)
        data:InitData(i, i)
        table.insert(myData, data)      
    end
    for k, data in pairs(myData) do       
        print(data.level .. data.coin .. data.Implement)
    end
end
