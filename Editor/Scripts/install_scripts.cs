using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;

namespace Mdal {

    public class Install: AssetPostprocessor
    {

        const string packageVersion = "1.3.1";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!SessionState.GetBool("MdalInitDone", false))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                EditorUtility.DisplayProgressBar("Restoring Conda Package", "MDAL", 0);

                if (Application.isEditor)
                {
                    try
                    {
                        if (Mdal.GetVersion() != packageVersion)
                        {
                            UpdatePackage();
                            AssetDatabase.Refresh();
                        }
                    }
                    catch
                    {
                        UpdatePackage();
                        AssetDatabase.Refresh();
                    };
                };

                EditorUtility.ClearProgressBar();
                stopwatch.Stop();
                Debug.Log($"Mdal refresh took {stopwatch.Elapsed.TotalSeconds} seconds");
            }
            SessionState.SetBool("MdalInitDone", true);
        }


        static void UpdatePackage() {
            Debug.Log("Mdal Install Script Awake");
            string path = Path.GetDirectoryName(new StackTrace(true).GetFrame(0).GetFileName());

            string response = Conda.Conda.Install($"mdal={packageVersion}");

            string condaLibrary;
            string condaShared;
            string condaBin;
#if UNITY_EDITOR_WIN
            condaLibrary = Path.Combine(Application.dataPath, "Conda", "Env", "Library");
            condaShared = Path.Combine(condaLibrary, "share");
            condaBin = Path.Combine(condaLibrary, "bin");
#else
            condaLibrary = Path.Combine(Application.dataPath, "Conda", "Env");
            condaShared = Path.Combine(condaLibrary, "share");
            condaBin = Path.Combine(condaLibrary, "lib");
#endif
            try
            {
                string sharedAssets = Application.streamingAssetsPath;
                if (Directory.Exists(Path.Combine(condaShared, "gdal")))
                {
                    if (!Directory.Exists(sharedAssets)) Directory.CreateDirectory(sharedAssets);
                    string gdalDir = Path.Combine(sharedAssets, "gdal");
                    if (!Directory.Exists(gdalDir)) Directory.CreateDirectory(gdalDir);
                    string projDir = Path.Combine(sharedAssets, "proj");
                    if (!Directory.Exists(projDir)) Directory.CreateDirectory(projDir);

                    foreach (var file in Directory.GetFiles(Path.Combine(condaShared, "gdal")))
                    {
                        File.Copy(file, Path.Combine(gdalDir, Path.GetFileName(file)), true);
                    }

                    foreach (var file in Directory.GetFiles(Path.Combine(condaShared, "proj")))
                    {
                        File.Copy(file, Path.Combine(projDir, Path.GetFileName(file)), true);
                    }
                }
            }
            catch (Exception e)
            {
                _ = e;
            }
            Conda.Conda.TreeShake();
        }
    }
}
