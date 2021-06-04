using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
namespace Mdal {

    public class Install{

        const string packageVersion = "0.8.1";

        [InitializeOnLoadMethod]
        static void OnProjectLoadedinEditor()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            EditorUtility.DisplayProgressBar("Restoring Conda Package", "MDAL", 0);

            if (Application.isEditor) {
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


        static void UpdatePackage() {
            Debug.Log("Mdal Install Script Awake");
            string path = Path.GetDirectoryName(new StackTrace(true).GetFrame(0).GetFileName());
#if UNITY_EDITOR_WIN
            path = Path.Combine(path, "install_script.ps1");
#else
            path = Path.Combine(path, "install_script.sh");
#endif
            string response = Conda.Conda.Install($"mdal={packageVersion}", path);
        }
    }
}
#endif
