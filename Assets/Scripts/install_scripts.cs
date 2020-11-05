using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

namespace Mdal {

    public class Install{

        const string sharedObject = "mdal.dll";
        const string packageVersion = "0.7.2";

        [InitializeOnLoadMethod]
        static void OnProjectLoadedinEditor()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            EditorUtility.DisplayProgressBar("Restoring Conda Package", "MDAL", 0);

            if (Application.isEditor) {
                try
                {
                    string pluginPath = Path.Combine(Application.dataPath, "Packages");
                    if (!Directory.Exists(pluginPath)) Directory.CreateDirectory(pluginPath);
                    pluginPath = Path.Combine(pluginPath, "Mdal");
                    if (!Directory.Exists(pluginPath)) Directory.CreateDirectory(pluginPath);
                    string file = Path.Combine(pluginPath, sharedObject);
                    if (!File.Exists(file))
                    {
                        UpdatePackage();
                    }
                    else if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        string currentVersion = Mdal.GetVersion();
                        if (currentVersion != packageVersion)
                        {
                            UpdatePackage();
                        }
                    }
                    AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    // do nothing
                    Debug.Log($"Error in Conda Package {sharedObject} : {e.ToString()}");
                };
            };

            EditorUtility.ClearProgressBar();
            stopwatch.Stop();
            Debug.Log($"Mdal refresh took {stopwatch.Elapsed.TotalSeconds} seconds");
        }
        static void UpdatePackage() {
            Debug.Log("Mdal Install Script Awake"); 
            string pluginPath = Path.Combine(Application.dataPath, "Packages", "Mdal");
            string path = Path.GetDirectoryName(new StackTrace(true).GetFrame(0).GetFileName());
            string exec = Path.Combine(path, "install_script.ps1");
            string response;
            string install = $"mdal={packageVersion}";
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = "powershell.exe";
                compiler.StartInfo.Arguments = $"-ExecutionPolicy Bypass {exec} -package mdal " +
                                                    $"-install mdal {install} " +
                                                    $"-destination {pluginPath} " +
                                                    $"-so_list mdal";
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.Start();

                response = compiler.StandardOutput.ReadToEnd();

                compiler.WaitForExit();
            }
            Debug.Log(response);
        }
    }
}
