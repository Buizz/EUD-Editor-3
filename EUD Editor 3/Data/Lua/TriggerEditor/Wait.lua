waittime = 0
--[================================[
@Language.ko-KR
@Summary
프레임 [Time]만큼 기다립니다.
@Group
대기하기
@param.Time.Number


@Language.us-EN
@Summary
프레임 [Time]만큼 기다립니다.
@Group
대기하기
@param.Time.Number
]================================]
function Wait(Time) --대기하기/Number/프레임 [Time]만큼 기다립니다.
	waittime = waittime + Time
	echo("}else if(WaitTimer == " .. waittime .. "){//")
end

--[================================[
@Language.ko-KR
@Summary
대기하기의 시작 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기의 시작 부분입니다.
@Group
대기하기
]================================]
function WaitStart() --대기하기//대기하기의 시작 부분입니다.
	waittime = 0
	echo("{static var WaitTimer = 0;")
end

--[================================[
@Language.ko-KR
@Summary
대기하기 조건 부의 시작 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기 조건 부의 시작 부분입니다.
@Group
대기하기
]================================]
function WaitConditionStart() --대기하기//대기하기 조건 부의 시작 부분입니다.
	echo("if (WaitTimer == 0 &&")
end

--[================================[
@Language.ko-KR
@Summary
대기하기 조건 부의 끝 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기 조건 부의 끝 부분입니다.
@Group
대기하기
]================================]
function WaitConditionEnd() --대기하기//대기하기 조건 부의 끝 부분입니다.
	echo("){WaitTimer = 1;}")
end

--[================================[
@Language.ko-KR
@Summary
대기하기 액션 부의 시작 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기 액션 부의 시작 부분입니다.
@Group
대기하기
]================================]
function WaitActionStart() --대기하기//대기하기 액션 부의 시작 부분입니다.
	echo("if(WaitTimer > 0){if (WaitTimer == 1){")
end

--[================================[
@Language.ko-KR
@Summary
대기하기 액션 부의 끝 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기 액션 부의 끝 부분입니다.
@Group
대기하기
]================================]
function WaitActionEnd() --대기하기//대기하기 액션 부의 끝 부분입니다.
	echo("}WaitTimer += 1;")
end

--[================================[
@Language.ko-KR
@Summary
대기하기의 끝 부분입니다.
@Group
대기하기


@Language.us-EN
@Summary
대기하기의 끝 부분입니다.
@Group
대기하기
]================================]
function WaitEnd() --대기하기//대기하기의 끝 부분입니다.
	echo("if (WaitTimer > " .. waittime .. "){WaitTimer = 0;}}}")
end