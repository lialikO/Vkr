namespace WinFormsApp1
{
    static class AutClass
    {
        public static bool IsAuthenticated = false;        
        public static int aut_id = 0;      
        public static string aut_fio = null;
        public static string aut_role_title = null;
        public static int aut_role = 0;
        public static bool vhod = false;
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new mainForm());
        }
    }
}