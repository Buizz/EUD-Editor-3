--[================================[
@Language.ko-KR
@Summary
SCA런처의 로그 창에 특정 텍스트를 출력합니다.
@Group
SCA
@param.logtext.string
출력할 텍스트입니다.

@Language.en-US
@Summary
SCA런처의 로그 창에 특정 텍스트를 출력합니다.
@Group
SCA
@param.logtext.string
출력할 텍스트입니다.
]================================]
function sca_log_print(logtext)
end

--[================================[
@Language.ko-KR
@Summary
SCA런처의 로그 창에 에러 텍스트를 출력합니다.
@Group
SCA
@param.logtext.string
출력할 텍스트입니다.

@Language.en-US
@Summary
SCA런처의 로그 창에 에러 텍스트를 출력합니다.
@Group
SCA
@param.logtext.string
출력할 텍스트입니다.
]================================]
function sca_log_error_print(logtext)
end

--[================================[
@Language.ko-KR
@Summary
값을 맵으로 반환하는 함수입니다.
맵에서 Lua 함수를 호출 시 자동으로 사용됩니다.
수동으로 호출하여 값을 여러 개 넘기는 식의 이용이 가능합니다.
반환 테이블의 인덱스가 10개를 넘길 수 없으며 배열은 사용 가능하나
이중 배열은 사용 불가능합니다.
@Group
SCA
@param.funcnum.int
로그 출력 용 함수 번호입니다.
@param.returnindex.int
반환 받을 때 사용하는 index 값입니다.
@param.table.table
값을 넘길 테이블입니다.

@Language.en-US
@Summary
값을 맵으로 반환하는 함수입니다.
맵에서 Lua 함수를 호출 시 자동으로 사용됩니다.
수동으로 호출하여 값을 여러 개 넘기는 식의 이용이 가능합니다.
반환 테이블의 인덱스가 10개를 넘길 수 없으며 배열은 사용 가능하나
이중 배열은 사용 불가능합니다.
@Group
SCA
@param.funcnum.int
로그 출력 용 함수 번호입니다.
@param.returnindex.int
반환 받을 때 사용하는 index 값입니다.
@param.table.table
값을 넘길 테이블입니다.
]================================]
function sys_return_val(funcnum, returnindex, table)
end

--[================================[
@Language.ko-KR
@Summary
현재 플레이중인 플레이어들의 SCAID 리스트를 불러옵니다.
@Group
SCA

@Language.en-US
@Summary
현재 플레이중인 플레이어들의 SCAID 리스트를 불러옵니다.
@Group
SCA
]================================]
function sca_get_room_SCAID()
end

--[================================[
@Language.ko-KR
@Summary
오버레이를 시작합니다.
@Group
SCA

@Language.en-US
@Summary
오버레이를 시작합니다.
@Group
SCA
]================================]
function sca_overlay_start()
end

--[================================[
@Language.ko-KR
@Summary
오버레이를 종료합니다.
@Group
SCA

@Language.en-US
@Summary
오버레이를 종료합니다.
@Group
SCA
]================================]
function sca_overlay_stop()
end

--[================================[
@Language.ko-KR
@Summary
유즈맵에서 저장된 변수를 씁니다.
@Group
SCA
@param.key.string
변수의 이름입니다.
@param.value.uint
저장할 값입니다.

@Language.en-US
@Summary
유즈맵에서 저장된 변수를 씁니다.
@Group
SCA
@param.key.string
변수의 이름입니다.
@param.value.uint
저장할 값입니다.
]================================]
function sca_write_script_variable(key, value)
end

--[================================[
@Language.ko-KR
@Summary
유즈맵에 저장된 변수를 값을 읽습니다.
@Group
SCA
@param.key.string
변수의 이름입니다.

@Language.en-US
@Summary
유즈맵에 저장된 변수를 값을 읽습니다.
@Group
SCA
@param.key.string
변수의 이름입니다.
]================================]
function sca_read_script_variable(key)
end

