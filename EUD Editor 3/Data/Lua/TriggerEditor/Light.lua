function SetLight(Modifier, Amount) -- TrgModifier,Number/화면 밝기를 [Amount]만큼 [Modifier]합니다.
    Modifier = ParseModifier(Modifier)
    Offset = LightOffset()
	echo(string.format("SetMemoryEPD(EPD(%s), %s, %s)", Offset, Modifier, Amount))
end
function GetLight() -- /화면 밝기의 값을 읽습니다.
    Offset = LightOffset()
	echo(string.format("dwread_epd(EPD(%s))", Offset))
end
function LightOffset() -- /화면 밝기의 주소를 반환합니다.
	return "0x657A9C"
end