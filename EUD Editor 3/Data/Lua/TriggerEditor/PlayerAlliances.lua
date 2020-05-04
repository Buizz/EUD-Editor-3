function SetPlayerAlliances(Player, DestPlayer, AllyStatus) -- TrgPlayer,TrgPlayer,TrgAllyStatus/[Player]이 보는 [DestPlayer]와의 동맹 관계를 [AllyStatus]로 설정합니다.
	Player = ParsePlayer(Player)
	DestPlayer = ParsePlayer(DestPlayer)
	AllyStatus = ParseAllyStatus(AllyStatus)
	offsetEPD = PlayerAlliancesEPD(Player)

	if IsNumber(DestPlayer) then
		Mod = DestPlayer % 4
		RPlayer = DestPlayer - Mod
		RPlayer = RPlayer / 4

		if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end

    	if IsNumber(AllyStatus) then
			rstr = string.format("SetMemoryXEPD(%s + %s, SetTo, %s, %s)", offsetEPD, RPlayer, AllyStatus * math.pow(256, Mod), Mask)
		else
			rstr = string.format("SetMemoryXEPD(%s + %s, SetTo, %s, %s)", offsetEPD, RPlayer, AllyStatus .. " * " .. math.pow(256, Mod), Mask)
    	end


	else
		offset = PlayerAlliances(Player)
		rstr = string.format("bwrite(%s + %s, %s)", offset, DestPlayer, AllyStatus)
	end

	echo(rstr)
end
function CurrentPlayerAlliances(Player, DestPlayer, AllyStatus) -- TrgPlayer,TrgPlayer,TrgAllyStatus/[Player]이 보는 [DestPlayer]와의 동맹 관계가 [AllyStatus]인지 확인합니다.
	Player = ParsePlayer(Player)
	DestPlayer = ParsePlayer(DestPlayer)
	AllyStatus = ParseAllyStatus(AllyStatus)
	offsetEPD = PlayerAlliancesEPD(Player)

	if IsNumber(DestPlayer) then
		Mod = DestPlayer % 4
		RPlayer = DestPlayer - Mod
		RPlayer = RPlayer / 4

		if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end

    	if IsNumber(AllyStatus) then
			rstr = string.format("MemoryXEPD(%s + %s, Exactly, %s, %s)", offsetEPD, RPlayer, AllyStatus * math.pow(256, Mod), Mask)
		else
			rstr = string.format("MemoryXEPD(%s + %s, Exactly, %s, %s)", offsetEPD, RPlayer, AllyStatus .. " * " .. math.pow(256, Mod), Mask)
    	end

	else
		offset = PlayerAlliances(Player)
		rstr = string.format("bread(%s + %s) == %s", offset, DestPlayer, AllyStatus)
	end

	echo(rstr)
end
function GetPlayerAlliances(Player, DestPlayer) -- TrgPlayer,TrgPlayer/[Player]이 보는 [DestPlayer]와의 동맹 관계를 반환합니다.
	Player = ParsePlayer(Player)
	DestPlayer = ParsePlayer(DestPlayer)
	offsetEPD = PlayerAlliancesEPD(Player)

	if IsNumber(DestPlayer) then
		Mod = DestPlayer % 4
		RPlayer = DestPlayer - Mod
		RPlayer = RPlayer / 4

		rstr = string.format("bread_epd(%s + %s, %s)", offsetEPD, RPlayer, Mod)
	else
		offset = PlayerAlliances(Player)
		rstr = string.format("bread(%s + %s)", offset, DestPlayer)
	end

	echo(rstr)
end
function PlayerAlliancesEPD(Player) -- TrgPlayer/[Player]의 시야 오프셋을 반환합니다.
	Player = ParsePlayer(Player)

	if IsNumber(Player) then
		return string.format("EPD(0x%X)", 0x58D634 + 0xC * Player) 
	else
		return string.format("EPD(0x%X) + %s * 3", 0x58D634 ,Player) 
	end
end
function PlayerAlliances(Player) -- TrgPlayer/[Player]의 시야 오프셋을 반환합니다.
	Player = ParsePlayer(Player)

	if IsNumber(Player) then
		return string.format("0x%X", 0x58D634 + 0xC * Player) 
	else
		return string.format("0x%X + %s * 12", 0x58D634 ,Player) 
	end
end
 --[[0x0058D634 + 0xC * n   (n은 플레이어넘버)

한 플레이어당 12개의 바이트가 주어지는데 앞에서부터
순서대로 다른 플레이어(P1~P12)와의 동맹 현황을 저장한다.

Value
0 = Enemy(적군)
1 = Ally(동맹)
2 = Ally Victory(동맹승리)

※동맹 트리거와는 달리 이 오프셋을 EUD실행부로 조작할 경우
자기 자신을 적으로 인식하도록 만들 수 있다.

※자신 플레이어의 동맹 Value가 2일 경우 다른 동맹 Value도
0이 아니라면 2로 변경된다.]]