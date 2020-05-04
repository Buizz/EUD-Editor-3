function SetScore(Score, Player, Modifier, Amount) -- EUDScore,TrgPlayer,TrgModifier,Number/[Player]의 [Score]를 [Amount]만큼 [Modifier]합니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
	OffsetEPD = ScoreEPD(Score, Player)


	rstr = string.format("SetMemoryEPD(%s, %s, %s)",OffsetEPD, Modifier, Amount)
	echo(rstr)
end
function CurrentScore(Score, Player, Comparison, Amount) -- EUDScore,TrgPlayer,TrgComparison,Number/[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)
	OffsetEPD = ScoreEPD(Score, Player)


	rstr = string.format("MemoryEPD(%s, %s, %s)",OffsetEPD, Comparison, Amount)
	echo(rstr)
end
function GetScore(Score, Player) -- EUDScore,TrgPlayer/[Player]의 [Score] 값을 읽습니다.
	ScoreType = ParseEUDScore(ScoreType)
	Player = ParsePlayer(Player)
	OffsetEPD = ScoreEPD(Score, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end
function ScoreEPD(Score, Player) -- EUDScore,TrgPlayer/[Player]의 [Score] 주소를 반환합니다.
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