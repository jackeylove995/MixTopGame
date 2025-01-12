--- author:author
--- create:2024/12/10 14:37:21
--- desc: MonoBehaviour标记，区分Mono和普通类

---@class MonoBehaviour
local MonoBehaviour = Class("MonoBehaviour")

MonoBehaviour.isMonoBehaviour = true

return MonoBehaviour