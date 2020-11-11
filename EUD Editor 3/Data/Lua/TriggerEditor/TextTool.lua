function Printf(Buffer, Text, Args) --텍스트출력/TrgString,FormatString,Arguments/[Text][Args]을 버퍼 [Buffer]를 사용해 출력합니다.
	preDefine("const " .. Buffer .. " = StringBuffer();")

	if Args == "" then
		stext = Buffer .. ".printf(\"" .. Text .. "\")"
	else
		stext = Buffer .. ".printf(\"" .. Text .. "\", " .. Args .. ")"
	end

	echo(stext)
end

function PrintfAt(Buffer, Line, Text, Args) --텍스트출력/TrgString,Number,FormatString,Arguments/[Text][Args]을 버퍼 [Buffer]를 사용해 [Line] 라인에 출력합니다.
	preDefine("const " .. Buffer .. " = StringBuffer();")
		
	if Args == "" then
		stext = Buffer .. ".printf(" .. Line .. ", \"" .. Text .. "\")"
	else
		stext = Buffer .. ".printf(" .. Line .. ", \"" .. Text .. "\", " .. Args .. ")"
	end

	echo(stext)
end

function SetTbl(TblID, Offset, Text, Args) --Tbl/Tbl,Number,FormatString,Arguments/[TblID]의 [Offset]위치에 [Text][Args]를 씁니다.
	if Args == "" then
		stext = "settblf(" .. TblID .. ", " .. Offset .. ", \"" .. Text .. "\")"
	else
		stext = "settblf(" .. TblID .. ", " .. Offset .. ", \"" .. Text .. "\", " .. Args .. ")"
	end
	echo(stext)
end

function ChangeTbl(TblID, Offset, Text, Args) --Tbl/Tbl,Number,FormatString,Arguments/[TblID]의 [Offset]위치를 [Text][Args]로 교체합니다.
	if Args == "" then
		stext = "settblf2(" .. TblID .. ", " .. Offset .. ", \"" .. Text .. "\")"
	else
		stext = "settblf2(" .. TblID .. ", " .. Offset .. ", \"" .. Text .. "\", " .. Args .. ")"
	end
	echo(stext)
end



function ErrorPrintf(Text, Args) --텍스트출력/FormatString,Arguments/[Text][Args]을 에러줄에 출력합니다.

	if Args == "" then
		stext = "eprintf(\"" .. Text .. "\")"
	else
		stext = "eprintf(\"" .. Text .. "\", " .. Args .. ")"
	end

	echo(stext)
end