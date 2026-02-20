--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 유닛보유수를 [Amount]만큼 [Modifier]합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number


@Language.en-US
@Summary
Modifies [Player]'s count of [Unit] by [Amount] using [Modifier].
@Group
Unit Count
@param.Unit.TrgUnit
@param.Player.TrgPlayer
@param.Modifier.TrgModifier
@param.Amount.Number
]================================]
function SetUnitCount(Unit, Player, Modifier, Amount) --일반/TrgUnit,TrgPlayer,TrgModifier,Number/[Player]의 [Unit]의 유닛보유수를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    Unit = ParseUnit(Unit)
    OffsetEPD = UnitCountEPD(0, Player)


	echo(string.format("SetDeaths(%s, %s, %s, %s)", OffsetEPD, Modifier, Amount, Unit))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 유닛보유수를 반환합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
@param.Player.TrgPlayer


@Language.en-US
@Summary
Returns [Player]'s count of [Unit].
@Group
Unit Count
@param.Unit.TrgUnit
@param.Player.TrgPlayer
]================================]
function GetUnitCount(Unit, Player) --일반/TrgUnit,TrgPlayer/[Player]의 [Unit]의 유닛보유수를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    OffsetEPD = UnitCountEPD(Unit, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit]의 유닛보유수의 주소를 반환합니다.
@Group
유닛보유수
@param.Unit.TrgUnit
@param.Player.TrgPlayer


@Language.en-US
@Summary
Returns the EPD address of [Player]'s count for [Unit].
@Group
Unit Count
@param.Unit.TrgUnit
@param.Player.TrgPlayer
]================================]
function UnitCountEPD(Unit, Player) --일반/TrgUnit,TrgPlayer/[Player]의 [Unit]의 유닛보유수의 주소를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)

    Offset = 0x582324
	if IsNumber(Player) and IsNumber(Unit) then
		Offset = Offset + (Unit * 12 + Player) * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s * 12 + %s", Offset, Unit, Player)
	end
end