--[================================[
@Language.ko-KR
@Summary
[Player]의 [SupplyType]를 [Amount]만큼 [Modifier]합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [SupplyType]를 [Amount]만큼 [Modifier]합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetSupply(SupplyType, Player, Modifier, Amount) --인구수/SupplyType,TrgPlayer,TrgModifier,Number/[Player]의 [SupplyType]를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
	OffsetEPD = SupplyEPD(SupplyType, Player)


	rstr = string.format("SetMemoryEPD(%s, %s, %s)",OffsetEPD, Modifier, Amount)
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [SupplyType]가 [Comparison] [Amount]인지 확인합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.us-EN
@Summary
[Player]의 [SupplyType]가 [Comparison] [Amount]인지 확인합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function CurrentSupply(SupplyType, Player, Comparison, Amount) --인구수/SupplyType,TrgPlayer,TrgComparison,Number/[Player]의 [SupplyType]가 [Comparison] [Amount]인지 확인합니다.
	Player = ParsePlayer(Player)
    Comparison = ParseComparison(Comparison)
	OffsetEPD = SupplyEPD(SupplyType, Player)


	rstr = string.format("MemoryEPD(%s, %s, %s)",OffsetEPD, Comparison, Amount)
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [SupplyType] 값을 읽습니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [SupplyType] 값을 읽습니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
]================================]
function GetSupply(SupplyType, Player) --인구수/SupplyType,TrgPlayer/[Player]의 [SupplyType] 값을 읽습니다.
	Player = ParsePlayer(Player)
	OffsetEPD = SupplyEPD(SupplyType, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [SupplyType] 주소를 반환합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer


@Language.us-EN
@Summary
[Player]의 [SupplyType] 주소를 반환합니다.
@Group
인구수
@param.SupplyType.SupplyType
@param.Player.TrgPlayer
]================================]
function SupplyEPD(SupplyType, Player) --인구수/SupplyType,TrgPlayer/[Player]의 [SupplyType] 주소를 반환합니다.
	Player = ParsePlayer(Player)
	SupplyIndex = ParseSupplyType(SupplyType)


	SupplyOffset = GetSupplyOffset(SupplyIndex)
	if IsNumber(Player) then
		SupplyOffset = SupplyOffset + Player * 4
		return string.format("EPD(0x%X)", SupplyOffset)
	else
		return string.format("EPD(%s) + %s", SupplyOffset, Player)
	end
end