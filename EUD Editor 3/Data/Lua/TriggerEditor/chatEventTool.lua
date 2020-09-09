function ChatEvent(Chat) --TrgString/채팅 [Chat]를 인식합니다. (^시작.**중간.**끝$)
	chatindex = AddChatEventPlugin(Chat)
	--chatarray = "VChat_" .. chatindex
	chatarray = "VChatIndex"

	
	echo("MemoryEPD(EPD(msqcvar." .. chatarray .. ") + getcurpl(), Exactly, " .. chatindex + 1 ..")")
end

