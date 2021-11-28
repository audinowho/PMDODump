import os

def write_script(script_path):
    """
    Generates a generic init lua file for zone exiting
    """
    num = script_path.split('_')[-1]
    file_name = os.path.join(script_path, "init.lua")
    with open(file_name, 'w', encoding='utf-8') as txt:
        txt.write("require 'common'\n" + \
            "\n" + \
            "local zone_"+num+" = {}\n" + \
            "--------------------------------------------------\n" + \
            "-- Map Callbacks\n" + \
            "--------------------------------------------------\n" + \
            "function zone_"+num+".Init(map)\n" + \
            "  DEBUG.EnableDbgCoro() --Enable debugging this coroutine\n" + \
            "  print(\"=>> Init_zone_"+num+"\")\n" + \
            "  \n" + \
            "\n" + \
            "end\n" + \
            "\n" + \
            "function zone_"+num+".ExitSegment(zone, result, segmentID)\n" + \
            "  DEBUG.EnableDbgCoro() --Enable debugging this coroutine\n" + \
            "  print(\"=>> ExitSegment_zone_"+num+" result \"..tostring(result)..\" segment \"..tostring(segmentID))\n" + \
            "  \n" + \
            "  if result ~= PMDOrigins.Data.GameProgress.ResultType.Cleared then\n" + \
            "    COMMON.EndDungeonDay(result, SV.checkpoint.Zone, SV.checkpoint.Structure, SV.checkpoint.Map, SV.checkpoint.Entry)\n" + \
            "  else\n" + \
            "    if segmentID == 0 then\n" + \
            "      COMMON.EndDungeonDay(result, 1, -1, 1, 0)\n" + \
            "    else\n" + \
            "      print(\"No exit procedure found!\")\n" + \
            "    end\n" + \
            "  end\n" + \
            "  \n" + \
            "end\n" + \
            "\n" + \
            "return zone_"+num+"\n")
