--[[
    author:author
    create:2024/12/10 22:10:08
    desc: 全局参数
]]

local GameObject = CS.UnityEngine.GameObject
local Camera = CS.UnityEngine.Camera
UIRoot = GameObject.Find("UIRoot").transform;
FullScreenPanelContainor = GameObject.Find("FullScreenPanelContainor").transform;
PopupPanelContainor = GameObject.Find("PopupPanelContainor").transform;
Sprite3DContainor = GameObject.Find("Sprite3DContainor").transform;
TMainCamera = Camera.main.transform;

PlayerZDepth = -1
FlyZDepth = -2

