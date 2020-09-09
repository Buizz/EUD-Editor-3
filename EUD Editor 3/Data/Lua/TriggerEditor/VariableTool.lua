function SetVariable(Variable, Value) -- Variable,Number/[Variable]을 [Number]으로 대입합니다.
	strStart, strEnd = string.find(Variable, "%.")
	str = ""
	if strStart == nil then
	    str = Variable .. " = " .. Value
	else
	    str = string.format("SetVariables(%s, %s)", Variable, Value);
	end
	echo(str)
end
function AddVariable(Variable, Value) -- Variable,Number/[Variable]을 [Number]만큼 더합니다.
	str = Variable .. " += " .. Value
	echo(str)
end
function SubtractVariable(Variable, Value) -- Variable,Number/[Variable]을 [Number]만큼 뺍니다.
	str = Variable .. " -= " .. Value
	echo(str)
end