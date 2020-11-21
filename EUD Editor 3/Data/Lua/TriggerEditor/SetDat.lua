--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.UnitsDat
@param.Index.TrgUnit
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.UnitsDat
@param.Index.TrgUnit
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetUnitsDat(DatType, Index, Value, Modifier) --DatFile/UnitsDat,TrgUnit,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Unit = ParseUnit(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("units", DatType, Unit, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.WeaponsDat
@param.Index.Weapon
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.WeaponsDat
@param.Index.Weapon
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetWeaponsDat(DatType, Index, Value, Modifier) --DatFile/WeaponsDat,Weapon,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Weapon = ParseWeapon(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("weapons", DatType, Weapon, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.FlingyDat
@param.Index.Flingy
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.FlingyDat
@param.Index.Flingy
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetFlingyDat(DatType, Index, Value, Modifier) --DatFile/FlingyDat,Flingy,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Flingy = ParseFlingy(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("flingy", DatType, Flingy, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.SpritesDat
@param.Index.Sprite
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.SpritesDat
@param.Index.Sprite
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetSpritesDat(DatType, Index, Value, Modifier) --DatFile/SpritesDat,Sprite,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Sprite = ParseSprites(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("sprites", DatType, Sprite, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.ImagesDat
@param.Index.Image
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.ImagesDat
@param.Index.Image
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetImagesDat(DatType, Index, Value, Modifier) --DatFile/ImagesDat,Image,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Image = ParseImages(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("images", DatType, Image, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.UpgradesDat
@param.Index.Upgrade
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.UpgradesDat
@param.Index.Upgrade
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetUpgradesDat(DatType, Index, Value, Modifier) --DatFile/UpgradesDat,Upgrade,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Upgrade = ParseUpgrades(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("upgrades", DatType, Upgrade, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.TechdataDat
@param.Index.Tech
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.TechdataDat
@param.Index.Tech
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetTechdataDat(DatType, Index, Value, Modifier) --DatFile/TechdataDat,Tech,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Tech = ParseTech(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("techdata", DatType, Tech, Value, Modifier)
	echo(str)
end

--[================================[
@Language.ko-KR
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.OrdersDat
@param.Index.Order
@param.Value.Number
@param.Modifier.TrgModifier


@Language.us-EN
@Summary
[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
@Group
DatFile
@param.DatType.OrdersDat
@param.Index.Order
@param.Value.Number
@param.Modifier.TrgModifier
]================================]
function SetOrdersDat(DatType, Index, Value, Modifier) --DatFile/OrdersDat,Order,Number,TrgModifier/[Index]의 [DatType]의 값을 [Value]로 [Modifier]합니다.
	Order = ParseOrder(Index)
	Modifier = ParseModifier(Modifier)
    str = SetDatFile("orders", DatType, Order, Value, Modifier)
	echo(str)
end