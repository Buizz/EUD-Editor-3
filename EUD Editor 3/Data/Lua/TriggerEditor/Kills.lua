--[================================[
@Language.ko-KR
@Summary
[Player]가 [Unit]을 죽인 수를 [Amount]만큼 [Modifier]합니다.
@Group
일반
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Modifier.TrgModifier
조절할 방법입니다.
@param.Amount.Number
설정할 값입니다.
@param.Unit.TrgUnit
설정할 유닛입니다.


@Language.en-US
@Summary
Modifies [Player]'s kill count of [Unit]: [Modifier] [Value].
@Group
General
@param.Player.TrgPlayer
Target player.
@param.Modifier.TrgModifier
Adjustment method.
@param.Amount.Number
Value to apply.
@param.Unit.TrgUnit
Unit to adjust.
]================================]
function SetKills(Player, Modifier, Amount, Unit) --일반/TrgPlayer,TrgModifier,Number,TrgUnit/[Player]의 [Unit]이 죽은 수를 [Amount]만큼 [Modifier]합니다.
	Player = ParsePlayer(Player)
    Modifier = ParseModifier(Modifier)
    Unit = ParseUnit(Unit)
    OffsetEPD = KillsEPD(0, Player)


	echo(string.format("SetDeaths(%s, %s, %s, %s)", OffsetEPD, Modifier, Amount, Unit))
end

--[================================[
@Language.ko-KR
@Summary
[Player]가 [Unit]이 죽은 수를 읽습니다.
@Group
일반
@param.Unit.TrgUnit
설정할 유닛입니다.
@param.Player.TrgPlayer
대상 플레이어입니다.


@Language.en-US
@Summary
Returns [Player]'s kill count of [Unit].
@Group
General
@param.Unit.TrgUnit
Unit to read.
@param.Player.TrgPlayer
Target player.
]================================]
function GetKills(Unit, Player) --일반/TrgUnit,TrgPlayer/[Player]의 [Unit]이 죽은 수를 읽습니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    OffsetEPD = KillsEPD(Unit, Player)

	echo(string.format("dwread_epd(%s)", OffsetEPD))
end

--[================================[
@Language.ko-KR
@Summary
[Player]가 [Unit]을 죽인 수의 주소를 반환합니다.
@Group
일반
@param.Unit.TrgUnit
설정할 유닛입니다.
@param.Player.TrgPlayer
대상 플레이어입니다.


@Language.en-US
@Summary
Returns the EPD address of [Player]'s kill count for [Unit].
@Group
General
@param.Unit.TrgUnit
Unit to read.
@param.Player.TrgPlayer
Target player.
]================================]
function KillsEPD(Unit, Player) --일반/TrgUnit,TrgPlayer/[Player]의 [Unit]이 죽은 수의 주소를 반환합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)

    Offset = 0x5878A4
	if IsNumber(Player) and IsNumber(Unit) then
		Offset = Offset + (Unit * 12 + Player) * 4
		return string.format("EPD(0x%X)", Offset)
	else
		return string.format("EPD(0x%X) + %s * 12 + %s", Offset, Unit, Player)
	end
end