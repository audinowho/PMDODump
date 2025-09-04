using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Net;
using System.IO.Compression;
using Mono.Unix;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace PMDOSetup
{
    public class Release
    {
        public string Name { get; set; }
        public string Tag_Name {get; set;}
        public string Body {get; set;}
    }

    class Program
    {
        static string updaterPath;
        static string updaterExe;
        static string curVerRepo;
        static string assetSubmodule;
        static string exeSubmodule;
        static List<string> filesToDelete;
        static List<string> executableFiles;

        static string saveDir;
        static string saveBackupDir;

        static Version lastUpdaterVersion;
        static Version lastVersion;
        static int argNum;
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            updaterPath = Path.GetDirectoryName(args[0]) + "/";
            updaterExe = Path.GetFileName(args[0]);
            argNum = -1;
            if (args.Length > 1)
                Int32.TryParse(args[1], out argNum);
            //1: detect platform and load defaults
            curVerRepo = "audinowho/PMDODump";
            assetSubmodule = "DumpAsset";
            exeSubmodule = "PMDC";
            saveDir = "PMDO/SAVE";
            saveBackupDir = "SAVE.bak/";
            lastVersion = new Version(0, 0, 0, 0);
            filesToDelete = new List<string>();
            executableFiles = new List<string>();
            //2: load xml-filename, xml name, last version, exclusions - if possible
            LoadXml();

            try
            {
                while (true)
                {
                    Console.Clear();
                    ConsoleKey choice = ConsoleKey.Enter;

                    if (lastVersion == new Version(0, 0, 0, 0))
                    {
                        Console.WriteLine("Preparing PMDO installation...");
                    }
                    else
                    {
                        Console.WriteLine("Updater Options:");
                        Console.WriteLine("1: Force Update");
                        Console.WriteLine("2: Update the Updater");
                        Console.WriteLine("3: Reset Updater XML");
                        Console.WriteLine("4: Uninstall (Retain Save Data)");
                        Console.WriteLine("5: Revert to an Older Version");
                        Console.WriteLine("Press any other key to check for game updates.");
                        if (argNum > -1)
                        {
                            if (argNum == 1)
                                choice = ConsoleKey.D1;
                            else if (argNum == 2)
                                choice = ConsoleKey.D2;
                            else if (argNum == 3)
                                choice = ConsoleKey.D3;
                            else if (argNum == 4)
                                choice = ConsoleKey.D4;
                            else if (argNum == 5)
                                choice = ConsoleKey.D5;
                            else
                                choice = ConsoleKey.Enter;
                        }
                        else
                        {
                            ConsoleKeyInfo keyInfo = Console.ReadKey();
                            choice = keyInfo.Key;
                        }
                    }

                    Console.WriteLine();
                    bool force = false;
                    Release specificRelease = null;
                    if (choice == ConsoleKey.D1)
                        force = true;
                    else if (choice == ConsoleKey.D2)
                    {
                        UpdateUpdater();
                        return;
                    }
                    else if (choice == ConsoleKey.D3)
                    {
                        Console.WriteLine("Resetting XML");
                        DefaultXml();
                        SaveXml();
                        Console.WriteLine("Done.");
                        ReadKey();
                        return;
                    }
                    else if (choice == ConsoleKey.D4)
                    {
                        Console.WriteLine("Uninstalling...");
                        foreach (string inclusion in filesToDelete)
                            DeleteWithExclusions(Path.Join(updaterPath, inclusion));
                        Console.WriteLine("Done.");
                        ReadKey();
                        return;
                    }
                    else if (choice == ConsoleKey.D5)
                    {
                        force = true;
                        specificRelease = GetSpecificRelease();
                        if (specificRelease == null)
                            continue;
                    }

                    Update(force, specificRelease);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                ReadKey();
            }
        }

        static void UpdateUpdater()
        {
            string tempUpdater;
            //3: read from site what version is uploaded. if greater than the current version, upgrade
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                string latestResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases/latest", curVerRepo));
                JsonElement latestJson = JsonDocument.Parse(latestResponse).RootElement;

                string uploadedVersionStr = latestJson.GetProperty("name").GetString();
                string changelog = latestJson.GetProperty("body").GetString();
                Version nextVersion = new Version(uploadedVersionStr);

                string tagStr = latestJson.GetProperty("tag_name").GetString();
                Regex pattern = new Regex(@"https://github\.com/(?<repo>\w+/\w+).git");

                string updaterFile = null;
                {
                    JsonElement assetsJson = latestJson.GetProperty("assets");
                    foreach (JsonElement assetJson in assetsJson.EnumerateArray())
                    {
                        string assetName = assetJson.GetProperty("name").GetString();
                        if (assetName == String.Format("setup-{0}.zip", GetCurrentPlatform()))
                        {
                            updaterFile = assetJson.GetProperty("browser_download_url").GetString();
                            break;
                        }
                    }
                }

                Console.WriteLine("Version {0} will be downloaded from the endpoints:\n  {1}.\n\nPress any key to continue.", nextVersion, updaterFile);
                ReadKey();

                File.Move(Path.Join(updaterPath, updaterExe), Path.Join(updaterPath, updaterExe + ".bak"), true);
                File.Copy(Path.Join(updaterPath, updaterExe + ".bak"), Path.Join(updaterPath, updaterExe));

                //4: download the respective zip from specified location
                if (!Directory.Exists(Path.Join(updaterPath, "temp")))
                    Directory.CreateDirectory(Path.Join(updaterPath, "temp"));

                tempUpdater = Path.Join(updaterPath, "temp", "setup-" + Path.GetFileName(updaterFile));

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                Console.WriteLine("Downloading from {0} to {1}. May take a while...", updaterFile, tempUpdater);
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                DownloadIncomplete = true;
                wc.DownloadFileAsync(new Uri(updaterFile), tempUpdater);
                while (DownloadIncomplete)
                    Thread.Sleep(1);
                Console.WriteLine();
            }

            Console.WriteLine("Unzipping...");

            using (ZipArchive archive = ZipFile.OpenRead(tempUpdater))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    bool setPerms = Path.GetFileName(entry.FullName) == updaterExe;
                    string currentPlatform = GetCurrentPlatform();
                    if (currentPlatform == "windows-x64" || currentPlatform == "windows-x86")
                        setPerms = false;
                    string destPath = Path.GetFullPath(Path.Join(updaterPath, Path.GetFileName(entry.FullName)));
                    if (!destPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                    {
                        entry.ExtractToFile(destPath, true);

                        if (setPerms)
                        {
                            var info = new UnixFileInfo(destPath);
                            info.FileAccessPermissions = FileAccessPermissions.AllPermissions;
                            info.Refresh();
                        }
                    }
                }
            }

            Console.WriteLine("Cleaning up {0}...", tempUpdater);
            File.Delete(tempUpdater);

            Console.WriteLine("Done.");
            ReadKey();
        }

        static bool DownloadIncomplete;
        static object lockObj = new object();

        private static void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lock (lockObj)
            {
                (int Left, int Top) cursor = Console.GetCursorPosition();
                Console.SetCursorPosition(0, cursor.Top);
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, cursor.Top);
                if (e.TotalBytesToReceive > -1)
                    Console.Write(String.Format("Progress: {0} / {1}", FileSizeToPrettyString(e.BytesReceived), FileSizeToPrettyString(e.TotalBytesToReceive)));
                else
                    Console.Write(String.Format("Progress: {0}", FileSizeToPrettyString(e.BytesReceived)));
            }
        }
        private static void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadIncomplete = false;
        }


        static Release GetSpecificRelease()
        {
            //3: read from site what version is uploaded. if greater than the current version, upgrade
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                string builds = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases", curVerRepo));

                List<Release> releases = new List<Release>();

                JsonElement releasesJson = JsonDocument.Parse(builds).RootElement;
                foreach (JsonElement buildJson in releasesJson.EnumerateArray())
                {
                    Release specificRelease = new Release();
                    specificRelease.Name = buildJson.GetProperty("name").GetString();
                    specificRelease.Body = buildJson.GetProperty("body").GetString();
                    specificRelease.Tag_Name = buildJson.GetProperty("tag_name").GetString();
                    releases.Add(specificRelease);
                }

                int offset = 0;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Choose a Version:");
                    for (int ii = 1; ii + offset < releases.Count && ii < 10; ii++)
                        Console.WriteLine("{0}: {1}", ii, releases[ii + offset].Name);
                    Console.WriteLine("Left/Right Arrow Keys to browse, ESC to cancel");

                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.LeftArrow && offset > 0)
                        offset -= 9;
                    else if (keyInfo.Key == ConsoleKey.RightArrow && offset + 9 < releases.Count)
                        offset += 9;
                    else if (keyInfo.Key == ConsoleKey.Escape)
                        return null;
                    
                    int choice = keyInfo.Key - ConsoleKey.D0;

                    Console.WriteLine();
                    if (choice >= 1 && choice <= 9 && choice <= releases.Count)
                        return releases[choice + offset];
                }
            }
        }

        static void Update(bool force, Release specificRelease)
        {
            Version nextVersion;
            string exeFile = null;
            string tempExe, tempAsset;

            bool firstInstall = lastVersion == new Version(0, 0, 0, 0);

            //3: read from site what version is uploaded. if greater than the current version, upgrade
            using (var wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                if (specificRelease == null)
                {
                    string latestResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases/latest", curVerRepo));
                    JsonElement latestJson = JsonDocument.Parse(latestResponse).RootElement;
                    specificRelease = new Release();
                    specificRelease.Name = latestJson.GetProperty("name").GetString();
                    specificRelease.Body = latestJson.GetProperty("body").GetString();
                    specificRelease.Tag_Name = latestJson.GetProperty("tag_name").GetString();
                }

                nextVersion = new Version(specificRelease.Name);
                if (firstInstall)
                    Console.WriteLine("Installing {0}", nextVersion);
                else if (force)
                {
                    Console.WriteLine("Update will be forced. {0} -> {1}", lastVersion, nextVersion);
                }
                else
                {
                    if (lastVersion >= nextVersion)
                    {
                        Console.WriteLine("You are up to date. {0} >= {1}", lastVersion, nextVersion);
                        ReadKey();
                        return;
                    }
                    else
                        Console.WriteLine("Update available. {0} < {1}", lastVersion, nextVersion);
                }

                Console.WriteLine();
                Console.WriteLine(specificRelease.Body);
                Console.WriteLine();

                string needed_zip;
                if (nextVersion < new Version(0, 8, 11))
                    needed_zip = String.Format("{0}.zip", GetCurrentPlatform());
                else
                    needed_zip = String.Format("pmdc-{0}.zip", GetCurrentPlatform());

                Regex pattern = new Regex(@"https://github\.com/(?<repo>\w+/\w+).git");
                {
                    //Get the exe submodule's version (commit) at this tag
                    wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                    string exeSubmoduleResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/contents/{1}?ref={2}", curVerRepo, exeSubmodule, specificRelease.Tag_Name));
                    JsonElement exeSubmoduleJson = JsonDocument.Parse(exeSubmoduleResponse).RootElement;

                    string exeUrl = exeSubmoduleJson.GetProperty("submodule_git_url").GetString();
                    Match match = pattern.Match(exeUrl);
                    string exeRepo = match.Groups["repo"].Value;

                    string refStr = exeSubmoduleJson.GetProperty("sha").GetString();

                    //Get the tag associated with the commit (there'd better be one)
                    wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                    string exeTagsResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/tags", exeRepo));
                    JsonElement exeTagsJson = JsonDocument.Parse(exeTagsResponse).RootElement;
                    //TODO: the above request only gets the first 30 results in a paginated whole.  We technically want to iterate all tags to properly search for the one we want.
                    //https://docs.github.com/en/rest/guides/traversing-with-pagination
                    foreach (JsonElement tagJson in exeTagsJson.EnumerateArray())
                    {
                        string tagSha = tagJson.GetProperty("commit").GetProperty("sha").GetString();
                        string tagName = tagJson.GetProperty("name").GetString();
                        if (tagSha == refStr)
                        {
                            wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                            string tagReleasesResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases/tags/{1}", exeRepo, tagName));
                            JsonElement tagReleasesJson = JsonDocument.Parse(tagReleasesResponse).RootElement;

                            JsonElement exeJson = tagReleasesJson.GetProperty("assets");
                            foreach (JsonElement assetJson in exeJson.EnumerateArray())
                            {
                                string assetName = assetJson.GetProperty("name").GetString();
                                if (assetName == needed_zip)
                                {
                                    exeFile = assetJson.GetProperty("browser_download_url").GetString();
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                if (exeFile == null)
                    throw new Exception(String.Format("Could not find exe download for {0}.", GetCurrentPlatform()));

                string assetFile;
                {
                    //Get the asset submodule's version at this tag
                    wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                    string assetSubmoduleResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/contents/{1}?ref={2}", curVerRepo, assetSubmodule, specificRelease.Tag_Name));
                    JsonElement assetSubmoduleJson = JsonDocument.Parse(assetSubmoduleResponse).RootElement;

                    string assetUrl = assetSubmoduleJson.GetProperty("submodule_git_url").GetString();
                    Match match = pattern.Match(assetUrl);
                    string assetRepo = match.Groups["repo"].Value;

                    string refStr = assetSubmoduleJson.GetProperty("sha").GetString();

                    //get the download link for it
                    assetFile = String.Format("https://api.github.com/repos/{0}/zipball/{1}", assetRepo, refStr);
                }

                Console.WriteLine("Version {0} will be downloaded from the endpoints:\n  {1}\n  {2}.", nextVersion, exeFile, assetFile);

                Console.WriteLine();

                if (!firstInstall)
                    Console.WriteLine("WARNING: Updates will invalidate existing quicksaves and pending rescues.  Be sure to finish them first!");

                Console.WriteLine("Press any key to continue.");
                ReadKey();

                //4: download the respective zip from specified location
                if (!Directory.Exists(Path.Join(updaterPath, "temp")))
                    Directory.CreateDirectory(Path.Join(updaterPath, "temp"));
                tempExe = Path.Join(updaterPath, "temp", Path.GetFileName(exeFile));
                tempAsset = Path.Join(updaterPath, "temp", "Asset.zip");

                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                Console.WriteLine("Downloading from {0} to {1}. May take a while...", exeFile, tempExe);
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                DownloadIncomplete = true;
                wc.DownloadFileAsync(new Uri(exeFile), tempExe);
                while (DownloadIncomplete)
                    Thread.Sleep(1);
                Console.WriteLine();

                Console.WriteLine("Downloading from {0} to {1}. May take a while...", assetFile, tempAsset);
                wc.Headers.Add("user-agent", "PMDOSetup/0.1.0");
                DownloadIncomplete = true;
                wc.DownloadFileAsync(new Uri(assetFile), tempAsset);
                while (DownloadIncomplete)
                    Thread.Sleep(1);
                Console.WriteLine();
            }

            if (!firstInstall && Directory.Exists(saveDir))
            {
                Console.WriteLine("Backing up {0} to {1}...", saveDir, saveBackupDir);

                copyFilesRecursively(new DirectoryInfo(saveDir), new DirectoryInfo(saveBackupDir));
            }

            //Delete old files
            Console.WriteLine("Deleting old files...");
            foreach (string inclusion in filesToDelete)
                DeleteWithExclusions(Path.Join(updaterPath, inclusion));

            //Console.WriteLine("Adjusting filenames...");
            //unzip the exe, rename, then rezip just to rename the file... ugh
            //RenameRezip(tempExe);

            //5: unzip and delete by directory - if you want to save your data be sure to make an exception in the xml (this is done by default)
            Unzip(tempExe, null, 0);
            Unzip(tempAsset, "PMDO", 1);

            Console.WriteLine("Cleaning up {0}...", exeFile);
            File.Delete(tempExe);
            File.Delete(tempAsset);

            Console.WriteLine("Incrementing version,", exeFile);
            lastVersion = nextVersion;

            //6: create a new xml and save
            SaveXml();
            Console.WriteLine("Done.");
            if (firstInstall)
                Console.WriteLine("Future runs of this file will check for updates. This window can now be closed.");

            ReadKey();
        }

        static void Unzip(string tempExe, string destRoot, int sourceDepth)
        {
            using (ZipArchive archive = ZipFile.OpenRead(tempExe))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string[] pathPcs = entry.FullName.Split("/");
                    if (sourceDepth > pathPcs.Length - 1)
                        continue;

                    List<string> destPcs = new List<string>();
                    if (destRoot != null)
                        destPcs.Add(destRoot);
                    for (int ii = sourceDepth; ii < pathPcs.Length; ii++)
                        destPcs.Add(pathPcs[ii]);

                    string destName = String.Join("/", destPcs.ToArray());
                    bool setPerms = executableFiles.Contains(destName);
                    string destPath = Path.GetFullPath(Path.Join(updaterPath, Path.Combine(".", destName)));

                    string folderPath = Path.GetDirectoryName(destPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    if (!destPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                    {
                        entry.ExtractToFile(destPath, true);
                        if (setPerms)
                        {
                            var info = new UnixFileInfo(destPath);
                            info.FileAccessPermissions = FileAccessPermissions.AllPermissions;
                            info.Refresh();
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(destPath))
                            Directory.CreateDirectory(destPath);
                        Console.WriteLine("Unzipping {0}", entry.FullName);
                    }
                }
            }
        }


        static void RenameRezip(string tempExe)
        {
            string unzipPath = Path.Join(updaterPath, "temp", "unzip");
            Directory.CreateDirectory(unzipPath);
            using (ZipArchive archive = ZipFile.OpenRead(tempExe))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Gets the full path to ensure that relative segments are removed.
                    string destinationPath = Path.GetFullPath(Path.Join(unzipPath, entry.FullName));
                    if (!destinationPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        entry.ExtractToFile(destinationPath);
                    else
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);
                    }
                }
            }

            File.Delete(tempExe);

            if (Directory.Exists(Path.Join(unzipPath, "PMDC")))
                Directory.Move(Path.Join(unzipPath, "PMDC"), Path.Join(unzipPath, "PMDO"));

            if (File.Exists(Path.Join(unzipPath, "PMDO", "PMDC.exe")))
                File.Move(Path.Join(unzipPath, "PMDO", "PMDC.exe"), Path.Join(unzipPath, "PMDO", "PMDO.exe"));

            if (File.Exists(Path.Join(unzipPath, "PMDO", "PMDC")))
                File.Move(Path.Join(unzipPath, "PMDO", "PMDC"), Path.Join(unzipPath, "PMDO", "PMDO"));

            using (ZipArchive archive = ZipFile.Open(tempExe, ZipArchiveMode.Create))
                zipRecursive(archive, unzipPath, "");

            Directory.Delete(unzipPath, true);
        }

        private static void zipRecursive(ZipArchive archive, string unzipPath, string destPath)
        {
            foreach (string path in Directory.GetFiles(unzipPath))
            {
                string file = Path.GetFileName(path);
                string destFile = Path.Join(destPath, file);
                archive.CreateEntryFromFile(path, destFile);
            }
            foreach (string dir in Directory.GetDirectories(unzipPath))
            {
                string file = Path.GetFileName(dir);
                string destFile = Path.Join(destPath, file);
                zipRecursive(archive, Path.Join(unzipPath, file), destFile);
            }
        }
        private static void copyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                copyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        static void ReadKey()
        {
            if (argNum == -1)
                Console.ReadKey();
        }

        static void LoadXml()
        {
            try
            {
                string path = Path.GetFullPath(Path.Join(updaterPath, "Updater.xml"));
                if (File.Exists(path))
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(path);

                    lastUpdaterVersion = new Version(xmldoc.SelectSingleNode("Config/UpdaterVersion").InnerText);

                    curVerRepo = xmldoc.SelectSingleNode("Config/ExeRepo").InnerText;
                    assetSubmodule = xmldoc.SelectSingleNode("Config/Asset").InnerText;
                    lastVersion = new Version(xmldoc.SelectSingleNode("Config/LastVersion").InnerText);

                    filesToDelete.Clear();
                    XmlNode keys = xmldoc.SelectSingleNode("Config/ToDelete");
                    foreach (XmlNode key in keys.SelectNodes("Deletion"))
                        filesToDelete.Add(key.InnerText);

                    executableFiles.Clear();
                    XmlNode exes = xmldoc.SelectSingleNode("Config/Executables");
                    foreach (XmlNode key in exes.SelectNodes("Exe"))
                        executableFiles.Add(key.InnerText);

                }
                else
                {
                    DefaultXml();
                    SaveXml();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.Write("Continuing with default settings...");
                DefaultXml();
                SaveXml();
            }
        }

        static void DefaultXml()
        {
            curVerRepo = "audinowho/PMDODump";
            assetSubmodule = "DumpAsset";
            lastUpdaterVersion = new Version(0, 0, 0, 0);
            lastVersion = new Version(0, 0, 0, 0);
            filesToDelete = new List<string>();
            executableFiles = new List<string>();
            filesToDelete.Add("WaypointServer/");
            filesToDelete.Add("PMDO/Base/");
            filesToDelete.Add("PMDO/Content/");
            filesToDelete.Add("PMDO/Controls/");
            filesToDelete.Add("PMDO/Data/");
            filesToDelete.Add("PMDO/Editor/");
            filesToDelete.Add("PMDO/Licenses/");
            filesToDelete.Add("PMDO/Strings/");
            filesToDelete.Add("PMDO/MODS/All_Starters");
            filesToDelete.Add("PMDO/MODS/Enable_Mission_Board");
            filesToDelete.Add("PMDO/MODS/Gender_Unlock");
            filesToDelete.Add("PMDO/MODS/Music_Notice");
            filesToDelete.Add("PMDO/MODS/Visible_Monster_Houses");
            filesToDelete.Add("PMDO/dev.bat");
            filesToDelete.Add("PMDO/FNA.pdb");
            filesToDelete.Add("PMDO/KeraLua.pdb");
            filesToDelete.Add("PMDO/NLua.pdb");
            filesToDelete.Add("PMDO/PMDC.pdb");
            filesToDelete.Add("PMDO/PMDO.exe");
            filesToDelete.Add("PMDO/PMDO.png");
            filesToDelete.Add("PMDO/README.txt");
            filesToDelete.Add("PMDO/RogueElements.pdb");
            filesToDelete.Add("PMDO/RogueEssence.Editor.Avalonia.pdb");
            filesToDelete.Add("PMDO/RogueEssence.pdb");
            filesToDelete.Add("PMDO/spritebot_credits.txt");
            executableFiles.Add("PMDO/PMDO");
            executableFiles.Add("PMDO/dev.sh");
            executableFiles.Add("PMDO/MapGenTest");
            executableFiles.Add("WaypointServer/WaypointServer");
            executableFiles.Add("WaypointServer.app/Contents/MacOS/WaypointServer");
        }

        static void SaveXml()
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();

                XmlNode docNode = xmldoc.CreateElement("Config");
                xmldoc.AppendChild(docNode);

                appendConfigNode(xmldoc, docNode, "ExeRepo", curVerRepo);
                appendConfigNode(xmldoc, docNode, "Asset", assetSubmodule);
                appendConfigNode(xmldoc, docNode, "LastUpdaterVersion", Assembly.GetEntryAssembly().GetName().Version.ToString());
                appendConfigNode(xmldoc, docNode, "LastVersion", lastVersion.ToString());

                XmlNode keys = xmldoc.CreateElement("ToDelete");
                foreach (string key in filesToDelete)
                {
                    XmlNode node = xmldoc.CreateElement("Deletion");
                    node.InnerText = key;
                    keys.AppendChild(node);
                }
                docNode.AppendChild(keys);

                XmlNode exes = xmldoc.CreateElement("Executables");
                foreach (string key in executableFiles)
                {
                    XmlNode node = xmldoc.CreateElement("Exe");
                    node.InnerText = key;
                    exes.AppendChild(node);
                }
                docNode.AppendChild(exes);

                xmldoc.Save(Path.Join(updaterPath, "Updater.xml"));
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        static bool DeleteWithExclusions(string path)
        {
            if (isExcluded(path))
                return false;

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            if (!Directory.Exists(path))
                return true;

            bool deletedAll = true;
            string[] listDir = Directory.GetDirectories(path);
            foreach (string dir in listDir)
            {
                bool deletedAllSub = DeleteWithExclusions(dir);
                if (!deletedAllSub)
                    deletedAll = false;
            }
            string[] listFiles = Directory.GetFiles(path);
            foreach (string file in listFiles)
            {
                bool deletedAllSub = DeleteWithExclusions(file);
                if (!deletedAllSub)
                    deletedAll = false;
            }

            if (deletedAll)
                Directory.Delete(path, false);

            return deletedAll;
        }

        static bool isExcluded(string path)
        {
            //ignore .git
            string filename = Path.GetFileName(path);
            if (filename == ".git")
                return true;

            return false;
        }

        private static string GetCurrentPlatform()
        {
            string[] platformNames = new string[]
            {
            "LINUX",
            "OSX",
            "WINDOWS",
            "FREEBSD",
            "NETBSD",
            "OPENBSD"
            };

            for (int i = 0; i < platformNames.Length; i += 1)
            {
                OSPlatform platform = OSPlatform.Create(platformNames[i]);
                if (RuntimeInformation.IsOSPlatform(platform))
                {
                    if (Environment.Is64BitOperatingSystem)
                        return platformNames[i].ToLowerInvariant() + "-x64";
                    else
                        return platformNames[i].ToLowerInvariant() + "-x86";
                }
            }

            return "unknown";
        }

        private static void appendConfigNode(XmlDocument doc, XmlNode parentNode, string name, string text)
        {
            XmlNode node = doc.CreateElement(name);
            node.InnerText = text;
            parentNode.AppendChild(node);
        }

        private static string FileSizeToPrettyString(double fileSize)
        {
            const bool useBase10Separator = true;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (fileSize >= (useBase10Separator ? 1000 : 1024) && order < sizes.Length - 1)
            {
                order++;
                fileSize = fileSize / (useBase10Separator ? 1000 : 1024);
            }
            
            return String.Format("{0:0.###} {1}", fileSize, sizes[order]);
        }
    }
}
