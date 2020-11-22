--[================================[
@Language.ko-KR
@Summary
[Player]의 [Number]번 선택유닛 오프셋을 반환합니다.
@Group
일반
@param.Player.TrgPlayer
@param.Number.Number


@Language.us-EN
@Summary
[Player]의 [Number]번 선택유닛 오프셋을 반환합니다.
@Group
일반
@param.Player.TrgPlayer
@param.Number.Number
]================================]
function SeletionEPD(Player, Number) --일반/TrgPlayer,Number/[Player]의 [Number]번 선택유닛 오프셋을 반환합니다.
	Player = ParsePlayer(Player)
	if IsNumber(Player) then
		return string.format("EPD(0x%X) + %s", 0x6284E8 + 0x30 * Player, Number) 
	else
		return string.format("EPD(0x%X) + %s * 0xC + %s", 0x6284E8 ,Player, Number) 
	end
end