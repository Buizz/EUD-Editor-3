--[================================[
@Language.ko-KR
@Summary
현재 카운트 타이머를 반환합니다.
@Group
시스템


@Language.us-EN
@Summary
현재 카운트 타이머를 반환합니다.
@Group
시스템
]================================]
function GetCounterTime() --일반//현재 카운트 타이머를 반환합니다.
	echo("dwread_epd(EPD(0x58D6F4))")
end

--[================================[
@Language.ko-KR
@Summary
현재 카운트 타이머 주소를 반환합니다.
@Group
시스템


@Language.us-EN
@Summary
현재 카운트 타이머 주소를 반환합니다.
@Group
시스템
]================================]
function CounterTimeOffset() --일반//현재 카운트 타이머 주소를 반환합니다.
	echo("0x58D6F4")
end
