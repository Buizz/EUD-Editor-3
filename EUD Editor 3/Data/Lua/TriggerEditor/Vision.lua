function SetVision(Player, DestPlayer, State) -- TrgPlayer,TrgPlayer,TrgSwitchState/[Player]에게 [DestPlayer]의 시야를 [State]합니다.
	State = ParseSwitchState(State)
	Player = ParsePlayer(Player)
	DestPlayer = ParsePlayer(DestPlayer)
	offsetEPD = VisionEPD(DestPlayer)


	if State == 2 then
		Amount = "0xFF"
	else
		Amount = "0x0"
	end

	if IsNumber(Player) then
		Mask = 2^Player
		rstr = string.format("SetMemoryXEPD(%s, SetTo, %s, %d)", offsetEPD, Amount, Mask)
	else
		preDefine("const VisionMask = EUDVArray(8)(list(1, 2, 4, 8, 16, 32, 64, 128));")
		rstr = string.format("SetMemoryXEPD(%s, SetTo, %s, VisionMask[%s])", offsetEPD, Amount, Player)
	end

	echo(rstr)
end
function GetVision(Player, DestPlayer) -- TrgPlayer,TrgPlayer/[Player]가 [DestPlayer]를 볼 수 있는지 확인합니다.
	-- DestPlayer 오프셋 + Player의 값
	Player = ParsePlayer(Player)
	DestPlayer = ParsePlayer(DestPlayer)
	offsetEPD = VisionEPD(DestPlayer)

	if IsNumber(Player) then
		Mask = 2^Player

		rstr = string.format("maskread_epd(%s, %d)", offsetEPD, Mask)
	else
		preDefine("const VisionMask = EUDVArray(8)(list(1, 2, 4, 8, 16, 32, 64, 128));")
		rstr = string.format("maskread_epd(%s, VisionMask[%s])", offsetEPD, Player)
	end

	echo(rstr)
end
function VisionEPD(Player) -- TrgPlayer/[Player]의 시야 오프셋을 반환합니다.
	Player = ParsePlayer(Player)

	if IsNumber(Player) then
		return string.format("EPD(0x%X)", 0x57F1EC + 0x4 * Player) 
	else
		return string.format("EPD(0x%X) + %s", 0x57F1EC ,Player) 
	end
end