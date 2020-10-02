using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoUpdateSpigot
{
    class Program
    {
        public static ObservableCollection<object> MCVersions;

        private static PaperAPI.PaperAPI _paperAPI { get; set; }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing PaperAPI..");
            _paperAPI = new PaperAPI.PaperAPI();

            Console.WriteLine("Loading config..");
            
            // Create config files if they dont exist
            
            if (!File.Exists("autoupdate-version.txt"))
            {
                File.WriteAllText("autoupdate-version.txt", "version");
            }
            if (!File.Exists("autoupdate-path.txt"))
            {
                File.WriteAllText("autoupdate-path.txt", "jarpath");
            }

            string version = File.ReadAllText("autoupdate-version.txt");
            string path = File.ReadAllText("autoupdate-path.txt");

            // Exit out of application if files are unedited

            if (version == "version")
            {
                Console.WriteLine("Please specifiy a version in the \"autoupdate-version.txt\"");
                Environment.Exit(1);
            }
            if (path == "jarpath")
            {
                Console.WriteLine("Please specifiy a path in the \"autoupdate-path.txt\"");
                Environment.Exit(1);
            }

            // Check for currently installed build

            string installedbuild = "";
            if (File.Exists($"{path}.version"))
            {
                installedbuild = File.ReadAllText($"{path}.version");
            }

            Console.WriteLine($"Selected versions: \"{version}\"");
            Console.WriteLine($"Selected path: \"{path}\"");

            Console.WriteLine($"Getting available Versions..");
            var versions = await _paperAPI.GetMCVersions();
            MCVersions = new ObservableCollection<object>(versions.versions);

            if (MCVersions.Contains($"{version}"))
            {
                Console.WriteLine($"Getting latest build..");
                var buildversions = await _paperAPI.GetBuildVersions(version);
                string latestbuild = buildversions.builds.latest;

                if (installedbuild == latestbuild)
                {
                    Console.WriteLine($"Latest build already installed.");
                    Environment.Exit(0);
                }

                Console.WriteLine($"Installed build is \"{installedbuild}\"");
                Console.WriteLine($"Latest build is \"{latestbuild}\".");

                // Backup replaced files

                if (File.Exists(path))
                {
                    if (File.Exists($"{path}.old.old"))
                    {
                        File.Delete($"{path}.old.old");
                    }
                    if (File.Exists($"{path}.old"))
                    {
                        File.Move($"{path}.old", $"{path}.old.old");
                    }
                    File.Move(path, $"{path}.old");
                }

                Console.WriteLine($"Downloading \"{version}\" \"{latestbuild}\" server jar..");
                Stream server = await _paperAPI.DownloadFile(version, latestbuild);
                using (var fileStream = File.Create(path))
                {
                    await server.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Saving downloaded build..");
                File.WriteAllText($"{path}.version", latestbuild);

                Console.WriteLine($"Done.");
                Console.WriteLine($"");

                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Selected version not available. Exiting..");

                Environment.Exit(0);
            }
        }
    }
}
