--- author:author
--- create:2024/12/7 17:34:45
--- desc: string操作
function string.split(inputstr, sep)
    if inputstr == nil or inputstr == "" then
        return ""
    end
    if sep == nil then
        sep = "%s"
    end
    local t = {} -- 用来存放分割后的结果
    for str in string.gmatch(inputstr, "([^" .. sep .. "]+)") do
        table.insert(t, str)
    end
    return t
end
