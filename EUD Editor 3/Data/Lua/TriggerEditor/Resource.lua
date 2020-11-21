--[================================[
@Language.ko-KR
@Summary
[Player]의 [ResourceType]을 [Amount]만큼 [Modifier]합니다.
@Group
자원
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
@param.ResourceType.TrgResource


@Language.us-EN
@Summary
[Player]의 [ResourceType]을 [Amount]만큼 [Modifier]합니다.
@Group
자원
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
@param.ResourceType.TrgResource
]================================]
function SetResource(Player, Modifier, Amount, ResourceType) --일반/TrgPlayer,TrgModifier,Number,TrgResource/[Player]의 [ResourceType]을 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    ResourceType = ParseResource(ResourceType)
    Offset = ResourceEPD(Player, ResourceType)
	echo(string.format("SetMemoryEPD(%s, %s, %s)", Offset, Modifier, Amount))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [ResourceType]을 읽습니다.
@Group
자원
@param.Player.TrgPlayer
@param.ResourceType.TrgResource


@Language.us-EN
@Summary
[Player]의 [ResourceType]을 읽습니다.
@Group
자원
@param.Player.TrgPlayer
@param.ResourceType.TrgResource
]================================]
function GetResource(Player, ResourceType) --일반/TrgPlayer,TrgResource/[Player]의 [ResourceType]을 읽습니다.
    Player = ParsePlayer(Player)
    Offset = ResourceEPD(Player, ResourceType)
    ResourceType = ParseResource(ResourceType)

    
	echo(string.format("dwread_epd(%s)", Offset))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Resource]의 주소를 반환합니다.
@Group
자원
@param.Player.TrgPlayer
@param.ResourceType.TrgResource


@Language.us-EN
@Summary
[Player]의 [Resource]의 주소를 반환합니다.
@Group
자원
@param.Player.TrgPlayer
@param.ResourceType.TrgResource
]================================]
function ResourceEPD(Player, ResourceType) --일반/TrgPlayer,TrgResource/[Player]의 [Resource]의 주소를 반환합니다.
    Player = ParsePlayer(Player)
	ResourceType = ParseResource(ResourceType)


	if ResourceType == 0 then
		-- Ore
		Offset = 0x57F0F0
	elseif ResourceType == 1 then
		-- Gas
		Offset = 0x57F120
	end


	if IsNumber(Player) then
		Offset = Offset + Player * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s", Offset, Player)
	end
end