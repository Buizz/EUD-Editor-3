function UnitsDat(DatType, Index, Value, Comparison) -- UnitsDat,TrgUnit,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Unit = ParseUnit(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("units", DatType, Unit, Value, Comparison)
	echo(str)
end
function WeaponsDat(DatType, Index, Value, Comparison) -- WeaponsDat,Weapon,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Weapon = ParseWeapon(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("weapons", DatType, Weapon, Value, Comparison)
	echo(str)
end
function FlingyDat(DatType, Index, Value, Comparison) -- FlingyDat,Flingy,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Flingy = ParseFlingy(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("flingy", DatType, Flingy, Value, Comparison)
	echo(str)
end
function SpritesDat(DatType, Index, Value, Comparison) -- SpritesDat,Sprite,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Sprite = ParseSprites(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("sprites", DatType, Sprite, Value, Comparison)
	echo(str)
end
function ImagesDat(DatType, Index, Value, Comparison) -- ImagesDat,Image,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Image = ParseImage(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("images", DatType, Image, Value, Comparison)
	echo(str)
end
function UpgradesDat(DatType, Index, Value, Comparison) -- UpgradesDat,Upgrade,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Upgrade = ParseUpgrade(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("upgrades", DatType, Upgrade, Value, Comparison)
	echo(str)
end
function TechdataDat(DatType, Index, Value, Comparison) -- TechdataDat,Tech,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Tech = ParseTech(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("techdata", DatType, Tech, Value, Comparison)
	echo(str)
end
function OrdersDat(DatType, Index, Value, Comparison) -- OrdersDat,Order,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Order = ParseOrder(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("orders", DatType, Order, Value, Comparison)
	echo(str)
end