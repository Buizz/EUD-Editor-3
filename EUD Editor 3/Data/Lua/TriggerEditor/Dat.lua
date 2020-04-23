function UnitsDat(DatType, Index, Value, Modifier) -- UnitsDat,TrgUnit,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Unit = ParseUnit(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("units", DatType, Unit, Value, Modifier)
	echo(str)
end
function WeaponsDat(DatType, Index, Value, Modifier) -- WeaponsDat,Weapon,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Weapon = ParseWeapon(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("weapons", DatType, Weapon, Value, Modifier)
	echo(str)
end
function FlingyDat(DatType, Index, Value, Modifier) -- FlingyDat,Flingy,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Flingy = ParseFlingy(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("flingy", DatType, Flingy, Value, Modifier)
	echo(str)
end
function SpritesDat(DatType, Index, Value, Modifier) -- SpritesDat,Sprite,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Sprite = ParseSprites(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("sprites", DatType, Sprite, Value, Modifier)
	echo(str)
end
function ImagesDat(DatType, Index, Value, Modifier) -- ImagesDat,Image,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Image = ParseImage(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("images", DatType, Image, Value, Modifier)
	echo(str)
end
function UpgradesDat(DatType, Index, Value, Modifier) -- UpgradesDat,Upgrade,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Upgrade = ParseUpgrade(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("upgrades", DatType, Upgrade, Value, Modifier)
	echo(str)
end
function TechdataDat(DatType, Index, Value, Modifier) -- TechdataDat,Tech,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Tech = ParseTech(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("techdata", DatType, Tech, Value, Modifier)
	echo(str)
end
function OrdersDat(DatType, Index, Value, Modifier) -- OrdersDat,Order,Number,TrgModifier/[Index]의 [DatType]의 값이 [Modifier] [Value]인지 판단합니다.
	Order = ParseOrder(Index)
	Modifier = ParseModifier(Modifier)
    str = ConditionDatFile("orders", DatType, Order, Value, Modifier)
	echo(str)
end