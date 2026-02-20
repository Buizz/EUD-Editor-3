--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 완료된 유닛보유수를 [Amount]만큼 [Modifier]합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
대상 유닛입니다. 이 유닛이 필요한 테크가 해금됩니다.
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Modifier.TrgModifier
수식입니다.
@param.Amount.Number
대상 플레이어입니다.


@Language.en-US
@Summary
Modifies [Player]'s completed count of [Unit] by [Amount] using [Modifier].
@Group
Unit Count
@param.Unit.TrgUnit
Target unit. Required tech for this unit will be unlocked.
@param.Player.TrgPlayer
Target player.
@param.Modifier.TrgModifier
Modifier operation.
@param.Amount.Number
Amount to apply.
]================================]
function SetCompletedUnitCount(Unit, Player, Modifier, Amount)
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    Unit = ParseUnit(Unit)
    OffsetEPD = CompletedUnitCountEPD(Unit, Player)


	echo(string.format("SetDeaths(%s, %s, %s, 0)", OffsetEPD, Modifier, Amount))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 완료된 유닛보유수를 반환합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
대상 유닛입니다. 이 유닛이 필요한 테크가 해금됩니다.
@param.Player.TrgPlayer
대상 플레이어입니다.


@Language.en-US
@Summary
Returns [Player]'s completed count of [Unit].
@Group
Unit Count
@param.Unit.TrgUnit
Target unit. Required tech for this unit will be unlocked.
@param.Player.TrgPlayer
Target player.
]================================]
function GetCompletedUnitCount(Unit, Player)
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    OffsetEPD = CompletedUnitCountEPD(Unit, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 완료된 유닛보유수의 주소를 반환합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
@param.Player.TrgPlayer


@Language.en-US
@Summary
Returns the EPD address of [Player]'s completed count for [Unit].
@Group
Unit Count
@param.Unit.TrgUnit
@param.Player.TrgPlayer
]================================]
function CompletedUnitCountEPD(Unit, Player) --일반/TrgUnit,TrgPlayer/[Player]의 [Unit]의 완료된 유닛보유수의 주소를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    Offset = 0x584DE4
	if IsNumber(Player) and IsNumber(Unit) then
		Offset = Offset + (Unit * 12 + Player) * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s * 12 + %s", Offset, Unit, Player)
	end
end