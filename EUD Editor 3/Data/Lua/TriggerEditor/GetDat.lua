function GetUnitsDat(DatType, Index) -- UnitsDat,TrgUnit/[Index]의 [DatType] 값을 반환합니다.
	Unit = ParseUnit(Index)
    str = GetDatFile("units", DatType, Unit)
	echo(str)
end
function GetWeaponsDat(DatType, Index) -- WeaponsDat,Weapon/[Index]의 [DatType] 값을 반환합니다.
	Weapon = ParseWeapon(Index)
    str = GetDatFile("weapons", DatType, Weapon)
	echo(str)
end
function GetFlingyDat(DatType, Index) -- FlingyDat,Flingy/[Index]의 [DatType] 값을 반환합니다.
	Flingy = ParseFlingy(Index)
    str = GetDatFile("flingy", DatType, Flingy)
	echo(str)
end
function GetSpritesDat(DatType, Index) -- SpritesDat,Sprite/[Index]의 [DatType] 값을 반환합니다.
	Sprite = ParseSprites(Index)
    str = GetDatFile("sprites", DatType, Sprite)
	echo(str)
end
function GetImagesDat(DatType, Index) -- ImagesDat,Image/[Index]의 [DatType] 값을 반환합니다.
	Image = ParseImage(Index)
    str = GetDatFile("images", DatType, Image)
	echo(str)
end
function GetUpgradesDat(DatType, Index) -- UpgradesDat,Upgrade/[Index]의 [DatType] 값을 반환합니다.
	Upgrade = ParseUpgrade(Index)
    str = GetDatFile("upgrades", DatType, Upgrade)
	echo(str)
end
function GetTechdataDat(DatType, Index) -- TechdataDat,Tech/[Index]의 [DatType] 값을 반환합니다.
	Tech = ParseTech(Index)
    str = GetDatFile("techdata", DatType, Tech)
	echo(str)
end
function GetOrdersDat(DatType, Index) -- OrdersDat,Order/[Index]의 [DatType] 값을 반환합니다.
	Order = ParseOrder(Index)
    str = GetDatFile("orders", DatType, Order)
	echo(str)
end