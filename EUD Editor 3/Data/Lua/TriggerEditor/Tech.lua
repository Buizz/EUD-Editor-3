--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 현재값을 [Amount]만큼 [Modifier]합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.en-US
@Summary
[Player]의 [Tech]의 현재값을 [Amount]만큼 [Modifier]합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetTech(Tech, Player, Modifier, Amount) --테크/Tech,TrgPlayer,TrgModifier,Number/[Player]의 [Tech]의 현재값을 [Amount]만큼 [Modifier]합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)


	offset = TechOffset(Tech, Player)
	Mod = Tech % 4
	Mask = "0xFF"
	for i = 1, Mod do Mask = Mask .. "00" end

	if IsNumber(offset) then
		ROffset = offset - Mod

		if IsNumber(Amount) then
			rstr = string.format("SetMemoryX(0x%X, %s, 0x%X, %s)", ROffset, Modifier, Amount * math.pow(256, Mod), Mask)
		else
			rstr = string.format("SetMemoryX(0x%X, %s, (%s)<<%s, %s)", ROffset, Modifier, Amount, 8*Mod, Mask)
		end

	else
		offsetEPD = TechEPD(Tech, Player)
		if Modifier == 7 then
			rstr = string.format("bwrite_epd(%s, %s, %s)", offsetEPD, Mod, Amount)
		else
			rstr = string.format("SetMemoryXEPD(%s, %s, (%s)<<%s, %s)", offsetEPD, Modifier, Amount, 8*Mod, Mask)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 현재값이 [Comparison] [Amount]인지 확인합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.en-US
