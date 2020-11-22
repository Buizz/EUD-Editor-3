--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.UnitsDat
파라미터입니다.
@param.Index.TrgUnit
대상 유닛입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.UnitsDat
파라미터입니다.
@param.Index.TrgUnit
대상 유닛입니다.
]================================]
function GetUnitsDat(DatType, Index) --DatFile/UnitsDat,TrgUnit/[Index]의 [DatType] 값을 반환합니다.
	Unit = ParseUnit(Index)
    str = GetDatFile("units", DatType, Unit)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.WeaponsDat
파라미터입니다.
@param.Index.Weapon
대상 무기입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.WeaponsDat
파라미터입니다.
@param.Index.Weapon
대상 무기입니다.
]================================]
function GetWeaponsDat(DatType, Index) --DatFile/WeaponsDat,Weapon/[Index]의 [DatType] 값을 반환합니다.
	Weapon = ParseWeapon(Index)
    str = GetDatFile("weapons", DatType, Weapon)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.FlingyDat
파라미터입니다.
@param.Index.Flingy
대상 비행정보입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.FlingyDat
파라미터입니다.
@param.Index.Flingy
대상 비행정보입니다.
]================================]
function GetFlingyDat(DatType, Index) --DatFile/FlingyDat,Flingy/[Index]의 [DatType] 값을 반환합니다.
	Flingy = ParseFlingy(Index)
    str = GetDatFile("flingy", DatType, Flingy)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.SpritesDat
파라미터입니다.
@param.Index.Sprite
대상 스프라이트입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.SpritesDat
파라미터입니다.
@param.Index.Sprite
대상 스프라이트입니다.
]================================]
function GetSpritesDat(DatType, Index) --DatFile/SpritesDat,Sprite/[Index]의 [DatType] 값을 반환합니다.
	Sprite = ParseSprites(Index)
    str = GetDatFile("sprites", DatType, Sprite)
	echo(str)
end


--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.ImagesDat
파라미터입니다.
@param.Index.Image
대상 이미지입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.ImagesDat
파라미터입니다.
@param.Index.Image
대상 이미지입니다.
]================================]
function GetImagesDat(DatType, Index) --DatFile/ImagesDat,Image/[Index]의 [DatType] 값을 반환합니다.
	Image = ParseImage(Index)
    str = GetDatFile("images", DatType, Image)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.UpgradesDat
파라미터입니다.
@param.Index.Upgrade
대상 업그레이드입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.UpgradesDat
파라미터입니다.
@param.Index.Upgrade
대상 업그레이드입니다.
]================================]
function GetUpgradesDat(DatType, Index) --DatFile/UpgradesDat,Upgrade/[Index]의 [DatType] 값을 반환합니다.
	Upgrade = ParseUpgrade(Index)
    str = GetDatFile("upgrades", DatType, Upgrade)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.TechdataDat
파라미터입니다.
@param.Index.Tech
대상 기술입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.TechdataDat
파라미터입니다.
@param.Index.Tech
대상 기술입니다.
]================================]
function GetTechdataDat(DatType, Index) --DatFile/TechdataDat,Tech/[Index]의 [DatType] 값을 반환합니다.
	Tech = ParseTech(Index)
    str = GetDatFile("techdata", DatType, Tech)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.OrdersDat
파라미터입니다.
@param.Index.Order
대상 명령입니다.


@Language.us-EN
@Summary
[Index]의 [DatType] 값을 반환합니다.
@Group
DatFile
@param.DatType.OrdersDat
파라미터입니다.
@param.Index.Order
대상 명령입니다.
]================================]
function GetOrdersDat(DatType, Index) --DatFile/OrdersDat,Order/[Index]의 [DatType] 값을 반환합니다.
	Order = ParseOrder(Index)
    str = GetDatFile("orders", DatType, Order)
	echo(str)
end