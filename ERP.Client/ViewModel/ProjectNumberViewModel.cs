using ERP.Client.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Client.ViewModel
{
    public class ProjectNumberViewModel
    {
        public ObservableCollection<ProjectNumberModel> ProjectNumbers { get; private set; }

        public ProjectNumberViewModel() => ProjectNumbers = new ObservableCollection<ProjectNumberModel>();

        public void Load(List<ProjectNumberModel> projectNumbers)
        {
            ProjectNumbers.Clear();
            foreach (var projectNumber in projectNumbers)
            {
                ProjectNumbers.Add(projectNumber);
            }
        }

        public ProjectNumberModel SelectedProjectNumber { get; set; }

    }
}
