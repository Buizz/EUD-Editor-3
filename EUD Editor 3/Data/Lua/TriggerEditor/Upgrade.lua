--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 현재값을 [Amount]만큼 [Modifier]합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 현재값을 [Amount]만큼 [Modifier]합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetUpgrade(Upgrade, Player, Modifier, Amount) --업그레이드/Upgrade,TrgPlayer,TrgModifier,Number/[Player]의 [Upgrade]의 현재값을 [Amount]만큼 [Modifier]합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)


	offset = UpgradeOffset(Upgrade, Player)

	if IsNumber(offset) then
		Mod = offset % 4
		ROffset = offset - Mod


    	if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end


    	if IsNumber(Amount) then
    		rstr = string.format("SetMemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Modifier, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Modifier, Amount .. " * " .. math.pow(256, Mod), Mask)
    	end

	else
		if Modifier == 7 then
			rstr = string.format("bwrite(%s, %s)", offset, Amount)
		elseif Modifier == 8 then
			rstr = string.format("bwrite(%s, bread(%s) + %s)", offset, offset, Amount)
		elseif Modifier == 9 then
			rstr = string.format("bwrite(%s, bread(%s) - %s)", offset, offset, Amount)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 현재값이 [Comparison] [Amount]인지 확인합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 현재값이 [Comparison] [Amount]인지 확인합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentUpgrade(Upgrade, Player, Comparison, Amount) --업그레이드/Upgrade,TrgPlayer,TrgComparison,Number/[Player]의 [Upgrade]의 현재값이 [Comparison] [Amount]인지 확인합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)


	offset = UpgradeOffset(Upgrade, Player)

	if IsNumber(offset) then
		Mod = offset % 4
		ROffset = offset - Mod


    	if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end


    	if IsNumber(Amount) then
    		rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Comparison, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Comparison, Amount .. " * " .. math.pow(256, Mod), Mask)
    	end

	else
		if Comparison == 0 then
			rstr = string.format("bread(%s) >= %s", offset, Amount)
		elseif Comparison == 1 then
			rstr = string.format("bread(%s) <= %s", offset, Amount)
		elseif Comparison == 10 then
			rstr = string.format("bread(%s) == %s", offset, Amount)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 현재값을 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 현재값을 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
]================================]
function GetUpgrade(Upgrade, Player) --업그레이드/Upgrade,TrgPlayer/[Player]의 [Upgrade]의 현재값을 반환합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)

	offset = UpgradeOffset(Upgrade, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		rstr = string.format("bread(%s)", offset)
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
]================================]
function UpgradeOffset(Upgrade, Player) --업그레이드/Upgrade,TrgPlayer/[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
	--일반/58D2B0 0 ~ 45
	--일반/58F32C 46
	Upgrade = ParseUpgrades(Upgrade) + 0 
	Player = ParsePlayer(Player)

	Size = 0
	if Upgrade <= 45 then
		offset = 0x58D2B0
		Size = 46
	else
		offset = 0x58F32C
		Size = 20
	end
	if IsNumber(Player) then
		offset = offset + Player * Size + Upgrade
		return offset
	else
		offset = string.format("0x%X + %s * %s + %s", offset, Player, Size, Upgrade)
		return offset
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 최대값을 [Amount]만큼 [Modifier]합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 최대값을 [Amount]만큼 [Modifier]합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetUpgradeMax(Upgrade, Player, Modifier, Amount) --업그레이드/Upgrade,TrgPlayer,TrgModifier,Number/[Player]의 [Upgrade]의 최대값을 [Amount]만큼 [Modifier]합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)


	offset = UpgradeOffsetMax(Upgrade, Player)

	if IsNumber(offset) then
		Mod = offset % 4
		ROffset = offset - Mod


    	if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end


    	if IsNumber(Amount) then
    		rstr = string.format("SetMemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Modifier, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Modifier, Amount .. " * " .. math.pow(256, Mod), Mask)
    	end

	else
		if Modifier == 7 then
			rstr = string.format("bwrite(%s, %s)", offset, Amount)
		elseif Modifier == 8 then
			rstr = string.format("bwrite(%s, bread(%s) + %s)", offset, offset, Amount)
		elseif Modifier == 9 then
			rstr = string.format("bwrite(%s, bread(%s) - %s)", offset, offset, Amount)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 최대값이 [Comparison] [Amount]인지 확인합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 최대값이 [Comparison] [Amount]인지 확인합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentUpgradeMax(Upgrade, Player, Comparison, Amount) --업그레이드/Upgrade,TrgPlayer,TrgComparison,Number/[Player]의 [Upgrade]의 최대값이 [Comparison] [Amount]인지 확인합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)


	offset = UpgradeOffsetMax(Upgrade, Player)

	if IsNumber(offset) then
		Mod = offset % 4
		ROffset = offset - Mod


    	if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end


    	if IsNumber(Amount) then
    		rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Comparison, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("MemoryXEPD(EPD(0x%X), %s, 0x%X, %s)", ROffset, Comparison, Amount .. " * " .. math.pow(256, Mod), Mask)
    	end

	else
		if Comparison == 0 then
			rstr = string.format("bread(%s) >= %s", offset, Amount)
		elseif Comparison == 1 then
			rstr = string.format("bread(%s) <= %s", offset, Amount)
		elseif Comparison == 10 then
			rstr = string.format("bread(%s) == %s", offset, Amount)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 최대값을 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 최대값을 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
]================================]
function GetUpgradeMax(Upgrade, Player) --업그레이드/Upgrade,TrgPlayer/[Player]의 [Upgrade]의 최대값을 반환합니다.
	Upgrade = ParseUpgrades(Upgrade)
	Player = ParsePlayer(Player)

	offset = UpgradeOffsetMax(Upgrade, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		rstr = string.format("bread(%s)", offset)
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
@Group
업그레이드
@param.Upgrade.Upgrade
@param.Player.TrgPlayer
]================================]
function UpgradeOffsetMax(Upgrade, Player) --업그레이드/Upgrade,TrgPlayer/[Player]의 [Upgrade]의 현재값 주소를 반환합니다.
	--일반/58D088 0 ~ 45
	--일반/58F278 46
	Upgrade = ParseUpgrades(Upgrade) + 0
	Player = ParsePlayer(Player)

	if Upgrade <= 45 then
		offset = 0x58D088
		offset = offset + Player * 46 + Upgrade
	else
		offset = 0x58F278
		offset = offset + Player * 20 + Upgrade
	end
	return offset
end