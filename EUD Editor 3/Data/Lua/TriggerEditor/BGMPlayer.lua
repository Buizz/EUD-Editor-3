--[================================[
@Language.ko-KR
@Summary
해당플레이어의 BGM을 [BGMName]로 설정합니다.
@Group
BGM
@param.BGMName.BGM


@Language.en-US
@Summary
해당플레이어의 BGM을 [BGMName]로 설정합니다.
@Group
BGM
@param.BGMName.BGM
]================================]
function SetBGM(BGMName)
	preDefine("import TriggerEditor.BGMPlayerWrapper as bg;")
	onPluginText("bg.loadSound();")
	beforeText("bg.Exec();")

	--bindex = GetBGMIndex(BGMName)

	echo("bg.SetBGM(" .. GetReturnBGMIndex(BGMName) .. ")")
end

--[================================[
@Language.ko-KR
@Summary
해당플레이어의 BGM을 재생합니다.
@Group
BGM


@Language.en-US
@Summary
해당플레이어의 BGM을 재생합니다.
@Group
BGM
]================================]
function BGMStart()
	preDefine("import TriggerEditor.BGMPlayerWrapper as bg;")

	echo("bg.BGMStart()")
end

--[================================[
@Language.ko-KR
@Summary
해당플레이어의 BGM을 멈춥니다.
@Group
BGM


@Language.en-US
@Summary
해당플레이어의 BGM을 멈춥니다.
@Group
BGM
]================================]
function BGMStop()
	preDefine("import TriggerEditor.BGMPlayerWrapper as bg;")

	echo("bg.BGMStop()")
end

--[================================[
@Language.ko-KR
@Summary
해당플레이어의 BGM을 재개니다.
@Group
BGM


@Language.en-US
@Summary
해당플레이어의 BGM을 재개니다.
@Group
BGM
]================================]
function BGMResume()
	preDefine("import TriggerEditor.BGMPlayerWrapper as bg;")

	echo("bg.BGMResume()")
end

--[================================[
@Language.ko-KR
@Summary
해당플레이어의 BGM을 일시정지입니다.
@Group
BGM


@Language.en-US
@Summary
해당플레이어의 BGM을 일시정지입니다.
@Group
BGM
]================================]
function BGMPause()
	preDefine("import TriggerEditor.BGMPlayerWrapper as bg;")

	echo("bg.BGMPause()")
end

