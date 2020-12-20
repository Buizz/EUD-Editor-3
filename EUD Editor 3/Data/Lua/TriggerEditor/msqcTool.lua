--[================================[
@Language.ko-KR
@Summary
키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyDown(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyDown_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyDown", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyUp(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyUp_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyUp", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Key.Key
]================================]
function KeyPress(Player, Key)
	Player = ParsePlayer(Player)
	keyarray = "VKeyPress_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyPress", "NotTyping")

	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
키인식
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
마우스 [Button]를 놓는 순간을 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]를 놓는 순간을 감지합니다.
@Group
키인식
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
마우스 [Button]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Button.Button
]================================]
function MosePress(Player, Button)
	Player = ParsePlayer(Player)
	keyarray = "VMousePress_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MousePress", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + " .. Player .. ", Exactly, 1)")
end
