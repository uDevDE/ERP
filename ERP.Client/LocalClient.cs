using ERP.Client.Model;

namespace ERP.Client
{
    public class LocalClient
    {
        private static bool _initialized = false;

        public static EmployeeModel Employee { get; set; }
        public static DivisionModel Division { get; set; }

        //public EmployeeModel Employees => EmployeeViewModel;

        public LocalClient()
        {
            if (!_initialized)
            {
                Employee = new EmployeeModel();
                _initialized = true;
            }
        }
    }
}
