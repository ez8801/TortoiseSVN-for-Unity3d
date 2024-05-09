using EZ.TortoiseSVN.Data;
using EZ.TortoiseSVN.Editor.View;
using EZ.TortoiseSVN.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EZ.TortoiseSVN.Editor
{
    public class PackageResources
    {
        public List<StyleSheet> StyleSheets;

        public PackageResources(string path)
        {
            StyleSheets = new List<StyleSheet>();

            var guids = AssetDatabase.FindAssets("t:stylesheet", new string[] { path });
            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(assetPath);
                if (styleSheet != null)
                {
                    StyleSheets.Add(styleSheet);
                }
            }
        }

        public StyleSheet GetStyleSheet(string name)
        {
            return StyleSheets.Find(x => x.name == name);
        }
    }

    public class LogMessagesWindow : EditorWindow
    {
        private Settings _settings;
        private PackageResources _packageResources;
        private List<SvnLog> _logs = new List<SvnLog>();

        [MenuItem("TortoiseSVN/Show Log")]
        public static void ShowWindow()
        {
            GetWindow<LogMessagesWindow>();
        }

        private void OnEnable()
        {
            _packageResources = new PackageResources(GetEditorPath());
            _settings = AssetDatabase.LoadAssetAtPath<Settings>(Settings.Default.AssetPath);
            bool settingsExist = (_settings != null);
            if (settingsExist)
            {
                SvnLog();   
            }

            var flexView = new VisualElement();
            {
                var logView = new SvnLogView(_packageResources);
                logView.SetItemsSource(_logs);
                logView.style.flexGrow = 1;
                flexView.Add(logView);

                var toolbar = new Toolbar();
                flexView.Add(toolbar);

                flexView.style.flexGrow = 1;
            }
            rootVisualElement.Add(flexView);

            var noneView = new VisualElement();
            {
                var label = new Label("Settings not found.");
                noneView.Add(label);

                var button = new Button(() =>
                {
                    if (false == Directory.Exists(Settings.Default.kFolder))
                        Directory.CreateDirectory(Settings.Default.kFolder);

                    var settings = CreateInstance<Settings>();
                    AssetDatabase.CreateAsset(settings, Settings.Default.AssetPath);
                    settings.Validate();

                    _settings = settings;

                    SvnLog();

                    flexView.style.display = DisplayStyle.Flex;
                    noneView.style.display = DisplayStyle.None;
                });
                button.text = "Create Settings";
                noneView.Add(button);

                noneView.style.flexGrow = 1;
                noneView.style.justifyContent = Justify.Center;
                noneView.style.alignSelf = Align.Center;
            }
            rootVisualElement.Add(noneView);

            flexView.style.display = (settingsExist) ? DisplayStyle.Flex : DisplayStyle.None;
            noneView.style.display = (settingsExist) ? DisplayStyle.None : DisplayStyle.Flex;

            //var repository = GetWorkSpaceRepositoryURL();
            //UnityEngine.Debug.Log(repository);
        }

        private string GetEditorPath()
        {
            var thisScript = MonoScript.FromScriptableObject(this);
            var scriptAssetPath = AssetDatabase.GetAssetPath(thisScript);            
            return Path.GetDirectoryName(scriptAssetPath);
        }

        private string SvnCommand(string path, string commandArgs)
        {
            UnityEngine.Debug.Log($"SvnCommand({path}, {commandArgs})");

            var procStart = new ProcessStartInfo(path, commandArgs);
            procStart.CreateNoWindow = true;
            procStart.RedirectStandardOutput = true;
            procStart.UseShellExecute = false;
            procStart.WindowStyle = ProcessWindowStyle.Hidden;
            procStart.StandardOutputEncoding = System.Text.Encoding.GetEncoding(949);

            Process proc = Process.Start(procStart);
            proc.WaitForExit(1000);

            StreamReader output = proc.StandardOutput;
            string outputText = output.ReadToEnd();

            return outputText;
        }

        private string GetWorkSpaceRepositoryURL()
        {
            string commandArg = "info --show-item url";
            return SvnCommand(_settings.SvnClientPath, commandArg);
        }

        private void SvnLog()
        {
            var repositoryUrl = _settings.RepositoryUrl;
            UnityEngine.Debug.Log(repositoryUrl);

            string commandArg = $"log -v --limit 15 {repositoryUrl}";
            string outputText = SvnCommand(_settings.SvnClientPath, commandArg);
            UnityEngine.Debug.Log(outputText);

            var logParser = new SvnLogParser();
            foreach (var log in logParser.ParseSvnLogData(outputText))
            {
                _logs.Add(log);
                UnityEngine.Debug.Log(log.ToString());
            }
            //_svnDataElementList.AddRange(ParseSvnLogData(outputText));
            //_svnTreeView.Reload();
        }
    }
}