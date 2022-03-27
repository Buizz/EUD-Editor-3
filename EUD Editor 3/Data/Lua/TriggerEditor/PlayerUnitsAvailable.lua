--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit] 유닛 사용 가능 값을 [State]합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.Unit.TrgUnit
@param.State.TrgSwitchState


@Language.en-US
@Summary
[Player]의 [Unit] 유닛 사용 가능 값을 [State]합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.Unit.TrgUnit
@param.State.TrgSwitchState
]================================]
function SetPlayerUnitsAvailable(Player, Unit, State) --일반/TrgPlayer,TrgUnit,TrgSwitchState/[Player]의 [Unit] 유닛 사용 가능 값을 [State]합니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)
    State = ParseSwitchState(State)

    if IsNumber(Unit) and IsNumber(Player) then
		Offset = 0x57F27C + Unit + Player * 228

 		if State == 2 then
    		Amount = "0x01010101"
    	else
    		Amount = "0x0"
    	end

		Mod = Unit % 4
		Mask = "0xFF"
		for i = 1, Mod do Mask = Mask .. "00" end

		rstr = string.format("SetMemoryX(0x%X, SetTo, %s, %s)", Offset, Amount, Mask)
    	echo(rstr)
    else
    	if State == 2 then
    		Amount = 1
    	else
    		Amount = 0
    	end

		Offset = string.format("%s + %s + %s * 228", PlayerUnitsAvailableOffset(), Unit, Player)
		echo(string.format("bwrite(%s, %s)", Offset, Amount))
    end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit] 유닛 사용 가능 값을 읽어옵니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.Unit.TrgUnit


@Language.en-US
@Summary
[Player]의 [Unit] 유닛 사용 가능 값을 읽어옵니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.Unit.TrgUnit
]================================]
function GetPlayerUnitsAvailable(Player, Unit) --일반/TrgPlayer,TrgUnit/[Player]의 [Unit] 유닛 사용 가능 값을 읽어옵니다.
	Player = ParsePlayer(Player)
    Unit = ParseUnit(Unit)

	Offset = string.format("%s + %s + %s * 228", PlayerUnitsAvailableOffset(), Unit, Player)

	echo(string.format("bread(%s)", Offset))
end

--[================================[
@Language.ko-KR
@Summary
플레이어 유닛 사용 가능 오프셋을 가져옵니다.
@Group
플레이어


@Language.en-US
@Summary
플레이어 유닛 사용 가능 오프셋을 가져옵니다.
@Group
플레이어
]================================]
function PlayerUnitsAvailableOffset() --일반//플레이어 유닛 사용 가능 오프셋을 가져옵니다.
	return "0x57F27C"
end