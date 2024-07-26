--[================================[
@Language.ko-KR
@Summary
[Key]를 안전한 문자로 변경합니다.
@Group
키인식
@param.Key.Key


@Language.en-US
@Summary
[Key]를 안전한 문자로 변경합니다.
@Group
키인식
@param.Key.Key
]================================]
function KeyParse(Key)
	index = -1
	spkeys = {"NUMPAD*", "NUMPAD+", "NUMPAD-", "NUMPAD.", "NUMPAD/", "*", "+", "-", ".", "/", "=", ",", "`", "[", "]", "\\", "'"}
	for k,v in pairs(spkeys) do
		if v == Key then
			index = k
			break
		end
	end

	if index ~= -1 then
		Key = "SP" .. index
	end

	return Key
end



--[================================[
@Language.ko-KR
@Summary
[Player]의 키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.en-US
@Summary
[Player]의 키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyDown(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyDown_" .. KeyParse(Key)
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyDown", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.en-US
@Summary
[Player]의 키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyUp(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyUp_" .. KeyParse(Key)
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyUp", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.en-US
@Summary
[Player]의 키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyPress(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyPress_" .. KeyParse(Key)
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyPress", "NotTyping")

	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.en-US
@Summary
[Player]의 마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button
]================================]
function MouseDown(Player, Button)
	Player = ParsePlayer(Player)
	keyarray = "VMouseDown_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MouseDown", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 마우스 [Button]를 놓는 순간을 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.en-US
@Summary
[Player]의 마우스 [Button]를 놓는 순간을 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button
]================================]
function MouseUp(Player, Button)
	Player = ParsePlayer(Player)
	keyarray = "VMouseUp_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MouseUp", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 마우스 [Button]를 누르고 있는지 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.en-US
@Summary
[Player]의 마우스 [Button]를 누르고 있는지 감지합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button
]================================]
function MousePress(Player, Button)
	Player = ParsePlayer(Player)
	keyarray = "VMousePress_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MousePress", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
마우스가 위치한 텍스트의 줄을 반환합니다.
@Group
마우스


@Language.en-US
@Summary
마우스가 위치한 텍스트의 줄을 반환합니다.
@Group
마우스
]================================]
function LocalMouseLine()
	echo("((dwread_epd(EPD(0x6CDDC8)) - 94) / 16)")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 마우스가 [Line]번째 줄을 X좌표 [MinX] ~ [MaxX]에서 클릭했을 경우를 인식합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Line.Number
라인입니다. 1 ~ 11이 유효합니다.
@param.MinX.Number
X좌표 최소 범위입니다.
@param.MaxX.Number
X좌표 최대 범위입니다.


@Language.en-US
@Summary
[Player]의 마우스가 [Line]번째 줄을 X좌표 [MinX] ~ [MaxX]에서 클릭했을 경우를 인식합니다.
@Group
마우스
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Line.Number
라인입니다. 1 ~ 11이 유효합니다.
@param.MinX.Number
X좌표 최소 범위입니다.
@param.MaxX.Number
X좌표 최대 범위입니다.
]================================]
function MouseClickLine(Player, Line, MinX, MaxX)
	Player = ParsePlayer(Player)
	clickindex = AddMouseEvent(Line, MinX, MaxX)

	echo("MemoryEPD(EPD(msqcvar.VMouseClickIndex" .. clickindex .. ") + " .. Player .. ", Exactly, 1)")
end
