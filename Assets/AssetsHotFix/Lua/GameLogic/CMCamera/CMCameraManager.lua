--- author:author
--- create:2025/3/16 12:29:04
--- desc: 
---@class CMCameraManager
local CMCameraManager = Class(CMCameraManager_lua)
local CinemachineVirtualCamera = CS.Cinemachine.CinemachineVirtualCamera

function CMCameraManager:VCMLookTo(trans)
    if self.vcam == nil then
        self.vcam = CS.UnityEngine.GameObject.Find("VCM_3rd"):GetComponent(typeof(CinemachineVirtualCamera))
    end
    self.vcam.m_LookAt = trans
    self.vcam.m_Follow = trans
end

return CMCameraManager
