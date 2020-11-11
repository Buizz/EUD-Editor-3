function SCALoad(Slot) --SCA/Number/[Slot] 슬롯의 데이터를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoad(".. Slot .. ")")
end

function SCASave(Slot) --SCA/Number/[Slot] 슬롯의 데이터를 저장합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaSave(".. Slot .. ")")
end

function SCALoadTime() --SCA//시간 정보를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadTime()")
end

function SCALoadGlobalData() --SCA//글로벌 변수를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadGlobal()")
end

function IsLoadComplete() --SCA//불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsLoadComplete()")
end

function IsSaveComplete() --SCA//저장 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsSaveComplete()")
end

function IsGlobalLoadComplete() --SCA//글로벌 변수의 불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsGlobalLoadComplete()")
end

function IsTimeLoadComplete() --SCA//시간 정보의 불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsTimeLoadComplete()")
end






function SCAGetGlobalData(Index) --SCA/Number,TrgComparison,Number/[Index]번 글로벌 데이터의 값을 반환합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")

	echo("scalua.GlobalData(" .. Index .. ")")
end

function SCAGlobalData(Index,Comparison,Value) --SCA/Number,TrgComparison,Number/[Index]번 글로벌 데이터의 값이 [Comparison] [Value]인지 판단합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")
	
    Comparison = ParseComparison(Comparison)
	Variable = "scalua.GlobalData(" .. Index .. ")"
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
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")

	Variable = ""
	if DateType == "Year" then
		Variable = "scalua.Year()"
	elseif DateType == "Month" then
		Variable = "scalua.Month()"
	elseif DateType == "Day" then
		Variable = "scalua.Day()"
	elseif DateType == "Hour" then
		Variable = "scalua.Hour()"
	elseif DateType == "Min" then
		Variable = "scalua.Min()"
	elseif DateType == "Week" then
		Variable = "scalua.Week()"
	end
	echo(Variable)
end
function SCATime(DateType,Comparison,Value) --SCA/DateType,TrgComparison,Number/[DateType]이 [Comparison] [Value]인지 판단합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")
	
    Comparison = ParseComparison(Comparison)
	Variable = ""
	if DateType == "Year" then
		Variable = "scalua.Year()"
	elseif DateType == "Month" then
		Variable = "scalua.Month()"
	elseif DateType == "Day" then
		Variable = "scalua.Day()"
	elseif DateType == "Hour" then
		Variable = "scalua.Hour()"
	elseif DateType == "Min" then
		Variable = "scalua.Min()"
	elseif DateType == "Week" then
		Variable = "scalua.Week()"
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
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")
	
	Variable = "scalua.Week()"
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