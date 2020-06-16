using ERP.Client.Core.Enums;
using ERP.Client.Model;

namespace ERP.Client.Summaries
{
    public class ProjectSummary
    {
        public PlantOrderModel PlantOrder { get; set; }
        public FileEntryModel FileEntry { get; set; }
        public FolderModel Folder { get; set; }
        public ProjectPreviewType ProjectPreviewType { get; set; }
    }
}
