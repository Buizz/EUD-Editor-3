--[================================[
@Language.ko-KR
@Summary
[Player]의 아이디를 [ID][Args]로 변경합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.ID.FormatString
@param.Args.Arguments


@Language.us-EN
@Summary
[Player]의 아이디를 [ID][Args]로 변경합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.ID.FormatString
@param.Args.Arguments
]================================]
function SetPlayerID(Player, ID, Args) --플레이어/TrgPlayer,FormatString,Arguments/[Player]의 아이디를 [ID][Args]로 변경합니다.
	--SetPNamef(cp, "[플레이어] {:n}", cp);
	--SetPNamef(cp, "{:t} \x07레벨: \x04{} {:c}{:n}", title, level, cp, cp);

	if Args == "" then
		echo("SetPNamef(" .. Player .. ", \"" .. ID .. "\")")
	else
		echo("SetPNamef(" .. Player .. ", \"" .. ID .. "\", " .. Args .. ")")
	end
end

--[================================[
@Language.ko-KR
@Summary
[Player]의 아이디가 [ID]인지 확인합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.ID.TrgString


@Language.us-EN
@Summary
[Player]의 아이디가 [ID]인지 확인합니다.
@Group
플레이어
@param.Player.TrgPlayer
@param.ID.TrgString
]================================]
function PlayerID(Player, ID) --플레이어/TrgPlayer,TrgString/[Player]의 아이디가 [ID]인지 확인합니다.
	--IsPName(player, name)
	echo("IsPName(" .. Player .. ", \"" .. ID .. "\")")
end