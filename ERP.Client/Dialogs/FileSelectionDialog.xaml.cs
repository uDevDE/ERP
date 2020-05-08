using ERP.Client.Model;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERP.Client.Dialogs
{
    public sealed partial class FileSelectionDialog : ContentDialog
    {
        public FolderModel Folder { get; private set; }
        public FileEntryModel Result { get; private set; }

        public FileSelectionDialog(FolderModel folder)
        {
            this.InitializeComponent();

            Folder = folder;
            LoadFolderIntoTreeView(Folder);
        }

        private void LoadFolderIntoTreeView(FolderModel folder)
        {
            TreeViewNode rootNode = CreateTreeViewNode(folder);
            
            foreach (var subFolder in Folder.SubFolders)
            {
                var item = LoadFolderIntoTreeViewAsync(subFolder.Value);
                rootNode.Children.Add(item);
            }

            rootNode.IsExpanded = true;
            FileTreeView.RootNodes.Add(rootNode);
        }

        private TreeViewNode LoadFolderIntoTreeViewAsync(FolderModel folder)
        {
            TreeViewNode node = CreateTreeViewNode(folder);
            foreach (var fileEntry in folder.Files)
            {
                node.Children.Add(CreateTreeViewNode(fileEntry.Value));
            }

            if (folder.SubFolders.Count > 0)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    node.Children.Add(CreateTreeViewNode(subFolder.Value));
                }
            }

            return node;
        }

        private StackPanel CreateTreeViewNode(FolderModel folder)
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            var icon = new SymbolIcon() { Symbol = Symbol.Folder };
            var text = new TextBlock() { Text = folder.Name };
            panel.Children.Add(icon);
            panel.Children.Add(text);
            //panel.DataContext = folder;

            return panel;
            //return new TreeViewNode() { Content = folder.Name };
        }

        private StackPanel CreateTreeViewNode(FileEntryModel fileEntry)
        {
            var panel = new StackPanel() { Orientation = Orientation.Horizontal };
            var icon = new SymbolIcon() { Symbol = Symbol.OpenFile };
            var text = new TextBlock() { Text = fileEntry.Name };
            panel.Children.Add(icon);
            panel.Children.Add(text);
            panel.DataContext = fileEntry;

            return panel;
            //return new TreeViewNode() { Content = fileEntry.Name };
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void FileTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
        }
    }
}
