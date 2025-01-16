--- author:author
--- create:2024/12/7 17:37:54
--- desc: table操作
    
function table.Last(t)
    return t[#t]
end

function table.First(t)
    for k, v in pairs(t) do
        return v
    end
end

function table.RemoveByObj(table, obj)
    for i, v in ipairs(table) do
        if v == obj then
            table.remove(table, i)
            return
        end
    end
end

function table.ToString(tab, name, indent, depth)
    if tab == nil then
        return string.format("%s%s = nil", indent or "", name)
    end
    local ret, IND = {}, "\t"
    local lookup_table = {}
    local function _tostring(prf, tbl, ind, dep)
        table.insert(ret, ind .. prf .. "{")
        for k, v in pairs(tbl) do
            local key
            if type(k) == "string" then
                key = string.format("[\"%s\"]", tostring(k))
            else
                key = string.format("[%s]", tostring(k))
            end
            if type(v) == "table" then
                if rawget(v, "__dontrace") then
                    table.insert(ret, string.format("%s%s%s = [%s]{...}", ind, IND, key, tostring(v)))
                else
                    if lookup_table[v] then
                        table.insert(ret, string.format("%s%s%s = [%s]", ind, IND, key, tostring(v)))
                    else
                        if depth and dep > depth then
                            table.insert(ret, string.format("%s%s%s = [%s]{...}", ind, IND, key, tostring(v)))
                        else
                            lookup_table[v] = true
                            _tostring(string.format("%s = [%s]", key, tostring(v)), v, ind .. IND, dep + 1)
                        end
                    end
                end
            else
                table.insert(ret, string.format("%s%s%s = %s,", ind, IND, key, tostring(v)))
            end
        end
        table.insert(ret, ind .. "},")
    end
    _tostring(name and string.format("%s = [%s]", name, tostring(tab)) or "", tab, indent or "", 1)
    ret[#ret] = (indent or "") .. "}" -- 最后的'},'替换为'}'
    return table.concat(ret, "\n")
end
