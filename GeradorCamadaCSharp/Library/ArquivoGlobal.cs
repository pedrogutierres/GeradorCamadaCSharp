using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorCamadaCSharp.Library
{
    public class ArquivoGlobal
    {
        public static string RetornaTextoArquivo(string pacote)
        {
            StringBuilder funcoes = new StringBuilder();
            funcoes.AppendLine("");
            funcoes.AppendLine("namespace " + pacote + ".Util");
            funcoes.AppendLine("{");
            funcoes.AppendLine("    class Global");
            funcoes.AppendLine("    {");
            funcoes.AppendLine("        private static Funcoes funcoes;");
            funcoes.AppendLine("        public static Funcoes Funcoes");
            funcoes.AppendLine("        {");
            funcoes.AppendLine("            get");
            funcoes.AppendLine("            {");
            funcoes.AppendLine("                if (funcoes == null)");
            funcoes.AppendLine("                    funcoes = Funcoes.newInstance();");
            funcoes.AppendLine("                return funcoes;");
            funcoes.AppendLine("            }");
            funcoes.AppendLine("        }");
            funcoes.AppendLine("    }");
            funcoes.AppendLine("}");
            return funcoes.ToString();
        }

    }
}
