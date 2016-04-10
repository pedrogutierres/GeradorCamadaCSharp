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

                        Classe = (!string.IsNullOrEmpty(SiglaInicial) ? SiglaInicial + "_" : "") + arquivo.ToString();
                    }
                    else
                        Classe = (!string.IsNullOrEmpty(SiglaInicial) ? SiglaInicial + "_" : "") + descricao.Substring(0, 1).ToUpper() + (descricao.Length > 1 ? descricao.Substring(1) : "");

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
                    else if (Classe.ToLower().EndsWith("ais"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 3, 3) + "al";
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
                    else if (Classe.ToLower().EndsWith("ns"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 2, 2) + "m";
                    }
                    else if (Classe.ToLower().EndsWith("s"))
                    {
                        ApelidoPlural = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                        Classe = Classe.Remove(Classe.Length - 1, 1);
                    }

                    ApelidoInfo = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                    ApelidoBo = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                    ApelidoDao = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");
                    ClasseWebService = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");

                    if (string.IsNullOrEmpty(ApelidoPlural))
                    {
                        if (ApelidoInfo.EndsWith("pao"))
                            ApelidoPlural = ApelidoInfo.Remove(ApelidoInfo.Length - 2, 2) + "aes";
                        else if (ApelidoInfo.EndsWith("ao"))
                            ApelidoPlural = ApelidoInfo.Remove(ApelidoInfo.Length - 2, 2) + "oes";
                        else if (ApelidoInfo.EndsWith("al"))
                            ApelidoPlural = ApelidoInfo.Remove(ApelidoInfo.Length - 2, 2) + "ais";
                        else if (ApelidoInfo.EndsWith("r"))
                            ApelidoPlural = ApelidoInfo + "es";
                        else if (ApelidoInfo.EndsWith("m"))
                            ApelidoPlural = ApelidoInfo + "ns";
                        else
                            ApelidoPlural = ApelidoInfo + "s";
                    }

                    ClasseInfo = Classe + "Info";
                    ClasseBo = Classe + "Bo";
                    ClasseDao = Classe + "Dao";
                    ClasseWebService = Classe;

                    if (ClasseWebService.Contains("_"))
                    {
                        bool capsLock = false;
                        string web = string.Empty;
                        foreach (char c in ClasseWebService.ToCharArray())
                        {
                            if (c != '_')
                                web += capsLock ? c.ToString().ToUpper() : c.ToString();

                            capsLock = false;

                            if (c == '_')
                                capsLock = true;
                        }
                        ClasseWebService = web;
                    }

                    ApelidoInfo += "Info";
                    ApelidoBo += "Bo";
                    ApelidoDao += "Dao";

                    ClassePlural = ApelidoPlural.Substring(0, 1).ToUpper() + (ApelidoPlural.Length > 1 ? ApelidoPlural.Substring(1) : "");

                    ArquivoModel = ClasseInfo + ".cs";
                    ArquivoBo = ClasseBo + ".cs";
                    ArquivoDao = ClasseDao + ".cs";
                    ArquivoWebService = ClasseWebService + ".asmx.cs";
                }
            }
            public string Classe { get; private set; }
            public string ClasseInfo { get; private set; }
            public string ClasseBo { get; private set; }
            public string ClasseDao { get; private set; }
            public string ClasseWebService { get; private set; }
            public string ClassePlural { get; private set; }
            public string ApelidoInfo { get; private set; }
            public string ApelidoBo { get; private set; }
            public string ApelidoDao { get; private set; }
            public string ApelidoWebService { get; private set; }
            public string ApelidoPlural { get; private set; }
            public string ArquivoModel { get; private set; }
            public string ArquivoBo { get; private set; }
            public string ArquivoDao { get; private set; }
            public string ArquivoWebService { get; private set; }

            public string SiglaInicial { get; set; }

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

                        descricao = descricao.Replace("-", "_");

                        if (descricao.Equals("_id"))
                        {
                            descricao = "Id";
                        }
                        else if (ChavePrimaria)
                        {
                            descricao = "Id";
                        }
                        else
                        {
                            if (descricao.EndsWith("_id") || (descricao.StartsWith("id") && !ChavePrimaria))
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
                    tipo = value.ToUpper();

                    if (tipo.Contains("VARCHAR") || tipo.Contains("TEXT") || tipo.Contains("CHAR"))
                    {
                        TipoVariavel = TipoVariavelEnum.String;
                    }
                    else if (tipo.Contains("BIGINT") || tipo.Contains("LONGBLOB"))
                    {
                        TipoVariavel = TipoVariavelEnum.Long;
                    }
                    else if (tipo.Contains("BOOL") || tipo.Contains("TINYINT") || tipo.Contains("BIT"))
                    {
                        TipoVariavel = TipoVariavelEnum.Bool;
                    }
                    else if (tipo.Contains("BLOB"))
                    {
                        TipoVariavel = TipoVariavelEnum.Imagem;
                    }
                    else if (tipo.Contains("INT"))
                    {
                        if (Descricao.ToUpper().Contains("ID") || Descricao.ToUpper().Contains("CODIGO") || Descricao.ToUpper().Contains("CNPJ") || Descricao.ToUpper().Contains("CPF"))
                            TipoVariavel = TipoVariavelEnum.Long;
                        else
                            TipoVariavel = TipoVariavelEnum.Int;
                    }
                    else if (tipo.Contains("DATE") || tipo.Contains("TIME"))
                    {
                        TipoVariavel = TipoVariavelEnum.DateTime;
                    }
                    else if (tipo.Contains("DECIMAL") || tipo.Contains("DOUBLE") || tipo.Contains("FLOAT"))
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
            Bool,
            [Description("Image")]
            Imagem
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

                if (!Directory.Exists(diretorio + "\\ORM\\BaseModel"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\BaseModel");

                if (!Directory.Exists(diretorio + "\\ORM\\Model"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\Model");

                if (!Directory.Exists(diretorio + "\\ORM\\BaseBLL"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\BaseBLL");

                if (!Directory.Exists(diretorio + "\\ORM\\BLL"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\BLL");

                if (!Directory.Exists(diretorio + "\\ORM\\BaseDAL"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\BaseDAL");

                if (!Directory.Exists(diretorio + "\\ORM\\DAL"))
                    Directory.CreateDirectory(diretorio + "\\ORM\\DAL");

                if (!Directory.Exists(diretorio + "\\WebService"))
                    Directory.CreateDirectory(diretorio + "\\WebService");

                if (!Directory.Exists(diretorio + "\\Comunicador\\BaseBLL"))
                    Directory.CreateDirectory(diretorio + "\\Comunicador\\BaseBLL");

                if (!Directory.Exists(diretorio + "\\Comunicador\\BLL"))
                    Directory.CreateDirectory(diretorio + "\\Comunicador\\BLL");

                if (!Directory.Exists(diretorio + "\\Util"))
                    Directory.CreateDirectory(diretorio + "\\Util");

                string pacoteORM = chkWebService.Checked ? "ORM" : txtPacote.Text;
                string pacote = txtPacote.Text;
                string pacoteWebService = chkWebService.Checked && !string.IsNullOrEmpty(txtPacoteWebService.Text) ? txtPacoteWebService.Text : pacote;
                string stringConexaoParams = chkStringConexao.Checked ? "string strConexao, " : "";
                string stringConexao = chkStringConexao.Checked ? "strConexao, " : "";

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
                    tabela.SiglaInicial = txtSiglaInicial.Text;
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
                            coluna.ChavePrimaria = rdr["column_key"].ToString().ToLower().Contains("pri");
                            coluna.Descricao = rdr["column_name"].ToString();
                            coluna.Tipo = rdr["column_type"].ToString();
                            coluna.Default = rdr["column_default"].ToString();
                            coluna.Comentario = rdr["column_comment"].ToString();
                            coluna.AceitaNulo = rdr["is_nullable"].ToString().ToLower().Contains("yes");
                            coluna.AutoIncremento = rdr["extra"].ToString().ToLower().Contains("auto_increment");
                            coluna.Index = rdr["column_key"].ToString().ToLower().Contains("mul");

                            tabela.colunas.Add(coluna);

                            if (rdr["column_key"].ToString().ToLower().Contains("pri"))
                                primaryKeys.Add(rdr["column_name"].ToString());
                        }
                    }

                    if (tabela.colunas.Find(p => p.ChavePrimaria) == null)
                    {
                        bool encontrouChavePrimaria = false;
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (c.DescricaoDB.ToLower().Contains("id"))
                            {
                                c.ChavePrimaria = true;
                                encontrouChavePrimaria = true;
                                break;
                            }
                        }
                        if (!encontrouChavePrimaria)
                        {
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                c.ChavePrimaria = true;
                                break;
                            }
                        }
                    }

                    #region CriaArquivo Base Model
                    File.Create(diretorio + "\\ORM\\BaseModel\\" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\BaseModel\\" + tabela.ArquivoModel))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);
                        bool existeComment = (tabela.colunas.Find(p => !string.IsNullOrEmpty(p.Comentario)) != null);

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");

                        if (existeComment)
                            arquivo.WriteLine("using Newtonsoft.Json;");

                        arquivo.WriteLine("using " + pacoteORM + ".BaseObjects;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.Model");
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

                            if (c.ChavePrimaria)
                                arquivo.WriteLine("        public long " + c.Descricao + " { get; set; }");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
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
                                {
                                    if (existeComment)
                                        arquivo.WriteLine("        [JsonProperty(\"" + c.ClasseRelacionalApelido + "\")]");

                                    arquivo.WriteLine("        public " + c.ClasseRelacionalInfo + " " + c.ClasseRelacionalApelido + " { get; set; }");
                                }
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
                    File.Create(diretorio + "\\ORM\\Model\\" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\Model\\" + tabela.ArquivoModel))
                    {
                        bool existeComment = (tabela.colunas.Find(p => !string.IsNullOrEmpty(p.Comentario)) != null);

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");

                        if (existeComment)
                            arquivo.WriteLine("using Newtonsoft.Json;");

                        arquivo.WriteLine("using " + pacoteORM + ".BaseObjects;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.Model");
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
                    File.Create(diretorio + "\\ORM\\BaseBLL\\Base" + tabela.ArquivoBo).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\BaseBLL\\Base" + tabela.ArquivoBo))
                    {
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.BaseBLL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public abstract class Base" + tabela.ClasseBo);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("        protected static Base" + tabela.ClasseBo + " " + tabela.ApelidoBo + ";");
                        arquivo.WriteLine("        protected static Funcoes mFuncoes;");
                        arquivo.WriteLine("        protected static " + tabela.ClasseDao + " " + tabela.ApelidoDao + ";");
                        arquivo.WriteLine("        ");
                        arquivo.WriteLine("        protected Base" + tabela.ClasseBo + "()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            mFuncoes = Funcoes.newInstance();");
                        arquivo.WriteLine("            " + tabela.ApelidoDao + " = DAOFactory.get" + tabela.ClasseDao + "();");
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
                        arquivo.WriteLine("        public bool Salvar(" + stringConexaoParams + tabela.ClasseInfo + " _obj)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                MySqlTransaction trans = mFuncoes.BeginTransaction(" + stringConexao.Replace(", ", "") + ");");
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
                        arquivo.WriteLine("        public bool Excluir(" + stringConexaoParams + "long Id)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                MySqlTransaction trans = mFuncoes.BeginTransaction(" + stringConexao.Replace(", ", "") + ");");
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
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " RetornaPorId(" + stringConexaoParams + "long Id, bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".RetornaPorId(" + stringConexao + "Id, lazyLoading);");
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

                            if (CriarSelect(c, ref lista, ref join))
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
                                    arquivo.WriteLine("        public " + tabela.ClasseInfo + " Retorna" + pesquisaPor + "(" + stringConexaoParams + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                return " + tabela.ApelidoDao + ".Retorna" + pesquisaPor + "(" + stringConexao + c.Descricao + variavelLazyLoading + ");");
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
                                    arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> Retorna" + pesquisaPor + "(" + stringConexaoParams + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                return " + tabela.ApelidoDao + ".Retorna" + pesquisaPor + "(" + stringConexao + c.Descricao + variavelLazyLoading + ");");
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

                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaPorParametros(" + stringConexaoParams + parametros.ToString().Remove(parametros.Length - 2, 2) + parametroLazyLoading + ")");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                return " + tabela.ApelidoDao + ".RetornaPorParametros(" + stringConexao + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + variavelLazyLoading + ");");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaTodos(" + (parametroLazyLoading.Length > 0 ? stringConexaoParams + parametroLazyLoading.Substring(2) : stringConexaoParams.Replace(", ", "")) + ")");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            try");
                        arquivo.WriteLine("            {");
                        arquivo.WriteLine("                return " + tabela.ApelidoDao + ".RetornaTodos(" + (variavelLazyLoading.Length > 0 ? stringConexao + variavelLazyLoading.Substring(2) : stringConexao.Replace(", ", "")) + ");");
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
                    File.Create(diretorio + "\\ORM\\BLL\\" + tabela.ArquivoBo).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\BLL\\" + tabela.ArquivoBo))
                    {
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.BaseBLL;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.BLL");
                        arquivo.WriteLine("{");
                        arquivo.WriteLine("    public class " + tabela.ClasseBo + " : Base" + tabela.ClasseBo);
                        arquivo.WriteLine("    {");
                        arquivo.WriteLine("        private " + tabela.ClasseBo + "()");
                        arquivo.WriteLine("        { }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        public static " + tabela.ClasseBo + " newInstance()");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            if (" + tabela.ApelidoBo + " == null)");
                        arquivo.WriteLine("                " + tabela.ApelidoBo + " = new " + tabela.ClasseBo + "();");
                        arquivo.WriteLine("            return (" + tabela.ClasseBo + ")" + tabela.ApelidoBo + ";");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Base DAL
                    File.Create(diretorio + "\\ORM\\BaseDAL\\" + tabela.ArquivoDao).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\BaseDAL\\" + tabela.ArquivoDao))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);
                        string chavePrimaria = tabela.colunas.Find(p => p.ChavePrimaria).Descricao;
                        string chavePrimariaDB = tabela.colunas.Find(p => p.ChavePrimaria).DescricaoDB;

                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("using System.Data;");
                        arquivo.WriteLine("using System.Linq;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.DAL");
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

                            if (!c.ChavePrimaria)
                            {
                                colunas.Append(c.DescricaoDB).Append(",");
                                colunasParametros.Append("?").Append(c.DescricaoDB).Append(",");
                                colunasUpdate.Append(c.DescricaoDB).Append("=?").Append(c.DescricaoDB).Append(",");
                            }
                        }

                        if (colunas.Length > 0)
                            colunas.Remove(colunas.Length - 1, 1);
                        if (colunasParametros.Length > 0)
                            colunasParametros.Remove(colunasParametros.Length - 1, 1);
                        if (colunasUpdate.Length > 0)
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

                            if (CriarSelect(c, ref lista, ref join))
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
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " RetornaPorId(" + stringConexaoParams + "long " + chavePrimaria + ", bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                        arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + chavePrimaria + ", MySqlDbType.Int64, " + chavePrimaria + ");");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(" + stringConexao + "CommandType.Text, cmdRetornaPorId, parms))");
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

                            if (CriarSelect(c, ref lista, ref join))
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
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Imagem))
                                    variavelMySqlType = "Image";

                                string pesquisaPor = "Por" + c.Descricao;

                                if (!lista)
                                {
                                    arquivo.WriteLine("        public " + tabela.ClasseInfo + " Retorna" + pesquisaPor + "(" + stringConexaoParams + variavel + " " + c.Descricao + ", bool lazyLoading = false)");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                                    arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType." + variavelMySqlType + ", " + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(" + stringConexao + "CommandType.Text, cmdRetorna" + pesquisaPor + ", parms))");
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
                                    arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> Retorna" + pesquisaPor + "(" + stringConexaoParams + variavel + " " + c.Descricao + ", bool lazyLoading = false)");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            MySqlParameter[] parms = new MySqlParameter[1];");
                                    arquivo.WriteLine("            parms[0] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType." + variavelMySqlType + ", " + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                                    arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(" + stringConexao + "CommandType.Text, cmdRetorna" + pesquisaPor + ", parms))");
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

                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaPorParametros(" + stringConexaoParams + parametros.ToString().Remove(parametros.Length - 2, 2) + ", bool lazyLoading = false)");
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
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Imagem))
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.LongBlob, mFuncoes.ConvertImageToByteArray(" + c.Descricao + "));");
                                else
                                    arquivo.WriteLine("            parms[" + xParmsJoin + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.String, " + c.Descricao + ");");

                                xParmsJoin++;
                            }

                            arquivo.WriteLine("");
                            arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                            arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(" + stringConexao + "CommandType.Text, cmdRetornaPorParametros, parms))");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                while (rdr.Read())");
                            arquivo.WriteLine("                    lst.Add(New" + tabela.ClasseInfo + "(rdr, lazyLoading));");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            return lst;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaTodos(" + stringConexaoParams + "bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lst = new List<" + tabela.ClasseInfo + ">();");
                        arquivo.WriteLine("            using (MySqlDataReader rdr = mFuncoes.ExecuteReader(" + stringConexao + "CommandType.Text, cmdRetornaTodos, null))");
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
                                else if (c.TipoVariavel.Equals(TipoVariavelEnum.Imagem))
                                    arquivo.WriteLine("            parms[" + xParms + "] = mFuncoes.CreateParameter(param" + c.Descricao + ", MySqlDbType.LongBlob, mFuncoes.ConvertImageToByteArray(_obj." + c.Descricao + "));");
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
                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " New" + tabela.ClasseInfo + "(MySqlDataReader rdr, bool lazyLoading = false)");
                        arquivo.WriteLine("        {");
                        arquivo.WriteLine("            " + tabela.ClasseInfo + " " + tabela.ApelidoInfo + " = new " + tabela.ClasseInfo + "();");

                        // Cria new info do data reader
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (c.ChavePrimaria)
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToInt64(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Int))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToInt32(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Long))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToInt64(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Decimal))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToDecimal(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.DateTime))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.GetDateTimeOrNull(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Bool))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToBoolean(rdr[\"" + c.DescricaoDB + "\"]);");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Imagem))
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToImage(rdr, \"" + c.DescricaoDB + "\");");
                            else
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = mFuncoes.ConvertToString(rdr[\"" + c.DescricaoDB + "\"]);");
                        }

                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            if (lazyLoading)");
                            arquivo.WriteLine("                LazyLoadingMethod(" + tabela.ApelidoInfo + ");");
                        }
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            return " + tabela.ApelidoInfo + ";");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");

                        // Cria método lazy loading
                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("        void LazyLoadingMethod(" + tabela.ClasseInfo + " " + tabela.ApelidoInfo + ")");
                            arquivo.WriteLine("        {");

                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacionalInfo))
                                {
                                    arquivo.WriteLine("            if (" + tabela.ApelidoInfo + "." + c.Descricao + " > 0)");
                                    arquivo.WriteLine("                " + tabela.ApelidoInfo + "." + c.ClasseRelacionalApelido + " = DAOFactory.get" + c.ClasseRelacionalDao + "().RetornaPorId(" + tabela.ApelidoInfo + "." + c.Descricao + ");");
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
                    File.Create(diretorio + "\\ORM\\DAL\\" + tabela.ArquivoDao).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\DAL\\" + tabela.ArquivoDao))
                    {
                        arquivo.WriteLine("using MySql.Data.MySqlClient;");
                        arquivo.WriteLine("using System;");
                        arquivo.WriteLine("using System.Collections.Generic;");
                        arquivo.WriteLine("using System.Data;");
                        arquivo.WriteLine("using System.Linq;");
                        arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                        arquivo.WriteLine("using " + pacoteORM + ".Util;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.DAL");
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

                    #region CriaArquivo WebService Base
                    if (chkWebService.Checked)
                    {
                        File.Create(diretorio + "\\WebService\\Base" + tabela.ArquivoWebService.Replace(".cs", "")).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\WebService\\Base" + tabela.ArquivoWebService.Replace(".cs", "")))
                        {
                            arquivo.WriteLine("<%@ WebService Language=\"C#\" CodeBehind=\"Base" + tabela.ArquivoWebService + "\" Class=\"" + pacoteWebService + "." + tabela.ClasseWebService + "\" %>");
                            arquivo.Flush();
                            arquivo.Close();
                        }

                        File.Create(diretorio + "\\WebService\\Base" + tabela.ArquivoWebService).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\WebService\\Base" + tabela.ArquivoWebService))
                        {
                            string chavePrimaria = tabela.colunas.Find(p => p.ChavePrimaria).Descricao;
                            string chavePrimariaDB = tabela.colunas.Find(p => p.ChavePrimaria).DescricaoDB;

                            arquivo.WriteLine("using Newtonsoft.Json;");
                            arquivo.WriteLine("using System;");
                            arquivo.WriteLine("using System.Collections.Generic;");
                            arquivo.WriteLine("using System.Configuration;");
                            arquivo.WriteLine("using System.Data;");
                            arquivo.WriteLine("using System.IO;");
                            arquivo.WriteLine("using System.Text;");
                            arquivo.WriteLine("using System.Web.Configuration;");
                            arquivo.WriteLine("using System.Web.Script.Services;");
                            arquivo.WriteLine("using System.Web.Services;");
                            arquivo.WriteLine("using " + pacoteWebService + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.BLL;");
                            arquivo.WriteLine("using System.Web;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                            arquivo.WriteLine("using " + pacoteWebService + ".Library.Model;");
                            arquivo.WriteLine("using MySql.Data.MySqlClient;");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("namespace " + pacoteWebService);
                            arquivo.WriteLine("{");
                            arquivo.WriteLine("    /// <summary>");
                            arquivo.WriteLine("    /// Summary description for " + tabela.ClasseWebService);
                            arquivo.WriteLine("    /// </summary>");
                            arquivo.WriteLine("    [WebService(Namespace = \"http://tempuri.org/\")]");
                            arquivo.WriteLine("    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]");
                            arquivo.WriteLine("    [System.ComponentModel.ToolboxItem(false)]");
                            arquivo.WriteLine("    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. ");
                            arquivo.WriteLine("    // [System.Web.Script.Services.ScriptService]");
                            arquivo.WriteLine("    public partial class " + tabela.ClasseWebService + " : System.Web.Services.WebService");
                            arquivo.WriteLine("    {");
                            arquivo.WriteLine("        class Accessor");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            static Funcoes funcoes;");
                            arquivo.WriteLine("            internal static Funcoes Funcoes");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (funcoes == null)");
                            arquivo.WriteLine("                            funcoes = Funcoes.newInstance();");
                            arquivo.WriteLine("                        return funcoes;");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            static " + pacoteORM + ".Util.Funcoes funcoesDB;");
                            arquivo.WriteLine("            internal static " + pacoteORM + ".Util.Funcoes FuncoesDB");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (funcoesDB == null)");
                            arquivo.WriteLine("                            funcoesDB = ORM.Util.Funcoes.newInstance();");
                            arquivo.WriteLine("                        return funcoesDB;");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            static " + tabela.ClasseBo + " " + tabela.ApelidoBo + ";");
                            arquivo.WriteLine("            internal static " + tabela.ClasseBo + " " + tabela.ClasseBo);
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (" + tabela.ApelidoBo + " == null)");
                            arquivo.WriteLine("                            " + tabela.ApelidoBo + " = " + tabela.ClasseBo + ".newInstance();");
                            arquivo.WriteLine("                        return " + tabela.ApelidoBo + ";");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        private void AjustaPaginaApenasJson(string json)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            HttpContext.Current.Response.ContentType = \"application/json\";");
                            arquivo.WriteLine("            HttpContext.Current.Response.Write(json);");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");

                            if (rdbSOAP.Checked)
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        public bool Salvar(" + tabela.ClasseInfo + " _obj)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                return SalvarInfo(_obj);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string SalvarJson(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                ResultWeb<" + tabela.ClasseInfo + "> info = JsonConvert.DeserializeObject<ResultWeb<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = SalvarInfo(info.ResultInfo);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(sucesso);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string SalvarJsonResult(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<" + tabela.ClasseInfo + "> info = new ResultWeb<" + tabela.ClasseInfo + ">();");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = JsonConvert.DeserializeObject<" + tabela.ClasseInfo + ">(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                if (!SalvarInfo(info.ResultInfo))");
                                arquivo.WriteLine("                    throw new Exception(\"Não foi possível salvar o registro.\");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }
                            else
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void SalvarJson(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                ResultWeb<" + tabela.ClasseInfo + "> info = JsonConvert.DeserializeObject<ResultWeb<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = SalvarInfo(info.ResultInfo);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(sucesso.ToString());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void SalvarJsonResult(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<" + tabela.ClasseInfo + "> info = new ResultWeb<" + tabela.ClasseInfo + ">();");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = JsonConvert.DeserializeObject<" + tabela.ClasseInfo + ">(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                if (!SalvarInfo(info.ResultInfo))");
                                arquivo.WriteLine("                    throw new Exception(\"Não foi possível salvar o registro.\");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            arquivo.WriteLine("        private bool SalvarInfo(" + tabela.ClasseInfo + " _obj)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                if (Accessor." + tabela.ClasseBo + ".Salvar(_obj))");
                            arquivo.WriteLine("                    return true;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            return false;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            if (rdbSOAP.Checked)
                            {
                                arquivo.WriteLine("		   [WebMethod]");
                                arquivo.WriteLine("        public bool SalvarList(List<" + tabela.ClasseInfo + "> _obj)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                return SalvarListInfo(_obj);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string SalvarJsonList(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                List<" + tabela.ClasseInfo + "> infos = JsonConvert.DeserializeObject<List<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = SalvarList(infos);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(sucesso);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string SalvarJsonListResult(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<List<" + tabela.ClasseInfo + ">> info = new ResultWeb<List<" + tabela.ClasseInfo + ">>();");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = JsonConvert.DeserializeObject<List<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                if (!SalvarList(info.ResultInfo))");
                                arquivo.WriteLine("                    throw new Exception(\"Não foi possível salvar os registros.\");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }
                            else
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void SalvarJsonList(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                List<" + tabela.ClasseInfo + "> infos = JsonConvert.DeserializeObject<List<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = SalvarList(infos);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(sucesso.ToString());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void SalvarJsonListResult(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<List<" + tabela.ClasseInfo + ">> info = new ResultWeb<List<" + tabela.ClasseInfo + ">>();");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = JsonConvert.DeserializeObject<List<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                if (!SalvarList(info.ResultInfo))");
                                arquivo.WriteLine("                    throw new Exception(\"Não foi possível salvar os registros.\");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            arquivo.WriteLine("        private bool SalvarListInfo(List<" + tabela.ClasseInfo + "> infos)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            bool sucesso = false;");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            MySqlTransaction trans = null;");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                trans = Accessor.FuncoesDB.BeginTransaction();");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("                foreach (" + tabela.ClasseInfo + " t in infos)");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    sucesso = Accessor." + tabela.ClasseBo + ".Salvar(t, trans);");
                            arquivo.WriteLine("                    if (!sucesso)");
                            arquivo.WriteLine("                        break;");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("                if (sucesso)");
                            arquivo.WriteLine("                    Accessor.FuncoesDB.CommitTransaction(trans);");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                    Accessor.FuncoesDB.RollbackTransaction(trans);");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch (MySqlException ex)");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                try");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    if (trans != null)");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.RollbackTransaction(trans);");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("                catch { }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch (Exception ex)");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                try");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    if (trans != null)");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.RollbackTransaction(trans);");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("                catch { }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            return sucesso;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");

                            if (rdbSOAP.Checked)
                            {
                                arquivo.WriteLine("		   [WebMethod]");
                                arquivo.WriteLine("        public bool Excluir(" + tabela.ClasseInfo + " _obj)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                return ExcluirInfo(_obj." + chavePrimaria + ");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public bool ExcluirPorId(long Id)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                sucesso = ExcluirInfo(Id);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            return sucesso;");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string ExcluirJson(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                ResultWeb<" + tabela.ClasseInfo + "> info = JsonConvert.DeserializeObject<ResultWeb<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = ExcluirInfo(info.ResultInfo." + chavePrimaria + ");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(sucesso);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }
                            else
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void Excluir(long Id)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                sucesso = ExcluirInfo(Id);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(sucesso.ToString());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void ExcluirJson(string json)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            bool sucesso = false;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                ResultWeb<" + tabela.ClasseInfo + "> info = JsonConvert.DeserializeObject<ResultWeb<" + tabela.ClasseInfo + ">>(json);");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                sucesso = ExcluirInfo(info.ResultInfo." + chavePrimaria + ");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            { }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(sucesso.ToString());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            arquivo.WriteLine("        private bool ExcluirInfo(long Id)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                if (Accessor." + tabela.ClasseBo + ".Excluir(Id))");
                            arquivo.WriteLine("                    return true;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            return false;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");

                            if (rdbSOAP.Checked)
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        public " + tabela.ClasseInfo + " RetornaPorId(long Id)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                return Accessor." + tabela.ClasseBo + ".RetornaPorId(Id);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string RetornaPorIdJson(long Id)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            " + tabela.ClasseInfo + " info = null;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info = Accessor." + tabela.ClasseBo + ".RetornaPorId(Id);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }
                            else
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void RetornaPorIdJson(long Id)");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<" + tabela.ClasseInfo + "> info = new ResultWeb<" + tabela.ClasseInfo + ">();");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = Accessor." + tabela.ClasseBo + ".RetornaPorId(Id);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            List<ColunaInfo> joins = new List<ColunaInfo>();
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                bool lista = false;
                                bool join = false;

                                if (CriarSelect(c, ref lista, ref join))
                                {
                                    if (join)
                                        joins.Add(c);

                                    string variavel = c.TipoVariavel.ToString().StartsWith("S") ? c.TipoVariavel.ToString() : c.TipoVariavel.ToString().ToLower();

                                    if (variavel.Equals("integer"))
                                        variavel = "int";

                                    string pesquisaPor = "Por" + c.Descricao;

                                    if (!lista)
                                    {
                                        if (rdbSOAP.Checked)
                                        {
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        public " + tabela.ClasseInfo + " Retorna" + pesquisaPor + "(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                return Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                throw;");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                            arquivo.WriteLine("        public string Retorna" + pesquisaPor + "Json(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            " + tabela.ClasseInfo + " info = null;");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info = Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                throw;");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                        }
                                        else
                                        {
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                            arquivo.WriteLine("        public void Retorna" + pesquisaPor + "Json(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            ResultWeb<" + tabela.ClasseInfo + "> info = new ResultWeb<" + tabela.ClasseInfo + ">();");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info.ResultInfo = Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch (Exception ex)");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                        }
                                    }
                                    else
                                    {
                                        if (rdbSOAP.Checked)
                                        {
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> Retorna" + pesquisaPor + "(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                return Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                throw;");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                            arquivo.WriteLine("        public string Retorna" + pesquisaPor + "Json(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> info = null;");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info = Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                throw;");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                        }
                                        else
                                        {
                                            arquivo.WriteLine("        [WebMethod]");
                                            arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                            arquivo.WriteLine("        public void Retorna" + pesquisaPor + "Json(" + variavel + " " + c.Descricao + ")");
                                            arquivo.WriteLine("        {");
                                            arquivo.WriteLine("            ResultWeb<List<" + tabela.ClasseInfo + ">> info = new ResultWeb<List<" + tabela.ClasseInfo + ">>();");
                                            arquivo.WriteLine("            try");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info.ResultInfo = Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + ");");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            catch (Exception ex)");
                                            arquivo.WriteLine("            {");
                                            arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                            arquivo.WriteLine("            }");
                                            arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                            arquivo.WriteLine("        }");
                                            arquivo.WriteLine("");
                                        }
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

                                if (rdbSOAP.Checked)
                                {
                                    arquivo.WriteLine("        [WebMethod]");
                                    arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaPorParametros(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                return " + tabela.ApelidoDao + ".RetornaPorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + ");");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            catch");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                throw;");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("        [WebMethod]");
                                    arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                    arquivo.WriteLine("        public string RetornaPorParametrosJson(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> info = null;");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                info = Accessor." + tabela.ClasseBo + ".RetornaPorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + ");");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            catch");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                throw;");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                                else
                                {
                                    arquivo.WriteLine("        [WebMethod]");
                                    arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                    arquivo.WriteLine("        public void RetornaPorParametrosJson(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ")");
                                    arquivo.WriteLine("        {");
                                    arquivo.WriteLine("            ResultWeb<List<" + tabela.ClasseInfo + ">> info = new ResultWeb<List<" + tabela.ClasseInfo + ">>();");
                                    arquivo.WriteLine("            try");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                info.ResultInfo = Accessor." + tabela.ClasseBo + ".RetornaPorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + ");");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            catch (Exception ex)");
                                    arquivo.WriteLine("            {");
                                    arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("");
                                }
                            }

                            if (rdbSOAP.Checked)
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaTodos()");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                return Accessor." + tabela.ClasseBo + ".RetornaTodos();");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public string RetornaTodosJson()");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> info = null;");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info = Accessor." + tabela.ClasseBo + ".RetornaTodos();");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            return JsonConvert.SerializeObject(info);");
                                arquivo.WriteLine("        }");
                            }
                            else
                            {
                                arquivo.WriteLine("        [WebMethod]");
                                arquivo.WriteLine("        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]");
                                arquivo.WriteLine("        public void RetornaTodosJson()");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            ResultWeb<List<" + tabela.ClasseInfo + ">> info = new ResultWeb<List<" + tabela.ClasseInfo + ">>();");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.ResultInfo = Accessor." + tabela.ClasseBo + ".RetornaTodos();");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch (Exception ex)");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                info.setErrorMessage(ex.Message);");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            AjustaPaginaApenasJson(info.ToJson());");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("}");

                            arquivo.Flush();
                            arquivo.Close();
                        }
                    }
                    #endregion

                    #region CriaArquivo WebService
                    if (chkWebService.Checked)
                    {
                        File.Create(diretorio + "\\WebService\\" + tabela.ArquivoWebService.Replace(".cs", "")).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\WebService\\" + tabela.ArquivoWebService.Replace(".cs", "")))
                        {
                            arquivo.WriteLine("<%@ WebService Language=\"C#\" CodeBehind=\"" + tabela.ArquivoWebService + "\" Class=\"" + pacoteWebService + "." + tabela.ClasseWebService + "\" %>");
                            arquivo.Flush();
                            arquivo.Close();
                        }

                        File.Create(diretorio + "\\WebService\\" + tabela.ArquivoWebService).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\WebService\\" + tabela.ArquivoWebService))
                        {
                            arquivo.WriteLine("using Newtonsoft.Json;");
                            arquivo.WriteLine("using System;");
                            arquivo.WriteLine("using System.Collections.Generic;");
                            arquivo.WriteLine("using System.Configuration;");
                            arquivo.WriteLine("using System.Data;");
                            arquivo.WriteLine("using System.IO;");
                            arquivo.WriteLine("using System.Text;");
                            arquivo.WriteLine("using System.Web.Configuration;");
                            arquivo.WriteLine("using System.Web.Script.Services;");
                            arquivo.WriteLine("using System.Web.Services;");
                            arquivo.WriteLine("using " + pacoteWebService + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.BLL;");
                            arquivo.WriteLine("using System.Web;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                            arquivo.WriteLine("using " + pacoteWebService + ".Library.Model;");
                            arquivo.WriteLine("using MySql.Data.MySqlClient;");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("namespace " + pacoteWebService);
                            arquivo.WriteLine("{");
                            arquivo.WriteLine("    public partial class " + tabela.ClasseWebService + " : System.Web.Services.WebService");
                            arquivo.WriteLine("    {");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("}");

                            arquivo.Flush();
                            arquivo.Close();
                        }
                    }
                    #endregion

                    #region CriaArquivo COMUNICADOR Base BLL
                    if (chkWebService.Checked && !string.IsNullOrEmpty(txtPacoteWebService.Text))
                    {
                        bool newInstance = !chkComunicadorSemNewInstance.Checked;

                        File.Create(diretorio + "\\Comunicador\\BaseBLL\\" + tabela.ArquivoBo).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\Comunicador\\BaseBLL\\" + tabela.ArquivoBo))
                        {
                            if (newInstance)
                                arquivo.WriteLine("using " + pacote + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".BaseObjects;");
                            arquivo.WriteLine("using " + pacoteORM + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                            arquivo.WriteLine("using System;");
                            arquivo.WriteLine("using MySql.Data.MySqlClient;");
                            arquivo.WriteLine("using System.Collections.Generic;");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("namespace " + pacote + ".Library.BLL");
                            arquivo.WriteLine("{");
                            arquivo.WriteLine("    public partial class " + tabela.ClasseBo);
                            arquivo.WriteLine("    {");
                            arquivo.WriteLine("        class Accessor");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            static Funcoes funcoes;");
                            arquivo.WriteLine("            internal static Funcoes Funcoes");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (funcoes == null)");
                            arquivo.WriteLine("                            funcoes = " + (newInstance ? " Funcoes.newInstance();" : "new Funcoes();"));
                            arquivo.WriteLine("                        return funcoes;");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            static " + pacoteORM + ".Util.Funcoes funcoesDB;");
                            arquivo.WriteLine("            internal static " + pacoteORM + ".Util.Funcoes FuncoesDB");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (funcoesDB == null)");
                            arquivo.WriteLine("                            funcoesDB = ORM.Util.Funcoes.newInstance();");
                            arquivo.WriteLine("                        return funcoesDB;");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            static " + pacoteORM + ".Library.BLL." + tabela.ClasseBo + " " + tabela.ApelidoBo + ";");
                            arquivo.WriteLine("            internal static " + pacoteORM + ".Library.BLL." + tabela.ClasseBo + " " + tabela.ClasseBo);
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (" + tabela.ApelidoBo + " == null)");
                            arquivo.WriteLine("                            " + tabela.ApelidoBo + " = " + pacoteORM + ".Library.BLL." + tabela.ClasseBo + ".newInstance();");
                            arquivo.WriteLine("                        return " + tabela.ApelidoBo + ";");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            static " + tabela.Classe + "Web." + tabela.Classe + "SoapClient _" + tabela.Classe + "Web;");
                            arquivo.WriteLine("            internal static " + tabela.Classe + "Web." + tabela.Classe + "SoapClient " + tabela.Classe + "Web");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                get");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    try");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        if (_" + tabela.Classe + "Web == null)");
                            arquivo.WriteLine("                            _" + tabela.Classe + "Web = new " + tabela.Classe + "Web." + tabela.Classe + "SoapClient();");
                            arquivo.WriteLine("                        return _" + tabela.Classe + "Web;");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                    catch (Exception err)");
                            arquivo.WriteLine("                    {");
                            arquivo.WriteLine("                        throw new Exception(err.Message);");
                            arquivo.WriteLine("                    }");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        public bool Salvar(" + tabela.ClasseInfo + " _obj, MySqlTransaction _trans)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                    return Accessor." + tabela.Classe + "Web.Salvar(InfoLocalToInfoWeb(_obj));");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".Salvar(_obj, _trans);");
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
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                    return Accessor." + tabela.Classe + "Web.Salvar(InfoLocalToInfoWeb(_obj));");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    MySqlTransaction trans = Accessor.FuncoesDB.BeginTransaction();");
                            arquivo.WriteLine("                    bool sucesso = Salvar(_obj, trans);");
                            arquivo.WriteLine("                    if (sucesso)");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.CommitTransaction(trans);");
                            arquivo.WriteLine("                    else");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.RollbackTransaction(trans);");
                            arquivo.WriteLine("                    return sucesso;");
                            arquivo.WriteLine("                }");
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
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                    return Accessor." + tabela.Classe + "Web.ExcluirPorId(Id);");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".Excluir(Id, _trans);");
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
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                    return Accessor." + tabela.Classe + "Web.ExcluirPorId(Id);");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    MySqlTransaction trans = Accessor.FuncoesDB.BeginTransaction();");
                            arquivo.WriteLine("                    bool sucesso = Excluir(Id, trans);");
                            arquivo.WriteLine("                    if (sucesso)");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.CommitTransaction(trans);");
                            arquivo.WriteLine("                    else");
                            arquivo.WriteLine("                        Accessor.FuncoesDB.RollbackTransaction(trans);");
                            arquivo.WriteLine("                    return sucesso;");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        public " + tabela.ClasseInfo + " RetornaPorId(long Id, bool lazyLoading = false)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                    return InfoLocalFromInfoWeb(Accessor." + tabela.Classe + "Web.RetornaPorId(Id));");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".RetornaPorId(Id, lazyLoading);");
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

                                if (CriarSelect(c, ref lista, ref join))
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
                                        arquivo.WriteLine("        public " + tabela.ClasseInfo + " Retorna" + pesquisaPor + "(" + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                        arquivo.WriteLine("        {");
                                        arquivo.WriteLine("            try");
                                        arquivo.WriteLine("            {");
                                        arquivo.WriteLine("                if (Configuracoes.WebService)");
                                        arquivo.WriteLine("                    return InfoLocalFromInfoWeb(Accessor." + tabela.Classe + "Web.Retorna" + pesquisaPor + "(" + c.Descricao + "));");
                                        arquivo.WriteLine("                else");
                                        arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + variavelLazyLoading + ");");
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
                                        arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> Retorna" + pesquisaPor + "(" + variavel + " " + c.Descricao + parametroLazyLoading + ")");
                                        arquivo.WriteLine("        {");
                                        arquivo.WriteLine("            try");
                                        arquivo.WriteLine("            {");
                                        arquivo.WriteLine("                if (Configuracoes.WebService)");
                                        arquivo.WriteLine("                {");
                                        arquivo.WriteLine("                    List<" + tabela.Classe + "Web." + tabela.ClasseInfo + "> lst = new List<" + tabela.Classe + "Web." + tabela.ClasseInfo + ">(Accessor." + tabela.Classe + "Web.Retorna" + pesquisaPor + "(" + c.Descricao + "));");
                                        arquivo.WriteLine("");
                                        arquivo.WriteLine("                    return ListLocalFromListWeb(lst);");
                                        arquivo.WriteLine("                }");
                                        arquivo.WriteLine("                else");
                                        arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".Retorna" + pesquisaPor + "(" + c.Descricao + variavelLazyLoading + ");");
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

                                arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaPorParametros(" + parametros.ToString().Remove(parametros.Length - 2, 2) + parametroLazyLoading + ")");
                                arquivo.WriteLine("        {");
                                arquivo.WriteLine("            try");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                if (Configuracoes.WebService)");
                                arquivo.WriteLine("                {");
                                arquivo.WriteLine("                    List<" + tabela.Classe + "Web." + tabela.ClasseInfo + "> lst = new List<" + tabela.Classe + "Web." + tabela.ClasseInfo + ">(Accessor." + tabela.Classe + "Web.RetornaPorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + ");");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("                    return ListLocalFromListWeb(lst);");
                                arquivo.WriteLine("                }");
                                arquivo.WriteLine("                else");
                                arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".RetornaPorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + variavelLazyLoading + ");");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("            catch");
                                arquivo.WriteLine("            {");
                                arquivo.WriteLine("                throw;");
                                arquivo.WriteLine("            }");
                                arquivo.WriteLine("        }");
                                arquivo.WriteLine("");
                            }

                            arquivo.WriteLine("        public List<" + tabela.ClasseInfo + "> RetornaTodos(" + (parametroLazyLoading.Length > 0 ? parametroLazyLoading.Substring(2) : "") + ")");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            try");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                if (Configuracoes.WebService)");
                            arquivo.WriteLine("                {");
                            arquivo.WriteLine("                    List<" + tabela.Classe + "Web." + tabela.ClasseInfo + "> lst = new List<" + tabela.Classe + "Web." + tabela.ClasseInfo + ">(Accessor." + tabela.Classe + "Web.RetornaTodos());");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("                    return ListLocalFromListWeb(lst);");
                            arquivo.WriteLine("                }");
                            arquivo.WriteLine("                else");
                            arquivo.WriteLine("                    return Accessor." + tabela.ClasseBo + ".RetornaTodos(" + (variavelLazyLoading.Length > 0 ? variavelLazyLoading.Substring(2) : "") + ");");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("            catch");
                            arquivo.WriteLine("            {");
                            arquivo.WriteLine("                throw;");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        protected List<" + tabela.ClasseInfo + "> ListLocalFromListWeb(List<" + tabela.Classe + "Web." + tabela.ClasseInfo + "> lst)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            List<" + tabela.ClasseInfo + "> lstFinal = new List<" + tabela.ClasseInfo + ">();");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            foreach (" + tabela.Classe + "Web." + tabela.ClasseInfo + " p in lst)");
                            arquivo.WriteLine("                lstFinal.Add(InfoLocalFromInfoWeb(p));");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("            return lstFinal;");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        protected " + tabela.Classe + "Web." + tabela.ClasseInfo + " InfoLocalToInfoWeb(" + tabela.ClasseInfo + " info)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            " + tabela.Classe + "Web." + tabela.ClasseInfo + " " + tabela.ApelidoInfo + " = new " + tabela.Classe + "Web." + tabela.ClasseInfo + "();");
                            arquivo.WriteLine("");
                            // Cria new info do data reader
                            foreach (ColunaInfo c in tabela.colunas)
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = info." + c.Descricao + ";");

                            arquivo.WriteLine("");
                            arquivo.WriteLine("            return " + tabela.ApelidoInfo + ";");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        protected " + tabela.ClasseInfo + " InfoLocalFromInfoWeb(" + tabela.Classe + "Web." + tabela.ClasseInfo + " info)");
                            arquivo.WriteLine("        {");
                            arquivo.WriteLine("            " + tabela.ClasseInfo + " " + tabela.ApelidoInfo + " = new " + tabela.ClasseInfo + "();");
                            arquivo.WriteLine("");
                            // Cria new info do data reader
                            foreach (ColunaInfo c in tabela.colunas)
                                arquivo.WriteLine("            " + tabela.ApelidoInfo + "." + c.Descricao + " = info." + c.Descricao + ";");

                            arquivo.WriteLine("");
                            arquivo.WriteLine("            return " + tabela.ApelidoInfo + ";");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("}");

                            arquivo.Flush();
                            arquivo.Close();
                        }
                    }
                    #endregion

                    #region CriaArquivo COMUNICADOR BLL
                    if (chkWebService.Checked && !string.IsNullOrEmpty(txtPacoteWebService.Text))
                    {
                        bool newInstance = !chkComunicadorSemNewInstance.Checked;

                        File.Create(diretorio + "\\Comunicador\\BLL\\" + tabela.ArquivoBo).Close();
                        using (TextWriter arquivo = File.AppendText(diretorio + "\\Comunicador\\BLL\\" + tabela.ArquivoBo))
                        {
                            if (newInstance)
                                arquivo.WriteLine("using " + pacote + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".BaseObjects;");
                            arquivo.WriteLine("using " + pacoteORM + ".Util;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.Model;");
                            arquivo.WriteLine("using " + pacoteORM + ".Library.DAL;");
                            arquivo.WriteLine("using System;");
                            arquivo.WriteLine("using MySql.Data.MySqlClient;");
                            arquivo.WriteLine("using System.Collections.Generic;");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("namespace " + pacote + ".Library.BLL");
                            arquivo.WriteLine("{");
                            arquivo.WriteLine("    public partial class " + tabela.ClasseBo);
                            arquivo.WriteLine("    {");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("}");

                            arquivo.Flush();
                            arquivo.Close();
                        }
                    }
                    #endregion

                    #region CriaArquivo Funcoes
                    File.Create(diretorio + "\\Util\\Funcoes.cs").Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\Util\\Funcoes.cs"))
                    {
                        arquivo.Write(Library.ArquivoFuncoes.RetornaTextoArquivoFuncoes(txtPacote.Text));

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion
                }

                if (tabelas.Count > 0)
                {
                    #region CriaArquivo DAOFactory
                    File.Create(diretorio + "\\ORM\\DAL\\DAOFactory.cs").Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\ORM\\DAL\\DAOFactory.cs"))
                    {
                        arquivo.WriteLine("namespace " + pacoteORM + ".Library.DAL");
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
                            arquivo.WriteLine("        public static " + t.ClasseDao + " get" + t.ClasseDao + "()");
                            arquivo.WriteLine("        {");
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

        bool CriarSelect(ColunaInfo c, ref bool lista, ref bool join)
        {
            string coluna = c.DescricaoDB.ToUpper();
            bool ok = true;

            if (coluna.Contains("CODIGO") || coluna.Contains("NUMERO") || coluna.Equals("EAN") || coluna.Equals("CPF") || coluna.Equals("CNPJ"))
            { }
            else if (coluna.Contains("DESCRICAO") || coluna.Contains("NOME") || coluna.Contains("RAZAO") || coluna.Contains("FANTASIA")
                || coluna.Equals("LOJA") || coluna.Equals("IDLOJA") || coluna.Equals("ID_LOJA"))
            {
                lista = true;
            }
            else if (!c.ChavePrimaria && (coluna.StartsWith("ID") || coluna.EndsWith("ID")))
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

                ControlaBotoes(true, true, true);
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

        private void chkWebService_CheckedChanged(object sender, EventArgs e)
        {
            rdbREST.Visible = rdbSOAP.Visible = chkComunicadorSemNewInstance.Visible = lblPacoteWebService.Visible = txtPacoteWebService.Visible = chkWebService.Checked;
        }
    }


}
