function GetDeaths(Unit,Player) -- TrgUnit,TrgPlayer/[Player]의 [Unit] 데스값을 반환합니다.
	Unit = ParseUnit(Unit)
    Player = ParsePlayer(Player)

	str = string.format("dwread_epd(EPD(0x58A364 + (%s * 12 + %s) * 4))",Unit , Player)
	echo(str)
end