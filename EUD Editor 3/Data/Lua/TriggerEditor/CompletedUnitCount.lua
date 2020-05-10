function SetCompletedUnitCount(Unit, Player, Modifier, Amount) -- TrgUnit,TrgPlayer,TrgModifier,Number/[Player]의 [Unit]의 완료된 유닛보유수를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    Unit = ParseUnit(Unit)
    OffsetEPD = CompletedUnitCountEPD(Unit, Player)


	echo(string.format("SetMemoryEPD(%s, %s, %s)", OffsetEPD, Modifier, Amount))
end
function GetCompletedUnitCount(Unit, Player) -- TrgUnit,TrgPlayer/[Player]의 [Unit]의 완료된 유닛보유수를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    OffsetEPD = CompletedUnitCountEPD(Unit, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end
function CompletedUnitCountEPD(Unit, Player) -- TrgUnit,TrgPlayer/[Player]의 [Unit]의 완료된 유닛보유수의 주소를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    Offset = 0x584DE4
	if IsNumber(Player) and IsNumber(Unit) then
		Offset = Offset + (Unit * 12 + Player) * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s * 12 + %s", Offset, Unit, Player)
	end
end