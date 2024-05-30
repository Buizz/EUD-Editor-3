--[================================[
@Language.ko-KR
@Summary
해당플레이어의 선택 유닛의 버튼셋 ID가 [Comparison] [Amount]인지 확인합니다.
@Group
선택인식
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.en-US
@Summary
해당플레이어의 선택 유닛의 버튼셋 ID가 [Comparison] [Amount]인지 확인합니다.
@Group
선택인식
@param.Comparison.TrgComparison
@param.Amount.Number

]================================]
function LocalSelectID(Comparison, Amount)
    Comparison = ParseComparison(Comparison)

	rstr = string.format("MemoryEPD(EPD(0x68C14C), %s, %s)",Comparison, Amount)
	echo(rstr)
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
선택인식
스코어
@param.Comparison.TrgComparison
@param.Amount.Number


@Language.en-US
@Summary
[Player]의 [Score]가 [Comparison] [Amount]인지 확인합니다.
선택인식
스코어
@param.Comparison.TrgComparison
@param.Amount.Number
]================================]
function LocalSelectPtr(Comparison, Amount)
    Comparison = ParseComparison(Comparison)

	rstr = string.format("MemoryEPD(EPD(0x6284B8), %s, %s)", Comparison, Amount)
	echo(rstr)
end