using System;
using System.Windows.Forms;

namespace SkypeRoller
{
    public static class Program
    {
        public static RollerTrayForm RollerTrayForm { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RollerTrayForm = new RollerTrayForm();
            Application.Run(RollerTrayForm);
        }
    }
}
