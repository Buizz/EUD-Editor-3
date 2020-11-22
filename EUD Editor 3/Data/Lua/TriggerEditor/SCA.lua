--[================================[
@Language.ko-KR
@Summary
[Slot] 슬롯의 데이터를 불러옵니다.
@Group
SCA
@param.Slot.Number


@Language.us-EN
@Summary
[Slot] 슬롯의 데이터를 불러옵니다.
@Group
SCA
@param.Slot.Number
]================================]
function SCALoad(Slot) --SCA/Number/[Slot] 슬롯의 데이터를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoad(".. Slot .. ")")
end

--[================================[
@Language.ko-KR
@Summary
[Slot] 슬롯의 데이터를 저장합니다.
@Group
SCA
@param.Slot.Number


@Language.us-EN
@Summary
[Slot] 슬롯의 데이터를 저장합니다.
@Group
SCA
@param.Slot.Number
]================================]
function SCASave(Slot) --SCA/Number/[Slot] 슬롯의 데이터를 저장합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaSave(".. Slot .. ")")
end

--[================================[
@Language.ko-KR
@Summary
시간 정보를 불러옵니다.
@Group
SCA


@Language.us-EN
@Summary
시간 정보를 불러옵니다.
@Group
SCA
]================================]
function SCALoadTime() --SCA//시간 정보를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadTime()")
end

--[================================[
@Language.ko-KR
@Summary
글로벌 변수를 불러옵니다.
@Group
SCA


@Language.us-EN
@Summary
글로벌 변수를 불러옵니다.
@Group
SCA
]================================]
function SCALoadGlobalData() --SCA//글로벌 변수를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadGlobal()")
end

--[================================[
@Language.ko-KR
@Summary
불러오기 완료를 확인합니다.
@Group
SCA


@Language.us-EN
@Summary
불러오기 완료를 확인합니다.
@Group
SCA
]================================]
function IsLoadComplete() --SCA//불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsLoadComplete()")
end

--[================================[
@Language.ko-KR
@Summary
저장 완료를 확인합니다.
@Group
SCA


@Language.us-EN
@Summary
저장 완료를 확인합니다.
@Group
SCA
]================================]
function IsSaveComplete() --SCA//저장 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsSaveComplete()")
end

--[================================[
@Language.ko-KR
@Summary
글로벌 변수의 불러오기 완료를 확인합니다.
@Group
SCA


@Language.us-EN
@Summary
글로벌 변수의 불러오기 완료를 확인합니다.
@Group
SCA
]================================]
function IsGlobalLoadComplete() --SCA//글로벌 변수의 불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsGlobalLoadComplete()")
end

--[================================[
@Language.ko-KR
@Summary
시간 정보의 불러오기 완료를 확인합니다.
@Group
SCA


@Language.us-EN
@Summary
시간 정보의 불러오기 완료를 확인합니다.
@Group
SCA
]================================]
function IsTimeLoadComplete() --SCA//시간 정보의 불러오기 완료를 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	echo("scalua.IsTimeLoadComplete()")
end







--[================================[
@Language.ko-KR
@Summary
[Index]번 글로벌 데이터의 값을 반환합니다.
@Group
SCA
@param.Index.Number


@Language.us-EN
@Summary
[Index]번 글로벌 데이터의 값을 반환합니다.
@Group
SCA
@param.Index.Number
]================================]
function SCAGetGlobalData(Index) --SCA/Number/[Index]번 글로벌 데이터의 값을 반환합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")

	echo("scalua.GlobalData(" .. Index .. ")")
end

--[================================[
@Language.ko-KR
@Summary
[Index]번 글로벌 데이터의 값이 [Comparison] [Value]인지 판단합니다.
@Group
SCA
@param.Index.Number
@param.Comparison.TrgComparison
@param.Value.Number


@Language.us-EN
@Summary
[Index]번 글로벌 데이터의 값이 [Comparison] [Value]인지 판단합니다.
@Group
SCA
@param.Index.Number
@param.Comparison.TrgComparison
@param.Value.Number
]================================]
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

--[================================[
@Language.ko-KR
@Summary
[DateType]을 반환합니다.
@Group
SCA
@param.DateType.DateType


@Language.us-EN
@Summary
[DateType]을 반환합니다.
@Group
SCA
@param.DateType.DateType
]================================]
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

--[================================[
@Language.ko-KR
@Summary
[DateType]이 [Comparison] [Value]인지 판단합니다.
@Group
SCA
@param.DateType.DateType
@param.Comparison.TrgComparison
@param.Value.Number


@Language.us-EN
@Summary
[DateType]이 [Comparison] [Value]인지 판단합니다.
@Group
SCA
@param.DateType.DateType
@param.Comparison.TrgComparison
@param.Value.Number
]================================]
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

--[================================[
@Language.ko-KR
@Summary
현재 날짜가 [Weekend]인지 확인합니다.
@Group
SCA
@param.Weekend.Weekend


@Language.us-EN
@Summary
현재 날짜가 [Weekend]인지 확인합니다.
@Group
SCA
@param.Weekend.Weekend
]================================]
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