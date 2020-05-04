function SetKills(Player, Modifier, Amount, Unit) -- TrgPlayer,TrgModifier,Number,TrgUnit/[Player]의 [Unit]이 죽은 수를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    Unit = ParseUnit(Unit)
    OffsetEPD = KillsEPD(Unit, Player)


	echo(string.format("SetMemoryEPD(%s, %s, %s)", OffsetEPD, Modifier, Amount))
end
function GetKills(Unit, Player) -- TrgUnit,TrgPlayer/[Player]의 [Unit]이 죽은 수를 읽습니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    OffsetEPD = KillsEPD(Unit, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end
function KillsEPD(Unit, Player) -- TrgUnit,TrgPlayer/[Player]의 [Unit]이 죽은 수의 주소를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)

    Offset = 0x5878A4
	if IsNumber(Player) and IsNumber(Unit) then
		Offset = Offset + (Unit * 12 + Player) * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s * 12 + %s", Offset, Unit, Player)
	end
end