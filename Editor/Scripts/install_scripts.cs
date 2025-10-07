using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using Conda;

namespace Mdal {

    public class Install: AssetPostprocessor
    {

        const string packageVersion = "1.3.1";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {

            if (!SessionState.GetBool("MdalInitDone", false))
            {
                Stopwatch stopwatch = new();
                string response = "";
                stopwatch.Start();

                EditorUtility.DisplayProgressBar("Restoring Conda Package", "MDAL", 0);

                if (Application.isEditor)
                {
                    CondaApp conda = new();
                    if (!conda.IsInstalled("mdal", packageVersion))
                    {
                        Debug.Log("Mdal Install Script Awake");
                        string path = Path.GetDirectoryName(new StackTrace(true).GetFrame(0).GetFileName());

                        conda.Add($"mdal={packageVersion}", new ConfigFile.Package()
                        {
                            Name = "mdal",
                            Cleans = new ConfigFile.Clean[] {},
                            Shared_Datas = new string[] {
                                "gdal", "proj"
                            }
                        });

                        AssetDatabase.Refresh();
                    }
                }

                EditorUtility.ClearProgressBar();
                stopwatch.Stop();
                Debug.Log($"Mdal refresh took {stopwatch.Elapsed.TotalSeconds} seconds" + response);
            }
            SessionState.SetBool("MdalInitDone", true);
        }
    }
}
