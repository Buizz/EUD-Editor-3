--[================================[
@Language.ko-KR
@Summary
[ptr]에 다음에 생성될 유닛의 주소를 반환합니다
@Group
구조오프셋
@param.ptr.Variable
ptr이 저장될 변수입니다.


@Language.us-EN
@Summary
[ptr]에 다음에 생성될 유닛의 주소를 반환합니다
@Group
구조오프셋
@param.ptr.Variable
ptr이 저장될 변수입니다.
]================================]
function SetNextUnitPtr(ptr)
    echo(ptr .. " = dwread_epd(EPD(0x628438))")
end

--[================================[
@Language.ko-KR
@Summary
[epd]에 다음에 생성될 유닛의 EPD를 반환합니다
@Group
구조오프셋
@param.epd.Variable
epd가 저장될 변수입니다.


@Language.us-EN
@Summary
[epd]에 다음에 생성될 유닛의 EPD를 반환합니다
@Group
구조오프셋
@param.epd.Variable
epd가 저장될 변수입니다.
]================================]
function SetNextUnitEpd(epd)
    echo(epd .. " = epdread_epd(EPD(0x628438))")
end

--[================================[
@Language.ko-KR
@Summary
[ptr]에 다음에 생성될 유닛의 PTR을, [epd]에 EPD를 반환합니다
@Group
구조오프셋
@param.ptr.Variable
ptr가 저장될 변수입니다.
@param.epd.Variable
epd가 저장될 변수입니다.


@Language.us-EN
@Summary
[ptr]에 다음에 생성될 유닛의 PTR을, [epd]에 EPD를 반환합니다
@Group
구조오프셋
@param.ptr.Variable
ptr가 저장될 변수입니다.
@param.epd.Variable
epd가 저장될 변수입니다.
]================================]
function SetNextUnitPtrEpd(ptr, epd)
    echo(ptr .. "," .. epd .. " = dwepdread_epd(EPD(0x628438))")
end


