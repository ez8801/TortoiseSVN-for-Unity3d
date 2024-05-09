using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;

public class PathWindow : EditorWindow
{
    [MenuItem("TortoiseSVN/Path")]
    public static void ShowWindow()
    {
        GetWindow<PathWindow>();
    }

    public void OnEnable()
    {
        var textField = new TextField("Path")
        {
            name = "Path"
        };
        rootVisualElement.Add(textField);

        var toggle = new Toggle("Directory");
        toggle.name = "Directory";
        rootVisualElement.Add(toggle);

        var dirField = new TextField("DirField");
        dirField.name = "DirField";
        rootVisualElement.Add(dirField);

        var dirField2 = new TextField("DirField2");
        dirField2.name = "DirField2";
        rootVisualElement.Add(dirField2);

        var fileToggle = new Toggle("File");
        fileToggle.name = "File";
        rootVisualElement.Add(fileToggle);

        var fileField = new TextField("FileField");
        fileField.name = "FileField";
        rootVisualElement.Add(fileField);

        var fileField2 = new TextField("FileField2");
        fileField2.name = "FileField2";
        rootVisualElement.Add(fileField2);

        textField.RegisterValueChangedCallback(evt =>
        {
            var existsDir = Directory.Exists(evt.newValue);
            var existsFile = File.Exists(evt.newValue);

            var dirToggle = rootVisualElement.Q<Toggle>("Directory");
            dirToggle.value = existsDir;

            var fileToggle = rootVisualElement.Q<Toggle>("File");
            fileToggle.value = existsFile;

            rootVisualElement.Q<TextField>("DirField2").value = Path.GetFullPath(evt.newValue);
            
            try
            {
                var directoryInfo = new DirectoryInfo(evt.newValue);
                rootVisualElement.Q<TextField>("DirField").value = directoryInfo.FullName;
            }
            catch
            {
                rootVisualElement.Q<TextField>("DirField").value = string.Empty;
            }
            
            try
            {
                var fileInfo = new FileInfo(evt.newValue);
                rootVisualElement.Q<TextField>("FileField").value = fileInfo.FullName;
            }
            catch
            {
                rootVisualElement.Q<TextField>("FileField").value = string.Empty;
            }
        });
    }

}
