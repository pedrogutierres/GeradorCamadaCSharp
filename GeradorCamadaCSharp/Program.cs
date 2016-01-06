using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeradorCamadaCSharp
{
    static class Program
    {
        public static string strIpBanco = "", strSenhaBanco = "", strNomeBanco = "", strUsuarioBanco = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string Nome = Application.StartupPath + @"\config.ini";
            if (!File.Exists(Nome))
            {
                File.Create(Application.StartupPath + @"\config.ini").Close();
                TextWriter arquivo = File.AppendText("config.ini");
                arquivo.WriteLine("NomeBanco=db_imperium");
                arquivo.WriteLine("UsuarioBanco=root");
                arquivo.WriteLine("IpBanco=localhost");
                arquivo.WriteLine("SenhaBanco=1");

                arquivo.Close();
            }
            using (StreamReader sr1 = File.OpenText(Nome))
            {
                String input;
                while ((input = sr1.ReadLine()) != null)
                {
                    char[] delimiterChars = { '=', '\t', ',' };
                    string text = input;
                    text = text.ToUpper();
                    string[] words = text.Split(delimiterChars);
                    string[] words1 = input.Split(delimiterChars);

                    if (words[0] == "NOMEBANCO") { strNomeBanco = words1[1]; }
                    if (words[0] == "IPBANCO") { strIpBanco = words1[1]; }
                    if (words[0] == "SENHABANCO") { strSenhaBanco = words1[1]; }
                    if (words[0] == "USUARIOBANCO") { strUsuarioBanco = words1[1]; }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPrincipal());
        }
    }
}
