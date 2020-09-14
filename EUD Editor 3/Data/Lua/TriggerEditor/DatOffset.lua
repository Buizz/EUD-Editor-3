function UnitsDatOffset(DatType) --DatFile/UnitsDat/[DatType]의 주소를 반환합니다
    str = DatOffset("units", DatType)
	return str
end
function WeaponsDatOffset(DatType) --DatFile/WeaponsDat/[DatType]의 주소를 반환합니다
    str = DatOffset("weapons", DatType)
	return str
end
function FlingyDatOffset(DatType) --DatFile/FlingyDat/[DatType]의 주소를 반환합니다
    str = DatOffset("flingy", DatType)
	return str
end
function SpritesDatOffset(DatType) --DatFile/SpritesDat/[DatType]의 주소를 반환합니다
    str = DatOffset("sprites", DatType)
	return str
end
function ImagesDatOffset(DatType) --DatFile/ImagesDat/[DatType]의 주소를 반환합니다
    str = DatOffset("images", DatType)
	return str
end
function UpgradesDatOffset(DatType) --DatFile/UpgradesDat/[DatType]의 주소를 반환합니다
    str = DatOffset("upgrades", DatType)
	return str
end
function TechdataDatOffset(DatType) --DatFile/TechdataDat/[DatType]의 주소를 반환합니다
    str = DatOffset("techdata", DatType)
	return str
end
function OrdersDatOffset(DatType) --DatFile/OrdersDat/[DatType]의 주소를 반환합니다
    str = DatOffset("orders", DatType)
	return str
end