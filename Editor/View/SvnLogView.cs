using EZ.TortoiseSVN.Data;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;

namespace EZ.TortoiseSVN.Editor.View
{
    public class SvnLogView : VisualElement
    {
        public MultiColumnListView MultiColumnListView { get; private set; }
        public TextField LogMessageField { get; private set; }

        private Columns _columns;
        private PackageResources _packageResources;

        public SvnLogView(PackageResources packageResources)
        {
            _packageResources = packageResources;

            _columns = new Columns
            {
                CreateReivisionColumn(),
                CreateAuthorColumn(),
                CreateDateColumn(),
                CreateMessageColumn()
            };

            var splitView = new TwoPaneSplitView(1, 240f, TwoPaneSplitViewOrientation.Vertical);

            //
            MultiColumnListView = new MultiColumnListView(_columns);
            MultiColumnListView.selectedIndicesChanged += (indices) =>
            {
                var index = indices.FirstOrDefault();
                var data = GetData(index);
                LogMessageField.value = data.ContextMessage;
            };

            //
            LogMessageField = new TextField();
            LogMessageField.isReadOnly = true;

            //var packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            //foreach (var package in packages)
            //{
            //    UnityEngine.Debug.Log(package.name);
            //    UnityEngine.Debug.Log(package.packageId);
            //    UnityEngine.Debug.Log(package.assetPath);
            //    UnityEngine.Debug.Log(package.resolvedPath);
            //}

            //var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath("Assets/Scripts/TortoiseSVN for Unity3d/package.json");
            //var virtualPath = "Packages/" + packageInfo.name;
            //var absolutePath = packageInfo.resolvedPath;

            var styleSheet = _packageResources.GetStyleSheet("TortoiseSVN");
            styleSheets.Add(styleSheet);

            LogMessageField.AddToClassList("excel2json__text-field");

            var splitView2 = new TwoPaneSplitView(0, 240f, TwoPaneSplitViewOrientation.Vertical);
            splitView2.style.minHeight = 120;

            var fixedPane2 = CreateFixedPane();
            var flexedPane2 = CreateFlexedPane();

            var fixedPane = CreateFixedPane();
            fixedPane.Add(splitView2);

            fixedPane2.Add(LogMessageField);

            var flexedPane = CreateFlexedPane();
            flexedPane.Add(MultiColumnListView);

            splitView.Add(flexedPane);
            splitView.Add(fixedPane);

            var affectedPath = new TextField();
            affectedPath.AddToClassList("excel2json__text-field");

            flexedPane2.Add(affectedPath);

            splitView2.Add(fixedPane2);
            splitView2.Add(flexedPane2);

            Add(splitView);
        }

        public VisualElement CreateFixedPane()
        {
            return new VisualElement()
            {
                name = "excel2json__fixed-pane"
            };
        }

        public VisualElement CreateFlexedPane()
        {
            return new VisualElement()
            {
                name = "excel2json__flexed-pane"
            };
        }

        public SvnLog GetData(int index)
        {
            return MultiColumnListView.itemsSource[index] as SvnLog;
        }

        #region Revision Column

        public Column CreateReivisionColumn()
        {
            return new Column()
            {
                name = "Revision",
                title = "Revision",
                bindCell = OnReivisionCellBind,
                makeCell = MakeRevisionCell,
                minWidth = 120,
            };
        }

        public VisualElement MakeRevisionCell()
        {
            return new Label();
        }

        public void OnReivisionCellBind(VisualElement view, int index)
        {
            var label = view as Label;
            var data = GetData(index);
            label.text = data.Revision.ToString();
        }

        #endregion Revision Column

        #region Author Column

        public Column CreateAuthorColumn()
        {
            return new Column()
            {
                name = "Author",
                title = "Author",
                bindCell = OnAuthorCellBind,
                makeCell = MakeAuthorCell,
                minWidth = 120,
            };
        }

        public VisualElement MakeAuthorCell()
        {
            return new Label();
        }

        public void OnAuthorCellBind(VisualElement view, int index)
        {
            var label = view as Label;
            var data = GetData(index);
            label.text = data.Author;
        }

        #endregion Author Column

        #region Date Column

        public Column CreateDateColumn()
        {
            return new Column()
            {
                name = "Date",
                title = "Date",
                bindCell = OnDateCellBind,
                makeCell = MakeDateCell,
                minWidth = 300,
            };
        }

        public VisualElement MakeDateCell()
        {
            return new Label();
        }

        public void OnDateCellBind(VisualElement view, int index)
        {
            var label = view as Label;
            var data = GetData(index);
            label.text = data.Date;
        }

        #endregion Date Column

        #region Message Column

        public Column CreateMessageColumn()
        {
            return new Column()
            {
                name = "Mssage",
                title = "Message",
                bindCell = OnMessageCellBind,
                makeCell = MakeMessageCell,
                minWidth = 120,
                stretchable = true,
            };
        }

        public VisualElement MakeMessageCell()
        {
            return new Label();
        }

        public void OnMessageCellBind(VisualElement view, int index)
        {
            var label = view as Label;
            var data = GetData(index);
            if (!string.IsNullOrEmpty(data.ContextMessage))
            {
                label.text = data.ContextMessage.Replace('\n', ' ');
            }
        }

        #endregion Message Column

        public void SetItemsSource(IList itemSource)
        {
            MultiColumnListView.itemsSource = itemSource;
        }

        public void ReloadData()
        {
            MultiColumnListView.RefreshItems();
        }
    }
}