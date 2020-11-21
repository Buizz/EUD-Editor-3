--[================================[
@Language.ko-KR
@Summary
키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Key.Key
]================================]
function KeyDown(Key) --키인식/Key/키 [Key]가 눌리는 순간을 감지합니다.
	keyarray = "VKeyDown_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyDown", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]가 놓는 순간을 감지합니다.
@Group
키인식
@param.Key.Key
]================================]
function KeyUp(Key) --키인식/Key/키 [Key]를 놓는 순간을 감지합니다.
	keyarray = "VKeyUp_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyUp", "NotTyping")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Key.Key


@Language.us-EN
@Summary
키 [Key]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Key.Key
]================================]
function KeyPress(Key) --키인식/Key/키 [Key]를 누르고 있는지 감지합니다.
	keyarray = "VKeyPress_" .. Key
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Key, keyarray, "KeyPress", "NotTyping")

	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]가 눌리는 순간을 감지합니다.
@Group
키인식
@param.Button.Button
]================================]
function MouseDown(Button) --마우스인식/Button/마우스 [Button]가 눌리는 순간을 감지합니다.
	keyarray = "VMouseDown_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MouseDown", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
마우스 [Button]를 놓는 순간을 감지합니다.
@Group
키인식
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]를 놓는 순간을 감지합니다.
@Group
키인식
@param.Button.Button
]================================]
function MouseUp(Button) --마우스인식/Button/마우스 [Button]를 놓는 순간을 감지합니다.
	keyarray = "VMouseUp_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MouseUp", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end

--[================================[
@Language.ko-KR
@Summary
마우스 [Button]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Button.Button


@Language.us-EN
@Summary
마우스 [Button]를 누르고 있는지 감지합니다.
@Group
키인식
@param.Button.Button
]================================]
function MosePress(Button) --마우스인식/Button/마우스 [Button]를 누르고 있는지 감지합니다.
	keyarray = "VMousePress_" .. Button
	--AddMSQCPlugin("NotTyping ; KeyDown(" .. Key .. ") : " .. keyarray .. ", 1")
	AddMSQCPlugin(Button, keyarray, "MousePress", "")


	echo("MemoryEPD(EPD(msqcvar." .. keyarray .. ") + getcurpl(), Exactly, 1)")
end
