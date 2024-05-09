using UnityEngine;

namespace EZ.TortoiseSVN.Editor
{
    [CreateAssetMenu(menuName = "TortoiseSVN/Settings")]
    public class Settings : ScriptableObject
    {
        public class Default
        {
            /// <summary>
            /// Default name for the addressable assets settings
            /// </summary>
            public const string kAssetName = "TortoiseSVN Settings";

            /// <summary>
            /// The default folder for the serialized version of this class.
            /// </summary>
            public const string kFolder = "Assets/Settings";

            /// <summary>
            /// Default path for settings assets.
            /// </summary>
            public static string AssetPath
            {
                get { return kFolder + "/" + kAssetName + ".asset"; }
            }
        }

        public const string DefaultSvnClientPath = "C:/Program Files/TortoiseSVN/bin/svn.exe";
        
        public string SvnClientPath;
        public string RepositoryUrl;

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(SvnClientPath))
                SvnClientPath = DefaultSvnClientPath;
        }
    }
}