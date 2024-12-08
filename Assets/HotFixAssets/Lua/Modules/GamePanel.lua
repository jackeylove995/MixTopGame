--[[
    author:author
    create:2024/12/5 21:11:38
    desc: 游戏主页面
]]


function Start()
    Debug.ShowDebugMes("enter game")
    Receive("PlayerMove", OnPlayMove)
    LoadGameObject(Player_prefab, self.transform, OnPlayerCreate)  
    LoadGameObject(JoyStick_prefab,self.transform, OnJoyStickCreate)
end

function OnPlayerCreate(go, lua)
    playergo = go
    playerlua = lua
    StartDoing()
end

function OnJoyStickCreate(go, lua)
    joygo = go
    joylua = lua
    StartDoing()
end

function StartDoing()
    if joygo == nil or playergo == nil or start then
        return
    end
    start = true
end

function OnPlayMove(dir)    
    if start then
        playerlua.Move(dir)
    end
end
