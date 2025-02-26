--- author:author
--- create:2024/12/19 10:34:52
--- desc: 

---@class LevelModel
local LevelModel = IOC.InjectClass(LevelModel_lua)

local WavesConfig = IOC.Inject(LevelConfig_Wave_lua)
local IncreaseConfig = IOC.Inject(IncreaseConfig_lua)

--配置信息: 1:levelWaveConfigId-增幅id,...
function LevelModel:SetAndResolveConfig(config)
    local waves = string.Split(config, ",")
    self.waveModels = {}
    self.maxWaveIndex = 1
    for i, v in ipairs(waves) do
        local oneWaveConfig = string.Split(v, ":")
        local waveId = tonumber(oneWaveConfig[1])
        local waveModelOrigin = oneWaveConfig[2]

        local waveModelConfigs = string.Split(waveModelOrigin, "-")
        local waveConfigId = tonumber(waveModelConfigs[1])
        local waveIncreaseConfigId = tonumber(waveModelConfigs[2])
        local waveModel = IOC.Inject(WaveModel_lua)
        waveModel:Init(WavesConfig[waveConfigId] , IncreaseConfig[waveIncreaseConfigId])

        self.waveModels[waveId] = waveModel
        self.maxWaveIndex = waveId
    end
    self.waveIndex = 1
    self.lastWave = nil
end

--- 得到下一个波次信息
function LevelModel:GetNextWave()

    --如果上一个波次为nil，说明是刚开始，返回第一个
    if self.lastWave == nil then
        self.lastWave = self.waveModels[self.waveIndex]
        return self.waveModels[self.waveIndex]
    end
    --最后一个波次
    if self.maxWaveIndex == self.waveIndex then
        return nil
    end

    self.waveIndex = self.waveIndex + 1

    --如果波次找不到，那么沿用上次的
    if self.waveModels[self.waveIndex] == nil then
        return self.lastWave
    end

    --正常返回波次
    return self.waveModels[self.waveIndex] 
end

return LevelModel