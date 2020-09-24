function Printf(Formatstring, ...) --텍스트출력/TrgString,*args/[Formatstring]을 StringBuffer을 사용해 출력합니다.
	bufferName = "tempStringBuffer"	

	preDefine("const " .. bufferName .. " = StringBuffer();")


	argText = ""
	for i=1, arg.n do
		argText = argText .. "," .. arg[i]
    end
	
	stext = bufferName .. ".printf(\"" .. Formatstring .. "\"" .. argText .. ")"

	echo(stext)
end

function PrintfAt(line,Formatstring, ...) --텍스트출력/Number,TrgString,*args/[Formatstring]을 StringBuffer을 사용해 [line]번 라인에 출력합니다.
	bufferName = "tempStringBuffer"	

	preDefine("const " .. bufferName .. " = StringBuffer();")


	argText = ""
	for i=1, arg.n do
		argText = argText .. "," .. arg[i]
    end
	
	stext = bufferName .. ".printf(" .. line .. ",\"" .. Formatstring .. "\"" .. argText .. ")"

	echo(stext)
end