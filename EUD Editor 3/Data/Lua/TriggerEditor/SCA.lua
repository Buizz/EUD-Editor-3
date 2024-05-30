--[================================[
@Language.ko-KR
@Summary
[ScriptName]을 실행하고 결과를 [ReturnIndex]에 반환합니다.
@Group
SCA
@param.ScriptName.SCAScript
실행할 스크립트의 이름입니다.
@param.ReturnIndex.Number
반환할 인덱스입니다.

@Language.en-US
@Summary
[ScriptName]을 실행하고 결과를 [ReturnIndex]에 반환합니다.
@Group
SCA
@param.ScriptName.SCAScript
실행할 스크립트의 이름입니다.
@param.ReturnIndex.Number
반환할 인덱스입니다.
]================================]
function SCARunScript(ScriptName, ReturnIndex, ...)
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	preDefine("const SCAArgArray = EUDArray(100);")
	beforeText("scalua.Exec();")

	argcount = 0
	for i,v in ipairs(arg) do
		echo("SCAArgArray[" .. (i - 1) .. "] = " .. tostring(v) .. ";")
		argcount = argcount + 1
	end

	echo("scalua.scaExecScript(".. ParseSCAScript(ScriptName) .. ", " .. ReturnIndex .. ", " .. argcount .. ", SCAArgArray)")
end



--[================================[
@Language.ko-KR
@Summary
Script 변수 [Variable]에 [Value]만큼 [Modifier]합니다.
@Group
SCA
@param.Variable.TrgString
변수 이름입니다.
@param.Value.Number
넣을 값 입니다.
@param.Modifier.TrgModifier
연산 종류 입니다.

@Language.en-US
@Summary
Script 변수 [Variable]에 [Value]만큼 [Modifier]합니다.
@Group
SCA
@param.Variable.TrgString
변수 이름입니다.
@param.Value.Number
넣을 값 입니다.
@param.Modifier.TrgModifier
연산 종류 입니다.
]================================]
function SCAWriteScriptVariable(Variable, Modifier, Value)
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")

	Modifier = ParseModifier(Modifier)

	echo(string.format("SetMemoryEPD(scalua.scf.SCAScriptVarEPD + %s, %s, %s)", ParseSCAScriptVariable(Variable), Modifier, Value))
end



--[================================[
@Language.ko-KR
@Summary
Script 변수 [Variable]를 읽습니다.
@Group
SCA
@param.Variable.TrgString
변수 이름입니다.

@Language.en-US
@Summary
Script 변수 [Variable]를 읽습니다.
@Group
SCA
@param.Variable.TrgString
변수 이름입니다.
]================================]
function SCAReadScriptVariable(Variable)
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")

	echo(string.format("dwread_epd(scalua.scf.SCAScriptVarEPD + %s)", ParseSCAScriptVariable(Variable)))
end



--[================================[
@Language.ko-KR
@Summary
[Slot] 슬롯의 데이터를 불러옵니다.
@Group
SCA
@param.Slot.Number


@Language.en-US
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


@Language.en-US
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
해당플레이어를 [BanType]으로 유즈맵에서 차단합니다.
@Group
SCA
@param.BanType.SCABanType


@Language.en-US
@Summary
해당플레이어를 [BanType]으로 유즈맵에서 차단합니다.
@Group
SCA
@param.BanType.SCABanType
]================================]
function SCABan(BanType)
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")
	
	bancode = {
		["OnlyBan"] = 0,
		["BanWithExit"] = 1
	}
	echo("scalua.scaBan(" + bancode[BanType] + ")")
end



--[================================[
@Language.ko-KR
@Summary
시간 정보를 불러옵니다.
@Group
SCA


@Language.en-US
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


@Language.en-US
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
시간 정보를 불러옵니다.
@Group
SCA


@Language.en-US
@Summary
시간 정보를 불러옵니다.
@Group
SCA
]================================]
function SCALoadTimeOnce() --SCA//시간 정보를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadTimeOnce()")
end



--[================================[
@Language.ko-KR
@Summary
글로벌 변수를 불러옵니다.
@Group
SCA


@Language.en-US
@Summary
글로벌 변수를 불러옵니다.
@Group
SCA
]================================]
function SCALoadGlobalDataOnce() --SCA//글로벌 변수를 불러옵니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")


	echo("scalua.scaLoadGlobalOnce()")
end



--[================================[
@Language.ko-KR
@Summary
불러오기 완료를 확인합니다.
@Group
SCA


@Language.en-US
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


@Language.en-US
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


@Language.en-US
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


@Language.en-US
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


@Language.en-US
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


@Language.en-US
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
	else
		str = Variable .. Comparison .. Value

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


@Language.en-US
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
	elseif DateType == "Timestamp" then
		Variable = "scalua.Timestamp()"
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


@Language.en-US
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
	elseif DateType == "Timestamp" then
		Variable = "scalua.Timestamp()"
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
현재 요일이 [Weekend]인지 확인합니다.
@Group
SCA
@param.Weekend.Weekend


@Language.en-US
@Summary
현재 요일이 [Weekend]인지 확인합니다.
@Group
SCA
@param.Weekend.Weekend
]================================]
function SCAWeek(Weekend) --SCA/Weekend/현재 요일이 [Weekend]인지 확인합니다.
	preDefine("import TriggerEditor.SCALuaWrapper as scalua;")
	beforeText("scalua.Exec();")
	
	Variable = "scalua.Week()"
	weekend = {
		["Monday"] = 0,
		["Tuesday"] = 1,
		["Wednesday"] = 2,
		["Thursday"] = 3,
		["Friday"] = 4,
		["Saturday"] = 5,
		["Sunday"] = 6
	}
	echo(Variable .. " == ")
	echo(weekend[Weekend])

	--_weekval = weekend[Weekend]
	--str = Variable .. " == " .. _weekval
	--echo(str)
end


