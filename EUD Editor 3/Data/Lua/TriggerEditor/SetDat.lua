function SetUnitsDat(DatType, Index, Value, Modifier) -- UnitsDat,TrgUnit,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Unit = ParseUnit(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("units", DatType, Unit, Value, Modifier)
	echo(str)
end
function SetWeaponsDat(DatType, Index, Value, Modifier) -- WeaponsDat,Weapon,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Weapon = ParseWeapon(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("weapons", DatType, Weapon, Value, Modifier)
	echo(str)
end
function SetFlingyDat(DatType, Index, Value, Modifier) -- FlingyDat,Flingy,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Flingy = ParseFlingy(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("flingy", DatType, Flingy, Value, Modifier)
	echo(str)
end
function SetSpritesDat(DatType, Index, Value, Modifier) -- SpritesDat,Sprite,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Sprite = ParseSprites(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("sprites", DatType, Sprite, Value, Modifier)
	echo(str)
end
function SetImagesDat(DatType, Index, Value, Modifier) -- ImagesDat,Image,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Image = ParseImage(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("images", DatType, Image, Value, Modifier)
	echo(str)
end
function SetUpgradesDat(DatType, Index, Value, Modifier) -- UpgradesDat,Upgrade,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Upgrade = ParseUpgrade(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("upgrades", DatType, Upgrade, Value, Modifier)
	echo(str)
end
function SetTechdataDat(DatType, Index, Value, Modifier) -- TechdataDat,Tech,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Tech = ParseTech(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("techdata", DatType, Tech, Value, Modifier)
	echo(str)
end
function SetOrdersDat(DatType, Index, Value, Modifier) -- OrdersDat,Order,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Order = ParseOrder(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("orders", DatType, Order, Value, Modifier)
	echo(str)
end