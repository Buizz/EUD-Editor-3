function ChatEvent(Chat) --채팅인식/TrgString/채팅 [Chat]를 인식합니다.
	chatindex = AddChatEventPlugin(Chat)
	--chatarray = "VChat_" .. chatindex
	chatarray = "VChatIndex"

	
	echo("MemoryEPD(EPD(msqcvar." .. chatarray .. ") + getcurpl(), Exactly, " .. chatindex + 1 ..")")
end

function ChatEventPattern(Start,Mid,End) --채팅인식/TrgString,TrgString,TrgString/채팅[Start],[Mid],[End]를 인식합니다.
	Chat = "^" .. Start .. ".*" .. Mid .. ".*" .. End .. "$"
	chatindex = AddChatEventPlugin(Chat)
	--chatarray = "VChat_" .. chatindex
	chatarray = "VChatIndex"

	
	echo("MemoryEPD(EPD(msqcvar." .. chatarray .. ") + getcurpl(), Exactly, " .. chatindex + 1 ..")")
end

