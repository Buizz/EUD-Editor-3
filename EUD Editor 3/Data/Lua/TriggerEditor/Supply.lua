function SetSupply(SupplyType, Player, Modifier, Amount) -- SupplyType,TrgPlayer,TrgModifier,Number/[Player]의 [SupplyType]를 [Amount]만큼 [Modifier]합니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
	OffsetEPD = SupplyEPD(SupplyType, Player)


	rstr = string.format("SetMemoryEPD(%s, %s, %s)",OffsetEPD, Modifier, Amount)
	echo(rstr)
end
function CurrentSupply(SupplyType, Player, Comparison, Amount) -- SupplyType,TrgPlayer,TrgComparison,Number/[Player]의 [SupplyType]가 [Comparison] [Amount]인지 확인합니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)
	OffsetEPD = SupplyEPD(SupplyType, Player)


	rstr = string.format("MemoryEPD(%s, %s, %s)",OffsetEPD, Comparison, Amount)
	echo(rstr)
end
function GetSupply(SupplyType, Player) -- SupplyType,TrgPlayer/[Player]의 [SupplyType] 값을 읽습니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
	OffsetEPD = SupplyEPD(SupplyType, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end
function SupplyEPD(SupplyType, Player) -- SupplyType,TrgPlayer/[Player]의 [SupplyType] 주소를 반환합니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
	ScoreOffset = GetEUDScoreOffset(ScoreType)
	if IsNumber(Player) then
		ScoreOffset = ScoreOffset + Player * 4
		return string.format("EPD(0x%X)", ScoreOffset)
	else
		return string.format("EPD(%s) + %s", ScoreOffset, Player)
	end
end