@Summary
[Player]의 [Tech]의 현재값이 [Comparison] [Amount]인지 확인합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentTech(Tech, Player, Comparison, Amount) --테크/Tech,TrgPlayer,TrgComparison,Number/[Player]의 [Tech]의 현재값이 [Comparison] [Amount]인지 확인합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)

	Mod = Tech % 4
	Mask = "0xFF"
	for i = 1, Mod do Mask = Mask .. "00" end
	offset = TechOffset(Tech, Player)

	if IsNumber(offset) then
		ROffset = offset - Mod

    	if IsNumber(Amount) then
    		rstr = string.format("MemoryX(0x%X, %s, 0x%X, %s)", ROffset, Comparison, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("MemoryX(0x%X, %s, (%s)<<%s, %s)", ROffset, Comparison, Amount, 8*Mod, Mask)
    	end

	else
		offsetEPD = TechEPD(Tech, Player)
		rstr = string.format("MemoryXEPD(%s, %s, (%s)<<%s, %s)", offsetEPD, Comparison, Amount, 8*Mod, Mask)
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 현재값을 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 현재값을 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function GetTech(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 현재값을 반환합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)

	offset = TechOffset(Tech, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		offsetEPD = TechEPD(Tech, Player)
		rstr = string.format("bread_epd(%s, %s)", offsetEPD, Tech%4)
	end
	echo(rstr)
end


--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 현재값 주소의 EPD를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 현재값 주소의 EPD를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function TechEPD(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 현재값 주소의 EPD를 반환합니다.
	--일반/58D2B0 0 ~ 45
	--일반/58F32C 46
	Tech = ParseTechdata(Tech) + 0
	Player = ParsePlayer(Player)

	Size = 0
	if Tech <= 23 then
		offset = 0x58CF44
		Size = 6
	else
		offset = 0x58F140
		Size = 5
		Tech = Tech - 24
	end
	if IsNumber(Player) then
		return math.floor((offset + Tech - 0x58A364) / 4) + Player * Size
	else
		offset = string.format("EPD(0x%X) + %s * %s", offset + Tech, Player, Size)
		return offset
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 현재값 주소를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 현재값 주소를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function TechOffset(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 현재값 주소를 반환합니다.
	--일반/58D2B0 0 ~ 45
	--일반/58F32C 46
	Tech = ParseTechdata(Tech) + 0
	Player = ParsePlayer(Player)

	Size = 0
	if Tech <= 23 then
		offset = 0x58CF44
		Size = 24
	else
		offset = 0x58F140
		Size = 20
		Tech = Tech - 24
	end
	if IsNumber(Player) then
		offset = offset + Player * Size + Tech
		return offset
	else
		offset = string.format("0x%X + %s * %s", offset + Tech, Player, Size)
		return offset
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 최대값을 [Amount]만큼 [Modifier]합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.en-US
@Summary
[Player]의 [Tech]의 최대값을 [Amount]만큼 [Modifier]합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetTechMax(Tech, Player, Modifier, Amount) --테크/Tech,TrgPlayer,TrgModifier,Number/[Player]의 [Tech]의 최대값을 [Amount]만큼 [Modifier]합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)

	Mod = Tech % 4
	Mask = "0xFF"
	for i = 1, Mod do Mask = Mask .. "00" end
	offset = TechMaxOffset(Tech, Player)

	if IsNumber(offset) then
		ROffset = offset - Mod

		if IsNumber(Amount) then
			rstr = string.format("SetMemoryX(0x%X, %s, 0x%X, %s)", ROffset, Modifier, Amount * math.pow(256, Mod), Mask)
    	else
			rstr = string.format("SetMemoryX(0x%X, %s, (%s)<<%s, %s)", ROffset, Modifier, Amount, 8*Mod, Mask)
    	end

	else
		offsetEPD = TechMaxEPD(Tech, Player)
		if Modifier == 7 then
			rstr = string.format("bwrite_epd(%s, %s, %s)", offsetEPD, Mod, Amount)
		else
			rstr = string.format("SetMemoryXEPD(%s, %s, (%s)<<%s, %s)", offsetEPD, Modifier, Amount, 8*Mod, Mask)
		end
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 최대값이 [Comparison] [Amount]인지 확인합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.en-US
@Summary
[Player]의 [Tech]의 최대값이 [Comparison] [Amount]인지 확인합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentTechMax(Tech, Player, Comparison, Amount) --테크/Tech,TrgPlayer,TrgComparison,Number/[Player]의 [Tech]의 최대값이 [Comparison] [Amount]인지 확인합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)

	Mod = Tech % 4
	Mask = "0xFF"
	for i = 1, Mod do Mask = Mask .. "00" end
	offset = TechMaxOffset(Tech, Player)

	if IsNumber(offset) then
		ROffset = offset - Mod

		if IsNumber(Amount) then
			rstr = string.format("MemoryX(0x%X, %s, 0x%X, %s)", ROffset, Comparison, Amount * math.pow(256, Mod), Mask)
		else
			rstr = string.format("MemoryX(0x%X, %s, (%s)<<%s, %s)", ROffset, Comparison, Amount, 8*Mod, Mask)
		end

	else
		offsetEPD = TechMaxEPD(Tech, Player)
		rstr = string.format("MemoryXEPD(%s, %s, (%s)<<%s, %s)", offset, Comparison, Amount, 8*Mod, Mask)
	end
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 최대값을 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 최대값을 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function GetTechMax(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 최대값을 반환합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)

	offset = TechMaxOffset(Tech, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		offsetEPD = TechMaxEPD(Tech, Player)
		rstr = string.format("bread_epd(%s, %s)", offsetEPD, Tech%4)
	end
	echo(rstr)
end



--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 최대값 주소의 EPD를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 최대값 주소의 EPD를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function TechMaxEPD(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 최대값 주소의 EPD를 반환합니다.
	--일반/58D088 0 ~ 45
	--일반/58F278 46
	Tech = ParseTechdata(Tech) + 0
	Player = ParsePlayer(Player)

	Size = 0
	if Tech <= 23 then
		offset = 0x58CE24
		Size = 6
	else
		offset = 0x58F050
		Size = 5
		Tech = Tech - 24
	end
	if IsNumber(Player) then
		offset = offset + Player * Size + Tech
		return math.floor((offset - 0x58A364) / 4)
	else
		offset = string.format("EPD(0x%X) + %s * %s", offset + Tech, Player, Size)
		return offset
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Tech]의 최대값 주소를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer


@Language.en-US
@Summary
[Player]의 [Tech]의 최ㅐ값 주소를 반환합니다.
@Group
테크
@param.Tech.Tech
@param.Player.TrgPlayer
]================================]
function TechMaxOffset(Tech, Player) --테크/Tech,TrgPlayer/[Player]의 [Tech]의 현재값 주소를 반환합니다.
	--일반/58D088 0 ~ 45
	--일반/58F278 46
	Tech = ParseTechdata(Tech) + 0
	Player = ParsePlayer(Player)

	if Tech <= 23 then
		offset = 0x58CE24
		Size = 24
	else
		offset = 0x58F050
		Size = 20
		Tech = Tech - 24
	end
	if IsNumber(Player) then
		offset = offset + Player * Size + Tech
		return offset
	else
		offset = string.format("0x%X + %s * %s", offset + Tech, Player, Size)
		return offset
	end
end
