--[================================[
@Language.ko-KR
@Summary
게임 진행 시간을 반환합니다.
@Group
시스템


@Language.us-EN
@Summary
게임 진행 시간을 반환합니다.
@Group
시스템
]================================]
function GetElapsedTime() --일반//게임 진행 시간을 반환합니다.
	echo("dwread_epd(EPD(0x58D6F8))")
end

--[================================[
@Language.ko-KR
@Summary
게임 진행 시간 주소를 반환합니다.
@Group
시스템


@Language.us-EN
@Summary
게임 진행 시간 주소를 반환합니다.
@Group
시스템
]================================]
function ElapsedTimeOffset() --일반//게임 진행 시간 주소를 반환합니다.
	echo("0x58D6F8")
end
