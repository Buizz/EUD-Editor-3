--[================================[
@Language.ko-KR
@Summary
[Player]의 채팅[Chat]을 인식합니다.
@Group
채팅인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Chat.TrgString
인식할 채팅입니다.


@Language.us-EN
@Summary
[Player]의 채팅[Chat]을 인식합니다.
@Group
채팅인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Chat.TrgString
인식할 채팅입니다.
]================================]
function ChatEvent(Player ,Chat)
	Player = ParsePlayer(Player)
	chatindex = AddChatEventPlugin(Chat)
	--chatarray = "VChat_" .. chatindex
	chatarray = "VChatIndex"

	
	echo("MemoryEPD(EPD(msqcvar." .. chatarray .. ") + " .. Player .. ", Exactly, " .. chatindex + 1 ..")")
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 채팅[Start],[Mid],[End]를 인식합니다.
@Group
채팅인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Start.TrgString
시작 할 앞부분 입니다. 비어있으면 아무 내용이나 가능합니다.
@param.Mid.TrgString
가운대에 포함될 내용입니다.
@param.End.TrgString
끝 부분입니다. 비어있으면 아무 내용이나 가능합니다.


@Language.us-EN
@Summary
[Player]의 채팅[Start],[Mid],[End]를 인식합니다.
@Group
채팅인식
@param.Player.TrgPlayer
대상 플레이어입니다.
@param.Start.TrgString
시작 할 앞부분 입니다. 비어있으면 아무 내용이나 가능합니다.
@param.Mid.TrgString
가운대에 포함될 내용입니다.
@param.End.TrgString
끝 부분입니다. 비어있으면 아무 내용이나 가능합니다.
]================================]
function ChatEventPattern(Player ,Start ,Mid ,End)
	Player = ParsePlayer(Player)
	Chat = "^" .. Start .. ".*" .. Mid .. ".*" .. End .. "$"
	chatindex = AddChatEventPlugin(Chat)
	--chatarray = "VChat_" .. chatindex
	chatarray = "VChatIndex"

	
	echo("MemoryEPD(EPD(msqcvar." .. chatarray .. ") + " .. Player .. ", Exactly, " .. chatindex + 1 ..")")
end

