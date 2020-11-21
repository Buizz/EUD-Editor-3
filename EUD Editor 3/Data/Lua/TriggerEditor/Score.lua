--[================================[
@Language.ko-KR
@Summary
[Player]의 [Score]를 [Amount]만큼 [Modifier]합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Score]를 [Amount]만큼 [Modifier]합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetScore(Score, Player, Modifier, Amount) --일반/EUDScore,TrgPlayer,TrgModifier,Number/[Player]의 [Score]를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
	OffsetEPD = ScoreEPD(Score, Player)


	rstr = string.format("SetMemoryEPD(%s, %s, %s)",OffsetEPD, Modifier, Amount)
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentScore(Score, Player, Comparison, Amount) --일반/EUDScore,TrgPlayer,TrgComparison,Number/[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)
	OffsetEPD = ScoreEPD(Score, Player)


	rstr = string.format("MemoryEPD(%s, %s, %s)",OffsetEPD, Comparison, Amount)
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Score] 값을 읽습니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Score] 값을 읽습니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
]================================]
function GetScore(Score, Player) --일반/EUDScore,TrgPlayer/[Player]의 [Score] 값을 읽습니다.
	Player = ParsePlayer(Player)
	OffsetEPD = ScoreEPD(Score, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Score] 주소를 반환합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Score] 주소를 반환합니다.
@Group
스코어
@param.Score.EUDScore
@param.Player.TrgPlayer
]================================]
function ScoreEPD(Score, Player) --일반/EUDScore,TrgPlayer/[Player]의 [Score] 주소를 반환합니다.
	Player = ParsePlayer(Player)
	ScoreIndex = ParseEUDScore(Score)


	ScoreOffset = GetEUDScoreOffset(ScoreIndex)
	if IsNumber(Player) then
		ScoreOffset = ScoreOffset + Player * 4
		return string.format("EPD(0x%X)", ScoreOffset)
	else
		return string.format("EPD(%s) + %s", ScoreOffset, Player)
	end
end