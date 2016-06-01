using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorCamadaCSharp.Library
{
    public class ArquivoEnumeradores
    {
        public static string RetornaTextoArquivo(string pacote)
        {
            StringBuilder funcoes = new StringBuilder();
            funcoes.AppendLine("using System;                                                                                            ");
            funcoes.AppendLine("using System.ComponentModel;                                                                             ");
            funcoes.AppendLine("using System.Reflection;                                                                                 ");
            funcoes.AppendLine("                                                                                                         ");
            funcoes.AppendLine("namespace " + pacote + ".ConstantValues                                                             ");
            funcoes.AppendLine("{                                                                                                        ");
            funcoes.AppendLine("    public enum ProviderEnum                                                                             ");
            funcoes.AppendLine("    {                                                                                                    ");
            funcoes.AppendLine("        None,                                                                                            ");
            funcoes.AppendLine("        [Description(\"MySql.Data.MySqlClient\")]                                                          ");
            funcoes.AppendLine("        MySql,                                                                                           ");
            funcoes.AppendLine("        [Description(\"System.Data.SqlClient\")]                                                           ");
            funcoes.AppendLine("        SQLServer                                                                                        ");
            funcoes.AppendLine("    }                                                                                                    ");
            funcoes.AppendLine("                                                                                                         ");
            funcoes.AppendLine("    public static class EnumDescription                                                                  ");
            funcoes.AppendLine("    {                                                                                                    ");
            funcoes.AppendLine("        public static string GetDescription<T>(this T enumerationValue)                                  ");
            funcoes.AppendLine("        where T : struct                                                                                 ");
            funcoes.AppendLine("        {                                                                                                ");
            funcoes.AppendLine("            Type type = enumerationValue.GetType();                                                      ");
            funcoes.AppendLine("            if (!type.IsEnum)                                                                            ");
            funcoes.AppendLine("            {                                                                                            ");
            funcoes.AppendLine("                throw new ArgumentException(\"EnumerationValue must be of Enum type\", \"enumerationValue\");");
            funcoes.AppendLine("            }                                                                                            ");
            funcoes.AppendLine("                                                                                                         ");
            funcoes.AppendLine("            //Tries to find a DescriptionAttribute for a potential friendly name                         ");
            funcoes.AppendLine("            //for the enum                                                                               ");
            funcoes.AppendLine("            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());                       ");
            funcoes.AppendLine("            if (memberInfo != null && memberInfo.Length > 0)                                             ");
            funcoes.AppendLine("            {                                                                                            ");
            funcoes.AppendLine("                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false); ");
            funcoes.AppendLine("                                                                                                         ");
            funcoes.AppendLine("                if (attrs != null && attrs.Length > 0)                                                   ");
            funcoes.AppendLine("                {                                                                                        ");
            funcoes.AppendLine("                    //Pull out the description value                                                     ");
            funcoes.AppendLine("                    return ((DescriptionAttribute)attrs[0]).Description;                                 ");
            funcoes.AppendLine("                }                                                                                        ");
            funcoes.AppendLine("            }                                                                                            ");
            funcoes.AppendLine("            //If we have no description attribute, just return the ToString of the enum                  ");
            funcoes.AppendLine("            return enumerationValue.ToString();                                                          ");
            funcoes.AppendLine("                                                                                                         ");
            funcoes.AppendLine("        }                                                                                                ");
            funcoes.AppendLine("    }                                                                                                    ");
            funcoes.AppendLine("}                                                                                                        ");
            return funcoes.ToString();
        }

    }
}
