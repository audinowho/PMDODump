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

namespace PMDOSetup
{
    class Program
    {
        static string updaterPath;
        static string curVerRepo;
        static string assetSubmodule;
        static string exeSubmodule;
        static List<string> excludedFiles;
        static List<string> executableFiles;
        static Version lastVersion;
        static int argNum;
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            updaterPath = Path.GetDirectoryName(args[0]) + "/";
            argNum = -1;
            if (args.Length > 1)
                Int32.TryParse(args[1], out argNum);
            //1: detect platform and load defaults
            curVerRepo = "audinowho/PMDODump";
            assetSubmodule = "DumpAsset";
            exeSubmodule = "PMDC";
            lastVersion = new Version(0, 0, 0, 0);
            excludedFiles = new List<string>();
            executableFiles = new List<string>();
            //2: load xml-filename, xml name, last version, exclusions - if possible
            LoadXml();

            try
            {
                ConsoleKey choice = ConsoleKey.Enter;

                if (lastVersion == new Version(0, 0, 0, 0))
                {
                    Console.WriteLine("Preparing PMDO installation...");
                }
                else
                {
                    Console.WriteLine("Updater Options:");
                    Console.WriteLine("1: Force Update");
                    Console.WriteLine("2: Uninstall (Retain Save Data)");
                    Console.WriteLine("3: Reset Updater XML");
                    Console.WriteLine("Press any other key to check for updates.");
                    if (argNum > -1)
                    {
                        if (argNum == 1)
                            choice = ConsoleKey.D1;
                        else if (argNum == 2)
                            choice = ConsoleKey.D2;
                        else if (argNum == 3)
                            choice = ConsoleKey.D3;
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
                if (choice == ConsoleKey.D1)
                    force = true;
                else if (choice == ConsoleKey.D2)
                {
                    Console.WriteLine("Uninstalling...");
                    DeleteWithExclusions(Path.Join(updaterPath, "PMDO"));
                    DeleteWithExclusions(Path.Join(updaterPath, "WaypointServer"));
                    DeleteWithExclusions(Path.Join(updaterPath, "temp"));
                    Console.WriteLine("Done.");
                    ReadKey();
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

                //3: read from site what version is uploaded. if greater than the current version, upgrade
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                    string latestResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases/latest", curVerRepo));
                    JsonElement latestJson = JsonDocument.Parse(latestResponse).RootElement;

                    string uploadedVersionStr = latestJson.GetProperty("name").GetString();
                    string changelog = latestJson.GetProperty("body").GetString();
                    Version nextVersion = new Version(uploadedVersionStr);

                    if (lastVersion >= nextVersion)
                    {
                        if (force)
                            Console.WriteLine("Update will be forced. {0} >= {1}", lastVersion, nextVersion);
                        else
                        {
                            Console.WriteLine("You are up to date. {0} >= {1}", lastVersion, nextVersion);
                            ReadKey();
                            return;
                        }
                    }
                    else
                        Console.WriteLine("Update available. {0} < {1}", lastVersion, nextVersion);

                    Console.WriteLine();
                    Console.WriteLine(changelog);
                    Console.WriteLine();
                    Console.WriteLine();

                    string tagStr = latestJson.GetProperty("tag_name").GetString();
                    Regex pattern = new Regex(@"https://github\.com/(?<repo>\w+/\w+).git");

                    string exeFile = null;
                    {
                        //Get the exe submodule's version (commit) at this tag
                        wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                        string exeSubmoduleResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/contents/{1}?ref={2}", curVerRepo, exeSubmodule, tagStr));
                        JsonElement exeSubmoduleJson = JsonDocument.Parse(exeSubmoduleResponse).RootElement;

                        string exeUrl = exeSubmoduleJson.GetProperty("submodule_git_url").GetString();
                        Match match = pattern.Match(exeUrl);
                        string exeRepo = match.Groups["repo"].Value;

                        string refStr = exeSubmoduleJson.GetProperty("sha").GetString();

                        //Get the tag associated with the commit (there'd better be one)
                        wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
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
                                wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                                string tagReleasesResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/releases/tags/{1}", exeRepo, tagName));
                                JsonElement tagReleasesJson = JsonDocument.Parse(tagReleasesResponse).RootElement;

                                JsonElement exeJson = tagReleasesJson.GetProperty("assets");
                                foreach (JsonElement assetJson in exeJson.EnumerateArray())
                                {
                                    string assetName = assetJson.GetProperty("name").GetString();
                                    if (assetName == String.Format("{0}.zip", GetCurrentPlatform()))
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
                        wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                        string assetSubmoduleResponse = wc.DownloadString(String.Format("https://api.github.com/repos/{0}/contents/{1}?ref={2}", curVerRepo, assetSubmodule, tagStr));
                        JsonElement assetSubmoduleJson = JsonDocument.Parse(assetSubmoduleResponse).RootElement;

                        string assetUrl = assetSubmoduleJson.GetProperty("submodule_git_url").GetString();
                        Match match = pattern.Match(assetUrl);
                        string assetRepo = match.Groups["repo"].Value;

                        string refStr = assetSubmoduleJson.GetProperty("sha").GetString();

                        //get the download link for it
                        assetFile = String.Format("https://api.github.com/repos/{0}/zipball/{1}", assetRepo, refStr);
                    }

                    Console.WriteLine("Version {0} will be downloaded from the endpoints:\n  {1}\n  {2}.\n\nPress any key to continue.", nextVersion, exeFile, assetFile);
                    ReadKey();

                    //4: download the respective zip from specified location
                    if (!Directory.Exists(Path.Join(updaterPath, "temp")))
                        Directory.CreateDirectory(Path.Join(updaterPath, "temp"));
                    string tempExe = Path.Join(updaterPath, "temp", Path.GetFileName(exeFile));
                    string tempAsset = Path.Join(updaterPath, "temp", "Asset.zip");

                    Console.WriteLine("Downloading from {0} to {1}. May take a while...", exeFile, tempExe);
                    wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                    wc.DownloadFile(exeFile, tempExe);
                    Console.WriteLine("Downloading from {0} to {1}. May take a while...", assetFile, tempAsset);
                    wc.Headers.Add("user-agent", "PMDOSetup/2.0.0");
                    wc.DownloadFile(assetFile, tempAsset);

                    Console.WriteLine("Adjusting filenames...");
                    //unzip the exe, rename, then rezip just to rename the file... ugh
                    RenameRezip(tempExe);

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
                    Console.WriteLine("Done.", exeFile);
                    ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                ReadKey();
            }
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

                    //go through the list of exemptions
                    bool exempt = false;
                    foreach (string exemption in excludedFiles)
                    {
                        if (exemption.StartsWith(destName, StringComparison.OrdinalIgnoreCase))
                        {
                            exempt = true;
                            break;
                        }
                    }
                    if (!exempt)
                    {
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
        }


        static void RenameRezip(string tempExe)
        {
            using (ZipArchive archive = ZipFile.OpenRead(tempExe))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Gets the full path to ensure that relative segments are removed.
                    string destinationPath = Path.GetFullPath(Path.Join(updaterPath, "temp", entry.FullName));
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

            if (Directory.Exists(Path.Join(updaterPath, "temp", "PMDC")))
                Directory.Move(Path.Join(updaterPath, "temp", "PMDC"), Path.Join(updaterPath, "temp", "PMDO"));

            if (File.Exists(Path.Join(updaterPath, "temp", "PMDO", "PMDC.exe")))
                File.Move(Path.Join(updaterPath, "temp", "PMDO", "PMDC.exe"), Path.Join(updaterPath, "temp", "PMDO", "PMDO.exe"));

            if (File.Exists(Path.Join(updaterPath, "temp", "PMDO", "PMDC")))
                File.Move(Path.Join(updaterPath, "temp", "PMDO", "PMDC"), Path.Join(updaterPath, "temp", "PMDO", "PMDO"));

            using (ZipArchive archive = ZipFile.Open(tempExe, ZipArchiveMode.Create))
            {
                foreach (string path in Directory.GetFiles(Path.Join(updaterPath, "temp", "PMDO")))
                {
                    string file = Path.GetFileName(path);
                    archive.CreateEntryFromFile(Path.Join(updaterPath, "temp", "PMDO", file), Path.Join("PMDO", file));
                }
                foreach (string path in Directory.GetFiles(Path.Join(updaterPath, "temp", "WaypointServer")))
                {
                    string file = Path.GetFileName(path);
                    archive.CreateEntryFromFile(Path.Join(updaterPath, "temp", "WaypointServer", file), Path.Join("WaypointServer", file));
                }
            }

            Directory.Delete(Path.Join(updaterPath, "temp", "PMDO"), true);
            Directory.Delete(Path.Join(updaterPath, "temp", "WaypointServer"), true);
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

                    curVerRepo = xmldoc.SelectSingleNode("Config/ExeRepo").InnerText;
                    assetSubmodule = xmldoc.SelectSingleNode("Config/Asset").InnerText;
                    lastVersion = new Version(xmldoc.SelectSingleNode("Config/LastVersion").InnerText);

                    excludedFiles.Clear();
                    XmlNode keys = xmldoc.SelectSingleNode("Config/Exclusions");
                    foreach (XmlNode key in keys.SelectNodes("Exclusion"))
                        excludedFiles.Add(key.InnerText);

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
            lastVersion = new Version(0, 0, 0, 0);
            excludedFiles = new List<string>();
            executableFiles = new List<string>();
            excludedFiles.Clear();
            excludedFiles.Add("PMDO/CONFIG/");
            excludedFiles.Add("PMDO/LOG/");
            excludedFiles.Add("PMDO/MODS/");
            excludedFiles.Add("PMDO/REPLAY/");
            excludedFiles.Add("PMDO/RESCUE/");
            excludedFiles.Add("PMDO/SAVE/");
            executableFiles.Clear();
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
                appendConfigNode(xmldoc, docNode, "LastVersion", lastVersion.ToString());

                XmlNode keys = xmldoc.CreateElement("Exclusions");
                foreach (string key in excludedFiles)
                {
                    XmlNode node = xmldoc.CreateElement("Exclusion");
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

            bool deletedAll = true;
            string[] listDir = Directory.GetDirectories(path);
            foreach (string dir in listDir)
            {
                bool deletedAllSub = DeleteWithExclusions(dir);

                if (deletedAllSub)
                    Directory.Delete(dir, false);
                else
                    deletedAll = false;
            }
            string[] listFiles = Directory.GetFiles(path);
            foreach (string file in listFiles)
            {
                if (!isExcluded(file))
                    File.Delete(file);
                else
                    deletedAll = false;
            }
            return deletedAll;
        }

        static bool isExcluded(string path)
        {
            string fullPath = Path.GetFullPath(path).Replace("\\", "/").Trim('/');
            foreach (string exclusion in excludedFiles)
            {
                string fullExclusion = Path.GetFullPath(exclusion).Replace("\\", "/").Trim('/');
                if (string.Equals(fullPath, fullExclusion, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
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
    }
}
