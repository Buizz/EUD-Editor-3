--[================================[
@Language.ko-KR
@Summary
버튼을 다시그립니다.
@Group
일반


@Language.en-US
@Summary
Refresh the button.
@Group
Basic
]================================]
function BtnRefresh()
	echo([[if (Memory(0x628438, AtLeast, 0x59CCA8)) {
    const checkCCMU = Memory(0x628438, Exactly, 0);
    const restoreBtn = SetMemoryX(0x6615AA, SetTo, 0, 0xFFFF0000);
    const setRemoveTimer = SetMemoryXEPD(0, SetTo, 1, 0xFFFF); 
    maskread_epd(EPD(0x6615AA), 0xFFFF0000, ret=list(EPD(restoreBtn) + 5));
    cunitepdread_epd(EPD(0x628438), ret=list(EPD(checkCCMU) + 2, EPD(setRemoveTimer) + 4));
    DoActions(list(
        SetMemoryX(0x6615AA, SetTo, 0x20000, 0xFFFF0000),
        CreateUnit(1, 73, 64, 7)
    ));
    if (!checkCCMU) {
        DoActions(list(
            restoreBtn,
            SetMemory(setRemoveTimer + 16, Add, 0x110/4),
            setRemoveTimer,
        ));
    }
}]])
end
