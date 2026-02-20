--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.UnitsDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.UnitsDat
Parameter.
]================================]
function UnitsDatOffset(DatType)
    str = DatOffset("units", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.WeaponsDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.WeaponsDat
Parameter.
]================================]
function WeaponsDatOffset(DatType)
    str = DatOffset("weapons", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.FlingyDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.FlingyDat
Parameter.
]================================]
function FlingyDatOffset(DatType)
    str = DatOffset("flingy", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.SpritesDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.SpritesDat
Parameter.
]================================]
function SpritesDatOffset(DatType)
    str = DatOffset("sprites", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.ImagesDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.ImagesDat
Parameter.
]================================]
function ImagesDatOffset(DatType)
    str = DatOffset("images", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.UpgradesDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.UpgradesDat
Parameter.
]================================]
function UpgradesDatOffset(DatType)
    str = DatOffset("upgrades", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.TechdataDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.TechdataDat
Parameter.
]================================]
function TechdataDatOffset(DatType)
    str = DatOffset("techdata", DatType)
	return str
end

--[================================[
@Language.ko-KR
@Summary
[DatType]의 주소를 반환합니다
@Group
DatFile
@param.DatType.OrdersDat
파라미터입니다.


@Language.en-US
@Summary
Returns the address of [DatType].
@Group
DatFile
@param.DatType.OrdersDat
Parameter.
]================================]
function OrdersDatOffset(DatType)
    str = DatOffset("orders", DatType)
	return str
end