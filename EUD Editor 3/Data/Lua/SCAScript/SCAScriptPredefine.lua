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
function sca_on_start_script(ScriptName, ReturnIndex, ...)
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

