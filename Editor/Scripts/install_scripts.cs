using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Mdal {

    public class Install: AssetPostprocessor
    {

        const string packageVersion = "1.2.0";

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
#if UNITY_EDITOR_WIN
            string script = "install_script.ps1";
#else
            string script = "install_script.sh";
#endif
            string response = Conda.Conda.Install($"mdal={packageVersion}", script, path);
        }
    }
}
