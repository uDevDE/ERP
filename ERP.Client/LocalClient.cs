using ERP.Client.ViewModel;

namespace ERP.Client
{
    public class LocalClient
    {
        private static bool _initialized = false;

        public static EmployeeViewModel EmployeeViewModel { get; set; }

        public EmployeeViewModel Employees => EmployeeViewModel;

        public LocalClient()
        {
            if (!_initialized)
            {
                EmployeeViewModel = new EmployeeViewModel();
                _initialized = true;
            }
        }
    }
}
