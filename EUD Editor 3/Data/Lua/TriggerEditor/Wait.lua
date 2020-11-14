waittime = 0
function Wait(Time) --대기하기/Number/프레임 [Time]만큼 기다립니다.
	waittime = waittime + Time
	echo("}else if(WaitTimer == " .. waittime .. "){//")
end

function WaitStart() --대기하기//대기하기의 시작 부분입니다.
	waittime = 0
	echo("{static var WaitTimer = 0;")
end

function WaitConditionStart() --대기하기//대기하기 조건 부의 시작 부분입니다.
	echo("if (WaitTimer == 0 &&")
end

function WaitConditionEnd() --대기하기//대기하기 조건 부의 끝 부분입니다.
	echo("){WaitTimer = 1;}")
end

function WaitActionStart() --대기하기//대기하기 액션 부의 시작 부분입니다.
	echo("if(WaitTimer > 0){if (WaitTimer == 1){")
end

function WaitActionEnd() --대기하기//대기하기 액션 부의 끝 부분입니다.
	echo("}WaitTimer += 1;")
end

function WaitEnd() --대기하기//대기하기의 끝 부분입니다.
	echo("if (WaitTimer > " .. waittime .. "){WaitTimer = 0;}}}")
end