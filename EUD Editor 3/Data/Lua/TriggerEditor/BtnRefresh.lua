--[================================[
@Language.ko-KR
@Summary
버튼을 다시그립니다.
@Group
일반


@Language.us-EN
@Summary
버튼을 다시그립니다.
@Group
일반
]================================]
function BtnRefresh()
	echo([[const btntemp1 = wread_epd(EPD(0x6615AA), 2);
SetMemoryX(0x6615AA, SetTo, 0x20000, 0xFFFF0000);
const btntemp2, btntemp3 = cunitepdread_epd(EPD(0x628438));
CreateUnit(1, 73, 64, 7);
if(!Memory(0x628438, Exactly, btntemp2)) {
wwrite_epd(btntemp3 + 0x110/4, 0, 1);
wwrite_epd(EPD(0x6615AA), 2, btntemp1);
}]])
end