--[================================[
@Language.ko-KR
@Summary
로컬 저장 폴더에 파일을 씁니다. 내 문서의 SCArchive폴더에 저장됩니다.
@Group
SCA
@param.filename.string
파일 이름입니다. 특수문자는 들어 갈 수 없습니다.
@param.data.table
넣을 데이터 입니다.
@param.isencryption.bool
암호화 여부입니다.

@Language.en-US
@Summary
로컬 저장 폴더에 파일을 씁니다. 내 문서의 SCArchive폴더에 저장됩니다.
@Group
SCA
@param.filename.string
파일 이름입니다. 특수문자는 들어 갈 수 없습니다.
@param.data.table
넣을 데이터 입니다.
@param.isencryption.bool
암호화 여부입니다.
]================================]
function sca_write_file(filename, data, isencryption)
end

--[================================[
@Language.ko-KR
@Summary
로컬 저 폴더의 파일을 읽습니다. 내 문서의 SCArchive폴더에 있는 파일을 읽습니다.
@Group
SCA
@param.filename.string
파일 이름입니다. 특수문자는 들어 갈 수 없습니다.
@param.isencryption.bool
암호화 여부입니다.

@Language.en-US
@Summary
로컬 저 폴더의 파일을 읽습니다. 내 문서의 SCArchive폴더에 있는 파일을 읽습니다.
@Group
SCA
@param.filename.string
파일 이름입니다. 특수문자는 들어 갈 수 없습니다.
@param.isencryption.bool
암호화 여부입니다.
]================================]
function sca_read_file(filename, isencryption)
end

--[================================[
@Language.ko-KR
@Summary
브러쉬를 추가합니다.
@Group
SCA
@param.key.string
오브젝트의 이름입니다.
@param.a.byte
투명도. (0~255)
@param.r.byte
빨강. (0~255)
@param.g.byte
초록. (0~255)
@param.b.byte
파랑. (0~255)

@Language.en-US
@Summary
브러쉬를 추가합니다.
@Group
SCA
@param.key.string
오브젝트의 이름입니다.
@param.a.byte
투명도. (0~255)
@param.r.byte
빨강. (0~255)
@param.g.byte
초록. (0~255)
@param.b.byte
파랑. (0~255)
]================================]
function sca_add_brush(key, a, r, g, b)
end

--[================================[
@Language.ko-KR
@Summary
컨트롤을 화면에 넣습니다.
@Group
SCA
@param.window.string
컨트롤을 넣을 창입니다.
@param.control.control
화면에 넣을 컨트롤 입니다.

@Language.en-US
@Summary
컨트롤을 화면에 넣습니다.
@Group
SCA
@param.window.string
컨트롤을 넣을 창입니다.
@param.control.control
화면에 넣을 컨트롤 입니다.
]================================]
function sca_add_object(window, control)
end

--[================================[
@Language.ko-KR
@Summary
화면에서 컨트롤을 찾아 반환합니다.
@Group
SCA
@param.window.string
컨트롤을 찾을 창입니다.
@param.key.string
컨트롤의 이름입니다.

@Language.en-US
@Summary
화면에서 컨트롤을 찾아 반환합니다.
@Group
SCA
@param.window.string
컨트롤을 찾을 창입니다.
@param.key.string
컨트롤의 이름입니다.
]================================]
function sca_get_object(window, key)
end

--[================================[
@Language.ko-KR
@Summary
화면에서 컨트롤을 찾아 제거합니다
@Group
SCA
@param.window.string
컨트롤을 찾을 창입니다.
@param.key.string
컨트롤의 이름입니다.

@Language.en-US
@Summary
화면에서 컨트롤을 찾아 제거합니다
@Group
SCA
@param.window.string
컨트롤을 찾을 창입니다.
@param.key.string
컨트롤의 이름입니다.
]================================]
function sca_remove_object(window, key)
end

--[================================[
@Language.ko-KR
@Summary
버튼을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.

@Language.en-US
@Summary
버튼을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.
]================================]
function sca_create_button(key, options)
end

--[================================[
@Language.ko-KR
@Summary
창을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.

@Language.en-US
@Summary
창을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.
]================================]
function sca_create_window(key, options)
end

--[================================[
@Language.ko-KR
@Summary
이미지를 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.

@Language.en-US
@Summary
이미지를 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.
]================================]
function sca_create_image(key, options)
end

