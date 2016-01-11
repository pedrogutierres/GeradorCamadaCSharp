using GeradorCamadaCSharp.Util;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GeradorCamadaCSharp
{
    public partial class frmPrincipal : Form
    {
        class Accessor
        {
            static Funcoes funcoes;
            internal static Funcoes Funcoes
            {
                get
                {
                    try
                    {
                        if (funcoes == null)
                            funcoes = new Funcoes();
                        return funcoes;
                    }
                    catch (Exception Err)
                    {
                        throw new Exception(Err.Message);
                    }
                }
            }
        }

        class TabelaInfo
        {
            public TabelaInfo()
            {
                colunas = new List<ColunaInfo>();
            }

            private string descricao;

            public string Descricao
            {
                get
                {
                    return descricao;
                }
                set
                {
                    descricao = value;

                    if (descricao.Contains("_"))
                    {
                        StringBuilder arquivo = new StringBuilder();

                        bool letraMaiscula = true;
                        foreach (char c in descricao.ToCharArray())
                        {
                            if (c == '_')
                                letraMaiscula = true;
                            else
                            {
                                arquivo.Append(letraMaiscula ? c.ToString().ToUpper() : c.ToString());

                                letraMaiscula = false;
                            }
                        }

                        Classe = arquivo.ToString();
                    }
                    else
                        Classe = descricao.Substring(0, 1).ToUpper() + (descricao.Length > 1 ? descricao.Substring(1) : "");

                    // Troca plural
                    if (Classe.ToLower().EndsWith("oes"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 3, 3) + "ao";
                    }
                    else if (Classe.ToLower().EndsWith("aos"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 3, 3) + "ao";
                    }
                    else if (Classe.ToLower().EndsWith("paes"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 3, 3) + "ao";
                    }
                    else if (Classe.ToLower().EndsWith("aes"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 3, 3) + "ae";
                    }
                    else if (Classe.ToLower().EndsWith("res"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 2, 2);
                    }
                    else if (Classe.ToLower().EndsWith("ras"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 1, 1);
                    }
                    else if (Classe.ToLower().EndsWith("s"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 1, 1);
                    }



                    ApelidoInfo = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                    ApelidoBo = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                    ApelidoDao = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");

                    if (string.IsNullOrEmpty(ApelidoPlural))
                    {
                        if (ApelidoInfo.EndsWith("pao"))
                            ApelidoPlural = ApelidoInfo.Remove(ApelidoInfo.Length - 2, 2) + "aes";
                        else if (ApelidoInfo.EndsWith("ao"))
                            ApelidoPlural = ApelidoInfo.Remove(ApelidoInfo.Length - 2, 2) + "oes";
                        else if (ApelidoInfo.EndsWith("r"))
                            ApelidoPlural = ApelidoInfo + "es";
                        else
                            ApelidoPlural = ApelidoInfo + "s";
                    }

                    ClasseInfo = Classe + "Info";
                    ClasseBo = Classe + "Bo";
                    ClasseDao = Classe + "Dao";

                    ApelidoInfo += "Info";
                    ApelidoBo += "Bo";
                    ApelidoDao += "Dao";

                    ClassePlural = ApelidoPlural.Substring(0, 1).ToUpper() + (ApelidoPlural.Length > 1 ? ApelidoPlural.Substring(1) : "");

                    ArquivoModel = ClasseInfo + ".cs";
                    ArquivoBo = ClasseBo + ".cs";
                    ArquivoDao = ClasseDao + ".cs";
                }
            }
            public string Classe { get; private set; }
            public string ClasseInfo { get; private set; }
            public string ClasseBo { get; private set; }
            public string ClasseDao { get; private set; }
            public string ClassePlural { get; private set; }
            public string ApelidoInfo { get; private set; }
            public string ApelidoBo { get; private set; }
            public string ApelidoDao { get; private set; }
            public string ApelidoPlural { get; private set; }
            public string ArquivoModel { get; private set; }
            public string ArquivoBo { get; private set; }
            public string ArquivoDao { get; private set; }

            public List<ColunaInfo> colunas = null;

        }
        class ColunaInfo
        {
            private string tipo;
            private string descricao;

            public string Descricao
            {
                get
                {
                    return descricao;
                }
                set
                {
                    descricao = value;

                    if (!string.IsNullOrEmpty(descricao))
                    {
                        DescricaoDB = descricao.ToLower();

                        if (descricao.Equals("_id"))
                        {
                            descricao = "Id";
                        }
                        else
                        {
                            if (descricao.EndsWith("_id"))
                            {
                                var t = new TabelaInfo();
                                t.Descricao = descricao.Remove(descricao.Length - 3, 3);

                                ClasseRelacional = t.Classe;
                                ClasseRelacionalInfo = t.ClasseInfo;
                                ClasseRelacionalDao = t.ClasseDao;
                                ClasseRelacionalApelido = t.ApelidoInfo;
                            }
                        }

                        descricao = descricao.Substring(0, 1).ToUpper() + (descricao.Length > 1 ? descricao.Substring(1) : "");

                        DescricaoReferencia = DescricaoDB.ToUpper();
                    }
                }
            }
            public string Tipo
            {
                get
                {
                    return tipo;
                }
                set
                {
                    if (Descricao.Contains("sincronizado"))
                    {

                    }

                    tipo = value.ToUpper();

                    if (tipo.Contains("VARCHAR") || tipo.Contains("TEXT"))
                    {
                        TipoVariavel = TipoVariavelEnum.String;
                    }
                    else if (tipo.Contains("BIGINT"))
                    {
                        TipoVariavel = TipoVariavelEnum.Long;
                    }
                    else if (tipo.Contains("BOOL") || tipo.Contains("TINYINT"))
                    {
                        TipoVariavel = TipoVariavelEnum.Bool;
                    }
                    else if (tipo.Contains("INT"))
                    {
                        if (Descricao.ToUpper().Contains("ID") || Descricao.ToUpper().Contains("CODIGO") || Descricao.ToUpper().Contains("CNPJ") || Descricao.ToUpper().Contains("CPF"))
                            TipoVariavel = TipoVariavelEnum.Long;
                        else
                            TipoVariavel = TipoVariavelEnum.Int;
                    }
                    else if (tipo.Contains("DATE"))
                    {
                        TipoVariavel = TipoVariavelEnum.DateTime;
                    }
                    else if (tipo.Contains("DECIMAL") || tipo.Contains("DOUBLE"))
                    {
                        TipoVariavel = TipoVariavelEnum.Decimal;
                    }
                    else
                    {
                        throw new Exception("Tipo não implementado: " + value);
                    }
                }
            }
            public string Default { get; set; }
            public string Comentario { get; set; }
            public bool ChavePrimaria { get; set; }
            public bool AceitaNulo { get; set; }
            public bool AutoIncremento { get; set; }
            public bool Index { get; set; }


            public string DescricaoDB { get; private set; }
            public string DescricaoReferencia { get; private set; }

            public string ClasseRelacional { get; private set; }
            public string ClasseRelacionalInfo { get; private set; }
            public string ClasseRelacionalDao { get; private set; }
            public string ClasseRelacionalApelido { get; private set; }

            public TipoVariavelEnum TipoVariavel
            {
                get; private set;
            }
        }

        enum TipoVariavelEnum
        {
            [Description("string")]
            String,
            [Description("int")]
            Int,
            [Description("long")]
            Long,
            [Description("DateTime")]
            DateTime,
            [Description("decimal")]
            Decimal,
            [Description("bool")]
            Bool
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }
        private void frmCriaAtualiza_Load(object sender, EventArgs e)
        {
            dgTabelasAlteradas.AutoGenerateColumns = false;
            CarregarDatabases();
            ControlaBotoes(true, false, true);
        }
        private void frmCriaAtualiza_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                LimparDiretorio(Application.StartupPath + "\\tempTabelas");
            }
            catch { }
        }

        void ControlaBotoes(bool Confirmar, bool Cancelar, bool Fechar)
        {
            btnConfirmarProcesso.Enabled = Confirmar;
            btnCancelar.Enabled = Cancelar;
            btnFechar.Enabled = Fechar;
        }
        ArrayList GetFiles(string _pastaCaminho)
        {
            ArrayList RetornaArquivos = new ArrayList();
            DirectoryInfo DirInfo = new DirectoryInfo(_pastaCaminho);
            if (DirInfo.Exists)
            {
                FileSystemInfo[] ArquivoInfo = DirInfo.GetFileSystemInfos();
                foreach (FileSystemInfo fil in ArquivoInfo)
                    RetornaArquivos.Add(fil.Name);
            }
            return RetornaArquivos;
        }
        void CarregarDatabases()
        {
            try
            {
                string comando = @"
                    select schema_name as `Database`
                       from information_schema.schemata";

                DataTable dtDatabases = Accessor.Funcoes.FillDataTable(CommandType.Text, comando, null);

                cmbDatabase.DataSource = dtDatabases;
                cmbDatabase.DisplayMember = "Database";
                cmbDatabase.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(Program.strNomeBanco))
                {
                    foreach (DataRow r in dtDatabases.Rows)
                    {
                        if (r["Database"].ToString() == Program.strNomeBanco)
                        {
                            cmbDatabase.Text = Program.strNomeBanco;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Accessor.Funcoes.MensagemDeErro("Não foi possível recuperar os databases.\n" + ex.Message);
            }
        }

        bool CriarArquivos(string diretorio)
        {
            try
            {
                LimparDiretorio(diretorio);

                if (!Directory.Exists(diretorio))
                    Directory.CreateDirectory(diretorio);

                if (!Directory.Exists(diretorio + "\\BaseModel"))
                    Directory.CreateDirectory(diretorio + "\\BaseModel");

                if (!Directory.Exists(diretorio + "\\Model"))
                    Directory.CreateDirectory(diretorio + "\\Model");

                if (!Directory.Exists(diretorio + "\\BaseBLL"))
                    Directory.CreateDirectory(diretorio + "\\BaseBLL");

                if (!Directory.Exists(diretorio + "\\BLL"))
                    Directory.CreateDirectory(diretorio + "\\BLL");

                if (!Directory.Exists(diretorio + "\\BaseDAL"))
                    Directory.CreateDirectory(diretorio + "\\BaseDAL");

                if (!Directory.Exists(diretorio + "\\DAL"))
                    Directory.CreateDirectory(diretorio + "\\DAL");

                string comando = @"
                    SELECT *, 0 as ordenar
                       FROM INFORMATION_SCHEMA.`TABLES`
                      WHERE TABLE_SCHEMA = ?TABLE_NAME
                        AND TABLE_TYPE   = 'BASE TABLE';";
                MySqlParameter[] parms = new MySqlParameter[1];
                parms[0] = Accessor.Funcoes.CreateParameter("?TABLE_NAME", MySqlDbType.VarChar, cmbDatabase.Text);

                DataTable dtTablesOrigin = Accessor.Funcoes.FillDataTable(CommandType.Text, comando, parms);

                List<TabelaInfo> tabelas = new List<TabelaInfo>();

                foreach (DataRow r in dtTablesOrigin.Rows)
                {
                    string nomeTabela = r["table_name"].ToString().ToLower();

                    TabelaInfo tabela = new TabelaInfo();
                    tabela.Descricao = nomeTabela;

                    tabelas.Add(tabela);

                    string recoverSelectColumn = @"
                        SELECT *, if (column_key = 'PRI', '9999', '') as ordernarKey
                           FROM INFORMATION_SCHEMA.`COLUMNS`
                          WHERE TABLE_NAME   = ?TABLE_NAME
                            AND TABLE_SCHEMA = ?TABLE_SCHEMA
                         ORDER BY ordernarKey desc, ORDINAL_POSITION;";

                    parms = new MySqlParameter[2];
                    parms[0] = Accessor.Funcoes.CreateParameter("?TABLE_NAME", MySqlDbType.VarChar, nomeTabela);
                    parms[1] = Accessor.Funcoes.CreateParameter("?TABLE_SCHEMA", MySqlDbType.VarChar, cmbDatabase.Text);

                    StringBuilder _columns = new StringBuilder();
                    List<string> primaryKeys = new List<string>();

                    ColunaInfo coluna = null;
                    using (MySqlDataReader rdr = Accessor.Funcoes.ExecuteReader(CommandType.Text, recoverSelectColumn, parms))
                    {
                        while (rdr.Read())
                        {
                            coluna = new ColunaInfo();
                            coluna.Descricao = rdr["column_name"].ToString();
                            coluna.Tipo = rdr["column_type"].ToString();
                            coluna.Default = rdr["column_default"].ToString();
                            coluna.Comentario = rdr["column_comment"].ToString();
                            coluna.ChavePrimaria = rdr["column_key"].ToString().ToLower().Contains("pri");
                            coluna.AceitaNulo = rdr["is_nullable"].ToString().ToLower().Contains("yes");
                            coluna.AutoIncremento = rdr["extra"].ToString().ToLower().Contains("auto_increment");
                            coluna.Index = rdr["column_key"].ToString().ToLower().Contains("mul");

                            tabela.colunas.Add(coluna);

                            if (rdr["column_key"].ToString().ToLower().Contains("pri"))
                                primaryKeys.Add(rdr["column_name"].ToString());
                        }
                    }

                    #region CriaArquivo Base Model
                    File.Create(diretorio + "\\BaseModel\\" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\BaseModel\\" + tabela.ArquivoModel))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);
                        bool existeComment = (tabela.colunas.Find(p => !string.IsNullOrEmpty(p.Comentario)) != null);

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");

                        if (existeComment)
                            arquivo.WriteLine("using Newtonsoft.Json;");

                        arquivo.WriteLine("using " + txtPacote.Text + ".BaseObjects;");
                        arquivo.WriteLine("using WebServiceSales.Library.DAL;");
                        arquivo.WriteLine("using WebServiceSales.Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.Model");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseInfo + " : BaseInfo");
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("        private Funcoes mFuncoes;");
                        arquivo.WriteLine("");

                        // Cria variaveis privadas
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (!string.IsNullOrEmpty(c.Comentario) && c.Comentario != "notjson")
                                arquivo.WriteLine("        [JsonProperty(\"" + c.Comentario + "\")]");

                            if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                arquivo.WriteLine("        public " + EnumDescription.GetDescription(c.TipoVariavel) + "? " + c.Descricao + " { get; set; }");
                            else
                                arquivo.WriteLine("        public " + EnumDescription.GetDescription(c.TipoVariavel) + " " + c.Descricao + " { get; set; }");
                        }

                        // Cria variaveis de classes relacionais
                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("");
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacionalInfo))
                                    arquivo.WriteLine("        public " + c.ClasseRelacionalInfo + " " + c.ClasseRelacionalApelido + " { get; set; }");
                            }
                        }

                        arquivo.WriteLine("        public " + tabela.ClasseInfo + "()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            mFuncoes = Funcoes.newInstance();");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + "(" + tabela.ClasseInfo + " t)");
                        arquivo.WriteLine("            : this()");
                        arquivo.WriteLine("        {");

                        // Cria variaveis no construtor da classe
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("            this." + c.Descricao + " = t." + c.Descricao + ";");
                        }

                        // Cria variaveis de classes relacionais no construtor da classe
                        if (existeLazyLoading)
                        {
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacionalInfo))
                                    arquivo.WriteLine("            this." + c.ClasseRelacionalApelido + " = t." + c.ClasseRelacionalApelido + ";");
                            }
                        }

                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");


                        arquivo.WriteLine("    }");

                        arquivo.WriteLine("");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Model
                    File.Create(diretorio + "\\Model\\" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\Model\\" + tabela.ArquivoModel))
                    {
                        bool existeComment = (tabela.colunas.Find(p => !string.IsNullOrEmpty(p.Comentario)) != null);

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");

                        if (existeComment)
                            arquivo.WriteLine("using Newtonsoft.Json;");

                        arquivo.WriteLine("using " + txtPacote.Text + ".BaseObjects;");
                        arquivo.WriteLine("using WebServiceSales.Library.DAL;");
                        arquivo.WriteLine("using WebServiceSales.Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.Model");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseInfo + " : BaseInfo");
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Base BLL
                    File.Create(diretorio + "\\BaseBLL\\Base" + tabela.ArquivoBo).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\BaseBLL\\Base" + tabela.ArquivoBo))
                    {
                        arquivo.WriteLine("using " + txtPacote.Text + ".BaseObjects;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Util;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.Model;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.DAL;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.BLL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseBo);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("        private static " + tabela.ClasseBo + " " + tabela.ApelidoBo + ";");
                        arquivo.WriteLine("        private static Funcoes mFuncoes;");
                        arquivo.WriteLine("        private static " + tabela.ClasseDao + " " + tabela.ApelidoDao + ";");
                        arquivo.WriteLine("        ");
                        arquivo.WriteLine("        private " + tabela.ClasseBo + "()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            mFuncoes = Funcoes.newInstance();");
                        arquivo.WriteLine("            " + tabela.ApelidoDao + " = DAOFactory.get" + tabela.ClasseDao + "();");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public static " + tabela.ClasseBo + " newInstance()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            if (" + tabela.ApelidoBo + " == null)");
                        arquivo.WriteLine("                " + tabela.ApelidoBo + " = new " + tabela.ClasseBo + "();");
                        arquivo.WriteLine("            return " + tabela.ApelidoBo + ";");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Salvar(" + tabela.ClasseInfo + " _obj, MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".Salvar(_obj, _trans);");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("        public bool Salvar(" + tabela.ClasseInfo + " _obj)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                MySqlTransaction trans = mFuncoes.BeginTransaction();");
                        arquivo.WriteLine("                bool sucesso = Salvar(_obj, trans);");
                        arquivo.WriteLine("                if (sucesso)");
                        arquivo.WriteLine("                    mFuncoes.CommitTransaction(trans);");
                        arquivo.WriteLine("                else");
                        arquivo.WriteLine("                    mFuncoes.RollbackTransaction(trans);");
                        arquivo.WriteLine("                return sucesso;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Excluir(long Id, MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".Excluir(Id, _trans);");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("        public bool Excluir(long Id)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                MySqlTransaction trans = mFuncoes.BeginTransaction();");
                        arquivo.WriteLine("                bool sucesso = Excluir(Id, trans);");
                        arquivo.WriteLine("                if (sucesso)");
                        arquivo.WriteLine("                    mFuncoes.CommitTransaction(trans);");
                        arquivo.WriteLine("                else");
                        arquivo.WriteLine("                    mFuncoes.RollbackTransaction(trans);");
                        arquivo.WriteLine("                return sucesso;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " get" + tabela.Classe + "ById(long Id, bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".get" + tabela.Classe + "ById(Id, lazyLoading);");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");

                        string parametroLazyLoading = string.Empty;
                        string variavelLazyLoading = string.Empty;

                        List<ColunaInfo> joins = new List<ColunaInfo>();
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            bool lista = false;
                            bool join = false;

                            if (CriarSelect(c.Descricao, ref lista, ref join))
                            {
                                if (join)
                                {
                                    joins.Add(c);

                                    parametroLazyLoading = ", bool lazyLoading = false";
                                    variavelLazyLoading = ", lazyLoading";
                                }

                                string variavel = c.TipoVariavel.ToString().StartsWith("S") ? c.TipoVariavel.ToString() : c.TipoVariavel.ToString().ToLower();

                                if (variavel.Equals("integer"))
                                    variavel = "int";

                                string pesquisaPor = "Por" + c.Descricao;

                                if (!lista)
                                {
                                    arquivo.WriteLine("        public " + tabela.ClasseInfo + " get" + tabela.Classe + pesquisaPor + "(" + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                return " + tabela.ApelidoDao + ".get" + tabela.Classe + pesquisaPor + "(" + c.Descricao + variavelLazyLoading + ");");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            catch");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                throw;");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                                else
                                {
                                    arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + pesquisaPor + "List(" + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                return " + tabela.ApelidoDao + ".get" + tabela.ClassePlural + pesquisaPor + "List(" + c.Descricao + variavelLazyLoading + ");");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            catch");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                throw;");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                            }
                        }

                        if (joins.Count > 1)
                        {
                            StringBuilder parametros = new StringBuilder();
                            StringBuilder parametrosPassar = new StringBuilder();

                            foreach (ColunaInfo c in joins)
                            {
                                string variavel = c.TipoVariavel.ToString().StartsWith("S") ? c.TipoVariavel.ToString() : c.TipoVariavel.ToString().ToLower();

                                parametros.Append(variavel + " " + c.Descricao + ", ");

                                parametrosPassar.Append(c.Descricao + ", ");
                            }

                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + "PorParametrosList(" + parametros.ToString().Remove(parametros.Length - 2, 2) + parametroLazyLoading + ")");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                return " + tabela.ApelidoDao + ".get" + tabela.ClassePlural + "PorParametrosList(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + variavelLazyLoading + ");");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + "List(" + (parametroLazyLoading.Length > 0 ? parametroLazyLoading.Substring(2) : "") + ")");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".get" + tabela.ClassePlural + "List(" + (variavelLazyLoading.Length > 0 ? variavelLazyLoading.Substring(2) : "") + ");");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            catch");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                throw;");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo BLL
                    File.Create(diretorio + "\\BLL\\" + tabela.ArquivoBo).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\BLL\\" + tabela.ArquivoBo))
                    {
                        arquivo.WriteLine("using " + txtPacote.Text + ".BaseObjects;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Util;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.Model;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.DAL;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.BLL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseBo);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Base DAL
                    File.Create(diretorio + "\\BaseDAL\\" + tabela.ArquivoDao).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\BaseDAL\\" + tabela.ArquivoDao))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);
                        string chavePrimaria = tabela.colunas.Find(p => p.ChavePrimaria).Descricao;
                        string chavePrimariaDB = tabela.colunas.Find(p => p.ChavePrimaria).DescricaoDB;

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("using System.Data;");
                        arquivo.WriteLine("using System.Linq;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.Model;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.DAL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseDao);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("        private static Funcoes mFuncoes;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public " + tabela.ClasseDao + "()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            mFuncoes = Funcoes.newInstance();");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        #region Parametros");

                        // Cria params das colunas, colunas, colunas de parametros e colunas de update

                        StringBuilder colunas = new StringBuilder();
                        StringBuilder colunasParametros = new StringBuilder();
                        StringBuilder colunasUpdate = new StringBuilder();

                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        const string param" + c.Descricao + " = \"?" + c.DescricaoDB + "\";");

                            colunas.Append(c.DescricaoDB).Append(",");

                            if (!c.ChavePrimaria)
                            {
                                colunasParametros.Append("?").Append(c.DescricaoDB).Append(",");
                                colunasUpdate.Append(c.DescricaoDB).Append("=?").Append(c.DescricaoDB);
                            }
                        }

                        colunas.Remove(colunas.Length - 1, 1);
                        colunasParametros.Remove(colunasParametros.Length - 1, 1);
                        colunasUpdate.Remove(colunasUpdate.Length - 1, 1);

                        arquivo.WriteLine("        #endregion");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        #region Sql Commands");
                        arquivo.WriteLine("        const string colunas = \"" + colunas + "\";");
                        arquivo.WriteLine("        const string colunasParametros = \"" + colunasParametros + "\";");
                        arquivo.WriteLine("        const string colunasUpdate = \"" + colunasUpdate + "\";");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        const string cmdInserir = \"insert into " + tabela.Descricao + " (\" + colunas + \") values (\" + colunasParametros + \");\";");
                        arquivo.WriteLine("        const string cmdAlterar = \"update " + tabela.Descricao + " set \" + colunasUpdate + \" where " + chavePrimariaDB + "=?" + chavePrimariaDB + "\";");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        const string cmdExcluiPorId = \"delete from " + tabela.Descricao + " where " + chavePrimariaDB + "=?" + chavePrimariaDB + "\";");
                        arquivo.WriteLine("        const string cmdRetornaPorId = \"select * from " + tabela.Descricao + " where " + chavePrimariaDB + "=?" + chavePrimariaDB + "\";");

                        List<ColunaInfo> joins = new List<ColunaInfo>();
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            bool lista = false;
                            bool join = false;

                            if (CriarSelect(c.Descricao, ref lista, ref join))
                            {
                                if (join)
                                    joins.Add(c);

                                string pesquisaPor = "Por" + c.Descricao;

                                arquivo.WriteLine("        const string cmdRetorna" + pesquisaPor + " = \"select * from " + tabela.Descricao + " where " + c.DescricaoDB + "=?" + c.DescricaoDB + "\";");
                            }
                        }

                        if (joins.Count > 1)
                        {
                            StringBuilder parametros = new StringBuilder();

                            foreach (ColunaInfo c in joins)
                                parametros.Append(c.DescricaoDB).Append("=?").Append(c.DescricaoDB).Append(" and ");

                            arquivo.WriteLine("        const string cmdRetornaPorParametros = \"select * from " + tabela.Descricao + " where " + parametros.Remove(parametros.Length - 5, 5) + "\";");
                        }


                        arquivo.WriteLine("        const string cmdRetornaTodos = \"select * from " + tabela.Descricao + ";\";");
                        arquivo.WriteLine("        #endregion");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Inserir(" + tabela.ClasseInfo + " _obj, MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            long id = 0;");
                        arquivo.WriteLine("            bool sucesso = mFuncoes.ExecuteNonQuery(_trans, CommandType.Text, cmdInserir, New" + tabela.Classe + "Parameters(_obj, false), out id);");
                        arquivo.WriteLine("            if (sucesso && id > 0)");
                        arquivo.WriteLine("                _obj." + chavePrimaria + " = id;");
                        arquivo.WriteLine("            return sucesso;");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Atualizar(" + tabela.ClasseInfo + " _obj, MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            return mFuncoes.ExecuteNonQuery(_trans, CommandType.Text, cmdAlterar, New" + tabela.Classe + "Parameters(_obj, true));");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Salvar(" + tabela.ClasseInfo + " _obj, MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            return _obj." + chavePrimaria + " == 0 ? Inserir(_obj, _trans) : Atualizar(_obj, _trans);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public bool Excluir(long " + chavePrimaria + ", MySqlTransaction _trans)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                        arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + chavePrimaria + ", MySqlDbType.Int64, " + chavePrimaria + ");");
                        arquivo.WriteLine("            return mFuncoes.ExecuteNonQuery(_trans, CommandType.Text, cmdExcluiPorId, parms);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " get" + tabela.Classe + "ById(long " + chavePrimaria + ", bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                        arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + chavePrimaria + ", MySqlDbType.Int64, " + chavePrimaria + ");");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(CommandType.Text, cmdRetornaPorId, parms))");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                if (rdr.Read())");
                        arquivo.WriteLine("                    return New" + tabela.ClasseInfo + "(rdr, lazyLoading);");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            return null;");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");

                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            bool lista = false;
                            bool join = false;

                            if (CriarSelect(c.Descricao, ref lista, ref join))
                            {
                                string variavel = EnumDescription.GetDescription(c.TipoVariavel);
                                string variavelMySqlType = "String";
                                if (c.TipoVariavel.Equals(TipoVariavelEnum.Int))
                                    variavelMySqlType = "Int32";
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Long))
                                    variavelMySqlType = "Int64";
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Decimal))
                                    variavelMySqlType = "Decimal";
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                    variavelMySqlType = "DateTime";
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Bool))
                                    variavelMySqlType = "Bool";

                                string pesquisaPor = "Por" + c.Descricao;

                                if (!lista)
                                {
                                    arquivo.WriteLine("        public " + tabela.ClasseInfo + " get" + tabela.Classe + pesquisaPor + "(" + variavel + " " + c.Descricao + ", bool lazyLoading = false)");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                                    arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType." + variavelMySqlType + ", " + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(CommandType.Text, cmdRetorna" + pesquisaPor + ", parms))");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                if (rdr.Read())");
                                    arquivo.WriteLine("                    return New" + tabela.ClasseInfo + "(rdr, lazyLoading);");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            return null;");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                                else
                                {
                                    arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + pesquisaPor + "List(" + variavel + " " + c.Descricao + ", bool lazyLoading = false)");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                                    arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType." + variavelMySqlType + ", " + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                                    arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(CommandType.Text, cmdRetorna" + pesquisaPor + ", parms))");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                while (rdr.Read())");
                                    arquivo.WriteLine("                    lst.Add(New" + tabela.ClasseInfo + "(rdr, lazyLoading));");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            return lst;");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                            }
                        }

                        if (joins.Count > 1)
                        {
                            StringBuilder parametros = new StringBuilder();

                            foreach (ColunaInfo c in joins)
                            {
                                string variavel = EnumDescription.GetDescription(c.TipoVariavel);

                                parametros.Append(variavel + " " + c.Descricao + ", ");
                            }

                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + "PorParametrosList(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ", bool lazyLoading = false)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[" + joins.Count + "];");

                            int xParmsJoin = 0;
                            foreach (ColunaInfo c in joins)
                            {
                                string variavel = EnumDescription.GetDescription(c.TipoVariavel);
                                if (c.TipoVariavel.Equals(TipoVariavelEnum.Int))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Int32, " + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Long))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Int64, " + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Decimal))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Decimal, " + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.DateTime, " + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Bool))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Bit, " + c.Descricao + ");");
                                else
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.String, " + c.Descricao + ");");

                                xParmsJoin++;
                            }

                            arquivo.WriteLine("");
                            arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                            arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(CommandType.Text, cmdRetornaPorParametros, parms))");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                while (rdr.Read())");
                            arquivo.WriteLine("                    lst.Add(New" + tabela.ClasseInfo + "(rdr, lazyLoading));");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            return lst;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> get" + tabela.ClassePlural + "List(bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                        arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(CommandType.Text, cmdRetornaTodos, null))");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                while (rdr.Read())");
                        arquivo.WriteLine("                    lst.Add(New" + tabela.ClasseInfo + "(rdr, lazyLoading));");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("            return lst;");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");

                        arquivo.WriteLine("        MySqlParameter[] New" + tabela.Classe + "Parameters(" + tabela.ClasseInfo + " _obj, bool withId)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[withId ? " + tabela.colunas.Count + " : " + (tabela.colunas.Count - 1) + "];");

                        // Adiciona os parametros de colunas que não sejam a chave primaria
                        int xParms = 0;
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (!c.ChavePrimaria)
                            {
                                if (c.TipoVariavel.Equals(TipoVariavelEnum.Int))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Int32, _obj." + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Long))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Int64, _obj." + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Decimal))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Decimal, _obj." + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.DateTime, _obj." + c.Descricao + ");");
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Bool))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.Bit, _obj." + c.Descricao + ");");
                                else
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.String, _obj." + c.Descricao + ");");

                                xParms++;
                            }
                        }
                        // Adiciona o parametro do ID
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            if (withId)");
                        arquivo.WriteLine("                parms[" + xParms + "] = mFuncoes.CreateParameter(param" + chavePrimaria + ", MySqlDbType.Int64, _obj." + chavePrimaria + ");");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            return parms;");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        " + tabela.ClasseInfo + " New" + tabela.ClasseInfo + "(MySqlDataReader rdr, bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            " + tabela.ClasseInfo + " " + tabela.ApelidoInfo + " = new " + tabela.ClasseInfo + "();");

                        // Cria new info do data reader
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (c.TipoVariavel.Equals(TipoVariavelEnum.Int))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToInt32(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Long))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToInt64(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Decimal))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToDecimal(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.GetDateTimeOrNull(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Bool))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToBoolean(rdr[\"" + c.DescricaoDB + "\"]);");
                            else
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToString(rdr[\"" + c.DescricaoDB + "\"]);");
                        }

                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            if (lazyLoading)");
                            arquivo.WriteLine("                lazyLoadingMethod(" + tabela.ApelidoInfo + ");");
                        }
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            return " + tabela.ApelidoInfo + ";");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");

                        // Cria método lazy loading
                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("        void lazyLoadingMethod(" + tabela.ClasseInfo + " " + tabela.ApelidoInfo + ")");
                            arquivo.WriteLine("        {");

                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacionalInfo))
                                {
                                    arquivo.WriteLine("            if (" + tabela.ApelidoInfo + "." + c.Descricao + " > 0)");
                                    arquivo.WriteLine("                " + tabela.ApelidoInfo + "." + c.ClasseRelacionalApelido + " = DAOFactory.get" + c.ClasseRelacionalDao + "().get" + c.ClasseRelacional + "ById(" + tabela.ApelidoInfo + "." + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                }
                            }

                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo DAL
                    File.Create(diretorio + "\\DAL\\" + tabela.ArquivoDao).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\DAL\\" + tabela.ArquivoDao))
                    {
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("using System.Data;");
                        arquivo.WriteLine("using System.Linq;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Library.Model;");
                        arquivo.WriteLine("using " + txtPacote.Text + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.DAL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public partial class " + tabela.ClasseDao);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion
                }

                if (tabelas.Count > 0)
                {
                    #region CriaArquivo DAOFactory
                    File.Create(diretorio + "\\DAL\\DAOFactory.cs").Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\DAL\\DAOFactory.cs"))
                    {
                        arquivo.WriteLine("namespace " + txtPacote.Text + ".Library.DAL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public class DAOFactory");
                        arquivo.WriteLine("    {");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("        private static " + t.ClasseDao + " " + t.ApelidoDao + ";");
                        }

                        arquivo.WriteLine("");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("        public static " + t.ClasseDao + " get" + t.ClasseDao + "() {");
                            arquivo.WriteLine("            if (" + t.ApelidoDao + " == null)");
                            arquivo.WriteLine("                " + t.ApelidoDao + " = new " + t.ClasseDao + "();");
                            arquivo.WriteLine("            return " + t.ApelidoDao + ";");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível criar os arquivos das tabelas.\n" + ex.Message);
            }
            return true;
        }
        void LimparDiretorio(string diretorio)
        {
            if (Directory.Exists(diretorio))
            {
                FileSystemInfo[] directory = new DirectoryInfo(diretorio).GetFileSystemInfos();
                foreach (FileSystemInfo file in directory)
                {
                    if (file.Attributes.Equals(FileAttributes.Directory))
                        LimparDiretorio(file.FullName);
                }

                FileInfo[] files = new DirectoryInfo(diretorio).GetFiles();
                foreach (FileInfo file in files)
                    File.Delete(file.FullName);

                Directory.Delete(diretorio);
            }
        }
        void AjustarGrid()
        {
            foreach (DataGridViewRow r in dgTabelasAlteradas.Rows)
            {
                if (r.Cells["ordenar"].Value.ToString() == "1")
                {
                    r.Cells["seleciona"].Value = true;
                    r.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                {
                    r.Cells["seleciona"].Value = false;
                    r.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        bool CriarSelect(string coluna, ref bool lista, ref bool join)
        {
            coluna = coluna.ToUpper();
            bool ok = true;

            if (coluna.Contains("CODIGO") || coluna.Contains("NUMERO") || coluna.Equals("EAN"))
            { }
            else if (coluna.Contains("DESCRICAO") || coluna.Contains("NOME") || coluna.Contains("RAZAO") || coluna.Contains("FANTASIA"))
            {
                lista = true;
            }
            else if (coluna != "ID" && coluna.EndsWith("ID"))
            {
                lista = true;
                join = true;
            }
            else
                ok = false;

            return ok;
        }
        string DefineOrdenacao(TabelaInfo tabela)
        {
            foreach (ColunaInfo c in tabela.colunas)
            {
                if (c.DescricaoReferencia.Contains("DESCRICAO") || c.DescricaoReferencia.Contains("NOME") || c.DescricaoReferencia.Contains("RAZAO") || c.DescricaoReferencia.Contains("FANTASIA"))
                    return tabela.ClasseInfo + ".Columns." + c.DescricaoReferencia;
            }
            return tabela.ClasseInfo + ".Columns._ID";
        }

        private void btnAtualizaDatabases_Click(object sender, EventArgs e)
        {
            CarregarDatabases();
        }
        private void btnConfirmarProcesso_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPacote.Text))
                {
                    Accessor.Funcoes.Aviso("Informar um pacote.");
                    return;
                }

                ControlaBotoes(false, false, false);
                CriarArquivos(Application.StartupPath + "\\tempTabelas");

                Accessor.Funcoes.Aviso("Terminou.");

                Process.Start("explorer.exe", Application.StartupPath + "\\" + "tempTabelas\\");
            }
            catch (Exception ex)
            {
                ControlaBotoes(true, false, true);
                Accessor.Funcoes.MensagemDeErro(ex.Message);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            dgTabelasAlteradas.Rows.Clear();
            ControlaBotoes(true, false, true);
        }
        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
            {
                foreach (DataGridViewRow r in dgTabelasAlteradas.Rows)
                {
                    r.Cells["seleciona"].Value = true;
                    r.Cells["ordenar"].Value = "1";
                    r.DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }
            else
            {
                foreach (DataGridViewRow r in dgTabelasAlteradas.Rows)
                {
                    r.Cells["seleciona"].Value = false;
                    r.Cells["ordenar"].Value = "0";
                    r.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dgTabelasAlteradas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(dgTabelasAlteradas.CurrentRow.Cells["seleciona"].FormattedValue) == true)
                dgTabelasAlteradas.CurrentRow.Cells["ordenar"].Value = "1";
            else
                dgTabelasAlteradas.CurrentRow.Cells["ordenar"].Value = "0";

            AjustarGrid();
        }
    }


}
