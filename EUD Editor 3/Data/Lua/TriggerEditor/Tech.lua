function SetTech(Tech, Player, Modifier, Amount) -- Tech,TrgPlayer,TrgModifier,Number/[Player]의 [Tech]의 현재값을 [Amount]만큼 [Modifier]합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)


	offset = TechOffset(Tech, Player)

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
function CurrentTech(Tech, Player, Comparison, Amount) -- Tech,TrgPlayer,TrgComparison,Number/[Player]의 [Tech]의 현재값이 [Comparison] [Amount]인지 확인합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)


	offset = TechOffset(Tech, Player)

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
function GetTech(Tech, Player) -- Tech,TrgPlayer/[Player]의 [Tech]의 현재값을 반환합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)

	offset = TechOffset(Tech, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		rstr = string.format("bread(%s)", offset)
	end
	echo(rstr)
end
function TechOffset(Tech, Player) -- Tech,TrgPlayer/[Player]의 [Tech]의 현재값 주소를 반환합니다.
	-- 58D2B0 0 ~ 45
	-- 58F32C 46
	Tech = ParseTechdata(Tech) + 0 
	Player = ParsePlayer(Player)

	Size = 0
	if Tech <= 23 then
		offset = 0x58CF44
		Size = 24
	else
		offset = 0x58F140
		Size = 20
	end
	if IsNumber(Player) then
		offset = offset + Player * Size + Tech
		return offset
	else
		offset = string.format("0x%X + %s * %s + %s", offset, Player, Tech)
		return offset
	end
end

function SetTechMax(Tech, Player, Modifier, Amount) -- Tech,TrgPlayer,TrgModifier,Number/[Player]의 [Tech]의 최대값을 [Amount]만큼 [Modifier]합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)


	offset = TechOffsetMax(Tech, Player)

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
function CurrentTechMax(Tech, Player, Comparison, Amount) -- Tech,TrgPlayer,TrgComparison,Number/[Player]의 [Tech]의 최대값이 [Comparison] [Amount]인지 확인합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)


	offset = TechOffsetMax(Tech, Player)

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
function GetTechMax(Tech, Player) -- Tech,TrgPlayer/[Player]의 [Tech]의 최대값을 반환합니다.
	Tech = ParseTechdata(Tech)
	Player = ParsePlayer(Player)

	offset = TechOffsetMax(Tech, Player)

	if IsNumber(offset) then
		rstr = string.format("bread(0x%X)", offset)
	else
		rstr = string.format("bread(%s)", offset)
	end
	echo(rstr)
end
function TechOffsetMax(Tech, Player) -- Tech,TrgPlayer/[Player]의 [Tech]의 현재값 주소를 반환합니다.
	-- 58D088 0 ~ 45
	-- 58F278 46
	Tech = ParseTechdata(Tech) + 0
	Player = ParsePlayer(Player)

	if Tech <= 23 then
		offset = 0x58CE24
		Size = 24
	else
		offset = 0x58F050
		Size = 20
	end
	return offset
end