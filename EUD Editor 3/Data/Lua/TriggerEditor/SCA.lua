function SCALoad(Slot, CompleteFunc) --SCA/Number,Function/[Slot] 슬롯의 데이터를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoad(".. Slot .. ", EUDFuncPtr(0, 0)(f_" .. funcname .. "))")
end

function SCASave(Slot, CompleteFunc) --SCA/Number,Function/[Slot] 슬롯의 데이터를 저장합니다. 저장이 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaSave(".. Slot .. ", EUDFuncPtr(0, 0)(f_" .. funcname .. "))")
end

function SCALoadTime(CompleteFunc) --SCA/Function/시간 정보를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoadTime(EUDFuncPtr(0, 0)(f_" .. funcname .. "))")
end

function SCALoadGlobalData(CompleteFunc) --SCA/Function/글로벌 변수를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoadGlobal(EUDFuncPtr(0, 0)(f_" .. funcname .. "))")
end

function SCAGetGlobalData(Index) --SCA/Number,TrgComparison,Number/[Index]번 글로벌 데이터의 값을 반환합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")

	echo("scaWrapper.GlobalData(" .. Index .. ")")
end

function SCAGlobalData(Index,Comparison,Value) --SCA/Number,TrgComparison,Number/[Index]번 글로벌 데이터의 값이 [Comparison] [Value]인지 판단합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")
	
    Comparison = ParseComparison(Comparison)
	Variable = "scaWrapper.GlobalData(" .. Index .. ")"
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

function SCAGetTime(DateType) --SCA/DateType/[DateType]을 반환합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")

	Variable = ""
	if DateType == "Year" then
		Variable = "scaWrapper.Year()"
	elseif DateType == "Month" then
		Variable = "scaWrapper.Month()"
	elseif DateType == "Day" then
		Variable = "scaWrapper.Day()"
	elseif DateType == "Hour" then
		Variable = "scaWrapper.Hour()"
	elseif DateType == "Min" then
		Variable = "scaWrapper.Min()"
	elseif DateType == "Week" then
		Variable = "scaWrapper.Week()"
	end
	echo(Variable)
end
function SCATime(DateType,Comparison,Value) --SCA/DateType,TrgComparison,Number/[DateType]이 [Comparison] [Value]인지 판단합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")
	
    Comparison = ParseComparison(Comparison)
	Variable = ""
	if DateType == "Year" then
		Variable = "scaWrapper.Year()"
	elseif DateType == "Month" then
		Variable = "scaWrapper.Month()"
	elseif DateType == "Day" then
		Variable = "scaWrapper.Day()"
	elseif DateType == "Hour" then
		Variable = "scaWrapper.Hour()"
	elseif DateType == "Min" then
		Variable = "scaWrapper.Min()"
	elseif DateType == "Week" then
		Variable = "scaWrapper.Week()"
	end

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
function SCAWeek(Weekend) --SCA/Weekend/현재 날짜가 [Weekend]인지 확인합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")
	
	Variable = "scaWrapper.Week()"
	weekend = {
		["월요일"] = 0,
		["화요일"] = 1,
		["수요일"] = 2,
		["목요일"] = 3,
		["금요일"] = 4,
		["토요일"] = 5,
		["일요일"] = 6
	}

	weekval = weekend[Weekend]
	str = Variable .. " == " .. weekval
	echo(str)
end