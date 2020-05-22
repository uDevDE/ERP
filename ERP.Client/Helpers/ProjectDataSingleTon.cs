
namespace ERP.Client.Helpers
{
    public class ProjectDataSingleTon
    {
        private static ProjectDataSingleTon _instance;
        private static readonly object _padlock = new object();

        public static ProjectDataSingleTon Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProjectDataSingleTon();
                    }

                    return _instance;
                }
            }
        }



    }
}
