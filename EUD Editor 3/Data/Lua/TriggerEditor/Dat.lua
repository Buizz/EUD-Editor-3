--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.UnitsDat
비교할 파라미터입니다.
@param.Index.TrgUnit
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.UnitsDat
비교할 파라미터입니다.
@param.Index.TrgUnit
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function UnitsDat(DatType, Index, Value, Comparison) --DatFile/UnitsDat,TrgUnit,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Unit = ParseUnit(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("units", DatType, Unit, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.WeaponsDat
비교할 파라미터입니다.
@param.Index.Weapon
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.WeaponsDat
비교할 파라미터입니다.
@param.Index.Weapon
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function WeaponsDat(DatType, Index, Value, Comparison) --DatFile/WeaponsDat,Weapon,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Weapon = ParseWeapon(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("weapons", DatType, Weapon, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.FlingyDat
비교할 파라미터입니다.
@param.Index.Flingy
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.FlingyDat
비교할 파라미터입니다.
@param.Index.Flingy
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function FlingyDat(DatType, Index, Value, Comparison) --DatFile/FlingyDat,Flingy,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Flingy = ParseFlingy(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("flingy", DatType, Flingy, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.SpritesDat
비교할 파라미터입니다.
@param.Index.Sprite
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.SpritesDat
비교할 파라미터입니다.
@param.Index.Sprite
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function SpritesDat(DatType, Index, Value, Comparison) --DatFile/SpritesDat,Sprite,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Sprite = ParseSprites(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("sprites", DatType, Sprite, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.ImagesDat
비교할 파라미터입니다.
@param.Index.Image
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.ImagesDat
비교할 파라미터입니다.
@param.Index.Image
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function ImagesDat(DatType, Index, Value, Comparison) --DatFile/ImagesDat,Image,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Image = ParseImage(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("images", DatType, Image, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.UpgradesDat
비교할 파라미터입니다.
@param.Index.Upgrade
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.UpgradesDat
비교할 파라미터입니다.
@param.Index.Upgrade
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function UpgradesDat(DatType, Index, Value, Comparison) --DatFile/UpgradesDat,Upgrade,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Upgrade = ParseUpgrade(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("upgrades", DatType, Upgrade, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.TechdataDat
비교할 파라미터입니다.
@param.Index.Tech
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.TechdataDat
비교할 파라미터입니다.
@param.Index.Tech
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function TechdataDat(DatType, Index, Value, Comparison) --DatFile/TechdataDat,Tech,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Tech = ParseTech(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("techdata", DatType, Tech, Value, Comparison)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.OrdersDat
비교할 파라미터입니다.
@param.Index.Order
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.


@Language.us-EN
@Summary
[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
@Group
DatFile
@param.DatType.OrdersDat
비교할 파라미터입니다.
@param.Index.Order
비교경할 오브젝트입니다.
@param.Value.Number
값입니다.
@param.Comparison.TrgComparison
비교 방식입니다.
]================================]
function OrdersDat(DatType, Index, Value, Comparison) --DatFile/OrdersDat,Order,Number,TrgComparison/[Index]의 [DatType]의 값이 [Comparison] [Value]인지 판단합니다.
	Order = ParseOrder(Index)
	Comparison = ParseComparison(Comparison)
    str = ConditionDatFile("orders", DatType, Order, Value, Comparison)
	echo(str)
end