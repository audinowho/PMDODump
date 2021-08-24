using System;
using System.Threading;
using System.Globalization;
using RogueEssence.Content;
using RogueEssence.Data;
using RogueEssence.Dev;
using RogueEssence.Dungeon;
using RogueEssence.Script;
using RogueEssence;
using PMDC.Dev;
using PMDC.Data;
using DataGenerator.Data;

namespace DataGenerator
{
    class Program
    {
        static void Main()
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            string[] args = Environment.GetCommandLineArgs();
            PathMod.InitExePath(System.IO.Path.GetDirectoryName(args[0]));
            DiagManager.InitInstance();
            DiagManager.Instance.UpgradeBinder = new UpgradeBinder();

            try
            {
                DiagManager.Instance.LogInfo("=========================================");
                DiagManager.Instance.LogInfo(String.Format("SESSION STARTED: {0}", String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)));
                DiagManager.Instance.LogInfo("Version: " + Versioning.GetVersion().ToString());
                DiagManager.Instance.LogInfo(Versioning.GetDotNetInfo());
                DiagManager.Instance.LogInfo("=========================================");


                GraphicsManager.InitParams();

                DiagManager.Instance.DevMode = true;

                Text.Init();
                Text.SetCultureCode("");


                LuaEngine.InitInstance();

                DataManager.InitInstance();
                DataManager.Instance.InitData();

                ZoneInfo.AddZoneData();
                RogueEssence.Dev.DevHelper.RunIndexing(DataManager.DataType.Zone);
            }
            catch (Exception ex)
            {
                DiagManager.Instance.LogError(ex);
                throw;
            }
        }
    }
}
