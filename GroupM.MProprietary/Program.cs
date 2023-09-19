using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public static class Program
    {
        public static string Username { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DBAccess connect = new DBAccess();
            string[] x1 = { "TOSSAPOLS", "ProprietaryGroup" };
            string[] x2 = { "TOSSAPOLS", "Client", "10AIAEXC", "AIA COMPANY LIMITED.." };
            string[] x3 = { "TOSSAPOLS", "Proprietary" };
            //args = x1;
            if (args.Length == 0)
            {
                Username = "TOSSAPOLS";
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                Username = args[0];
                string open = args[1];
                if (open == "ProprietaryGroup")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmProprietary());
                }
                if (open == "Client")
                {
                    string clientID = args[2];
                    string clientName = args[3];
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmClientMapping(clientID, clientName));
                }
                if (open == "Proprietary")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmProprietaryMaster());
                }
            }
        }
    }
}
