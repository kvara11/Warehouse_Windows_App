using Store.Repository;
using System;
using System.Windows.Forms;

namespace Store.App
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var userRep = new UserRepository();
            var userID = userRep.Login("Lasha", "123", out string message);

        }
    }
}
