--[================================[
@Language.ko-KR
@Summary
화면 밝기를 [Amount]만큼 [Modifier]합니다.
@Group
시스템
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.us-EN
@Summary
화면 밝기를 [Amount]만큼 [Modifier]합니다.
@Group
시스템
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetLight(Modifier, Amount) --일반/TrgModifier,Number/화면 밝기를 [Amount]만큼 [Modifier]합니다.
    Modifier = ParseModifier(Modifier)
    Offset = LightOffset()
	echo(string.format("SetMemoryEPD(EPD(%s), %s, %s)", Offset, Modifier, Amount))
end

--[================================[
@Language.ko-KR
@Summary
화면 밝기의 값을 읽습니다.
@Group
시스템


@Language.us-EN
@Summary
화면 밝기의 값을 읽습니다.
@Group
시스템
]================================]
function GetLight() --일반//화면 밝기의 값을 읽습니다.
    Offset = LightOffset()
	echo(string.format("dwread_epd(EPD(%s))", Offset))
end

--[================================[
@Language.ko-KR
@Summary
화면 밝기의 주소를 반환합니다.
@Group
시스템


@Language.us-EN
@Summary
화면 밝기의 주소를 반환합니다.
@Group
시스템
]================================]
function LightOffset() --일반//화면 밝기의 주소를 반환합니다.
	return "0x657A9C"
end