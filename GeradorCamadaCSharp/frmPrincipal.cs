using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

                    Apelido = Classe.Substring(0, 1).ToLower() + (Classe.Length > 1 ? Classe.Substring(1) : "");

                    if (string.IsNullOrEmpty(ApelidoPlural))
                    {
                        if (Apelido.EndsWith("pao"))
                            ApelidoPlural = Apelido.Remove(Apelido.Length - 2, 2) + "aes";
                        else if (Apelido.EndsWith("ao"))
                            ApelidoPlural = Apelido.Remove(Apelido.Length - 2, 2) + "oes";
                        else if (Apelido.EndsWith("r"))
                            ApelidoPlural = Apelido + "es";
                        else
                            ApelidoPlural = Apelido + "s";
                    }

                    ClassePlural = ApelidoPlural.Substring(0, 1).ToUpper() + (ApelidoPlural.Length > 1 ? ApelidoPlural.Substring(1) : "");

                    ArquivoModel = Classe + ".java";
                    ArquivoDAO = Classe + "DAO.java";
                    ArquivoHandler = Classe + "Handler.java";

                    ProviderCODE = "CODE_" + descricao.ToUpper();
                    ProviderID = "CODE_" + descricao.ToUpper() + "_ID";
                    ProviderRAW = "CODE_" + descricao.ToUpper() + "_RAW";

                    CONTENT_URI = "CONTENT_" + descricao.ToUpper() + "_URI";
                    CONTENT_URI_RAW = "CONTENT_" + descricao.ToUpper() + "_RAW_URI";
                }
            }
            public string Classe { get; private set; }
            public string ClassePlural { get; private set; }
            public string Apelido { get; private set; }
            public string ApelidoPlural { get; private set; }
            public string ArquivoModel { get; private set; }
            public string ArquivoDAO { get; private set; }
            public string ArquivoHandler { get; private set; }
            public string ProviderCODE { get; private set; }
            public string ProviderID { get; private set; }
            public string ProviderRAW { get; private set; }
            public string CONTENT_URI { get; private set; }
            public string CONTENT_URI_RAW { get; private set; }


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
                            descricao = "id";

                            DescricaoGet = "getId";
                            DescricaosSet = "setId";
                        }
                        else
                        {
                            DescricaoGet = "get" + descricao.Substring(0, 1).ToUpper() + (descricao.Length > 1 ? descricao.Substring(1) : "");
                            DescricaosSet = "set" + descricao.Substring(0, 1).ToUpper() + (descricao.Length > 1 ? descricao.Substring(1) : "");

                            if (descricao.EndsWith("_id"))
                            {
                                var t = new TabelaInfo();
                                t.Descricao = descricao.Remove(descricao.Length - 3, 3);

                                ClasseRelacional = t.Classe;
                                ClasseRelacionalApelido = t.Apelido;
                            }
                        }

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
                        TipoVariavel = TipoVariavelEnum.Boolean;
                    }
                    else if (tipo.Contains("INT"))
                    {
                        if (Descricao.ToUpper().Contains("ID") || Descricao.ToUpper().Contains("CODIGO") || Descricao.ToUpper().Contains("CNPJ") || Descricao.ToUpper().Contains("CPF"))
                            TipoVariavel = TipoVariavelEnum.Long;
                        else
                            TipoVariavel = TipoVariavelEnum.Integer;
                    }
                    else if (tipo.Contains("DATE"))
                    {
                        TipoVariavel = TipoVariavelEnum.Date;
                    }
                    else if (tipo.Contains("DECIMAL") || tipo.Contains("DOUBLE"))
                    {
                        TipoVariavel = TipoVariavelEnum.Double;
                    }
                    else
                    {
                        throw new Exception("Tipo não implementado: " + value);
                    }
                }
            }
            public string Default { get; set; }
            public bool ChavePrimaria { get; set; }
            public bool AceitaNulo { get; set; }
            public bool AutoIncremento { get; set; }
            public bool Index { get; set; }

            public string DescricaoGet { get; private set; }
            public string DescricaosSet { get; private set; }
            public string DescricaoDB { get; private set; }
            public string DescricaoReferencia { get; private set; }

            public string ClasseRelacional { get; private set; }
            public string ClasseRelacionalApelido { get; private set; }

            public TipoVariavelEnum TipoVariavel
            {
                get; private set;
            }
        }

        enum TipoVariavelEnum { String, Integer, Long, Date, Double, Boolean }

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

                if (!Directory.Exists(diretorio + "\\model"))
                    Directory.CreateDirectory(diretorio + "\\model");

                if (!Directory.Exists(diretorio + "\\basemodel"))
                    Directory.CreateDirectory(diretorio + "\\basemodel");

                if (!Directory.Exists(diretorio + "\\provider"))
                    Directory.CreateDirectory(diretorio + "\\provider");

                if (!Directory.Exists(diretorio + "\\basedal"))
                    Directory.CreateDirectory(diretorio + "\\basedal");

                if (!Directory.Exists(diretorio + "\\dal"))
                    Directory.CreateDirectory(diretorio + "\\dal");

                string comando = @"
                    SELECT *, 0 as ordenar
                       FROM INFORMATION_SCHEMA.`TABLES`
                      WHERE TABLE_SCHEMA = ?TABLE_NAME
                        AND TABLE_TYPE   = 'BASE TABLE';";
                MySqlParameter[] parms = new MySqlParameter[1];
                parms[0] = Accessor.Funcoes.CreateParameter("?TABLE_NAME", MySqlDbType.VarChar, cmbDatabase.Text);

                DataTable dtTablesOrigin = Accessor.Funcoes.FillDataTable(CommandType.Text, comando, parms);

                if (File.Exists(diretorio + "\\indice.txt"))
                    File.Delete(diretorio + "\\indice.txt");

                using (File.Create(diretorio + "\\indice.txt")) { }

                int quantidadeInicio = 4;
                int quantidadeTabelas = 0;
                int quantidadeInserts = 3;
                int quantidadeAtualiza = 20;
                int quantidadeConfigModuloProcedures = 3;
                int totalBarraProgress = 0;

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
                            coluna.ChavePrimaria = rdr["column_key"].ToString().ToLower().Contains("pri");
                            coluna.AceitaNulo = rdr["is_nullable"].ToString().ToLower().Contains("yes");
                            coluna.AutoIncremento = rdr["extra"].ToString().ToLower().Contains("auto_increment");
                            coluna.Index = rdr["column_key"].ToString().ToLower().Contains("mul");

                            tabela.colunas.Add(coluna);

                            if (rdr["column_key"].ToString().ToLower().Contains("pri"))
                                primaryKeys.Add(rdr["column_name"].ToString());
                        }
                    }

                    using (TextWriter arquivoIndice = File.AppendText(diretorio + "\\indice.txt"))
                    {
                        arquivoIndice.WriteLine("");
                        quantidadeTabelas++;
                        arquivoIndice.Flush();
                        arquivoIndice.Close();
                    }

                    #region CriaArquivo Base Model
                    File.Create(diretorio + "\\basemodel\\Base" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\basemodel\\Base" + tabela.ArquivoModel))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);

                        arquivo.WriteLine("package " + txtPacote.Text + ".model;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.ContentValues;");
                        arquivo.WriteLine("import android.content.Context;");
                        arquivo.WriteLine("import android.database.Cursor;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import java.io.Serializable;");
                        arquivo.WriteLine("import java.util.Date;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import " + txtPacote.Text + ".dal.DAOFactory;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public abstract class Base" + tabela.Classe + " implements Serializable {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public static final String TABLE_NAME = \"" + tabela.Descricao + "\";");
                        arquivo.WriteLine("");

                        // Cria variaveis privadas
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("    private " + c.TipoVariavel.ToString() + " " + c.Descricao + ";");
                        }

                        // Cria variaveis de classes relacionais
                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("");
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacional))
                                    arquivo.WriteLine("    private " + c.ClasseRelacional + " " + c.ClasseRelacionalApelido + ";");
                            }
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public Base" + tabela.Classe + "(Base" + tabela.Classe + " t) {");
                        arquivo.WriteLine("        super();");

                        // Cria variaveis no construtor da classe
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        this." + c.Descricao + " = t." + c.Descricao + ";");
                        }

                        // Cria variaveis de classes relacionais no construtor da classe
                        if (existeLazyLoading)
                        {
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacional))
                                    arquivo.WriteLine("        this." + c.ClasseRelacionalApelido + " = t." + c.ClasseRelacionalApelido + ";");
                            }
                        }

                        arquivo.WriteLine("   }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public Base" + tabela.Classe + "() {");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");

                        // Cria get e set
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("    public " + c.TipoVariavel.ToString() + " " + c.DescricaoGet + "() {");
                            arquivo.WriteLine("        return " + c.Descricao + ";");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    public void " + c.DescricaosSet + "(" + c.TipoVariavel.ToString() + " " + c.Descricao + ") {");
                            arquivo.WriteLine("        this." + c.Descricao + " = " + c.Descricao + ";");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                        }

                        // Cria get do lazy loading
                        if (existeLazyLoading)
                        {
                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacional))
                                {
                                    arquivo.WriteLine("    public void set" + c.ClasseRelacional + "(" + c.ClasseRelacional + " " + c.ClasseRelacionalApelido + ") {");
                                    arquivo.WriteLine("        this." + c.ClasseRelacionalApelido + " = " + c.ClasseRelacionalApelido + ";");
                                    arquivo.WriteLine("    }");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("    public " + c.ClasseRelacional + " get" + c.ClasseRelacional + "() {");
                                    arquivo.WriteLine("        return " + c.ClasseRelacionalApelido + ";");
                                    arquivo.WriteLine("    }");
                                    arquivo.WriteLine("");
                                }
                            }
                        }

                        arquivo.WriteLine("    public ContentValues values() {");
                        arquivo.WriteLine("        ContentValues values = new ContentValues();");

                        // Cria content values
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            if (!c.TipoVariavel.Equals(TipoVariavelEnum.Date))
                            {
                                if (!c.Descricao.Equals("id"))
                                {
                                    arquivo.WriteLine("        if (" + c.Descricao + " != null) {");
                                    arquivo.WriteLine("            values.put(Columns." + c.DescricaoReferencia + ", " + c.Descricao + ");");
                                    arquivo.WriteLine("        }");
                                }
                            }
                            else
                            {
                                switch (c.Descricao)
                                {
                                    case "datahora_criacao":
                                        arquivo.WriteLine("        if (datahora_criacao == null) {");
                                        arquivo.WriteLine("            datahora_criacao = new Date(System.currentTimeMillis());");
                                        arquivo.WriteLine("        }");
                                        arquivo.WriteLine("");
                                        arquivo.WriteLine("        values.put(Columns.DATAHORA_CRIACAO, datahora_criacao.getTime());");
                                        break;

                                    case "datahora_alteracao":
                                        arquivo.WriteLine("        if (id != null) {");
                                        arquivo.WriteLine("            datahora_alteracao = new Date(System.currentTimeMillis());");
                                        arquivo.WriteLine("        }");
                                        arquivo.WriteLine("");
                                        arquivo.WriteLine("        if (datahora_alteracao != null) {");
                                        arquivo.WriteLine("            values.put(Columns.DATAHORA_ALTERACAO, datahora_alteracao.getTime());");
                                        arquivo.WriteLine("        }");
                                        break;

                                    default:
                                        arquivo.WriteLine("        if (" + c.Descricao + " != null) {");
                                        arquivo.WriteLine("            values.put(Columns." + c.DescricaoReferencia + ", " + c.Descricao + ".getTime());");
                                        arquivo.WriteLine("        }");
                                        break;
                                }
                            }

                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("        return values;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    protected void loadFromCursor(Cursor c, boolean lazyLoading, Context context) {");
                        arquivo.WriteLine("        clear();");
                        arquivo.WriteLine("");

                        // Cria load do cursor
                        bool primeiroLoop = true;
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        " + (primeiroLoop ? "int " : "") + "index = c.getColumnIndex(Columns." + c.DescricaoReferencia + ");");
                            arquivo.WriteLine("        if (index > -1 && !c.isNull(index)) {");

                            if (c.TipoVariavel.Equals(TipoVariavelEnum.Integer))
                                arquivo.WriteLine("            " + c.DescricaosSet + "(c.getInt(index));");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Date))
                                arquivo.WriteLine("            " + c.DescricaosSet + "(new Date((c.getLong(index))));");
                            else if (c.TipoVariavel.Equals(TipoVariavelEnum.Boolean))
                                arquivo.WriteLine("            " + c.DescricaosSet + "(c.getString(index).contains(\"1\"));");
                            else
                                arquivo.WriteLine("            " + c.DescricaosSet + "(c.get" + c.TipoVariavel.ToString() + "(index));");

                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("");

                            primeiroLoop = false;
                        }

                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("        if (lazyLoading)");
                            arquivo.WriteLine("            lazyLoading(context);");
                        }

                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    protected void clear() {");

                        // Cria metodo clear
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        " + c.DescricaosSet + "(null);");
                        }

                        // Cria clear dos lazy loading
                        if (existeLazyLoading)
                        {
                            foreach (ColunaInfo c in tabela.colunas)
                                if (!string.IsNullOrEmpty(c.ClasseRelacional))
                                    arquivo.WriteLine("        " + c.ClasseRelacionalApelido + " = null;");
                        }

                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");

                        // Cria método lazy loading
                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("    protected void lazyLoading(Context context) {");

                            foreach (ColunaInfo c in tabela.colunas)
                            {
                                if (!string.IsNullOrEmpty(c.ClasseRelacional))
                                {
                                    arquivo.WriteLine("        if (" + c.Descricao + " != null)");
                                    arquivo.WriteLine("            " + c.ClasseRelacionalApelido + " = DAOFactory.get" + c.ClasseRelacional + "DAO(context).get" + c.ClasseRelacional + "ById(" + c.Descricao + ");");
                                    arquivo.WriteLine("");
                                }
                            }

                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("    public static final class Columns {");

                        // Cria referencia Columns
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        public static final String " + c.DescricaoReferencia + " = \"" + c.DescricaoDB + "\";");
                        }

                        arquivo.WriteLine("    }");

                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public static final class FullColumns {");

                        // Cria referencia Columns
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        public static final String " + c.DescricaoReferencia + " = TABLE_NAME + \".\" + Columns." + c.DescricaoReferencia + ";");
                        }

                        arquivo.WriteLine("    }");

                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public static final class Aliases {");

                        // Cria referencia Columns
                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("        public static final String " + c.DescricaoReferencia + " = TABLE_NAME + \"_\" + Columns." + c.DescricaoReferencia + ";");
                        }

                        arquivo.WriteLine("    }");

                        arquivo.WriteLine("");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Model
                    File.Create(diretorio + "\\model\\" + tabela.ArquivoModel).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\model\\" + tabela.ArquivoModel))
                    {
                        bool existeLazyLoading = (tabela.colunas.Find(p => p.Descricao != "id" && p.Descricao.EndsWith("_id")) != null);

                        arquivo.WriteLine("package " + txtPacote.Text + ".model;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.Context;");
                        arquivo.WriteLine("import android.database.Cursor;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import " + txtPacote.Text + ".basemodel.Base" + tabela.Classe + ";");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public class " + tabela.Classe + " extends Base" + tabela.Classe + " {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public void loadFromCursor(Cursor c) {");
                        arquivo.WriteLine("        loadFromCursor(c, false, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public void loadFromCursor(Cursor c, boolean lazyLoading, Context context) {");
                        arquivo.WriteLine("        super.loadFromCursor(c, lazyLoading, context);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public void clear() {");
                        arquivo.WriteLine("        super.clear();");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");

                        if (existeLazyLoading)
                        {
                            arquivo.WriteLine("    public void lazyLoading(Context context) {");
                            arquivo.WriteLine("        super.lazyLoading(context);");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Handler
                    File.Create(diretorio + "\\provider\\" + tabela.ArquivoHandler).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\provider\\" + tabela.ArquivoHandler))
                    {
                        arquivo.WriteLine("package " + txtPacote.Text + ".provider;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.ContentUris;");
                        arquivo.WriteLine("import android.content.ContentValues;");
                        arquivo.WriteLine("import android.content.Context;");
                        arquivo.WriteLine("import android.database.Cursor;");
                        arquivo.WriteLine("import android.net.Uri;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import " + txtPacote.Text + ".model." + tabela.Classe + ";");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public class " + tabela.Classe + "Handler extends DataHandler {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    protected " + tabela.Classe + "Handler(Context context) {");
                        arquivo.WriteLine("        super(context);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public Cursor query(int code, Uri uri, String[] projection, String selection, String[] selectionArgs, String sortOrder) {");
                        arquivo.WriteLine("        String[] columns = {");

                        foreach (ColunaInfo c in tabela.colunas)
                        {
                            arquivo.WriteLine("                " + tabela.Classe + ".FullColumns." + c.DescricaoReferencia + ",");
                        }

                        arquivo.WriteLine("        };");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        if (code == DataProvider." + tabela.ProviderCODE + ") {");
                        arquivo.WriteLine("            return db().query(" + tabela.Classe + ".TABLE_NAME, projection != null ? projection : columns, selection, selectionArgs, null, null, (sortOrder != null ? sortOrder : " + DefineOrdenacao(tabela) + "));");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        } else if (code == DataProvider." + tabela.ProviderRAW + ") {");
                        arquivo.WriteLine("            return db().rawQuery(selection, selectionArgs);");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        } else {");
                        arquivo.WriteLine("            long id = ContentUris.parseId(uri);");
                        arquivo.WriteLine("            return db().query(" + tabela.Classe + ".TABLE_NAME, projection != null ? projection : columns, " + tabela.Classe + ".Columns._ID + \" = ? \", new String[]{String.valueOf(id)}, null, null, " + tabela.Classe + ".Columns._ID);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public Cursor rawQuery(String selection, String[] selectionArgs) {");
                        arquivo.WriteLine("        return db().rawQuery(selection, selectionArgs);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public Uri insert(int code, Uri uri, ContentValues values) {");
                        arquivo.WriteLine("        if (code == DataProvider." + tabela.ProviderID + " || code == DataProvider." + tabela.ProviderRAW + ") {");
                        arquivo.WriteLine("            throw new InvalidURIException(uri);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        long id = db().insert(" + tabela.Classe + ".TABLE_NAME, null, values);");
                        arquivo.WriteLine("        return ContentUris.withAppendedId(uri, id);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public int update(int code, Uri uri, ContentValues values, String selection, String[] selectionArgs) {");
                        arquivo.WriteLine("        if (code == DataProvider." + tabela.ProviderCODE + " || code == DataProvider." + tabela.ProviderRAW + ") {");
                        arquivo.WriteLine("            throw new InvalidURIException(uri);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        long id = ContentUris.parseId(uri);");
                        arquivo.WriteLine("        return db().update(" + tabela.Classe + ".TABLE_NAME, values, " + tabela.Classe + ".Columns._ID + \" = ? \", new String[]{String.valueOf(id)});");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public int delete(int code, Uri uri, String selection, String[] selectionArgs) {");
                        arquivo.WriteLine("        if (code == DataProvider." + tabela.ProviderCODE + " || code == DataProvider." + tabela.ProviderRAW + ") {");
                        arquivo.WriteLine("            throw new InvalidURIException(uri);");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        long id = ContentUris.parseId(uri);");
                        arquivo.WriteLine("        return db().delete(" + tabela.Classe + ".TABLE_NAME, " + tabela.Classe + ".Columns._ID + \" = ? \", new String[]{String.valueOf(id)});");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Base DAO
                    File.Create(diretorio + "\\basedal\\Base" + tabela.ArquivoDAO).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\basedal\\Base" + tabela.ArquivoDAO))
                    {
                        arquivo.WriteLine("package " + txtPacote.Text + ".basedal;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.ContentUris;");
                        arquivo.WriteLine("import android.content.CursorLoader;");
                        arquivo.WriteLine("import android.database.Cursor;");
                        arquivo.WriteLine("import android.net.Uri;");
                        arquivo.WriteLine("import android.os.Bundle;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import java.util.ArrayList;");
                        arquivo.WriteLine("import java.util.List;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import " + txtPacote.Text + ".dal.DAO;");
                        arquivo.WriteLine("import " + txtPacote.Text + ".model." + tabela.Classe + ";");
                        arquivo.WriteLine("import " + txtPacote.Text + ".provider.DataProvider;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public abstract class Base" + tabela.Classe + "DAO extends DAO {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public Base" + tabela.Classe + "DAO() {");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public void insert(" + tabela.Classe + " " + tabela.Apelido + ") {");
                        arquivo.WriteLine("        Uri newUri = contentResolver().insert(DataProvider." + tabela.CONTENT_URI + ", " + tabela.Apelido + ".values()); ");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        long id = ContentUris.parseId(newUri);");
                        arquivo.WriteLine("        " + tabela.Apelido + ".setId(id);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public int update(" + tabela.Classe + " " + tabela.Apelido + ") {");
                        arquivo.WriteLine("        Uri uri = ContentUris.withAppendedId(DataProvider." + tabela.CONTENT_URI + ", " + tabela.Apelido + ".getId());");
                        arquivo.WriteLine("        return contentResolver().update(uri, " + tabela.Apelido + ".values(), null, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public int delete(" + tabela.Classe + " " + tabela.Apelido + ") {");
                        arquivo.WriteLine("        Uri uri = ContentUris.withAppendedId(DataProvider." + tabela.CONTENT_URI + ", " + tabela.Apelido + ".getId());");
                        arquivo.WriteLine("        return contentResolver().delete(uri, null, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public " + tabela.Classe + " get" + tabela.Classe + "ById(long id, boolean... lazyLoading) {");
                        arquivo.WriteLine("        " + tabela.Classe + " " + tabela.Apelido + " = new " + tabela.Classe + "();");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        Cursor c = get" + tabela.Classe + "ByIdLoader(id).loadInBackground();");
                        arquivo.WriteLine("        try {");
                        arquivo.WriteLine("            if (c.moveToFirst())");
                        arquivo.WriteLine("                " + tabela.Apelido + ".loadFromCursor(c, lazyLoading.length > 0 && lazyLoading[0], context());");
                        arquivo.WriteLine("        } finally {");
                        arquivo.WriteLine("            c.close();");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("        return " + tabela.Apelido + ";");
                        arquivo.WriteLine("    }");
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

                                    parametroLazyLoading = ", boolean... lazyLoading";
                                    variavelLazyLoading = ", lazyLoading.length > 0 && lazyLoading[0], context()";
                                }

                                string variavel = c.TipoVariavel.ToString().StartsWith("S") ? c.TipoVariavel.ToString() : c.TipoVariavel.ToString().ToLower();

                                if (variavel.Equals("integer"))
                                    variavel = "int";

                                string pesquisaPor = "Por" + c.DescricaoGet.Substring(3);

                                if (!lista)
                                {
                                    arquivo.WriteLine("    public " + tabela.Classe + " get" + tabela.Classe + pesquisaPor + "(" + variavel + " " + c.Descricao + parametroLazyLoading + ") {");
                                    arquivo.WriteLine("        " + tabela.Classe + " " + tabela.Apelido + " = new " + tabela.Classe + "();");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("        Cursor c = get" + tabela.Classe + pesquisaPor + "Cursor(" + c.Descricao + ");");
                                    arquivo.WriteLine("        try {");
                                    arquivo.WriteLine("            if (c.moveToFirst())");
                                    arquivo.WriteLine("                " + tabela.Apelido + ".loadFromCursor(c" + variavelLazyLoading + ");");
                                    arquivo.WriteLine("        } finally {");
                                    arquivo.WriteLine("            c.close();");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("        return " + tabela.Apelido + ";");
                                    arquivo.WriteLine("    }");
                                    arquivo.WriteLine("");
                                }
                                else
                                {
                                    arquivo.WriteLine("    public List<" + tabela.Classe + "> get" + tabela.ClassePlural + pesquisaPor + "List(" + variavel + " " + c.Descricao + parametroLazyLoading + ") {");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("        List<" + tabela.Classe + "> " + tabela.ApelidoPlural + " = new ArrayList<>();");
                                    arquivo.WriteLine("");
                                    arquivo.WriteLine("        Cursor c = get" + tabela.ClassePlural + pesquisaPor + "Cursor(" + c.Descricao + ");");
                                    arquivo.WriteLine("        try {");
                                    arquivo.WriteLine("            if (c.moveToFirst()) {");
                                    arquivo.WriteLine("                do {");
                                    arquivo.WriteLine("                    " + tabela.Classe + " t = new " + tabela.Classe + "();");
                                    arquivo.WriteLine("                    t.loadFromCursor(c" + variavelLazyLoading + ");");
                                    arquivo.WriteLine("                    " + tabela.ApelidoPlural + ".add(t);");
                                    arquivo.WriteLine("                } while (c.moveToNext());");
                                    arquivo.WriteLine("            }");
                                    arquivo.WriteLine("        } finally {");
                                    arquivo.WriteLine("            c.close();");
                                    arquivo.WriteLine("        }");
                                    arquivo.WriteLine("        return " + tabela.ApelidoPlural + ";");
                                    arquivo.WriteLine("    }");
                                    arquivo.WriteLine("");
                                }

                                arquivo.WriteLine("    public Cursor get" + (lista ? tabela.ClassePlural : tabela.Classe) + pesquisaPor + "Cursor(" + variavel + " " + c.Descricao + ") {");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        String selection = " + tabela.Classe + ".FullColumns." + c.DescricaoReferencia + " + \" = ? \";");
                                arquivo.WriteLine("        String[] selectionArgs = {String.valueOf(" + c.Descricao + ")};");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        return contentResolver().query(DataProvider." + tabela.CONTENT_URI + ", null, selection, selectionArgs, null);");
                                arquivo.WriteLine("    }");
                                arquivo.WriteLine("");

                                arquivo.WriteLine("    public CursorLoader get" + (lista ? tabela.ClassePlural : tabela.Classe) + pesquisaPor + "Loader(" + variavel + " " + c.Descricao + ") {");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        String selection = " + tabela.Classe + ".FullColumns." + c.DescricaoReferencia + " + \" = ? \";");
                                arquivo.WriteLine("        String[] selectionArgs = {String.valueOf(" + c.Descricao + ")};");
                                arquivo.WriteLine("");
                                arquivo.WriteLine("        return new CursorLoader(context(), DataProvider." + tabela.CONTENT_URI + ", null, selection, selectionArgs, null);");
                                arquivo.WriteLine("    }");
                                arquivo.WriteLine("");
                            }
                        }

                        if (joins.Count > 1)
                        {
                            StringBuilder parametros = new StringBuilder();
                            StringBuilder parametrosPassar = new StringBuilder();
                            StringBuilder selection = new StringBuilder();
                            StringBuilder selectionArgs = new StringBuilder();

                            foreach (ColunaInfo c in joins)
                            {
                                string variavel = c.TipoVariavel.ToString().StartsWith("S") ? c.TipoVariavel.ToString() : c.TipoVariavel.ToString().ToLower();

                                parametros.Append(variavel + " " + c.Descricao + ", ");

                                parametrosPassar.Append(c.Descricao + ", ");

                                selection.Append(tabela.Classe + ".FullColumns." + c.DescricaoReferencia + " + \" = ? AND \" + ");

                                selectionArgs.Append("String.valueOf(" + c.Descricao + "), ");
                            }

                            arquivo.WriteLine("    public List<" + tabela.Classe + "> get" + tabela.ClassePlural + "PorParametrosList(" + parametros.ToString().Remove(parametros.Length - 2, 2) + parametroLazyLoading + ") {");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        List<" + tabela.Classe + "> " + tabela.ApelidoPlural + " = new ArrayList<>();");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        Cursor c = get" + tabela.ClassePlural + "PorParametros(" + parametrosPassar.ToString().Remove(parametrosPassar.Length - 2, 2) + ");");
                            arquivo.WriteLine("        try {");
                            arquivo.WriteLine("            if (c.moveToFirst()) {");
                            arquivo.WriteLine("                do {");
                            arquivo.WriteLine("                    " + tabela.Classe + " t = new " + tabela.Classe + "();");
                            arquivo.WriteLine("                    t.loadFromCursor(c" + variavelLazyLoading + ");");
                            arquivo.WriteLine("                    " + tabela.ApelidoPlural + ".add(t);");
                            arquivo.WriteLine("                } while (c.moveToNext());");
                            arquivo.WriteLine("            }");
                            arquivo.WriteLine("        } finally {");
                            arquivo.WriteLine("            c.close();");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("        return " + tabela.ApelidoPlural + ";");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    public Cursor get" + tabela.ClassePlural + "PorParametros(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ") {");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        String selection = " + selection.ToString().Remove(selection.Length - 8, 8) + "\";");
                            arquivo.WriteLine("        String[] selectionArgs = {" + selectionArgs.ToString().Remove(selectionArgs.Length - 2, 2) + "};");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        return contentResolver().query(DataProvider." + tabela.CONTENT_URI + ", null, selection, selectionArgs, null);");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("    public CursorLoader get" + tabela.ClassePlural + "PorParametrosLoader(" + parametros.ToString().Remove(parametros.Length - 2, 2) + ") {");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        String selection = " + selection.ToString().Remove(selection.Length - 8, 8) + "\";");
                            arquivo.WriteLine("        String[] selectionArgs = {" + selectionArgs.ToString().Remove(selectionArgs.Length - 2, 2) + "};");
                            arquivo.WriteLine("");
                            arquivo.WriteLine("        return new CursorLoader(context(), DataProvider." + tabela.CONTENT_URI + ", null, selection, selectionArgs, null);");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("    public List<" + tabela.Classe + "> get" + tabela.ClassePlural + "List(" + (parametroLazyLoading.Length > 0 ? parametroLazyLoading.Substring(2) : "") + ") {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        List<" + tabela.Classe + "> " + tabela.ApelidoPlural + " = new ArrayList<>();");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        Cursor c = get" + tabela.ClassePlural + "();");
                        arquivo.WriteLine("        try {");
                        arquivo.WriteLine("            if (c.moveToFirst()) {");
                        arquivo.WriteLine("                do {");
                        arquivo.WriteLine("                    " + tabela.Classe + " t = new " + tabela.Classe + "();");
                        arquivo.WriteLine("                    t.loadFromCursor(c" + variavelLazyLoading + ");");
                        arquivo.WriteLine("                    " + tabela.ApelidoPlural + ".add(t);");
                        arquivo.WriteLine("                } while (c.moveToNext());");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        } finally {");
                        arquivo.WriteLine("            c.close();");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("        return " + tabela.ApelidoPlural + ";");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public Cursor get" + tabela.ClassePlural + "() {");
                        arquivo.WriteLine("        return contentResolver().query(DataProvider." + tabela.CONTENT_URI + ", null, null, null, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public CursorLoader get" + tabela.ClassePlural + "Loader() {");
                        arquivo.WriteLine("        return new CursorLoader(context(), DataProvider." + tabela.CONTENT_URI + ", null, null, null, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public CursorLoader get" + tabela.ClassePlural + "Loader(Bundle args) {");
                        arquivo.WriteLine("        String selection = null;");
                        arquivo.WriteLine("        String[] selectionArgs = null;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        String orderBy = null;");
                        arquivo.WriteLine("        if (args != null) {");
                        arquivo.WriteLine("            orderBy = args.getString(\"orderBy\");");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("            if (args.getString(\"where\") != null) {");
                        arquivo.WriteLine("                selection = args.getString(\"where\");");
                        arquivo.WriteLine("                selectionArgs = args.getStringArray(\"whereArray\");");
                        arquivo.WriteLine("            }");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("        return new CursorLoader(context(), DataProvider." + tabela.CONTENT_URI + ", null, selection, selectionArgs, orderBy);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public CursorLoader get" + tabela.Classe + "ByIdLoader(long id) {");
                        arquivo.WriteLine("        Uri uri = ContentUris.withAppendedId(DataProvider." + tabela.CONTENT_URI + ", id);");
                        arquivo.WriteLine("        return new CursorLoader(context(), uri, null, null, null, null);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    public void notify" + tabela.Classe + "() {");
                        arquivo.WriteLine("        contentResolver().notifyChange(DataProvider." + tabela.CONTENT_URI + ", null, true);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo DAO
                    File.Create(diretorio + "\\dal\\" + tabela.ArquivoDAO).Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\dal\\" + tabela.ArquivoDAO))
                    {
                        arquivo.WriteLine("package " + txtPacote.Text + ".dal;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import " + txtPacote.Text + ".basedal.Base" + tabela.Classe + "DAO;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public class " + tabela.Classe + "DAO extends Base" + tabela.Classe + "DAO {");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion
                }

                if (tabelas.Count > 0)
                {
                    #region CriaArquivo DAO
                    File.Create(diretorio + "\\dal\\DAOFactory.java").Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\dal\\DAOFactory.java"))
                    {
                        arquivo.WriteLine("package " + txtPacote.Text + ".dal;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.Context;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("public class DAOFactory {");
                        arquivo.WriteLine("");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("    private static " + t.Classe + "DAO " + t.Apelido + "DAO;");
                        }

                        arquivo.WriteLine("");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("    public static " + t.Classe + "DAO get" + t.Classe + "DAO(Context context) {");
                            arquivo.WriteLine("        if (" + t.Apelido + "DAO == null) {");
                            arquivo.WriteLine("            " + t.Apelido + "DAO = new " + t.Classe + "DAO();");
                            arquivo.WriteLine("            " + t.Apelido + "DAO.init(context.getApplicationContext());");
                            arquivo.WriteLine("        }");
                            arquivo.WriteLine("        return " + t.Apelido + "DAO;");
                            arquivo.WriteLine("    }");
                            arquivo.WriteLine("");
                        }

                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion

                    #region CriaArquivo Provider
                    File.Create(diretorio + "\\provider\\DataProvider.java").Close();
                    using (TextWriter arquivo = File.AppendText(diretorio + "\\provider\\DataProvider.java"))
                    {
                        arquivo.WriteLine("package " + txtPacote.Text + ".provider;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("import android.content.ContentProvider;");
                        arquivo.WriteLine("import android.content.ContentValues;");
                        arquivo.WriteLine("import android.content.UriMatcher;");
                        arquivo.WriteLine("import android.database.Cursor;");
                        arquivo.WriteLine("import android.net.Uri;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("// Content provider para acessar o banco de dados SQLite");
                        arquivo.WriteLine("public class DataProvider extends ContentProvider {");
                        arquivo.WriteLine("");

                        int posicao = 0;
                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("    static final int " + t.ProviderCODE + " = " + (++posicao).ToString() + ";");
                            arquivo.WriteLine("    static final int " + t.ProviderID + " = " + (++posicao).ToString() + ";");
                            arquivo.WriteLine("    static final int " + t.ProviderRAW + " = " + (++posicao).ToString() + ";");
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("    private static final String AUTHORITY = \"" + txtPacote.Text + ".provider.DataProvider\";");
                        arquivo.WriteLine("");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("    public static final Uri " + t.CONTENT_URI + " = Uri.parse(\"content://\" + AUTHORITY + \"/" + t.Descricao + "\");");
                            arquivo.WriteLine("    public static final Uri " + t.CONTENT_URI_RAW + " = Uri.parse(\"content://\" + AUTHORITY + \"/" + t.Descricao + "_raw\");");
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("private UriMatcher matcher;");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    // Os handlers são usados para tratar requisições, de acordo com o padrão da URI utilizada");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("    private DataHandler " + t.Apelido + "Handler;");
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public boolean onCreate() {");
                        arquivo.WriteLine("        // Cria os handlers");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("        " + t.Apelido + "Handler = new " + t.Classe + "Handler(getContext());");
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("        // Configura o URI matcher");
                        arquivo.WriteLine("        matcher = new UriMatcher(UriMatcher.NO_MATCH);");
                        arquivo.WriteLine("");

                        foreach (TabelaInfo t in tabelas)
                        {
                            arquivo.WriteLine("        matcher.addURI(AUTHORITY, \"" + t.Descricao + "\", " + t.ProviderCODE + ");");
                            arquivo.WriteLine("        matcher.addURI(AUTHORITY, \"" + t.Descricao + "/#\", " + t.ProviderID + ");");
                            arquivo.WriteLine("        matcher.addURI(AUTHORITY, \"" + t.Descricao + "_raw\", " + t.ProviderRAW + ");");
                        }

                        arquivo.WriteLine("");
                        arquivo.WriteLine("        return true;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public Cursor query(Uri uri, String[] projection, String selection, String[] selectionArgs, String sortOrder) {");
                        arquivo.WriteLine("        int code = matcher.match(uri);");
                        arquivo.WriteLine("        DataHandler handler = getHandler(code);");
                        arquivo.WriteLine("        Cursor c = handler.query(code, uri, projection, selection, selectionArgs, sortOrder);");
                        arquivo.WriteLine("        handler.setNotificationUri(c, uri);");
                        arquivo.WriteLine("        return c;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public Uri insert(Uri uri, ContentValues values) {");
                        arquivo.WriteLine("        int code = matcher.match(uri);");
                        arquivo.WriteLine("        DataHandler handler = getHandler(code);");
                        arquivo.WriteLine("        Uri newUri = handler.insert(code, uri, values);");
                        arquivo.WriteLine("        handler.notifyChange(newUri);");
                        arquivo.WriteLine("        return newUri;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public int update(Uri uri, ContentValues values, String selection, String[] selectionArgs) {");
                        arquivo.WriteLine("        int code = matcher.match(uri);");
                        arquivo.WriteLine("        DataHandler handler = getHandler(code);");
                        arquivo.WriteLine("        int count = handler.update(code, uri, values, selection, selectionArgs);");
                        arquivo.WriteLine("        handler.notifyChange(uri);");
                        arquivo.WriteLine("        return count;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public int delete(Uri uri, String selection, String[] selectionArgs) {");
                        arquivo.WriteLine("        int code = matcher.match(uri);");
                        arquivo.WriteLine("        DataHandler handler = getHandler(code);");
                        arquivo.WriteLine("        int count = handler.delete(code, uri, selection, selectionArgs);");
                        arquivo.WriteLine("        handler.notifyChange(uri);");
                        arquivo.WriteLine("        return count;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    @Override");
                        arquivo.WriteLine("    public String getType(Uri uri) {");
                        arquivo.WriteLine("        int code = matcher.match(uri);");
                        arquivo.WriteLine("");

                        int posicaoTabela = 0;
                        bool primeiraVezIf = true;
                        foreach (TabelaInfo t in tabelas)
                        {
                            posicaoTabela++;

                            arquivo.WriteLine("        " + (primeiraVezIf ? "if (" : "        || ") + "code == " + t.ProviderID + (posicaoTabela >= tabelas.Count ? ") {" : ""));

                            primeiraVezIf = false;
                        }
                        arquivo.WriteLine("            return \"vnd.android.cursor.item/vnd.br.com.imperiumsolucoes\";");
                        arquivo.WriteLine("");

                        posicaoTabela = 0;
                        primeiraVezIf = true;
                        foreach (TabelaInfo t in tabelas)
                        {
                            posicaoTabela++;

                            arquivo.WriteLine("        " + (primeiraVezIf ? "} else if (" : "        || ") + "code == " + t.ProviderCODE + " || code == " + t.ProviderRAW + (posicaoTabela >= tabelas.Count ? ") {" : ""));

                            primeiraVezIf = false;
                        }


                        arquivo.WriteLine("            return \"vnd.android.cursor.dir/vnd.br.com.imperiumsolucoes\";");
                        arquivo.WriteLine("        }");
                        arquivo.WriteLine("        throw new IllegalArgumentException(\"URI not supported: \" + uri);");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("");
                        arquivo.WriteLine("    private DataHandler getHandler(int code) {");

                        posicaoTabela = 0;
                        primeiraVezIf = true;
                        foreach (TabelaInfo t in tabelas)
                        {
                            posicaoTabela++;

                            arquivo.WriteLine("        " + (primeiraVezIf ? "if (" : "} else if (") + "code == " + t.ProviderCODE + " || code == " + t.ProviderID + " || code == " + t.ProviderRAW + ") {");
                            arquivo.WriteLine("            return " + t.Apelido + "Handler;");
                            arquivo.WriteLine("");

                            if (posicaoTabela >= tabelas.Count)
                                arquivo.WriteLine("        }");

                            primeiraVezIf = false;
                        }

                        arquivo.WriteLine("        return null;");
                        arquivo.WriteLine("    }");
                        arquivo.WriteLine("}");

                        arquivo.Flush();
                        arquivo.Close();
                    }
                    #endregion
                }


                using (TextWriter arquivoIndice = File.AppendText(diretorio + "\\indice.txt"))
                {
                    totalBarraProgress = quantidadeInicio + quantidadeTabelas + quantidadeInserts + quantidadeAtualiza + quantidadeConfigModuloProcedures + 1;

                    arquivoIndice.WriteLine("");
                    arquivoIndice.WriteLine("//Total Quantidade da Barra de Progress " + totalBarraProgress);
                    arquivoIndice.Flush();
                    arquivoIndice.Close();
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
                    return tabela.Classe + ".Columns." + c.DescricaoReferencia;
            }
            return tabela.Classe + ".Columns._ID";
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