--[================================[
@Language.ko-KR
@Summary
Offset의 주소와 크기를 반환합니다.
@Group
구조오프셋
@param.Offset.CUnitOffset
CUint의 주소의 이름입니다.


@Language.us-EN
@Summary
Offset의 주소와 크기를 반환합니다.
@Group
구조오프셋
@param.Offset.CUnitOffset
CUint의 주소의 이름입니다.
]================================]
function GetCUnitOffset(Offset)
	t = {
		["prev"]={0x000,4},
		["next"]={0x004,4},
		["hitPoints"]={0x008,4},
		["sprite"]={0x00C,4},
		["moveTargetX"]={0x010,2},
		["moveTargetY"]={0x012,2},
		["moveTargetUnit"]={0x014,4},
		["nextMovementWaypointX"]={0x018,2},
		["nextMovementWaypointY"]={0x01A,2},
		["nextTargetWaypointX"]={0x01C,2},
		["nextTargetWaypointY"]={0x01E,2},
		["movementFlags"]={0x020,1},
		["currentDirection1"]={0x021,1},
		["flingyTurnRadius"]={0x022,1},
		["velocityDirection1"]={0x023,1},
		["flingyID"]={0x024,2},
		["_unknown_0x026"]={0x026,1},
		["flingyMovementType"]={0x027,1},
		["positionX"]={0x028,2},
		["positionY"]={0x02A,2},
		["haltX"]={0x02C,4},
		["haltY"]={0x030,4},
		["flingyTopSpeed"]={0x034,4},
		["current_speed1"]={0x038,4},
		["current_speed2"]={0x03C,4},
		["current_speedX"]={0x040,4},
		["current_speedY"]={0x044,4},
		["flingyAcceleration"]={0x048,2},
		["currentDirection2"]={0x04A,1},
		["velocityDirection2"]={0x04B,1},
		["playerID"]={0x04C,1},
		["orderID"]={0x04D,1},
		["orderState"]={0x04E,1},
		["orderSignal"]={0x04F,1},
		["orderUnitType"]={0x050,2},
		["__0x52"]={0x052,2},
		["mainOrderTimer"]={0x054,1},
		["groundWeaponCooldown"]={0x055,1},
		["airWeaponCooldown"]={0x056,1},
		["spellCooldown"]={0x057,1},
		["orderTargetX"]={0x058,2},
		["orderTargetY"]={0x05A,2},
		["orderTargetUnit"]={0x05C,4},
		["shieldPoints"]={0x060,4},
		["unitType"]={0x064,2},
		["__0x66"]={0x066,2},
		["previousPlayerUnit"]={0x068,4},
		["nextPlayerUnit"]={0x06C,4},
		["subUnit"]={0x070,4},
		["orderQueueHead"]={0x074,4},
		["orderQueueTail"]={0x078,4},
		["autoTargetUnit"]={0x07C,4},
		["connectedUnit"]={0x080,4},
		["orderQueueCount"]={0x084,1},
		["orderQueueTimer"]={0x085,1},
		["_unknown_0x086"]={0x086,1},
		["attackNotifyTimer"]={0x087,1},
		["previousUnitType"]={0x088,2},
		["lastEventTimer"]={0x08A,1},
		["lastEventColor"]={0x08B,1},
		["_unused_0x08C"]={0x08C,2},
		["rankIncrease"]={0x08E,1},
		["killCount"]={0x08F,1},
		["lastAttackingPlayer"]={0x090,1},
		["secondaryOrderTimer"]={0x091,1},
		["AIActionFlag"]={0x092,1},
		["userActionFlags"]={0x093,1},
		["currentButtonSet"]={0x094,2},
		["isCloaked"]={0x096,1},
		["movementState"]={0x097,1},
		["buildQueue[1]"]={0x098,2},
		["buildQueue[2]"]={0x09A,2},
		["buildQueue[3]"]={0x09C,2},
		["buildQueue[4]"]={0x09E,2},
		["buildQueue[5]"]={0x0A0,2},
		["energy"]={0x0A2,2},
		["buildQueueSlot"]={0x0A4,1},
		["uniquenessIdentifier"]={0x0A5,1},
		["secondaryOrderID"]={0x0A6,1},
		["buildingOverlayState"]={0x0A7,1},
		["hpGain"]={0x0A8,2},
		["shieldGain"]={0x0AA,2},
		["remainingBuildTime"]={0x0AC,2},
		["previousHP"]={0x0AE,2},
		["loadedUnitIndex[1]"]={0x0B0,2},
		["loadedUnitIndex[2]"]={0x0B2,2},
		["loadedUnitIndex[3]"]={0x0B4,2},
		["loadedUnitIndex[4]"]={0x0B6,2},
		["loadedUnitIndex[5]"]={0x0B8,2},
		["loadedUnitIndex[6]"]={0x0BA,2},
		["loadedUnitIndex[7]"]={0x0BC,2},
		["loadedUnitIndex[8]"]={0x0BE,2},
		["VULTURE:spiderMineCount"]={0x0C0,1},
		["CARRIER:pInHanger"]={0x0C0,4},
		["CARRIER:pOutHanger"]={0x0C4,4},
		["CARRIER:inHangerCount"]={0x0C8,1},
		["CARRIER:outHangerCount"]={0x0C9,1},
		["FIGHTER:parent"]={0x0C0,4},
		["FIGHTER:prev"]={0x0C4,4},
		["FIGHTER:next"]={0x0C8,4},
		["FIGHTER:inHanger"]={0x0CC,1},
		["BEACON:_unknown_00"]={0x0C0,4},
		["BEACON:_unknown_04"]={0x0C4,4},
		["BEACON:flagSpawnFrame"]={0x0C8,4},
		["BUILDING:addon"]={0x0C0,4},
		["BUILDING:addonBuildType"]={0x0C4,2},
		["BUILDING:upgradeResearchTime"]={0x0C6,2},
		["BUILDING:techType"]={0x0C8,1},
		["BUILDING:upgradeType"]={0x0C9,1},
		["BUILDING:larvaTimer"]={0x0CA,1},
		["BUILDING:landingTimer"]={0x0CB,1},
		["BUILDING:creepTimer"]={0x0CC,1},
		["BUILDING:upgradeLevel"]={0x0CD,1},
		["BUILDING:__E"]={0x0CE,2},
		["WORKER:pPowerup"]={0x0C0,4},
		["WORKER:targetResourceX"]={0x0C4,2},
		["WORKER:targetResourceY"]={0x0C6,2},
		["WORKER:targetResourceUnit"]={0x0C8,4},
		["WORKER:repairResourceLossTimer"]={0x0CC,2},
		["WORKER:isCarryingSomething"]={0x0CE,1},
		["WORKER:resourceCarryCount"]={0x0CF,1},
		["WORKER:harvestTarget"]={0x0D0,4},
		["WORKER:prevHarvestUnit"]={0x0D4,4},
		["WORKER:nextHarvestUnit"]={0x0D8,4},
		["BUILDING:RESOURCE:resourceCount"]={0x0D0,2},
		["BUILDING:RESOURCE:resourceIscript"]={0x0D2,1},
		["BUILDING:RESOURCE:gatherQueueCount"]={0x0D3,1},
		["BUILDING:RESOURCE:nextGatherer"]={0x0D4,4},
		["BUILDING:RESOURCE:resourceGroup"]={0x0D8,1},
		["BUILDING:RESOURCE:resourceBelongsToAI"]={0x0D9,1},
		["BUILDING:NYDUS:exit"]={0x0D0,4},
		["BUILDING:GHOST:nukeDot"]={0x0D0,4},
		["BUILDING:PYLON:pPowerTemplate"]={0x0D0,4},
		["BUILDING:SILO:pNuke"]={0x0D0,4},
		["BUILDING:SILO:bReady"]={0x0D4,1},
		["BUILDING:HATCHERY:harvestValueLeft"]={0x0D0,2},
		["BUILDING:HATCHERY:harvestValueTop"]={0x0D2,2},
		["BUILDING:HATCHERY:harvestValueRight"]={0x0D4,2},
		["BUILDING:HATCHERY:harvestValueBottom"]={0x0D6,2},
		["BUILDING:POWERUP:originX"]={0x0D0,2},
		["BUILDING:POWERUP:originY"]={0x0D2,2},
		["statusFlags"]={0x0DC,4},
		["resourceType"]={0x0E0,1},
		["wireframeRandomizer"]={0x0E1,1},
		["secondaryOrderState"]={0x0E2,1},
		["recentOrderTimer"]={0x0E3,1},
		["visibilityStatus"]={0x0E4,4},
		["secondaryOrderPositionX"]={0x0E8,2},
		["secondaryOrderPositionY"]={0x0EA,2},
		["currentBuildUnit"]={0x0EC,4},
		["previousBurrowedUnit"]={0x0F0,4},
		["nextBurrowedUnit"]={0x0F4,4},
		["RALLY:positionX"]={0x0F8,2},
		["RALLY:positionY"]={0x0FA,2},
		["RALLY:unit"]={0x0FC,4},
		["PYLON:prevPsiProvider"]={0x0F8,4},
		["PYLON:nextPsiProvider"]={0x0FC,4},
		["path"]={0x100,4},
		["pathingCollisionInterval"]={0x104,1},
		["pathingFlags"]={0x105,1},
		["_unused_0x106"]={0x106,1},
		["isBeingHealed"]={0x107,1},
		["contourBoundsLeft"]={0x108,2},
		["contourBoundsTop"]={0x10A,2},
		["contourBoundsRight"]={0x10C,2},
		["contourBoundsBottom"]={0x10E,2},
		["STATUS:removeTimer"]={0x110,2},
		["STATUS:defenseMatrixDamage"]={0x112,2},
		["STATUS:defenseMatrixTimer"]={0x114,1},
		["STATUS:stimTimer"]={0x115,1},
		["STATUS:ensnareTimer"]={0x116,1},
		["STATUS:lockdownTimer"]={0x117,1},
		["STATUS:irradiateTimer"]={0x118,1},
		["STATUS:stasisTimer"]={0x119,1},
		["STATUS:plagueTimer"]={0x11A,1},
		["STATUS:stormTimer"]={0x11B,1},
		["STATUS:irradiatedBy"]={0x11C,4},
		["STATUS:irradiatePlayerID"]={0x120,1},
		["STATUS:parasiteFlags"]={0x121,1},
		["STATUS:cycleCounter"]={0x122,1},
		["STATUS:isBlind"]={0x123,1},
		["STATUS:maelstromTimer"]={0x124,1},
		["STATUS:_unused_0x125"]={0x125,1},
		["STATUS:acidSporeCount"]={0x126,1},
		["STATUS:acidSporeTime[1]"]={0x127,1},
		["STATUS:acidSporeTime[2]"]={0x128,1},
		["STATUS:acidSporeTime[3]"]={0x129,1},
		["STATUS:acidSporeTime[4]"]={0x12A,1},
		["STATUS:acidSporeTime[5]"]={0x12B,1},
		["STATUS:acidSporeTime[6]"]={0x12C,1},
		["STATUS:acidSporeTime[7]"]={0x12D,1},
		["STATUS:acidSporeTime[8]"]={0x12E,1},
		["STATUS:acidSporeTime[9]"]={0x12F,1},
		["STATUS:bulletBehaviour3by3AttackSequence"]={0x130,2},
		["_padding_0x132"]={0x132,2},
		["pAI"]={0x134,4},
		["airStrength"]={0x138,2},
		["groundStrength"]={0x13A,2},
		["FINDER:Left"]={0x13C,1},
		["FINDER:Right"]={0x140,1},
		["FINDER:Top"]={0x144,1},
		["FINDER:Bottom"]={0x148,1},
		["_repulseUnknown"]={0x14C,1},
		["repulseAngle"]={0x14D,1},
		["bRepMtxX"]={0x14E,1},
		["bRepMtxY"]={0x14F,1}
	}


	return t[Offset]