--[================================[
@Language.ko-KR
@Summary
TextLabel을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.

@Language.en-US
@Summary
TextLabel을 생성합니다.
@Group
SCA
@param.key.string
컨트롤의 이름입니다.
@param.options.string
컨트롤의 속성입니다.
]================================]
function sca_create_textlabel(key, options)
end





--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드가 재생 중인지 확인합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
@Summary
key를 가진 사운드가 재생 중인지 확인합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
]================================]
function sca_is_sound_playing(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드가 존재 중인지 확인합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
key를 가진 사운드가 존재 중인지 확인합니다.
@Group
SCA@Summary

@param.key.string
사운드의 key입니다.
]================================]
function sca_is_sound_exist(key)
end

--[================================[
@Language.ko-KR
@Summary
스크립트에서 출력하는 음량을 조절합니다.
@Group
SCA
@param.volume.float
음량입니다. 0~1이 범위입니다.

@Language.en-US
@Summary
스크립트에서 출력하는 음량을 조절합니다.
@Group
SCA
@param.volume.float
음량입니다. 0~1이 범위입니다.
]================================]
function sca_set_script_volume(volume)
end

--[================================[
@Language.ko-KR
@Summary
특정 사운드에서 출력하는 음량을 조절합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.volume.float
음량입니다. 0~1이 범위입니다.

@Language.en-US
@Summary
특정 사운드에서 출력하는 음량을 조절합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.volume.float
음량입니다. 0~1이 범위입니다.
]================================]
function sca_set_sound_volume(key, volume)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드를 중지합니다. 재생 시점이 초기화됩니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
@Summary
key를 가진 사운드를 중지합니다. 재생 시점이 초기화됩니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
]================================]
function sca_sound_stop(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드를 다시 재생합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
@Summary
key를 가진 사운드를 다시 재생합니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
]================================]
function sca_sound_resume(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드를 멈춥니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
@Summary
key를 가진 사운드를 멈춥니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
]================================]
function sca_sound_pause(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드를 서서히 킵니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.frame.int
완료 될 때 까지의 프레임입니다.

@Language.en-US
@Summary
key를 가진 사운드를 서서히 킵니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.frame.int
완료 될 때 까지의 프레임입니다.
]================================]
function sca_sound_fadein(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드를 서서히 끕니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.frame.int
완료 될 때 까지의 프레임입니다.

@Language.en-US
@Summary
key를 가진 사운드를 서서히 끕니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
@param.frame.int
완료 될 때 까지의 프레임입니다.
]================================]
function sca_sound_fadeout(key)
end

--[================================[
@Language.ko-KR
@Summary
soundfile을 key로 재생합니다.
@Group
SCA
@param.soundfile.SoundFile
사운드파일의 이름입니다.
@param.key.string
사운드의 key입니다.
@param.type.string
'se','me','bgs','bgm'
@param.volume.float
시작 음량 크기입니다. 0~1의 값을 가집니다.

@Language.en-US
@Summary
key를 가진 사운드가 재생 중인지 확인합니다.
@Group
SCA
@param.soundfile.SoundFile
사운드파일의 이름입니다.
@param.key.string
사운드의 key입니다.
@param.type.string
'se','me','bgs','bgm'
@param.volume.float
시작 음량 크기입니다. 0~1의 값을 가집니다.
]================================]
function sca_soundplay(soundfile ,key, type, volume)
end

--[================================[
@Language.ko-KR
@Summary
soundfile을 key로 재생합니다. 범위를 설정 할 수 있습니다.
@Group
SCA
@param.soundfile.SoundFile
사운드파일의 이름입니다.
@param.key.string
사운드의 key입니다.
@param.type.string
'se','me','bgs','bgm'
@param.volume.float
시작 음량 크기입니다. 0~1의 값을 가집니다.
@param.range.table
{{x, y, range},{x, y, range},...}

@Language.en-US
@Summary
key를 가진 사운드가 재생 중인지 확인합니다.
@Group
SCA
@param.soundfile.SoundFile
사운드파일의 이름입니다.
@param.key.string
사운드의 key입니다.
@param.type.string
'se','me','bgs','bgm'
@param.volume.float
시작 음량 크기입니다. 0~1의 값을 가집니다.
@param.range.table
{{x, y, range},{x, y, range},...}
]================================]
function sca_soundplay_range(soundfile ,key, type, volume, range)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.key.string
사운드의 key입니다.

