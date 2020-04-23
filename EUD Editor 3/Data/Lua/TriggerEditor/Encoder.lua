-- Code from TrigEditPlus
-- https://github.com/phu54321/TrigEditPlus/blob/master/TrigEditPlus/Editor/Lua/basescript/constparser.lua

All = {__trg_magic="trgconst"}
Enemy = {__trg_magic="trgconst"}
Ally = {__trg_magic="trgconst"}
AlliedVictory = {__trg_magic="trgconst"}
AtLeast = {__trg_magic="trgconst"}
AtMost = {__trg_magic="trgconst"}
Exactly = {__trg_magic="trgconst"}
SetTo = {__trg_magic="trgconst"}
Add = {__trg_magic="trgconst"}
Subtract = {__trg_magic="trgconst"}
Move = {__trg_magic="trgconst"}
Patrol = {__trg_magic="trgconst"}
Attack = {__trg_magic="trgconst"}
P1 = {__trg_magic="trgconst"}
P2 = {__trg_magic="trgconst"}
P3 = {__trg_magic="trgconst"}
P4 = {__trg_magic="trgconst"}
P5 = {__trg_magic="trgconst"}
P6 = {__trg_magic="trgconst"}
P7 = {__trg_magic="trgconst"}
P8 = {__trg_magic="trgconst"}
P9 = {__trg_magic="trgconst"}
P10 = {__trg_magic="trgconst"}
P11 = {__trg_magic="trgconst"}
P12 = {__trg_magic="trgconst"}
CurrentPlayer = {__trg_magic="trgconst"}
Foes = {__trg_magic="trgconst"}
Allies = {__trg_magic="trgconst"}
NeutralPlayers = {__trg_magic="trgconst"}
AllPlayers = {__trg_magic="trgconst"}
Force1 = {__trg_magic="trgconst"}
Force2 = {__trg_magic="trgconst"}
Force3 = {__trg_magic="trgconst"}
Force4 = {__trg_magic="trgconst"}
NonAlliedVictoryPlayers = {__trg_magic="trgconst"}
Enable = {__trg_magic="trgconst"}
Disable = {__trg_magic="trgconst"}
Toggle = {__trg_magic="trgconst"}
Ore = {__trg_magic="trgconst"}
Gas = {__trg_magic="trgconst"}
OreAndGas = {__trg_magic="trgconst"}
Total = {__trg_magic="trgconst"}
Units = {__trg_magic="trgconst"}
Buildings = {__trg_magic="trgconst"}
UnitsAndBuildings = {__trg_magic="trgconst"}
Razings = {__trg_magic="trgconst"}
KillsAndRazings = {__trg_magic="trgconst"}
Custom = {__trg_magic="trgconst"}
Set = {__trg_magic="trgconst"}
Clear = {__trg_magic="trgconst"}
Random = {__trg_magic="trgconst"}
Cleared = {__trg_magic="trgconst"}
Kills = {__trg_magic="trgconst"}

local AllyStatusDict = {
    [Enemy] = 0,
    [Ally] = 1,
    [AlliedVictory] = 2,
}

local ComparisonDict = {
    [AtLeast] =  0,
    [AtMost] =  1,
    [Exactly] =  10,
}

local ModifierDict = {
    [SetTo] =  7,
    [Add] =  8,
    [Subtract] =  9,
}

local OrderDict = {
    [Move] =  0,
    [Patrol] =  1,
    [Attack] =  2,
}

local PlayerDict = {
    [P1] =  0,
    [P2] =  1,
    [P3] =  2,
    [P4] =  3,
    [P5] =  4,
    [P6] =  5,
    [P7] =  6,
    [P8] =  7,
    [P9] =  8,
    [P10] =  9,
    [P11] =  10,
    [P12] =  11,
    [CurrentPlayer] =  13,
    [Foes] =  14,
    [Allies] =  15,
    [NeutralPlayers] =  16,
    [AllPlayers] =  17,
    [Force1] =  18,
    [Force2] =  19,
    [Force3] =  20,
    [Force4] =  21,
    [NonAlliedVictoryPlayers] =  26,
}