end

--[================================[
@Language.ko-KR
@Summary
[ptr]의 [Offset]을 [Value]만큼 [Modifier]합니다.
@Group
구조오프셋
@param.ptr.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Modifier.TrgModifier
연산할 방식입니다.


@Language.us-EN
@Summary
[ptr]의 [Offset]을 [Value]만큼 [Modifier]합니다.
@Group
구조오프셋
@param.ptr.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Modifier.TrgModifier
연산할 방식입니다.
]================================]
function SetCUnitptr(ptr, Offset, Value, Modifier)
	Modifier = ParseModifier(Modifier)
	table = GetCUnitOffset(Offset)
	address = table[1]
	size = table[2]

	rd = math.floor(address / 4) * 4

	bp = address % 4

	if IsNumber(Value) then
		Value = Value * 256^bp
	else
		Value = Value .. " * " .. 256^bp
	end
	mask = "0x"
	for i=1,size do
		mask = mask .. "FF"
	end
	for i=1,bp,1 do
		mask = mask .. "00"
	end

	outstr = "SetMemoryX(" .. ptr .. " + " .. rd .. ", " .. Modifier ..", " .. Value .. "," .. mask .. ")"
	echo(outstr)
end

--[================================[
@Language.ko-KR
@Summary
[epd]의 [Offset]을 [Value]만큼 [Modifier]합니다.
@Group
구조오프셋
@param.epd.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Modifier.TrgModifier
연산할 방식입니다.


@Language.us-EN
@Summary
[epd]의 [Offset]을 [Value]만큼 [Modifier]합니다.
@Group
구조오프셋
@param.epd.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Modifier.TrgModifier
연산할 방식입니다.
]================================]
function SetCUnitepd(epd, Offset, Value, Modifier) --구조오프셋/Variable,CUnitOffset,Number,TrgModifier/[epd]의 [Offset]을 [Value]만큼 [Modifier]합니다.
	Modifier = ParseModifier(Modifier)
	table = GetCUnitOffset(Offset)
	address = table[1]
	size = table[2]

	rd = math.floor(address / 4)

	bp = address % 4

	if IsNumber(Value) then
		Value = Value * 256^bp
	else
		Value = Value .. " * " .. 256^bp
	end
	mask = "0x"
	for i=1,size do
		mask = mask .. "FF"
	end
	for i=1,bp,1 do
		mask = mask .. "00"
	end

	outstr = "SetMemoryXEPD(" .. epd .. " + " .. rd .. ", " .. Modifier ..", " .. Value .. "," .. mask .. ")"
	echo(outstr)
