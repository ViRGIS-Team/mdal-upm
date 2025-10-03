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

                        conda.Install($"mdal={packageVersion}");
                        try
                        {
                            string sharedAssets = Application.streamingAssetsPath;
                            if (Directory.Exists(Path.Combine(conda.condaShared, "gdal")))
                            {
                                if (!Directory.Exists(sharedAssets)) Directory.CreateDirectory(sharedAssets);
                                string gdalDir = Path.Combine(sharedAssets, "gdal");
                                if (!Directory.Exists(gdalDir)) Directory.CreateDirectory(gdalDir);
                                string projDir = Path.Combine(sharedAssets, "proj");
                                if (!Directory.Exists(projDir)) Directory.CreateDirectory(projDir);

                                if (Directory.Exists(Path.Combine(conda.condaShared, "gdal")))
                                    foreach (var file in Directory.GetFiles(Path.Combine(conda.condaShared, "gdal")))
                                    {
                                        File.Copy(file, Path.Combine(gdalDir, Path.GetFileName(file)), true);
                                    }

                                if (Directory.Exists(Path.Combine(conda.condaShared, "proj"))) 
                                    foreach (var file in Directory.GetFiles(Path.Combine(conda.condaShared, "proj")))
                                    {
                                        File.Copy(file, Path.Combine(projDir, Path.GetFileName(file)), true);
                                    }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                        conda.TreeShake();
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