@Language.en-US
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.key.string
사운드의 key입니다.
]================================]
function sca_get_rangetable(key)
end

--[================================[
@Language.ko-KR
@Summary
현재 플레이어를 해당 맵에서 Ban합니다.
@Group
SCA

@Language.en-US
@Summary
현재 플레이어를 해당 맵에서 Ban합니다.
@Group
SCA
]================================]
function sca_ban()
end


--[================================[
@Language.ko-KR
@Summary
SCA서버에 데이터를 저장합니다.
@Group
SCA
@param.slot.string
저장할 슬롯입니다.
@param.data.string
data[key] = data:index의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key로도 저장됩니다.

@Language.en-US
@Summary
SCA서버에 데이터를 저장합니다.
@Group
SCA
@param.slot.string
저장할 슬롯입니다.
@param.data.string
data[key] = data:index의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key로도 저장됩니다.
]================================]
function sca_save(slot, data)
end

--[================================[
@Language.ko-KR
@Summary
SCA서버에서 데이터를 읽어옵니다.
@Group
SCA
@param.slot.string
읽어올 슬롯입니다.
@param.loadtags.string
loadtags[i] = key의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key는 읽어 올 수 없습니다.

@Language.en-US
@Summary
SCA서버에서 데이터를 읽어옵니다.
@Group
SCA
@param.slot.string
읽어올 슬롯입니다.
@param.loadtags.string
loadtags[i] = key의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key는 읽어 올 수 없습니다.
]================================]
function sca_load(slot, loadtags)
end

--[================================[
@Language.ko-KR
@Summary
SCA 글로벌 데이터를 로드합니다.
@Group
SCA

@Language.en-US
@Summary
SCA 글로벌 데이터를 로드합니다.
@Group
SCA
]================================]
function sca_global_load(key)
end

--[================================[
@Language.ko-KR
@Summary
SCA 서버의 시간을 가져옵니다.
@Group
SCA

@Language.en-US
@Summary
SCA 서버의 시간을 가져옵니다.
@Group
SCA
]================================]
function sca_time_load(key)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.scaid.uint
불러올 유저의 scaid입니다.
@param.slot.int
읽어올 슬롯입니다.
@param.loadtags.table
loadtags[i] = key의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key는 읽어 올 수 없습니다.

@Language.en-US
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.scaid.uint
불러올 유저의 scaid입니다.
@param.slot.int
읽어올 슬롯입니다.
@param.loadtags.table
loadtags[i] = key의 형태로 저장할 값을 넘겨줍니다. 서버에 등록되지 않은 key는 읽어 올 수 없습니다.

]================================]
function sca_load_from_scaid(scaid, slot, loadtags)
end

--[================================[
@Language.ko-KR
@Summary
SCA 순위표를 가져옵니다.
@Group
SCA
@param.loadtags.table
읽어 올 tag를 설정합니다.
datakey[i] = key의 형태입니다.
@param.start.int
첫 순위입니다.
@param.end.int
마지막 순위입니다.

@Language.en-US
@Summary
SCA 순위표를 가져옵니다.
@Group
SCA
@param.loadtags.table
읽어 올 tag를 설정합니다.
datakey[i] = key의 형태입니다.
@param.start.int
첫 순위입니다.
@param.end.int
마지막 순위입니다.
]================================]
function sca_get_scoreboard(loadtags, start, end)
end

--[================================[
@Language.ko-KR
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.loadtags.table
읽어 올 tag를 설정합니다.
loadtags[i] = key의 형태입니다.
@param.searchtag.string
검색할 기준의 값입니다.
@param.min.int
최소 값입니다.
@param.max.int
최대 값입니다.

@Language.en-US
@Summary
key를 가진 사운드의 rangetable을 가져옵니다.
@Group
SCA
@param.loadtags.table
읽어 올 tag를 설정합니다.
loadtags[i] = key의 형태입니다.
@param.searchtag.string
검색할 기준의 값입니다.
@param.min.int
최소 값입니다.
@param.max.int
최대 값입니다.
]================================]
function sca_search_user(loadtags, searchtag, min, max)
end