end

--[================================[
@Language.ko-KR
@Summary
[ptr]의 [Offset]가 [Comparison] [Value]인지 확인합니다.
@Group
구조오프셋
@param.ptr.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgModifier
비교 방식입니다.


@Language.us-EN
@Summary
[ptr]의 [Offset]가 [Comparison] [Value]인지 확인합니다.
@Group
구조오프셋
@param.ptr.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgModifier
비교 방식입니다.
]================================]
function CUnitptr(ptr, Offset, Value, Comparison)
	Comparison = ParseComparison(Comparison)
	table = GetCUnitOffset(Offset)
	address = table[1]
	size = table[2]

	rd = math.floor(address / 4) * 4

	bp = address % 4

	if IsNumber(Value) then
		Value = Value * 256^bp
	else
		Value = Value .. " * " .. 256^bp
	end
	mask = "0x"
	for i=1,size do
		mask = mask .. "FF"
	end
	for i=1,bp,1 do
		mask = mask .. "00"
	end

	outstr = "MemoryX(" .. ptr .. " + " .. rd .. ", " .. Comparison ..", " .. Value .. "," .. mask .. ")"
	echo(outstr)
end

--[================================[
@Language.ko-KR
@Summary
[epd]의 [Offset]가 [Comparison] [Value]인지 확인합니다.
@Group
구조오프셋
@param.epd.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgModifier
비교 방식입니다.


@Language.us-EN
@Summary
[epd]의 [Offset]가 [Comparison] [Value]인지 확인합니다.
@Group
구조오프셋
@param.epd.Variable
대상 유닛입니다.
@param.Offset.CUnitOffset
변경할 항목입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgModifier
비교 방식입니다.
]================================]
function CUnitepd(epd, Offset, Value, Comparison)
	Comparison = ParseComparison(Comparison)
	table = GetCUnitOffset(Offset)
	address = table[1]
	size = table[2]

	rd = math.floor(address / 4)

	bp = address % 4

	if IsNumber(Value) then
		Value = Value * 256^bp
	else
		Value = Value .. " * " .. 256^bp
	end
	mask = "0x"
	for i=1,size do
		mask = mask .. "FF"
	end
	for i=1,bp,1 do
		mask = mask .. "00"
	end

	outstr = "MemoryXEPD(" .. epd .. " + " .. rd .. ", " .. Comparison ..", " .. Value .. "," .. mask .. ")"
	echo(outstr)
end