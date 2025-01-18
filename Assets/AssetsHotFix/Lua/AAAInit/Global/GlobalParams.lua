--- author:author
--- create:2024/12/10 22:10:08
--- desc: 全局参数 
    
local globalParams = {}
globalParams.null = nil
local GameObject = CS.UnityEngine.GameObject
local Camera = CS.UnityEngine.Camera
globalParams.UIRoot = UIRoot or GameObject.Find("UIRoot").transform;
globalParams.FullScreenPanelContainor = FullScreenPanelContainor or GameObject.Find("FullScreenPanelContainor").transform;
globalParams.PopupPanelContainor = PopupPanelContainor or GameObject.Find("PopupPanelContainor").transform;
globalParams.Sprite3DContainor = Sprite3DContainor or GameObject.Find("Sprite3DContainor").transform;
globalParams.TMainCamera = TMainCamera or Camera.main.transform;

globalParams.PlayerZDepth = -1
globalParams.FlyZDepth = -2

return globalParams

