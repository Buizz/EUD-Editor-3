--[================================[
@Language.ko-KR
@Summary
[Variable]을 [Value]으로 [Modifier]합니다.
@Group
변수
@param.Variable.Variable
@param.Modifier.TrgModifier
@param.Value.Number


@Language.us-EN
@Summary
[Variable]을 [Value]으로 [Modifier]합니다.
@Group
변수
@param.Variable.Variable
@param.Modifier.TrgModifier
@param.Value.Number
]================================]
function SetVariable(Variable, Modifier, Value) --변수/Variable,TrgModifier,Number/[Variable]을 [Value]으로 [Modifier]합니다.
	Modifier = ParseModifier(Modifier)

	if Modifier == 7 then
		strStart, strEnd = string.find(Variable, "%.")
		str = ""
		if strStart == nil then
			str = Variable .. " = " .. Value
		else
			str = string.format("SetVariables(%s, %s)", Variable, Value);
		end
		echo(str)
	elseif Modifier == 8 then
		str = Variable .. " += " .. Value

	
		echo(str)
	elseif Modifier == 9 then
		str = Variable .. " -= " .. Value

	
		echo(str)
	end
end

--[================================[
@Language.ko-KR
@Summary
[Variable]이 [Comparison][Value]인지 판단합니다.
@Group
변수
@param.Variable.Variable
@param.Comparison.TrgComparison
@param.Value.Number


@Language.us-EN
@Summary
[Variable]이 [Comparison][Value]인지 판단합니다.
@Group
변수
@param.Variable.Variable
@param.Comparison.TrgComparison
@param.Value.Number
]================================]
function Variable(Variable, Comparison, Value) --변수/Variable,TrgComparison,Number/[Variable]이 [Comparison][Value]인지 판단합니다.
	Comparison = ParseComparison(Comparison)

	if Comparison == 0 then
		str = Variable .. " >= " .. Value

		echo(str)
	elseif Comparison == 1 then
		str = Variable .. " <= " .. Value

		echo(str)
	elseif Comparison == 10 then
		str = Variable .. " == " .. Value

		echo(str)
	end
end