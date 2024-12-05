--Auto Generate
--生成全局资源映射，不用手写地址
--映射规则：
--key : 统一为 文件名_后缀，整个项目不要使用同名资源
--value : lua文件为路径用.分割，用以require，预制体路径用/分割，用来Addressables.LoadAsset
Resources = "Resources"
EditorSceneList = "EditorSceneList"
GamePanel_lua = "Assets.HotFixAssets.Game.GamePanel"
GamePanel_prefab = "Assets/HotFixAssets/Game/GamePanel.prefab"
image_jpg = "Assets/HotFixAssets/Login/image.jpg"
LoginPanel_lua = "Assets.HotFixAssets.Login.LoginPanel"
login_bg_jpg = "Assets/HotFixAssets/Login/login_bg.jpg"
LoginPanel_prefab = "Assets/HotFixAssets/Login/LoginPanel.prefab"
bg3_jpg = "Assets/HotFixAssets/Login/bg3.jpg"
init_lua = "Assets.HotFixAssets.AAAInit.init"
LuaDebug_lua = "Assets.HotFixAssets.AAAInit.LuaDebug"
BasePanel_prefab = "Assets/HotFixAssets/AABase/BasePanel.prefab"
AddressMap_lua = "Assets.HotFixAssets.AddressMap.AddressMap"
