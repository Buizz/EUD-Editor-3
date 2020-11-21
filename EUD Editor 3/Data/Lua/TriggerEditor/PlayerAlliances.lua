--[================================[
@Language.ko-KR
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계를 [AllyStatus]로 설정합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer
@param.AllyStatus.TrgAllyStatus


@Language.us-EN
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계를 [AllyStatus]로 설정합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer
@param.AllyStatus.TrgAllyStatus
]================================]
function SetPlayerAlliances(Player, DestPlayer, AllyStatus) --동맹/TrgPlayer,TrgPlayer,TrgAllyStatus/[Player]이 보는 [DestPlayer]와의 동맹 관계를 [AllyStatus]로 설정합니다.
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

--[================================[
@Language.ko-KR
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계가 [AllyStatus]인지 확인합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer
@param.AllyStatus.TrgAllyStatus


@Language.us-EN
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계가 [AllyStatus]인지 확인합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer
@param.AllyStatus.TrgAllyStatus
]================================]
function CurrentPlayerAlliances(Player, DestPlayer, AllyStatus) --동맹/TrgPlayer,TrgPlayer,TrgAllyStatus/[Player]이 보는 [DestPlayer]와의 동맹 관계가 [AllyStatus]인지 확인합니다.
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

--[================================[
@Language.ko-KR
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계를 반환합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer


@Language.us-EN
@Summary
[Player]이 보는 [DestPlayer]와의 동맹 관계를 반환합니다.
@Group
동맹
@param.Player.TrgPlayer
@param.DestPlayer.TrgPlayer
]================================]
function GetPlayerAlliances(Player, DestPlayer) --동맹/TrgPlayer,TrgPlayer/[Player]이 보는 [DestPlayer]와의 동맹 관계를 반환합니다.
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

--[================================[
@Language.ko-KR
@Summary
[Player]의 동맹 오프셋을 반환합니다.
@Group
동맹
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 동맹 오프셋을 반환합니다.
@Group
동맹
@param.Player.TrgPlayer
]================================]
function PlayerAlliancesEPD(Player) --동맹/TrgPlayer/[Player]의 동맹 오프셋을 반환합니다.
	Player = ParsePlayer(Player)

	if IsNumber(Player) then
		return string.format("EPD(0x%X)", 0x58D634 + 0xC * Player) 
	else
		return string.format("EPD(0x%X) + %s * 3", 0x58D634 ,Player) 
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 동맹 오프셋을 반환합니다.
@Group
동맹
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 동맹 오프셋을 반환합니다.
@Group
동맹
@param.Player.TrgPlayer
]================================]
function PlayerAlliances(Player) --동맹/TrgPlayer/[Player]의 동맹 오프셋을 반환합니다.
	Player = ParsePlayer(Player)

	if IsNumber(Player) then
		return string.format("0x%X", 0x58D634 + 0xC * Player) 
	else
		return string.format("0x%X + %s * 12", 0x58D634 ,Player) 
	end
end
