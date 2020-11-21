--[================================[
@Language.ko-KR
@Summary
[Player]의 [Unit] 유닛 사용 가능 값을 [State]합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.Unit.TrgUnit
@param.State.TrgSwitchState


@Language.us-EN
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
    	Offset = PlayerUnitsAvailableOffset()

    	rOffset =  Player * 57

    	Mod = Unit % 4
    	Unit = Unit - Mod
    	Unit = Unit / 4

 		if State == 2 then
    		Amount = "0x01010101"
    	else
    		Amount = "0x0"
    	end

    	if Mod == 0 then
    		Mask = "0xFF"
    	elseif Mod == 1 then
    		Mask = "0xFF00"
    	elseif Mod == 2 then
    		Mask = "0xFF0000"
    	elseif Mod == 3 then
    		Mask = "0xFF000000"
    	end



    	rstr = string.format("SetMemoryXEPD(EPD(0x%X) + %s + %s, SetTo, %s, %s)", Offset, rOffset, Unit, Amount, Mask)
    	echo(rstr)
    else
    	if State == 2 then
    		Amount = 1
    	else
    		Amount = 0
    	end

	    Offset = string.format("%s + %s * 228 + %s", PlayerUnitsAvailableOffset(), Player, Unit)
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


@Language.us-EN
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

    Offset = string.format("%s + %s * 228 + %s", PlayerUnitsAvailableOffset(), Player, Unit)

	echo(string.format("bread(%s)", Offset))
end

--[================================[
@Language.ko-KR
@Summary
플레이어 유닛 사용 가능 오프셋을 가져옵니다.
@Group
플레이어


@Language.us-EN
@Summary
플레이어 유닛 사용 가능 오프셋을 가져옵니다.
@Group
플레이어
]================================]
function PlayerUnitsAvailableOffset() --일반//플레이어 유닛 사용 가능 오프셋을 가져옵니다.
	return "0x57F27C"
end