local PropStateDict = {
    [Enable] =  4,
    [Disable] =  5,
    [Toggle] =  6,
}

local ResourceDict = {
    [Ore] =  0,
    [Gas] =  1,
    [OreAndGas] =  2,
}

local ScoreDict = {
    [Total] =  0,
    [Units] =  1,
    [Buildings] =  2,
    [UnitsAndBuildings] =  3,
    [Kills] =  4,
    [Razings] =  5,
    [KillsAndRazings] =  6,
    [Custom] =  7,
}

local SwitchActionDict = {
    [Set] =  4,
    [Clear] =  5,
    [Toggle] =  6,
    [Random] =  11,
}

local SwitchStateDict = {
    [Set] =  2,
    [Cleared] =  3,
}


local AIScriptDict = {
    ["Terran Custom Level"] = 1967344980,
    ["Zerg Custom Level"] = 1967344986,
    ["Protoss Custom Level"] = 1967344976,
    ["Terran Expansion Custom Level"] = 2017676628,
    ["Zerg Expansion Custom Level"] = 2017676634,
    ["Protoss Expansion Custom Level"] = 2017676624,
    ["Terran Campaign Easy"] = 1716472916,
    ["Terran Campaign Medium"] = 1145392468,
    ["Terran Campaign Difficult"] = 1716078676,
    ["Terran Campaign Insane"] = 1347769172,
    ["Terran Campaign Area Town"] = 1163018580,
    ["Zerg Campaign Easy"] = 1716472922,
    ["Zerg Campaign Medium"] = 1145392474,
    ["Zerg Campaign Difficult"] = 1716078682,
    ["Zerg Campaign Insane"] = 1347769178,
    ["Zerg Campaign Area Town"] = 1163018586,
    ["Protoss Campaign Easy"] = 1716472912,
    ["Protoss Campaign Medium"] = 1145392464,
    ["Protoss Campaign Difficult"] = 1716078672,
    ["Protoss Campaign Insane"] = 1347769168,
    ["Protoss Campaign Area Town"] = 1163018576,
    ["Expansion Terran Campaign Easy"] = 2018462804,
    ["Expansion Terran Campaign Medium"] = 2017807700,
    ["Expansion Terran Campaign Difficult"] = 2018068564,
    ["Expansion Terran Campaign Insane"] = 2018857812,
    ["Expansion Terran Campaign Area Town"] = 2018656596,
    ["Expansion Zerg Campaign Easy"] = 2018462810,
    ["Expansion Zerg Campaign Medium"] = 2017807706,
    ["Expansion Zerg Campaign Difficult"] = 2018068570,
    ["Expansion Zerg Campaign Insane"] = 2018857818,
    ["Expansion Zerg Campaign Area Town"] = 2018656602,
    ["Expansion Protoss Campaign Easy"] = 2018462800,
    ["Expansion Protoss Campaign Medium"] = 2017807696,
    ["Expansion Protoss Campaign Difficult"] = 2018068560,
    ["Expansion Protoss Campaign Insane"] = 2018857808,
    ["Expansion Protoss Campaign Area Town"] = 2018656592,
    ["Send All Units on Strategic Suicide Missions"] = 1667855699,
    ["Send All Units on Random Suicide Missions"] = 1382643027,
    ["Switch Computer Player to Rescue Passive"] = 1969451858,
    ["Turn ON Shared Vision for Player 1"] = 812209707,
    ["Turn ON Shared Vision for Player 2"] = 828986923,
    ["Turn ON Shared Vision for Player 3"] = 845764139,
    ["Turn ON Shared Vision for Player 4"] = 862541355,
    ["Turn ON Shared Vision for Player 5"] = 879318571,
    ["Turn ON Shared Vision for Player 6"] = 896095787,
    ["Turn ON Shared Vision for Player 7"] = 912873003,
    ["Turn ON Shared Vision for Player 8"] = 929650219,
    ["Turn OFF Shared Vision for Player 1"] = 812209709,
    ["Turn OFF Shared Vision for Player 2"] = 828986925,
    ["Turn OFF Shared Vision for Player 3"] = 845764141,
    ["Turn OFF Shared Vision for Player 4"] = 862541357,
    ["Turn OFF Shared Vision for Player 5"] = 879318573,
    ["Turn OFF Shared Vision for Player 6"] = 896095789,
    ["Turn OFF Shared Vision for Player 7"] = 912873005,
    ["Turn OFF Shared Vision for Player 8"] = 929650221,
    ["Move Dark Templars to Region"] = 1700034125,
    ["Clear Previous Combat Data"] = 1131572291,
    ["Set Player to Enemy"] = 2037214789,
    ["Set Player to Ally  "] = 2037148737,
    ["Value This Area Higher"] = 1098214486,
    ["Enter Closest Bunker"] = 1799515717,
    ["Set Generic Command Target"] = 1733588051,
    ["Make These Units Patrol"] = 1951429715,
    ["Enter Transport"] = 1918135877,
    ["Exit Transport"] = 1918138437,
    ["AI Nuke Here"] = 1699247438,
    ["AI Harass Here"] = 1699242312,
    ["Set Unit Order To: Junk Yard Dog"] = 1732532554,
    ["Disruption Web Here"] = 1699239748,
    ["Recall Here"] = 1699243346,
    ["Terran 3 - Zerg Town"] = 863135060,
    ["Terran 5 - Terran Main Town"] = 896689492,
    ["Terran 5 - Terran Harvest Town"] = 1211458900,
    ["Terran 6 - Air Attack Zerg"] = 913466708,
    ["Terran 6 - Ground Attack Zerg"] = 1647732052,
    ["Terran 6 - Zerg Support Town"] = 1664509268,
    ["Terran 7 - Bottom Zerg Town"] = 930243924,
    ["Terran 7 - Right Zerg Town"] = 1933010260,
    ["Terran 7 - Middle Zerg Town"] = 1832346964,
    ["Terran 8 - Confederate Town"] = 947021140,
    ["Terran 9 - Light Attack"] = 1278833236,
    ["Terran 9 - Heavy Attack"] = 1211724372,
    ["Terran 10 - Confederate Towns"] = 808543572,
    ["Terran 11 - Zerg Town"] = 2050044244,
    ["Terran 11 - Lower Protoss Town"] = 1630613844,
    ["Terran 11 - Upper Protoss Town"] = 1647391060,
    ["Terran 12 - Nuke Town"] = 1311912276,
    ["Terran 12 - Phoenix Town"] = 1345466708,
    ["Terran 12 - Tank Town"] = 1412575572,
    ["Terran 1 - Electronic Distribution"] = 826557780,
    ["Terran 2 - Electronic Distribution"] = 843334996,
    ["Terran 3 - Electronic Distribution"] = 860112212,
    ["Terran 1 - Shareware"] = 827806548,
    ["Terran 2 - Shareware"] = 844583764,
    ["Terran 3 - Shareware"] = 861360980,
    ["Terran 4 - Shareware"] = 878138196,
    ["Terran 5 - Shareware"] = 894915412,
    ["Zerg 1 - Terran Town"] = 829580634,
    ["Zerg 2 - Protoss Town"] = 846357850,
    ["Zerg 3 - Terran Town"] = 863135066,
    ["Zerg 4 - Right Terran Town"] = 879912282,
    ["Zerg 4 - Lower Terran Town"] = 1395942746,
    ["Zerg 6 - Protoss Town"] = 913466714,
    ["Zerg 7 - Air Town"] = 1631023706,
    ["Zerg 7 - Ground Town"] = 1731687002,
    ["Zerg 7 - Support Town"] = 1933013594,
    ["Zerg 8 - Scout Town"] = 947021146,
    ["Zerg 8 - Templar Town"] = 1412982106,
    ["Zerg 9 - Teal Protoss"] = 963798362,
    ["Zerg 9 - Left Yellow Protoss"] = 2037135706,
    ["Zerg 9 - Right Yellow Protoss"] = 2037528922,
    ["Zerg 9 - Left Orange Protoss"] = 1869363546,
    ["Zerg 9 - Right Orange Protoss"] = 1869756762,
    ["Zerg 10 - Left Teal (Attack"] = 1630548314,
    ["Zerg 10 - Right Teal (Support"] = 1647325530,
    ["Zerg 10 - Left Yellow (Support"] = 1664102746,
    ["Zerg 10 - Right Yellow (Attack"] = 1680879962,
    ["Zerg 10 - Red Protoss"] = 1697657178,
    ["Protoss 1 - Zerg Town"] = 829387344,
    ["Protoss 2 - Zerg Town"] = 846164560,
    ["Protoss 3 - Air Zerg Town"] = 1379103312,
    ["Protoss 3 - Ground Zerg Town"] = 1194553936,
    ["Protoss 4 - Zerg Town"] = 879718992,
    ["Protoss 5 - Zerg Town Island"] = 1228239440,
    ["Protoss 5 - Zerg Town Base"] = 1110798928,
    ["Protoss 7 - Left Protoss Town"] = 930050640,
    ["Protoss 7 - Right Protoss Town"] = 1110930000,
    ["Protoss 7 - Shrine Protoss"] = 1396142672,
    ["Protoss 8 - Left Protoss Town"] = 946827856,
    ["Protoss 8 - Right Protoss Town"] = 1110995536,
    ["Protoss 8 - Protoss Defenders"] = 1144549968,
    ["Protoss 9 - Ground Zerg"] = 963605072,
    ["Protoss 9 - Air Zerg"] = 1463382608,
    ["Protoss 9 - Spell Zerg"] = 1496937040,
    ["Protoss 10 - Mini-Towns"] = 808546896,
    ["Protoss 10 - Mini-Town Master"] = 1127231824,
    ["Protoss 10 - Overmind Defenders"] = 1865429328,
    ["Brood Wars Protoss 1 - Town A"] = 1093747280,
    ["Brood Wars Protoss 1 - Town B"] = 1110524496,
    ["Brood Wars Protoss 1 - Town C"] = 1127301712,
    ["Brood Wars Protoss 1 - Town D"] = 1144078928,
    ["Brood Wars Protoss 1 - Town E"] = 1160856144,
    ["Brood Wars Protoss 1 - Town F"] = 1177633360,
    ["Brood Wars Protoss 2 - Town A"] = 1093812816,
    ["Brood Wars Protoss 2 - Town B"] = 1110590032,
    ["Brood Wars Protoss 2 - Town C"] = 1127367248,
    ["Brood Wars Protoss 2 - Town D"] = 1144144464,
    ["Brood Wars Protoss 2 - Town E"] = 1160921680,
    ["Brood Wars Protoss 2 - Town F"] = 1177698896,
    ["Brood Wars Protoss 3 - Town A"] = 1093878352,
    ["Brood Wars Protoss 3 - Town B"] = 1110655568,
    ["Brood Wars Protoss 3 - Town C"] = 1127432784,
    ["Brood Wars Protoss 3 - Town D"] = 1144210000,
    ["Brood Wars Protoss 3 - Town E"] = 1160987216,
    ["Brood Wars Protoss 3 - Town F"] = 1177764432,
    ["Brood Wars Protoss 4 - Town A"] = 1093943888,
    ["Brood Wars Protoss 4 - Town B"] = 1110721104,
    ["Brood Wars Protoss 4 - Town C"] = 1127498320,
    ["Brood Wars Protoss 4 - Town D"] = 1144275536,
    ["Brood Wars Protoss 4 - Town E"] = 1161052752,
    ["Brood Wars Protoss 4 - Town F"] = 1177829968,
    ["Brood Wars Protoss 5 - Town A"] = 1094009424,
    ["Brood Wars Protoss 5 - Town B"] = 1110786640,
    ["Brood Wars Protoss 5 - Town C"] = 1127563856,
    ["Brood Wars Protoss 5 - Town D"] = 1144341072,
    ["Brood Wars Protoss 5 - Town E"] = 1161118288,
    ["Brood Wars Protoss 5 - Town F"] = 1177895504,
    ["Brood Wars Protoss 6 - Town A"] = 1094074960,
    ["Brood Wars Protoss 6 - Town B"] = 1110852176,
    ["Brood Wars Protoss 6 - Town C"] = 1127629392,
    ["Brood Wars Protoss 6 - Town D"] = 1144406608,
    ["Brood Wars Protoss 6 - Town E"] = 1161183824,
    ["Brood Wars Protoss 6 - Town F"] = 1177961040,
    ["Brood Wars Protoss 7 - Town A"] = 1094140496,
    ["Brood Wars Protoss 7 - Town B"] = 1110917712,
    ["Brood Wars Protoss 7 - Town C"] = 1127694928,
    ["Brood Wars Protoss 7 - Town D"] = 1144472144,
    ["Brood Wars Protoss 7 - Town E"] = 1161249360,
    ["Brood Wars Protoss 7 - Town F"] = 1178026576,
    ["Brood Wars Protoss 8 - Town A"] = 1094206032,
    ["Brood Wars Protoss 8 - Town B"] = 1110983248,
    ["Brood Wars Protoss 8 - Town C"] = 1127760464,
    ["Brood Wars Protoss 8 - Town D"] = 1144537680,
    ["Brood Wars Protoss 8 - Town E"] = 1161314896,
    ["Brood Wars Protoss 8 - Town F"] = 1178092112,
    ["Brood Wars Terran 1 - Town A"] = 1093747284,
    ["Brood Wars Terran 1 - Town B"] = 1110524500,
    ["Brood Wars Terran 1 - Town C"] = 1127301716,
    ["Brood Wars Terran 1 - Town D"] = 1144078932,
    ["Brood Wars Terran 1 - Town E"] = 1160856148,
    ["Brood Wars Terran 1 - Town F"] = 1177633364,
    ["Brood Wars Terran 2 - Town A"] = 1093812820,
    ["Brood Wars Terran 2 - Town B"] = 1110590036,
    ["Brood Wars Terran 2 - Town C"] = 1127367252,
    ["Brood Wars Terran 2 - Town D"] = 1144144468,
    ["Brood Wars Terran 2 - Town E"] = 1160921684,
    ["Brood Wars Terran 2 - Town F"] = 1177698900,
    ["Brood Wars Terran 3 - Town A"] = 1093878356,
    ["Brood Wars Terran 3 - Town B"] = 1110655572,
    ["Brood Wars Terran 3 - Town C"] = 1127432788,
    ["Brood Wars Terran 3 - Town D"] = 1144210004,
    ["Brood Wars Terran 3 - Town E"] = 1160987220,
    ["Brood Wars Terran 3 - Town F"] = 1177764436,
    ["Brood Wars Terran 4 - Town A"] = 1093943892,
    ["Brood Wars Terran 4 - Town B"] = 1110721108,
    ["Brood Wars Terran 4 - Town C"] = 1127498324,
    ["Brood Wars Terran 4 - Town D"] = 1144275540,
    ["Brood Wars Terran 4 - Town E"] = 1161052756,
    ["Brood Wars Terran 4 - Town F"] = 1177829972,
    ["Brood Wars Terran 5 - Town A"] = 1094009428,
    ["Brood Wars Terran 5 - Town B"] = 1110786644,
    ["Brood Wars Terran 5 - Town C"] = 1127563860,
    ["Brood Wars Terran 5 - Town D"] = 1144341076,
    ["Brood Wars Terran 5 - Town E"] = 1161118292,
    ["Brood Wars Terran 5 - Town F"] = 1177895508,
    ["Brood Wars Terran 6 - Town A"] = 1094074964,
    ["Brood Wars Terran 6 - Town B"] = 1110852180,
    ["Brood Wars Terran 6 - Town C"] = 1127629396,
    ["Brood Wars Terran 6 - Town D"] = 1144406612,
    ["Brood Wars Terran 6 - Town E"] = 1161183828,
    ["Brood Wars Terran 6 - Town F"] = 1177961044,
    ["Brood Wars Terran 7 - Town A"] = 1094140500,
    ["Brood Wars Terran 7 - Town B"] = 1110917716,
    ["Brood Wars Terran 7 - Town C"] = 1127694932,
    ["Brood Wars Terran 7 - Town D"] = 1144472148,
    ["Brood Wars Terran 7 - Town E"] = 1161249364,
    ["Brood Wars Terran 7 - Town F"] = 1178026580,
    ["Brood Wars Terran 8 - Town A"] = 1094206036,
    ["Brood Wars Terran 8 - Town B"] = 1110983252,
    ["Brood Wars Terran 8 - Town C"] = 1127760468,
    ["Brood Wars Terran 8 - Town D"] = 1144537684,
    ["Brood Wars Terran 8 - Town E"] = 1161314900,
    ["Brood Wars Terran 8 - Town F"] = 1178092116,
    ["Brood Wars Zerg 1 - Town A"] = 1093747290,
    ["Brood Wars Zerg 1 - Town B"] = 1110524506,
    ["Brood Wars Zerg 1 - Town C"] = 1127301722,
    ["Brood Wars Zerg 1 - Town D"] = 1144078938,
    ["Brood Wars Zerg 1 - Town E"] = 1160856154,
    ["Brood Wars Zerg 1 - Town F"] = 1177633370,
    ["Brood Wars Zerg 2 - Town A"] = 1093812826,
    ["Brood Wars Zerg 2 - Town B"] = 1110590042,
    ["Brood Wars Zerg 2 - Town C"] = 1127367258,
    ["Brood Wars Zerg 2 - Town D"] = 1144144474,
    ["Brood Wars Zerg 2 - Town E"] = 1160921690,
    ["Brood Wars Zerg 2 - Town F"] = 1177698906,
    ["Brood Wars Zerg 3 - Town A"] = 1093878362,
    ["Brood Wars Zerg 3 - Town B"] = 1110655578,
    ["Brood Wars Zerg 3 - Town C"] = 1127432794,
    ["Brood Wars Zerg 3 - Town D"] = 1144210010,
    ["Brood Wars Zerg 3 - Town E"] = 1160987226,
    ["Brood Wars Zerg 3 - Town F"] = 1177764442,
    ["Brood Wars Zerg 4 - Town A"] = 1093943898,
    ["Brood Wars Zerg 4 - Town B"] = 1110721114,
    ["Brood Wars Zerg 4 - Town C"] = 1127498330,
    ["Brood Wars Zerg 4 - Town D"] = 1144275546,
    ["Brood Wars Zerg 4 - Town E"] = 1161052762,
    ["Brood Wars Zerg 4 - Town F"] = 1177829978,
    ["Brood Wars Zerg 5 - Town A"] = 1094009434,
    ["Brood Wars Zerg 5 - Town B"] = 1110786650,
    ["Brood Wars Zerg 5 - Town C"] = 1127563866,
    ["Brood Wars Zerg 5 - Town D"] = 1144341082,
    ["Brood Wars Zerg 5 - Town E"] = 1161118298,
    ["Brood Wars Zerg 5 - Town F"] = 1177895514,
    ["Brood Wars Zerg 6 - Town A"] = 1094074970,
    ["Brood Wars Zerg 6 - Town B"] = 1110852186,
    ["Brood Wars Zerg 6 - Town C"] = 1127629402,
    ["Brood Wars Zerg 6 - Town D"] = 1144406618,
    ["Brood Wars Zerg 6 - Town E"] = 1161183834,
    ["Brood Wars Zerg 6 - Town F"] = 1177961050,
    ["Brood Wars Zerg 7 - Town A"] = 1094140506,
    ["Brood Wars Zerg 7 - Town B"] = 1110917722,
    ["Brood Wars Zerg 7 - Town C"] = 1127694938,
    ["Brood Wars Zerg 7 - Town D"] = 1144472154,
    ["Brood Wars Zerg 7 - Town E"] = 1161249370,
    ["Brood Wars Zerg 7 - Town F"] = 1178026586,
    ["Brood Wars Zerg 8 - Town A"] = 1094206042,
    ["Brood Wars Zerg 8 - Town B"] = 1110983258,
    ["Brood Wars Zerg 8 - Town C"] = 1127760474,
    ["Brood Wars Zerg 8 - Town D"] = 1144537690,
    ["Brood Wars Zerg 8 - Town E"] = 1161314906,
    ["Brood Wars Zerg 8 - Town F"] = 1178092122,
    ["Brood Wars Zerg 9 - Town A"] = 1094271578,
    ["Brood Wars Zerg 9 - Town B"] = 1111048794,
    ["Brood Wars Zerg 9 - Town C"] = 1127826010,
    ["Brood Wars Zerg 9 - Town D"] = 1144603226,
    ["Brood Wars Zerg 9 - Town E"] = 1161380442,
    ["Brood Wars Zerg 9 - Town F"] = 1178157658,
    ["Brood Wars Zerg 10 - Town A"] = 1093681754,
    ["Brood Wars Zerg 10 - Town B"] = 1110458970,
    ["Brood Wars Zerg 10 - Town C"] = 1127236186,
    ["Brood Wars Zerg 10 - Town D"] = 1144013402,
    ["Brood Wars Zerg 10 - Town E"] = 1160790618,
    ["Brood Wars Zerg 10 - Town F"] = 1177567834,
}




