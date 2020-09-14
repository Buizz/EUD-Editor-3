function SCALoad(Slot, CompleteFunc) --SCA/Number,Function/[Slot]의 데이터를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoad(".. Slot .. ", EUDFuncPtr(0, 0)(f_" .. funcname .. "))");
end
function SCASave(Slot, CompleteFunc) --SCA/Number,Function/[Slot]의 데이터를 저장합니다. 저장이 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaSave(".. Slot .. ", EUDFuncPtr(0, 0)(f_" .. funcname .. "))");
end
function SCATime(CompleteFunc) --SCA/Function/시간 정보를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoadTime(EUDFuncPtr(0, 0)(f_" .. funcname .. "))");
end
function SCAGlobalData(CompleteFunc) --SCA/Function/글로벌 변수를 불러옵니다. 불러오기가 완료되면 [CompleteFunc]를 호출합니다.
	preDefine("import TriggerEditor.SCAWrapper as scaWrapper;")
	beforeText("scaWrapper.Exec();")


	local funcname = string.match(CompleteFunc, "%a+")
		
	echo("scaWrapper.scaLoadGlobal(EUDFuncPtr(0, 0)(f_" .. funcname .. "))");
end