local function EncodeConst(d, s)
    local val = d[s]
    if val == nil then
        return s
    else
        return val
    end
end


function EncodeAllyStatus(s)
    return EncodeConst(AllyStatusDict, s)
end


function EncodeComparison(s)
    return EncodeConst(ComparisonDict, s)
end


function EncodeModifier(s)
    return EncodeConst(ModifierDict, s)
end


function EncodeOrder(s)
    return EncodeConst(OrderDict, s)
end


function EncodePlayer(s)
    return EncodeConst(PlayerDict, s)
end


function EncodePropState(s)
    return EncodeConst(PropStateDict, s)
end


function EncodeResource(s)
    return EncodeConst(ResourceDict, s)
end


function EncodeScore(s)
    return EncodeConst(ScoreDict, s)
end


function EncodeSwitchAction(s)
    return EncodeConst(SwitchActionDict, s)
end


function EncodeSwitchState(s)
    return EncodeConst(SwitchStateDict, s)
end


function EncodeAIScript(s)
    return EncodeConst(AIScriptDict, s)
end


function EncodeCount(s)
    if s == All then
        return 0
    else
        return s
    end
end


--- Aliases

ParseConst = EncodeConst
ParseAllyStatus = EncodeAllyStatus
ParseComparison = EncodeComparison
ParseModifier = EncodeModifier
ParseOrder = EncodeOrder
ParsePlayer = EncodePlayer
ParsePropState = EncodePropState
ParseResource = EncodeResource
ParseScore = EncodeScore
ParseSwitchAction = EncodeSwitchAction
ParseSwitchState = EncodeSwitchState
ParseAIScript = EncodeAIScript
ParseCount = EncodeCount