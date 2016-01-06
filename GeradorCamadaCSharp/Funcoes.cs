using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Xml;
using System.ComponentModel;
using System.Xml.Linq;
using System.Net.Sockets;

namespace GeradorCamadaCSharp
{
    public class CepInfo
    {
        public CepInfo(string _cep = "")
        {
            Cep = _cep;
            LogradouroTipo = "";
            Logradouro = "";
            Bairro = "";
            UF = "";
            Cidade = "";
        }

        public string Cep { get; set; }
        public string LogradouroTipo { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string UF { get; set; }
        public string Cidade { get; set; }
    }

    class Funcoes
    {
        [DllImport("user32.dll")]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private static string mFormulaMargemVenda = string.Empty;

        public string StringConexao
        {
            get { return @"server=" + Program.strIpBanco + ";user id=" + (string.IsNullOrEmpty(Program.strUsuarioBanco) ? "root" : Program.strUsuarioBanco) + ";Password=" + Program.strSenhaBanco + ";database=" + Program.strNomeBanco + "; pooling=false; Allow User Variables=True;"; }
            //get { return @"server=" + Program.strIpBanco + ";user id=lpm;Password=" + Program.strSenhaBanco + ";database=" + Program.strNomeBanco + "; pooling=false; Allow User Variables=True;"; }
        }

        private Byte[] m_ByteCodes;
        private string m_126;
        private string m_8;
        private string m_96;
        private string m_39;
        private string m_94;
        private Encoding m_Ascii;

        public MySqlConnection AbreConexao()
        {
            ////Supermercado Lança *** verificar
            //MySql.Data.MySqlClient.MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.strSysManager);
            //conn.Open();
            //return conn;

            //novo
            MySqlConnection conn = new MySqlConnection(StringConexao);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            return conn;
        }
        public MySqlConnection AbreConexaoManutencao()
        {
            MySqlConnection conn = new MySqlConnection(@"server = " + Program.strIpBanco + "; user id = " + (string.IsNullOrEmpty(Program.strUsuarioBanco) ? "root" : Program.strUsuarioBanco) + "; password = " + Program.strSenhaBanco + "; pooling = false; Allow User Variables=True;");
            //MySqlConnection conn = new MySqlConnection(@"server = " + Program.strIpBanco + "; user id = lpm; password = " + Program.strSenhaBanco + "; pooling = false; Allow User Variables=True;");
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch
            { }
            return conn;
        }
        public void FechaConexao(MySqlConnection _conn)
        {
            _conn.Close();
            return;
        }

        public void EnabledFields(Control _control, bool status = true)
        {
            for (int i = 0; i <= _control.Controls.Count - 1; i++)
            {
                if (_control.Controls[i] is TextBoxBase)
                    (_control.Controls[i] as TextBoxBase).Enabled = status;
                else if (_control.Controls[i] is ListControl)
                    (_control.Controls[i] as ListControl).Enabled = status;
                else if (_control.Controls[i] is DataGridView)
                    (_control.Controls[i] as DataGridView).Enabled = status;
                else if (_control.Controls[i] is Label)
                    (_control.Controls[i] as Label).Enabled = status;
                else if (_control.Controls[i] is CheckBox)
                    (_control.Controls[i] as CheckBox).Enabled = status;
                else if (_control.Controls[i] is DateTimePicker)
                    (_control.Controls[i] as DateTimePicker).Enabled = status;
                else if (_control.Controls[i] is ButtonBase)
                    (_control.Controls[i] as ButtonBase).Enabled = status;
                else
                    EnabledFields(_control.Controls[i], status);
            }
        }

        public string AjustaValor(string _valor)
        {
            try
            {
                if (string.IsNullOrEmpty(_valor))
                    return "0.00";

                decimal x = Convert.ToDecimal(_valor);
                return _valor.Replace(".", "#").Replace(",", ".").Replace("#", "");
            }
            catch
            {
                return "0.00";
            }
        }

        public bool TemPermissao(int _idUsuario, string _reduzido)
        {
            string msg = "Acesso Negado!!! ";
            bool flag = false;
            try
            {
                string comando = @"
                    select count(*) 
                        from permissao p
                           inner join modulo m on m.idmodulo = p.idmodulo and m.reduzido = ?reduz
                       where p.idusuario = ?idusuario ";

                MySqlParameter[] parms = new MySqlParameter[2];
                parms[0] = CreateParameter("?idusuario", MySqlDbType.Int32, _idUsuario);
                parms[1] = CreateParameter("?reduz", MySqlDbType.VarChar, _reduzido);

                int existe = Convert.ToInt32(ExecuteScalar(CommandType.Text, comando, parms));
                if (existe > 0 || _idUsuario == 1)
                    flag = true;
            }
            catch (Exception err)
            {
                msg += "\n" + err.Message;
            }

            if (!flag)
                Aviso(msg);

            return flag;
        }

        public DialogResult Aviso(string _msg)
        {
            return MessageBox.Show(_msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public DialogResult MensagemDeErro(string _msg)
        {
            return MessageBox.Show(_msg, "E r r o", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public DialogResult MensagemDeConfirmacao(string _msg, string _caption = "Atenção")
        {
            return MessageBox.Show(_msg, _caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public DialogResult MensagemDeConfirmacao(string _msg, MessageBoxDefaultButton _defaultButton, string _caption = "Atenção")
        {
            return MessageBox.Show(_msg, _caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, _defaultButton);
        }
        public DialogResult MensagemDeConfirmacaoComCancel(string _msg, string _caption = "Atenção")
        {
            return MessageBox.Show(_msg, _caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public DialogResult MensagemDeConfirmacaoComCancel(string _msg, MessageBoxDefaultButton _defaultButton, string _caption = "Atenção")
        {
            return MessageBox.Show(_msg, _caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, _defaultButton);
        }

        public double Truncar(double _valor, int precisao)
        {
            return Math.Floor(_valor * Math.Pow(10, precisao)) / Math.Pow(10, precisao);
        }
        public decimal Truncar(decimal _valor, int precisao)
        {
            return Math.Floor(_valor * (decimal)Math.Pow(10, precisao)) / (decimal)Math.Pow(10, precisao);
        }

        public string ArredondaParaCima(string _valor)
        {
            string[] words = _valor.Split('.');
            return words[0] + "." + Math.Ceiling(Convert.ToDouble(words[1].PadRight(4, '0').Insert(2, ","))).ToString().PadRight(2, '0');
        }
        public double ArredondaParaCima(double _valor, int precisao)
        {
            bool negative = false;
            if (_valor < 0D)
            {
                _valor = _valor * -1;
                negative = true;
            }

            double valorArredondado3 = Math.Round(_valor, precisao + 1);
            double valorArredondado2 = Math.Round(_valor, precisao);

            double valor = 1;
            for (int i = 0; i < precisao; i++)
                valor = Math.Round(valor * 10, precisao);

            if ((valorArredondado2 - valorArredondado3) <= 0D)
                valorArredondado2 += Math.Round(1 / valor, precisao);

            if (negative)
                valorArredondado2 = valorArredondado2 * -1;
            return valorArredondado2;
        }

        public string Zeros(string _valor, int _tamanho)
        {
            return _valor.PadLeft(_tamanho).Replace(" ", "0");
        }

        public string CortaPalavra(string _texto, int _tamanho)
        {
            return _texto.PadRight(_tamanho).Substring(0, _tamanho);
        }

        public void GravaControleDeAcesso(string Usuario, string Historico, string Situacao, int Loja)
        {
            try
            {
                string m_Comando = @"
                     insert into controle_acesso(usuario, dataalteracao, historico, situacao, loja) 
                                         values (?usuario, sysdate(), ?historico, ?situacao, ?loja) ";

                MySqlParameter[] parms = new MySqlParameter[4];
                parms[0] = CreateParameter("?usuario", MySqlDbType.VarChar, Usuario);
                parms[1] = CreateParameter("?historico", MySqlDbType.Text, Historico);
                parms[2] = CreateParameter("?situacao", MySqlDbType.VarChar, Situacao);
                parms[3] = CreateParameter("?loja", MySqlDbType.Int32, Loja);

                ExecuteNonQuery(CommandType.Text, m_Comando, parms);
            }
            catch (Exception err)
            {
                MensagemDeErro("Erro ao tentar gravar Controle_Acesso.\n" + err.Message);
            }
        }

        public string RecuperaParametro(string _parametro)
        {
            try
            {
                string comando = "select valor from config where parametro like ?parametro order by loja, parametro";

                MySqlParameter[] parms = new MySqlParameter[1];
                parms[0] = CreateParameter("?parametro", MySqlDbType.VarChar, "%" + _parametro + "%");

                using (MySqlDataReader rdr = ExecuteReader(CommandType.Text, comando, parms))
                {
                    if (rdr.Read())
                        return rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string RecuperaParametro(string _parametro, int _loja)
        {
            try
            {
                string comando = "select valor from config where loja = ?loja and parametro like ?parametro order by loja, parametro";

                MySqlParameter[] parms = new MySqlParameter[2];
                parms[0] = CreateParameter("?parametro", MySqlDbType.VarChar, "%" + _parametro + "%");
                parms[1] = CreateParameter("?loja", MySqlDbType.Int16, _loja);

                using (MySqlDataReader rdr = ExecuteReader(CommandType.Text, comando, parms))
                {
                    if (rdr.Read())
                        return rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string RecuperaParametro(string _parametro, int _loja, string _padrao)
        {
            try
            {
                string comando = "select valor from config where loja = ?loja and parametro = ?parametro order by parametro";

                MySqlParameter[] parms = new MySqlParameter[2];
                parms[0] = CreateParameter("?parametro", MySqlDbType.VarChar, _parametro);
                parms[1] = CreateParameter("?loja", MySqlDbType.Int16, _loja);

                using (MySqlDataReader rdr = ExecuteReader(CommandType.Text, comando, parms))
                {
                    if (rdr.Read())
                        return rdr.IsDBNull(0) ? _padrao : rdr.GetString(0);
                }
                return _padrao;
            }
            catch
            {
                return _padrao;
            }
        }

        public string PadCenter(string _texto, int _tamanho)
        {
            int tamanhoSemEspaco = 0;
            double resultado = 0;

            try
            {
                tamanhoSemEspaco = _texto.Trim().Length;
                resultado = (_tamanho - tamanhoSemEspaco) / 2;
                resultado = Math.Round(resultado, 0);
                _texto = _texto.PadLeft(Convert.ToInt32(resultado + tamanhoSemEspaco));
            }
            catch
            { }

            return _texto;
        }

        public string CompletaPalavra(string _p, string _p2, int _p3)
        {
            throw new NotImplementedException();
        }

        public string ExtensoValor(decimal _pdblValor)
        {
            if (_pdblValor <= 0m)
                return string.Empty;

            string strValorExtenso = ""; //Variável que irá armazenar o valor por extenso do número informado
            string strNumero = "";       //Irá armazenar o número para exibir por extenso 
            string strCentena = "";
            string strDezena = "";
            string strDezCentavo = "";

            decimal dblCentavos = 0;
            decimal dblValorInteiro = 0;
            int intContador = 0;
            bool bln_Bilhao = false;
            bool bln_Milhao = false;
            bool bln_Mil = false;
            bool bln_Unidade = false;

            //Verificar se foi informado um dado indevido 
            if (_pdblValor == 0 || _pdblValor <= 0)
                throw new Exception("Valor não suportado pela Função. Verificar se há valor negativo ou nada foi informado");
            if (_pdblValor > (decimal)9999999999.99)
                throw new Exception("Valor não suportado pela Função. Verificar se o Valor está acima de 9999999999.99");
            else //Entrada padrão do método
            {
                //Gerar Extenso Centavos 
                _pdblValor = (decimal.Round(_pdblValor, 2));
                dblCentavos = _pdblValor - (long)_pdblValor;

                //Gerar Extenso parte Inteira
                dblValorInteiro = (long)_pdblValor;
                if (dblValorInteiro > 0)
                {
                    if (dblValorInteiro > 999)
                        bln_Mil = true;
                    if (dblValorInteiro > 999999)
                    {
                        bln_Milhao = true;
                        bln_Mil = false;
                    }
                    if (dblValorInteiro > 999999999)
                    {
                        bln_Mil = false;
                        bln_Milhao = false;
                        bln_Bilhao = true;
                    }

                    for (int i = (dblValorInteiro.ToString().Trim().Length) - 1; i >= 0; i--)
                    {
                        strNumero = Mid(dblValorInteiro.ToString().Trim(), (dblValorInteiro.ToString().Trim().Length - i) - 1, 1);
                        switch (i)
                        {            /*******/
                            case 9:  /*Bilhão*
                                     /*******/
                                {
                                    strValorExtenso = NumeroUnidade(strNumero) + ((int.Parse(strNumero) > 1) ? " Bilhões e" : " Bilhão e");
                                    bln_Bilhao = true;
                                    break;
                                }
                            case 8: /********/
                            case 5: //Centena*
                            case 2: /********/
                                {
                                    if (int.Parse(strNumero) > 0)
                                    {
                                        strCentena = Mid(dblValorInteiro.ToString().Trim(), (dblValorInteiro.ToString().Trim().Length - i) - 1, 3);
                                        if (int.Parse(strCentena) > 100 && int.Parse(strCentena) < 200)
                                            strValorExtenso = strValorExtenso + " Cento e ";
                                        else
                                            strValorExtenso = strValorExtenso + " " + NumeroCentena(strNumero);
                                        if (intContador == 8)
                                            bln_Milhao = true;
                                        else if (intContador == 5)
                                            bln_Mil = true;
                                    }
                                    break;
                                }
                            case 7: /*****************/
                            case 4: //Dezena de Milhão*
                            case 1: /*****************/
                                {
                                    if (int.Parse(strNumero) > 0)
                                    {
                                        strDezena = Mid(dblValorInteiro.ToString().Trim(), (dblValorInteiro.ToString().Trim().Length - i) - 1, 2);//
                                        if (int.Parse(strDezena) > 10 && int.Parse(strDezena) < 20)
                                        {
                                            strValorExtenso = strValorExtenso + (Right(strValorExtenso, 5).Trim() == "entos" ? " e " : " ")
                                            + NumeroDezena0(Right(strDezena, 1));//corrigido

                                            bln_Unidade = true;
                                        }
                                        else
                                        {
                                            strValorExtenso = strValorExtenso + (Right(strValorExtenso, 5).Trim() == "entos" ? " e " : " ")
                                            + NumeroDezena1(Left(strDezena, 1));//corrigido 

                                            bln_Unidade = false;
                                        }
                                        if (intContador == 7)
                                            bln_Milhao = true;
                                        else if (intContador == 4)
                                            bln_Mil = true;
                                    }
                                    break;
                                }
                            case 6: /******************/
                            case 3: //Unidade de Milhão* 
                            case 0: /******************/
                                {
                                    if (int.Parse(strNumero) > 0 && !bln_Unidade)
                                    {
                                        if ((Right(strValorExtenso, 5).Trim()) == "entos" || (Right(strValorExtenso, 3).Trim()) == "nte" || (Right(strValorExtenso, 3).Trim()) == "nta")
                                            strValorExtenso = strValorExtenso + " e ";
                                        else
                                            strValorExtenso = strValorExtenso + " ";
                                        strValorExtenso = strValorExtenso + NumeroUnidade(strNumero);
                                    }
                                    if (i == 6)
                                    {
                                        if (bln_Milhao || int.Parse(strNumero) > 0)
                                        {
                                            strValorExtenso = strValorExtenso + ((int.Parse(strNumero) == 1) && !bln_Unidade ? " Milhão" : " Milhões");
                                            strValorExtenso = strValorExtenso + ((int.Parse(strNumero) > 1000000) ? " " : " e");
                                            bln_Milhao = true;
                                        }
                                    }
                                    if (i == 3)
                                    {
                                        if (bln_Mil || int.Parse(strNumero) > 0)
                                        {
                                            strValorExtenso = strValorExtenso + " Mil";
                                            strValorExtenso = strValorExtenso + ((int.Parse(strNumero) > 1000) ? " " : " e");
                                            bln_Mil = true;
                                        }
                                    }
                                    if (i == 0)
                                    {
                                        if ((bln_Bilhao && !bln_Milhao && !bln_Mil && Right((dblValorInteiro.ToString().Trim()), 3) == "0") || (!bln_Bilhao && bln_Milhao && !bln_Mil && Right((dblValorInteiro.ToString().Trim()), 3) == "0"))
                                            strValorExtenso = strValorExtenso + " e ";
                                        strValorExtenso = strValorExtenso + ((long.Parse(dblValorInteiro.ToString())) > 1 ? " Reais" : " Real");
                                    }
                                    bln_Unidade = false;
                                    break;
                                }
                        }
                    }//
                }
                if (dblCentavos > 0)
                {

                    if (dblCentavos > 0 && dblCentavos < 0.1M)
                    {
                        strNumero = Right((decimal.Round(dblCentavos, 2)).ToString().Trim(), 1);
                        strValorExtenso = strValorExtenso + ((dblCentavos > 0) ? " e " : " ")
                        + NumeroUnidade(strNumero) + ((dblCentavos > 0.01M) ? " Centavos" : " Centavo");
                    }
                    else if (dblCentavos > 0.1M && dblCentavos < 0.2M)
                    {
                        strNumero = Right(((decimal.Round(dblCentavos, 2) - (decimal)0.1).ToString().Trim()), 1);
                        strValorExtenso = strValorExtenso + ((dblCentavos > 0) ? " " : " e ")
                        + NumeroDezena0(strNumero) + " Centavos ";
                    }
                    else
                    {
                        strNumero = Right(dblCentavos.ToString().Trim(), 2);
                        strDezCentavo = Mid(dblCentavos.ToString().Trim(), 2, 1);

                        strValorExtenso = strValorExtenso + ((int.Parse(strNumero) > 0) ? " e " : " ");
                        strValorExtenso = strValorExtenso + NumeroDezena1(Left(strDezCentavo, 1));

                        if ((dblCentavos.ToString().Trim().Length) > 2)
                        {
                            strNumero = Right((decimal.Round(dblCentavos, 2)).ToString().Trim(), 1);
                            if (int.Parse(strNumero) > 0)
                            {
                                if (dblValorInteiro <= 0)
                                {
                                    if (Mid(strValorExtenso.Trim(), strValorExtenso.Trim().Length - 2, 1) == "e")
                                        strValorExtenso = strValorExtenso + " e " + NumeroUnidade(strNumero);
                                    else
                                        strValorExtenso = strValorExtenso + " e " + NumeroUnidade(strNumero);
                                }
                                else
                                    strValorExtenso = strValorExtenso + " e " + NumeroUnidade(strNumero);
                            }
                        }
                        strValorExtenso = strValorExtenso + " Centavos ";
                    }
                }
                if (dblValorInteiro < 1) strValorExtenso = Mid(strValorExtenso.Trim(), 2, strValorExtenso.Trim().Length - 2);
            }

            return strValorExtenso.Trim();
        }
        string NumeroDezena0(string _pstrDezena0)
        {
            //Vetor que irá conter o número por extenso 
            ArrayList arrayDezena0 = new ArrayList();

            arrayDezena0.Add("Onze");
            arrayDezena0.Add("Doze");
            arrayDezena0.Add("Treze");
            arrayDezena0.Add("Quatorze");
            arrayDezena0.Add("Quinze");
            arrayDezena0.Add("Dezesseis");
            arrayDezena0.Add("Dezessete");
            arrayDezena0.Add("Dezoito");
            arrayDezena0.Add("Dezenove");

            return arrayDezena0[((int.Parse(_pstrDezena0)) - 1)].ToString();
        }
        string NumeroDezena1(string _pstrDezena1)
        {
            //Vetor que irá conter o número por extenso
            ArrayList arrayDezena1 = new ArrayList();

            arrayDezena1.Add("Dez");
            arrayDezena1.Add("Vinte");
            arrayDezena1.Add("Trinta");
            arrayDezena1.Add("Quarenta");
            arrayDezena1.Add("Cinquenta");
            arrayDezena1.Add("Sessenta");
            arrayDezena1.Add("Setenta");
            arrayDezena1.Add("Oitenta");
            arrayDezena1.Add("Noventa");

            return arrayDezena1[Int16.Parse(_pstrDezena1) - 1].ToString();
        }
        string NumeroCentena(string _pstrCentena)
        {
            //Vetor que irá conter o número por extenso
            ArrayList arrayCentena = new ArrayList();

            arrayCentena.Add("Cem");
            arrayCentena.Add("Duzentos");
            arrayCentena.Add("Trezentos");
            arrayCentena.Add("Quatrocentos");
            arrayCentena.Add("Quinhentos");
            arrayCentena.Add("Seiscentos");
            arrayCentena.Add("Setecentos");
            arrayCentena.Add("Oitocentos");
            arrayCentena.Add("Novecentos");

            return arrayCentena[((int.Parse(_pstrCentena)) - 1)].ToString();
        }
        string NumeroUnidade(string _pstrUnidade)
        {
            //Vetor que irá conter o número por extenso
            ArrayList arrayUnidade = new ArrayList();

            arrayUnidade.Add("Um");
            arrayUnidade.Add("Dois");
            arrayUnidade.Add("Três");
            arrayUnidade.Add("Quatro");
            arrayUnidade.Add("Cinco");
            arrayUnidade.Add("Seis");
            arrayUnidade.Add("Sete");
            arrayUnidade.Add("Oito");
            arrayUnidade.Add("Nove");

            return arrayUnidade[(int.Parse(_pstrUnidade) - 1)].ToString();
        }

        //Começa aqui os Métodos de Compatibilazação com VB 6 .........Left() Right() Mid() 
        static string Left(string _param, int _length)
        {
            //we start at 0 since we want to get the characters starting from the 
            //left and with the specified lenght and assign it to a variable
            if (_param == "")
                return "";
            string result = _param.Substring(0, _length);
            //return the result of the operation 
            return result;
        }
        static string Right(string _param, int _length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable 
            if (_param == "")
                return "";
            string result = _param.Substring(_param.Length - _length, _length);
            //return the result of the operation
            return result;
        }
        static string Mid(string _param, int _startIndex, int _length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable 
            string result = _param.Substring(_startIndex, _length);
            //return the result of the operation
            return result;
        }
        static string Mid(string _param, int _startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = _param.Substring(_startIndex);
            //return the result of the operation 
            return result;
        }

        public decimal RetornaMargemVenda(decimal venda, decimal custo, int loja = 0)
        {
            decimal margem = 0;
            try
            {
                string formula = string.Empty;
                if (string.IsNullOrEmpty(mFormulaMargemVenda))
                {
                    if (loja == 0)
                        mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda").ToUpper();
                    else
                        mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda", loja).ToUpper();
                }

                if (string.IsNullOrEmpty(mFormulaMargemVenda.Trim()))
                    mFormulaMargemVenda = "(((VENDA / CUSTO) -1) * 100)";

                formula = mFormulaMargemVenda.Replace("CUSTO", AjustaValor(custo.ToString())).Replace("VENDA", AjustaValor(venda.ToString()));

                margem = Math.Round(this.ConvertToDecimal(new DataTable().Compute(formula, "")), 2);
            }
            catch { }
            return margem;
        }
        public string RetornaStringMargemItensVenda(bool somar, int loja = 0)
        {
            string formula = string.Empty;
            if (string.IsNullOrEmpty(mFormulaMargemVenda))
            {
                if (loja == 0)
                    mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda").ToUpper();
                else
                    mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda", loja).ToUpper();
            }

            if (string.IsNullOrEmpty(mFormulaMargemVenda.Trim()))
                mFormulaMargemVenda = "(((VENDA / CUSTO) -1) * 100)";

            formula = mFormulaMargemVenda.Replace("CUSTO", (somar ? "sum" : "") + "(i.quantidade * i.custoproduto)").Replace("VENDA", (somar ? "sum" : "") + "(i.valor)");

            return formula;
        }
        public string RetornaStringMargemVenda(string venda, string custo, bool somar, int loja = 0)
        {
            string formula = string.Empty;
            if (string.IsNullOrEmpty(mFormulaMargemVenda))
            {
                if (loja == 0)
                    mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda").ToUpper();
                else
                    mFormulaMargemVenda = RecuperaParametro("FormulaMargemVenda", loja).ToUpper();
            }

            if (string.IsNullOrEmpty(mFormulaMargemVenda.Trim()))
                mFormulaMargemVenda = "(((VENDA / CUSTO) -1) * 100)";

            formula = mFormulaMargemVenda.Replace("CUSTO", (somar ? "sum" : "") + "(" + custo + ")").Replace("VENDA", (somar ? "sum" : "") + "(" + venda + ")");

            return formula;
        }

        public bool EnviaEmail(string _texto, string _formulario)
        {
            SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
            string caminho = Application.StartupPath + "\\logerro.txt";
            cliente.EnableSsl = true;

            MailAddress remetente = new MailAddress("suporte.sysrearguard@gmail.com", "1");
            MailAddress destinatario = new MailAddress("paulo.lpm@uol.com.br", "Suporte Imperium");
            MailMessage mensagem = new MailMessage(remetente, destinatario);
            Attachment arquivo = new Attachment(caminho, MediaTypeNames.Application.Octet);
            mensagem.Attachments.Add(arquivo);
            mensagem.Body = "ERRO RETORNADO DO SISTEMA: \n" + _texto;
            mensagem.Subject = "Erro no Sistema - " + _formulario + " - " + "1";

            NetworkCredential credenciais = new NetworkCredential("suporte.sysrearguard", "chuva123", "");

            cliente.Credentials = credenciais;

            try
            {
                cliente.Send(mensagem);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool EnviaEmailNFe(string emailDest, string assunto, string msg, string pathFile)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("nfe\\eMail");

            try
            {
                using (SmtpClient client = new SmtpClient(key.GetValue("ServidorSMTP").ToString()))
                {
                    client.EnableSsl = (key.GetValue("SSL").ToString() == "1");
                    client.Credentials = new NetworkCredential(key.GetValue("eMail").ToString(), key.GetValue("Senha").ToString());
                    client.Port = Convert.ToInt32(key.GetValue("Porta").ToString());
                    MailMessage message = new MailMessage();
                    message.Sender = new MailAddress(key.GetValue("eMail").ToString(), key.GetValue("NomeExibicao").ToString());
                    message.From = new MailAddress(key.GetValue("eMail").ToString(), key.GetValue("NomeExibicao").ToString());

                    string[] emails = emailDest.Split(';');
                    for (int i = 0; i < emails.Length; i++)
                    {
                        if (emails[i] != null && !string.IsNullOrEmpty(emails[i]))
                            message.To.Add(new MailAddress(emails[i], emails[i]));
                    }

                    message.Headers.Add("Disposition-Notification-To", "<" + key.GetValue("eMail").ToString() + ">");

                    message.Subject = assunto;
                    message.Body = msg;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.Normal;

                    if (!string.IsNullOrEmpty(pathFile))
                    {
                        string[] anexos = pathFile.Split(';');

                        for (int i = 0; i < anexos.Length; i++)
                        {
                            if (anexos[i] != string.Empty)
                            {
                                if (!File.Exists(anexos[i]))
                                    MessageBox.Show("Anexo não encontrado!\r\n" + anexos[i]);
                                else
                                {
                                    Attachment at = new Attachment(anexos[i]);
                                    message.Attachments.Add(at);
                                }
                            }
                        }
                    }
                    client.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log("LOGERRO.TXT", ex.Message, "Erro ao tentar enviar email");
            }
            finally
            {
                key.Close();
            }
            return false;
        }
        public bool EnviaEmailPedidoCompra(string numeroPedido, string emailDest, string msg, bool enviaExcel, string pathFile = "")
        {
            string comando = @"select * from empresa_cadastroemail where enviaPedido = 'S'";
            using (MySqlDataReader rdr = ExecuteReader(CommandType.Text, comando, null))
            {
                if (rdr.Read())
                {
                    string servidor = rdr["servidor"].ToString();
                    string senha = rdr["senha"].ToString();
                    string usuario = rdr["usuario"].ToString();
                    int porta = Convert.ToInt32(rdr["porta"].ToString());
                    bool ativaSSL = (rdr["ativaSSL"].ToString().ToUpper() == "S");

                    using (SmtpClient cliente = new SmtpClient(servidor, porta))
                    {
                        NetworkCredential credenciais = new NetworkCredential(usuario, senha);
                        cliente.EnableSsl = ativaSSL;

                        pathFile = string.IsNullOrEmpty(pathFile) ? Application.StartupPath + "\\PedidoCompra\\" + numeroPedido + ".pdf" : pathFile;
                        if (!File.Exists(pathFile))
                        {
                            Aviso("Arquivo do pedido de compra não encontrado ou não gerado.\nFavor confirmar o pedido para ser gerado novamente o arquivo.");
                            return false;
                        }

                        MailAddress remetente = new MailAddress(usuario);
                        MailMessage mensagem = new MailMessage();
                        mensagem.Sender = new MailAddress(usuario, usuario);
                        mensagem.From = new MailAddress(usuario, usuario);

                        string[] emails = emailDest.Split(Convert.ToChar(";"));
                        for (int a = 0; a < emails.Length; a++)
                        {
                            if (emails[a] != null && !string.IsNullOrEmpty(emails[a]))
                                mensagem.To.Add(new MailAddress(emails[a], emails[a]));
                        }

                        //anexar aquivo
                        Attachment arquivo = new Attachment(pathFile, MediaTypeNames.Application.Pdf);
                        mensagem.Attachments.Add(arquivo);

                        if (enviaExcel)
                        {
                            bool encontrouExcel = false;
                            string caminhoExcel = Application.StartupPath + "\\PedidoCompra\\Excel\\" + numeroPedido + ".xls";
                            if (File.Exists(caminhoExcel))
                                encontrouExcel = true;
                            else
                            {
                                caminhoExcel = Application.StartupPath + "\\PedidoCompra\\Excel\\" + numeroPedido + ".xlsx";
                                if (File.Exists(caminhoExcel))
                                    encontrouExcel = true;
                            }

                            if (encontrouExcel)
                            {
                                Attachment arquivoExcel = new Attachment(caminhoExcel, MediaTypeNames.Application.Octet);
                                mensagem.Attachments.Add(arquivoExcel);
                            }
                        }

                        mensagem.Subject = "Envio Automatico de Pedido Compra (Imperium - E.R.P)";
                        mensagem.Body += msg + "\n\nSegue em Anexo Arquivo de Pedido...";

                        cliente.Credentials = credenciais;

                        try
                        {
                            cliente.Send(mensagem);

                            arquivo.Dispose();
                            mensagem.Dispose();
                            remetente = null;
                            credenciais = null;

                            return true;
                        }
                        catch (Exception ex)
                        {
                            MensagemDeErro("Falha ao enviar email as " + DateTime.Now + " horas" + "," + emailDest + "\n\n" + ex.Message);
                        }
                    }
                }
            }
            return false;
        }

        public void Log(string _arquivo, string _logMensagem, string _modulo)
        {
            using (StreamWriter w = File.AppendText(_arquivo))
            {
                w.WriteLine(" ");
                w.WriteLine(DateTime.Now.ToString("dd/MM/yy-HH:mm") + "-->" + _modulo);
                w.WriteLine("{0}", _logMensagem);
                w.Flush();
                w.Close();
            }
        }

        public bool IsInteger(string _text)
        {
            try
            {
                Convert.ToInt32(_text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string LimpaCaracteres(string _text)
        {
            string accents = @"ÁÀÃÂÄàáãâäÈÉÊËèéêëÌÍÎÏìíîïÒÓÕÔÖòóõôöÙÚÛÜùúûüÇç";
            string noAccents = @"AAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUUuuuuCc";
            for (int i = 0; i < accents.Length; i++)
                _text = _text.Replace(accents[i], noAccents[i]);
            return _text;
        }

        public string CleanText(string _text)
        {
            m_ByteCodes = new Byte[] { 126, 8, 96, 39, 94 };
            m_Ascii = Encoding.ASCII;
            m_126 = m_Ascii.GetString(m_ByteCodes, 0, 1);
            m_8 = m_Ascii.GetString(m_ByteCodes, 1, 1);
            m_96 = m_Ascii.GetString(m_ByteCodes, 2, 1);
            m_39 = m_Ascii.GetString(m_ByteCodes, 3, 1);
            m_94 = m_Ascii.GetString(m_ByteCodes, 4, 1);

            StringBuilder s = new StringBuilder(_text);
            s = s.Replace("à", m_96 + m_8 + "a");
            s = s.Replace("â", m_94 + m_8 + "a");
            s = s.Replace("ê", m_96 + m_8 + "e");
            s = s.Replace("ô", m_96 + m_8 + "o");
            s = s.Replace("û", m_96 + m_8 + "u");
            s = s.Replace("ã", m_126 + m_8 + "a");
            s = s.Replace("õ", m_126 + m_8 + "o");
            s = s.Replace("á", m_39 + m_8 + "a");
            s = s.Replace("é", m_39 + m_8 + "e");
            s = s.Replace("í", m_39 + m_8 + "i");
            s = s.Replace("ó", m_39 + m_8 + "o");
            s = s.Replace("ú", m_39 + m_8 + "u");
            s = s.Replace("ç", "," + m_8 + "c");
            s = s.Replace("ü", "u");
            s = s.Replace("À", m_96 + m_8 + "A");
            s = s.Replace("Â", m_94 + m_8 + "A");
            s = s.Replace("Ê", m_94 + m_8 + "E");
            s = s.Replace("Ô", m_94 + m_8 + "O");
            s = s.Replace("Û", m_94 + m_8 + "U");
            s = s.Replace("Ã", m_126 + m_8 + "A");
            s = s.Replace("Õ", m_126 + m_8 + "O");
            s = s.Replace("Á", m_39 + m_8 + "A");
            s = s.Replace("É", m_39 + m_8 + "E");
            s = s.Replace("Í", m_39 + m_8 + "I");
            s = s.Replace("Ó", m_39 + m_8 + "O");
            s = s.Replace("Ú", m_39 + m_8 + "U");
            s = s.Replace("Ç", "," + m_8 + "C");
            s = s.Replace("Ü", "U");
            s = s.Replace("&", " e ");
            s = s.Replace("º", ".o");
            s = s.Replace("ª", ".a");
            s = s.Replace("°", "");
            s = s.Replace("^", "");
            s = s.Replace("'", "");
            s = s.Replace("~", "");
            s = s.Replace("?", "");
            s = s.Replace("´", "");
            s = s.Replace("\b", "");
            s = s.Replace(",", "");
            s = s.Replace("<", "");
            s = s.Replace(">", "");
            s = s.Replace(@"\", "");
            s = s.Replace("/", "");
            s = s.Replace("¦", "");
            return s.ToString().Trim();
        }
        public string RemoveCaracteresEspeciais(string _texto)
        {
            if (_texto == null)
                return null;

            _texto = Regex.Replace(_texto, @"[^\w\.\/\@\- ]|[_]&*'", ""); //Remove os Caracteres Especiais
            _texto = _texto.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _texto.Length; i++) // Remove Acentos Cedilha e outros
            {
                Char c = _texto[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }

        public string Criptografa(string _texto)
        {
            int x;
            string xCripto;

            int[] aCripto, aAux;
            aCripto = new int[_texto.Length];
            aAux = new int[_texto.Length];

            xCripto = "";

            //Carrega Valores Codificados...
            for (x = 0; x <= _texto.Length - 1; x++)
            {
                aCripto[x] = (int)_texto[x];
            }

            //Invertendo Valores....
            for (x = 0; x <= _texto.Length - 1; x++)
            {
                aAux[x] = aCripto[(_texto.Length - 1) - x];
                //aAux[x] = aCripto[(pTxt.Length - 1) - x] + x;
            }

            for (x = 0; x <= _texto.Length - 1; x++)
            {
                xCripto += Convert.ToChar(aAux[x] + x + 1);
            }

            return xCripto;
        }
        public string Descriptografa(string _texto)
        {
            int x;
            string xCripto;

            int[] aCripto, aAux;
            aCripto = new int[_texto.Length];
            aAux = new int[_texto.Length];

            xCripto = "";

            for (x = 0; x <= _texto.Length - 1; x++)
            {
                aCripto[x] = (int)_texto[x];
            }

            ///////////////////////////////////////////////////////////////////////////////////

            //Reverter o que foi criptografado...

            for (x = 0; x <= _texto.Length - 1; x++)
            {
                aAux[x] = aCripto[_texto.Length - 1 - x];
            }

            ///////////////////////////////////////////////////////////////////////////////////

            for (x = 0; x <= _texto.Length - 1; x++)
            {
                xCripto += Convert.ToChar(aAux[x] - _texto.Length + x);
            }

            //////////////////////////////////////////////////////////////////////////////////
            return xCripto;
        }

        public string DiaCorte(string _dataVencimento, string _diaSemanaCorte)
        {
            string data = _dataVencimento;
            string dia = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(Convert.ToDateTime(data).DayOfWeek);
            for (int i = 0; i < 7; i++)
            {
                if (dia == _diaSemanaCorte)
                {
                    return data;
                }
                data = Convert.ToDateTime(data).AddDays(1).ToString();
                dia = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(Convert.ToDateTime(data).DayOfWeek);
            }
            return "01/01/2000";
        }

        public DateTime RecuperaPrimeiroDiaMes(DateTime _data)
        {
            return Convert.ToDateTime("01/" + _data.Month + "/" + _data.Year);
        }
        public DateTime RecuperaUltimoDiaMes(DateTime _data)
        {
            return Convert.ToDateTime("01/" + _data.AddMonths(1).Month + "/" + _data.AddMonths(1).Year).AddDays(-1);
        }
        public DateTime GetNetworkTime()
        {
            try
            {
                //Servidor nacional para melhor latência
                const string ntpServer = "a.ntp.br";

                // Tamanho da mensagem NTP - 16 bytes (RFC 2030)
                var ntpData = new byte[48];

                //Indicador de Leap (ver RFC), Versão e Modo
                ntpData[0] = 0x1B; //LI = 0 (sem warnings), VN = 3 (IPv4 apenas), Mode = 3 (modo cliente)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;

                //123 é a porta padrão do NTP
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                //NTP usa UDP
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                socket.Connect(ipEndPoint);

                //Caso NTP esteja bloqueado, ao menos nao trava o app
                socket.ReceiveTimeout = 3000;

                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                //Offset para chegar no campo "Transmit Timestamp" (que é
                //o do momento da saída do servidor, em formato 64-bit timestamp
                const byte serverReplyTime = 40;

                //Pegando os segundos
                ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

                //e a fração de segundos
                ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                //Passando de big-endian pra little-endian
                intPart = SwapEndianness(intPart);
                fractPart = SwapEndianness(fractPart);

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                //Tempo em **UTC**
                var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

                return networkDateTime.ToLocalTime();
            }
            catch { throw; }
        }
        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        //Criando Parametros

        public MySqlParameter CreateParameter(string _nomeParam, MySqlDbType _tipoParam, object _valorParam)
        {
            return CreateParameter(_nomeParam, ParameterDirection.Input, _tipoParam, _valorParam, null);
        }
        public MySqlParameter CreateParameter(string _nomeParam, ParameterDirection _paramDirection, MySqlDbType _tipoParam, object _valorParam)
        {
            return CreateParameter(_nomeParam, _paramDirection, _tipoParam, _valorParam, null);
        }
        public MySqlParameter CreateParameter(string _nomeParam, MySqlDbType _tipoParam, object _valorParam, object _valorNulo)
        {
            return CreateParameter(_nomeParam, ParameterDirection.Input, _tipoParam, _valorParam, _valorNulo);
        }
        public MySqlParameter CreateParameter(string _nomeParam, ParameterDirection _paramDirection, MySqlDbType _tipoParam, object _valorParam, object _valorNulo)
        {
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = _nomeParam;
            param.MySqlDbType = _tipoParam;
            param.Direction = _paramDirection;
            if (_valorParam == null || _valorParam == DBNull.Value)
                param.Value = DBNull.Value;
            else if (_valorNulo != null && _valorNulo.ToString().Equals(_valorParam.ToString()))
                param.Value = DBNull.Value;
            else
            {
                switch (_tipoParam)
                {
                    case MySqlDbType.VarChar:
                    case MySqlDbType.String:
                        {
                            if (String.IsNullOrEmpty(_valorParam.ToString()))
                                param.Value = DBNull.Value;
                            else
                                param.Value = _valorParam.ToString();
                            break;
                        }
                    case MySqlDbType.DateTime:
                    case MySqlDbType.Date:
                        {
                            DateTime dateTime = Convert.ToDateTime(_valorParam);
                            if (dateTime.Equals(DateTime.MinValue))
                                param.Value = DBNull.Value;
                            else
                                param.Value = dateTime;
                            break;
                        }
                    default:
                        {
                            param.Value = _valorParam;
                            break;
                        }
                }
            }
            return param;
        }

        //Trabalhando com String de Conexão Constante

        public bool ExecuteNonQuery(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(StringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        int i = cmd.ExecuteNonQuery();
                        _lastInsertId = cmd.LastInsertedId;

                        return i > 0;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public bool ExecuteNonQuery(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQuery(_cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public int ExecuteNonQueryRows(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(StringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        int i = cmd.ExecuteNonQuery();
                        _lastInsertId = cmd.LastInsertedId;

                        return i;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public int ExecuteNonQueryRows(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQueryRows(_cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public object ExecuteScalar(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(StringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        object i = cmd.ExecuteScalar();
                        return i;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public MySqlDataReader ExecuteReader(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                MySqlConnection cnx = new MySqlConnection(StringConexao);
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = cnx;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    if (cnx.State != ConnectionState.Open)
                        cnx.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return rdr;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public DataTable FillDataTable(CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlConnection cnx = new MySqlConnection(StringConexao);
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = cnx;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                return dt;
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        //Trabalhando com String de Conexao Variável

        public bool ExecuteNonQuery(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(_stringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        int i = cmd.ExecuteNonQuery();
                        _lastInsertId = cmd.LastInsertedId;

                        return i > 0;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public bool ExecuteNonQuery(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQuery(_stringConexao, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public int ExecuteNonQueryRows(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(_stringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        int i = cmd.ExecuteNonQuery();
                        _lastInsertId = cmd.LastInsertedId;

                        return i;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public int ExecuteNonQueryRows(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQueryRows(_stringConexao, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public object ExecuteScalar(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(_stringConexao))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Parameters.Clear();
                        if (_parmsSQL != null)
                            cmd.Parameters.AddRange(_parmsSQL);

                        cmd.Connection = cnx;
                        cmd.CommandType = _cmdType;
                        cmd.CommandText = _cmdSQL;
                        if (_commandTimeout > 0)
                            cmd.CommandTimeout = _commandTimeout;

                        if (cnx.State != ConnectionState.Open)
                            cnx.Open();

                        object i = cmd.ExecuteScalar();
                        return i;
                    }
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public MySqlDataReader ExecuteReader(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                MySqlConnection cnx = new MySqlConnection(_stringConexao);
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = cnx;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    if (cnx.State != ConnectionState.Open)
                        cnx.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return rdr;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public DataTable FillDataTable(string _stringConexao, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                DataTable dt = new DataTable();
                MySqlConnection cnx = new MySqlConnection(_stringConexao);
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = cnx;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                return dt;
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        //Trabalhando com Objeto de Conexão

        public MySqlConnection OpenConnection(string _connectionString)
        {
            if (String.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException();

            try
            {
                MySqlConnection cnx = new MySqlConnection(_connectionString);
                if (cnx.State != ConnectionState.Open)
                    cnx.Open();

                return cnx;
            }
            catch (MySqlException dbError)
            {
                throw new Exception(dbError.Message, dbError);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message, error);
            }

        }
        public MySqlConnection OpenConnection()
        {
            return OpenConnection(StringConexao);
        }
        public void CloseConnection(MySqlConnection _connection)
        {
            try
            {
                if (_connection == null)
                    return;

                _connection.Close();
            }
            catch (MySqlException dbError)
            {
                throw new Exception(dbError.Message, dbError);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message, error);
            }
        }

        public bool ExecuteNonQuery(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    cmd.Connection = _connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    int i = cmd.ExecuteNonQuery();
                    _lastInsertId = cmd.LastInsertedId;
                    return i > 0;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public bool ExecuteNonQuery(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQuery(_connection, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public int ExecuteNonQueryRows(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    cmd.Connection = _connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    int i = cmd.ExecuteNonQuery();
                    _lastInsertId = cmd.LastInsertedId;

                    return i;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public int ExecuteNonQueryRows(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQueryRows(_connection, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public object ExecuteScalar(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    cmd.Connection = _connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    object i = cmd.ExecuteScalar();
                    return i;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public MySqlDataReader ExecuteReader(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return rdr;
                }
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public DataTable FillDataTable(MySqlConnection _connection, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                return dt;
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        //Trabalhando com Transação

        MySqlTransaction BeginTransaction(string _connectionString, IsolationLevel _isolationLevel)
        {
            MySqlTransaction trans = null;
            try
            {
                MySqlConnection conn = new MySqlConnection(_connectionString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                trans = conn.BeginTransaction(_isolationLevel);
                return trans;
            }
            catch (MySqlException dbError)
            {
                throw new Exception(dbError.Message, dbError);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message, error);
            }
        }
        public MySqlTransaction BeginTransaction(string _connectionString)
        {
            if (String.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException();
            return BeginTransaction(_connectionString, IsolationLevel.ReadCommitted);
        }
        public MySqlTransaction BeginTransaction()
        {
            if (String.IsNullOrEmpty(StringConexao))
                throw new ArgumentNullException();
            return BeginTransaction(StringConexao, IsolationLevel.ReadCommitted);
        }
        public void CommitTransaction(MySqlTransaction _transaction)
        {
            if (_transaction != null)
            {
                try
                {
                    MySqlConnection conn = _transaction.Connection;
                    _transaction.Commit();
                    if (conn != null)
                        conn.Dispose();
                }
                catch (MySqlException dbError)
                {
                    throw new Exception(dbError.Message, dbError);
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message, error);
                }
            }
            else
                throw new ArgumentException("Object transaction cannot be null");
        }
        public void RollbackTransaction(MySqlTransaction _transaction)
        {
            if (_transaction != null)
            {
                try
                {
                    MySqlConnection conn = _transaction.Connection;
                    _transaction.Rollback();
                    if (conn != null)
                        conn.Dispose();
                }
                catch (MySqlException dbError)
                {
                    throw new Exception(dbError.Message, dbError);
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message, error);
                }
            }
            else
                throw new ArgumentException("Object transaction cannot be null");
        }

        public bool ExecuteNonQuery(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            MySqlConnection cnx = _transaction.Connection;
            MySqlCommand cmd = new MySqlCommand();

            cmd.Parameters.Clear();
            if (_parmsSQL != null)
                cmd.Parameters.AddRange(_parmsSQL);

            cmd.Connection = cnx;
            cmd.CommandType = _cmdType;
            cmd.CommandText = _cmdSQL;
            if (_commandTimeout > 0)
                cmd.CommandTimeout = _commandTimeout;

            if (cnx.State != ConnectionState.Open)
                cnx.Open();

            if (_transaction != null)
                cmd.Transaction = _transaction;

            int i = cmd.ExecuteNonQuery();
            _lastInsertId = cmd.LastInsertedId;

            return i > 0;
        }
        public bool ExecuteNonQuery(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQuery(_transaction, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public int ExecuteNonQueryRows(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, out long _lastInsertId, int _commandTimeout = 60000)
        {
            MySqlConnection cnx = _transaction.Connection;
            MySqlCommand cmd = new MySqlCommand();

            cmd.Parameters.Clear();
            if (_parmsSQL != null)
                cmd.Parameters.AddRange(_parmsSQL);

            cmd.Connection = cnx;
            cmd.CommandType = _cmdType;
            cmd.CommandText = _cmdSQL;
            if (_commandTimeout > 0)
                cmd.CommandTimeout = _commandTimeout;

            if (cnx.State != ConnectionState.Open)
                cnx.Open();

            if (_transaction != null)
                cmd.Transaction = _transaction;

            int i = cmd.ExecuteNonQuery();
            _lastInsertId = cmd.LastInsertedId;

            return i;
        }
        public int ExecuteNonQueryRows(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            long zero = 0;
            return ExecuteNonQueryRows(_transaction, _cmdType, _cmdSQL, _parmsSQL, out zero, _commandTimeout);
        }
        public object ExecuteScalar(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            MySqlConnection cnx = _transaction.Connection;
            MySqlCommand cmd = new MySqlCommand();

            cmd.Parameters.Clear();
            if (_parmsSQL != null)
                cmd.Parameters.AddRange(_parmsSQL);

            cmd.Connection = cnx;
            cmd.CommandType = _cmdType;
            cmd.CommandText = _cmdSQL;
            if (_commandTimeout > 0)
                cmd.CommandTimeout = _commandTimeout;

            if (cnx.State != ConnectionState.Open)
                cnx.Open();

            if (_transaction != null)
                cmd.Transaction = _transaction;

            object i = cmd.ExecuteScalar();

            return i;
        }
        public MySqlDataReader ExecuteReader(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            MySqlConnection cnx = _transaction.Connection;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnx;
            cmd.CommandType = _cmdType;
            cmd.CommandText = _cmdSQL;
            if (_commandTimeout > 0)
                cmd.CommandTimeout = _commandTimeout;

            cmd.Parameters.Clear();
            if (_parmsSQL != null)
                cmd.Parameters.AddRange(_parmsSQL);

            if (cnx.State != ConnectionState.Open)
                cnx.Open();

            if (_transaction != null)
                cmd.Transaction = _transaction;

            MySqlDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }
        public DataTable FillDataTable(MySqlTransaction _transaction, CommandType _cmdType, string _cmdSQL, MySqlParameter[] _parmsSQL, int _commandTimeout = 60000)
        {
            try
            {
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _transaction.Connection;
                    cmd.CommandType = _cmdType;
                    cmd.CommandText = _cmdSQL;
                    if (_commandTimeout > 0)
                        cmd.CommandTimeout = _commandTimeout;

                    cmd.Parameters.Clear();
                    if (_parmsSQL != null)
                        cmd.Parameters.AddRange(_parmsSQL);

                    if (_transaction != null)
                        cmd.Transaction = _transaction;

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                return dt;
            }
            catch (MySqlException mErr)
            {
                throw new Exception(mErr.Message);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        ///////////////////////////////

        public string ShowSQLCommand(string _commandText, IDataParameter[] _cmdParameters, string[] _ignoreParameters = null)
        {
            string parameterValue = string.Empty;
            for (int indexParameters = _cmdParameters.Length - 1; indexParameters >= 0; indexParameters--)
            {
                parameterValue = string.Empty;
                string identifier = _cmdParameters[indexParameters].ParameterName.Contains("?") ? string.Empty : "?";

                if (_ignoreParameters != null && _ignoreParameters.ToList().Exists(p => ("?" + p.ToLower()).Equals(_cmdParameters[indexParameters].ParameterName.ToLower())))
                    continue;
                if (_cmdParameters[indexParameters].DbType == DbType.DateTime)
                    parameterValue = string.IsNullOrEmpty(_cmdParameters[indexParameters].Value.ToString()) ? "NULL" : "'" + Convert.ToDateTime(_cmdParameters[indexParameters].Value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                else if (_cmdParameters[indexParameters].DbType == DbType.Date)
                    parameterValue = string.IsNullOrEmpty(_cmdParameters[indexParameters].Value.ToString()) ? "NULL" : "'" + Convert.ToDateTime(_cmdParameters[indexParameters].Value).ToString("yyyy-MM-dd") + "'";
                else if (_cmdParameters[indexParameters].DbType == DbType.Decimal || _cmdParameters[indexParameters].DbType == DbType.Double || _cmdParameters[indexParameters].DbType == DbType.Single)
                    parameterValue = _cmdParameters[indexParameters].Value == DBNull.Value ? "NULL" : AjustaValor(_cmdParameters[indexParameters].Value.ToString());
                else if (_cmdParameters[indexParameters].DbType == DbType.Int16 || _cmdParameters[indexParameters].DbType == DbType.Int32 || _cmdParameters[indexParameters].DbType == DbType.Int64)
                    parameterValue = _cmdParameters[indexParameters].Value == DBNull.Value ? "NULL" : ConvertToInt64(_cmdParameters[indexParameters].Value).ToString();
                else
                    parameterValue = string.IsNullOrEmpty(_cmdParameters[indexParameters].Value.ToString()) ? "NULL" : "'" + _cmdParameters[indexParameters].Value.ToString() + "'";
                _commandText = _commandText.Replace(identifier + _cmdParameters[indexParameters].ParameterName, parameterValue);
            }
            return _commandText.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
        }
        public string ShowSQLCommand(string _commandText, DbParameterCollection _parametersCollection, string[] _ignoreParameters = null)
        {
            try
            {
                int count = 0;
                DbParameter[] parms = new DbParameter[_parametersCollection.Count];
                foreach (DbParameter item in _parametersCollection)
                {
                    parms[count] = item;
                    count++;
                }
                return ShowSQLCommand(_commandText, parms, _ignoreParameters);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public string RecuperaVersaoBanco()
        {
            try
            {
                string comando = "select valor from config where parametro = 'versao' order by parametro";
                using (MySqlDataReader rdr = ExecuteReader(CommandType.Text, comando, null))
                {
                    if (rdr.Read())
                        return rdr.GetString(0);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public string ConverterDataParaLetra(DateTime _data)
        {
            string ddMMyyyy = _data.ToString("ddMMyyyy");

            string strDate = @"0123456789";
            string strLetter = @"ABCDEFGHIJ";
            for (int i = 0; i < strDate.Length; i++)
                ddMMyyyy = ddMMyyyy.Replace(strDate[i], strLetter[i]);

            return ddMMyyyy.ToUpper();
        }

        public short ConvertToInt16(object _value, short defaltulValue = 0)
        {
            try
            {
                return Convert.ToInt16(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public int ConvertToInt32(object _value, int defaltulValue = 0)
        {
            try
            {
                return Convert.ToInt32(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public long ConvertToInt64(object _value, long defaltulValue = 0)
        {
            try
            {
                return Convert.ToInt64(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public float ConvertToSingle(object _value, float defaltulValue = 0f)
        {
            try
            {
                return Convert.ToSingle(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public double ConvertToDouble(object _value, double defaltulValue = 0d)
        {
            try
            {
                return Convert.ToDouble(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public decimal ConvertToDecimal(object _value, decimal defaltulValue = 0m)
        {
            try
            {
                return Convert.ToDecimal(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public bool ConvertToBoolean(object _value, bool defaltulValue = false)
        {
            try
            {
                return Convert.ToBoolean(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }
        public DateTime ConvertToDateTime(object _value)
        {
            try
            {
                return Convert.ToDateTime(_value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public DateTime ConvertToDateTimeWithDefault(object _value, DateTime defaultValue)
        {
            try
            {
                return Convert.ToDateTime(_value);
            }
            catch
            {
                return defaultValue;
            }
        }
        public DateTime ConvertToDateTime_yyyyMMdd(string _value)
        {
            try
            {
                return new DateTime
                    (Convert.ToInt32(_value.Substring(0, 4)),
                     Convert.ToInt32(_value.Substring(4, 2)),
                     Convert.ToInt32(_value.Substring(6, 2)));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("Erro ao tentar converter Data: " + _value.ToString());
            }
        }
        public DateTime GetDateToNfpExtension(object _extensionValue)
        {
            try
            {
                string extensionValue = ConvertToString(_extensionValue).ToUpper().Replace(".", "");

                IDictionary<string, int> dic = new Dictionary<string, int>();
                dic.Add("1", 1); dic.Add("2", 2); dic.Add("3", 3); dic.Add("4", 4); dic.Add("5", 5);
                dic.Add("6", 6); dic.Add("7", 7); dic.Add("8", 8); dic.Add("9", 9); dic.Add("A", 10);
                dic.Add("B", 11); dic.Add("C", 12); dic.Add("D", 13); dic.Add("E", 14); dic.Add("F", 15);
                dic.Add("G", 16); dic.Add("H", 17); dic.Add("I", 18); dic.Add("J", 19); dic.Add("K", 20); dic.Add("L", 21);
                dic.Add("M", 22); dic.Add("N", 23); dic.Add("O", 24); dic.Add("P", 25); dic.Add("Q", 26);
                dic.Add("R", 27); dic.Add("S", 28); dic.Add("T", 29); dic.Add("U", 30); dic.Add("V", 31);
                dic.Add("X", 33); dic.Add("Z", 33);

                string data = "";
                int x = 0;
                for (int i = 0; i < 3; i++)
                {
                    x = ConvertToInt32(dic[extensionValue[i].ToString()]);
                    data += x.ToString("00");
                }

                return DateTime.ParseExact(data, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DateTime? GetDateTimeOrNull(object _value)
        {
            try
            {
                return Convert.ToDateTime(_value);
            }
            catch
            {
                return null;
            }
        }
        public string ConvertToString(object _value, string defaltulValue = "")
        {
            try
            {
                return Convert.ToString(_value);
            }
            catch
            {
                return defaltulValue;
            }
        }

        public double FormataMoeda(object _value, int _precisao)
        {
            string value = _value.ToString().Replace(",", "").Replace(".", "");
            return Convert.ToDouble(value) / Math.Pow(10, _precisao);
        }
        public Control GetControlFocus(Control _control)
        {
            foreach (Control item in _control.Controls)
            {
                if (item.Focused)
                    return item;
                else if (item.Controls.Count > 0)
                    return GetControlFocus(item);
            }
            return null;
        }

        public void GerarAquivoXml(DataTable _dataTable, string _pathAndFileName, string _dataTableName, string _dataSetName)
        {
            try
            {
                if (_dataTable.Rows.Count > 0)
                {
                    var ds = new DataSet(_dataSetName);
                    _dataTable.TableName = _dataTableName;
                    ds.Tables.Add(_dataTable);

                    ds.WriteXml(_pathAndFileName);

                    ds.Tables.Remove(_dataTable);
                    ds = null;
                    _dataTable.TableName = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int RetornaDigitoEan13(string _codigo)
        {
            string CodigoTemp = _codigo;
            int iSoma = 0;
            int iDigito = 0;

            for (int i = CodigoTemp.Length; i >= 1; i--)
            {
                iDigito = Convert.ToInt32(CodigoTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                {	// par
                    iSoma += iDigito * 3;
                }
                else
                {	// impar
                    iSoma += iDigito * 1;
                }
            }

            int iDigitoVericador = (10 - (iSoma % 10)) % 10;

            return iDigitoVericador;

        }

        FormWindowState oldWindowState;
        Size sizeMDIForm;
        public void AbrirFormMDI(Type _form, Form _formOwner, bool _isMDIContainer = false)
        {
            bool bolCtl = false;
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType().Equals(_form))
                {
                    if (form.WindowState == FormWindowState.Minimized)
                        form.WindowState = FormWindowState.Normal;
                    form.BringToFront();
                    form.Focus();
                    bolCtl = true;

                    for (int i = 0; i < Application.OpenForms.Count; i++)
                    {
                        if (form != Application.OpenForms[i] && Application.OpenForms[i].Owner != null)
                        {
                            if (Application.OpenForms[i].Visible)
                                Application.OpenForms[i].Close();
                        }
                    }
                    break;
                }
            }

            if (!bolCtl)
            {
                if (Application.OpenForms.Count == 10)
                {
                    Aviso("Para um melhor funcionamento da aplicação,\nnão é possível abrir mais de 8 formulários.");
                    return;
                }

                if (!_isMDIContainer)
                    _formOwner.VisibleChanged += new EventHandler(_formOwner_OwnerVisibleChanged);
                Form frm = (Form)Activator.CreateInstance(_form);
                oldWindowState = frm.WindowState;
                frm.ShowInTaskbar = false;
                bool returnOldWindowsState = true;
                if (_isMDIContainer)
                {
                    frm.MdiParent = _formOwner;
                    if (sizeMDIForm.Height == 0 && sizeMDIForm.Width == 0)
                    {
                        foreach (var item in _formOwner.Controls)
                        {
                            if (item is MdiClient)
                            {
                                sizeMDIForm = ((MdiClient)item).Size;
                                break;
                            }
                        }
                    }
                    if (frm.WindowState == FormWindowState.Maximized && sizeMDIForm.Height != 0 && sizeMDIForm.Width != 0)
                    {
                        frm.WindowState = FormWindowState.Normal;
                        frm.MaximizeBox = false;
                        frm.AutoScaleMode = AutoScaleMode.None;
                        frm.Size = new Size(sizeMDIForm.Width - 5, sizeMDIForm.Height - 5);
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        returnOldWindowsState = false;
                    }
                }
                else
                    frm.Owner = _formOwner;

                frm.Show();
                if (returnOldWindowsState)
                    frm.WindowState = oldWindowState;
                frm.BringToFront();
                if (!_isMDIContainer)
                    EnableWindow(_formOwner.Handle, false);

                frm.FormClosed += frm_FormClosed;
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    if (frm != Application.OpenForms[i] && Application.OpenForms[i].Owner != null)
                    {
                        if (Application.OpenForms[i].Visible)
                            Application.OpenForms[i].Close();
                    }
                }
            }
        }
        void _formOwner_OwnerVisibleChanged(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            foreach (Form child in form.OwnedForms)
            {
                child.Visible = form.Visible;
                if (child.Visible)
                {
                    child.BringToFront();
                    child.Focus();
                }
            }
        }
        void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form)
            {
                Form form = (Form)sender;
                try
                {
                    try
                    {
                        if (form.Owner != null)
                        {
                            form.Owner.VisibleChanged -= new EventHandler(_formOwner_OwnerVisibleChanged);
                            form.Owner.BeginInvoke(new EventHandler(delegate (object obj, EventArgs ev)
                            {
                                Form owner = (Form)obj;
                                EnableWindow(owner.Handle, true);
                                Application.DoEvents();
                                owner = null;
                                obj = null;
                            }), form.Owner, EventArgs.Empty);
                        }
                    }
                    catch
                    { }
                    form.FormClosed -= frm_FormClosed;
                }
                catch { }
                form.Visible = false;
                form.Dispose();
                form = null;
            }
            sender = null;
        }

        public void BuscaCep(CepInfo cepinfo)
        {
            try
            {
                HttpWebRequest requisicao = (HttpWebRequest)WebRequest.Create("http://www.buscacep.correios.com.br/servicos/dnec/consultaLogradouroAction.do?Metodo=listaLogradouro&CEP=" + cepinfo.Cep + "&TipoConsulta=cep");
                HttpWebResponse resposta = (HttpWebResponse)requisicao.GetResponse();

                int cont;
                byte[] buffer = new byte[1000];
                StringBuilder sb = new StringBuilder();
                string temp;

                Stream stream = resposta.GetResponseStream();

                do
                {
                    cont = stream.Read(buffer, 0, buffer.Length);
                    temp = Encoding.Default.GetString(buffer, 0, cont).Trim();
                    sb.Append(temp);
                }
                while (cont > 0);

                string pagina = sb.ToString();

                if (pagina.IndexOf("<font color=\"black\">CEP NAO ENCONTRADO</font>") >= 0)
                    cepinfo.Cep = "<b style=\"color:red\">CEP não localizado.</b>";
                else
                {
                    try
                    {
                        cepinfo.Logradouro = Regex.Match(pagina, "<td width=\"268\" style=\"padding: 2px\">(.*)</td>").Groups[1].Value;
                        cepinfo.Logradouro = RemoveCaracteresEspeciais(CleanText(cepinfo.Logradouro.Replace("'", " ")));
                    }
                    catch { };
                    try
                    {
                        cepinfo.Bairro = Regex.Matches(pagina, "<td width=\"140\" style=\"padding: 2px\">(.*)</td>")[0].Groups[1].Value;
                        cepinfo.Bairro = RemoveCaracteresEspeciais(CleanText(cepinfo.Bairro.Replace("'", " ")));
                    }
                    catch { };
                    try
                    {
                        cepinfo.Cidade = Regex.Matches(pagina, "<td width=\"140\" style=\"padding: 2px\">(.*)</td>")[1].Groups[1].Value;
                        cepinfo.Cidade = RemoveCaracteresEspeciais(CleanText(cepinfo.Cidade.Replace("'", " ")));
                    }
                    catch { };
                    try
                    {
                        cepinfo.UF = Regex.Match(pagina, "<td width=\"25\" style=\"padding: 2px\">(.*)</td>").Groups[1].Value;
                        cepinfo.UF = RemoveCaracteresEspeciais(CleanText(cepinfo.UF.Replace("'", " ")));
                    }
                    catch { };
                }
            }
            catch { MessageBox.Show("Não foi possível localizar o CEP, verifique sua conexão com a internet!!!"); }
        }
        public List<string> RetornarTiposDeLogradouro()
        {
            var lst = new List<string>();

            lst.Add("10ª Rua");
            lst.Add("10ª Travessa");
            lst.Add("11ª Rua");
            lst.Add("11ª Travessa");
            lst.Add("12ª Rua");
            lst.Add("12ª Travessa");
            lst.Add("13ª Travessa");
            lst.Add("14ª Travessa");
            lst.Add("15ª Travessa");
            lst.Add("16ª Travessa");
            lst.Add("17ª Travessa");
            lst.Add("18ª Travessa");
            lst.Add("19ª Travessa");
            lst.Add("1ª Avenida");
            lst.Add("1ª Paralela");
            lst.Add("1ª Rua");
            lst.Add("1ª Subida");
            lst.Add("1ª Travessa");
            lst.Add("1ª Travessa da Rodovia");
            lst.Add("1ª Vila");
            lst.Add("1º Alto");
            lst.Add("1º Beco");
            lst.Add("20ª Travessa");
            lst.Add("21ª Travessa");
            lst.Add("22ª Travessa");
            lst.Add("2ª Avenida");
            lst.Add("2ª Ladeira");
            lst.Add("2ª Paralela");
            lst.Add("2ª Rua");
            lst.Add("2ª Subida");
            lst.Add("2ª Travessa");
            lst.Add("2ª Travessa da Rodovia");
            lst.Add("2ª Vila");
            lst.Add("2º Alto");
            lst.Add("2º Beco");
            lst.Add("3ª Avenida");
            lst.Add("3ª Ladeira");
            lst.Add("3ª Paralela");
            lst.Add("3ª Rua");
            lst.Add("3ª Subida");
            lst.Add("3ª Travessa");
            lst.Add("3ª Vila");
            lst.Add("3º Alto");
            lst.Add("3º Beco");
            lst.Add("4ª Avenida");
            lst.Add("4ª Paralela");
            lst.Add("4ª Rua");
            lst.Add("4ª Subida");
            lst.Add("4ª Travessa");
            lst.Add("4ª Vila");
            lst.Add("5ª Avenida");
            lst.Add("5ª Rua");
            lst.Add("5ª Subida");
            lst.Add("5ª Travessa");
            lst.Add("5ª Vila");
            lst.Add("6ª Avenida");
            lst.Add("6ª Rua");
            lst.Add("6ª Subida");
            lst.Add("6ª Travessa");
            lst.Add("7ª Rua");
            lst.Add("7ª Travessa");
            lst.Add("8ª Travessa");
            lst.Add("9ª Rua");
            lst.Add("9ª Travessa");
            lst.Add("Acampamento");
            lst.Add("Acesso");
            lst.Add("Acesso Local");
            lst.Add("Adro");
            lst.Add("Aeroporto");
            lst.Add("Alameda");
            lst.Add("Alto");
            lst.Add("Anel Viário");
            lst.Add("Antiga Estação");
            lst.Add("Antiga Estrada");
            lst.Add("Área");
            lst.Add("Área Especial");
            lst.Add("Área Verde");
            lst.Add("Artéria");
            lst.Add("Atalho");
            lst.Add("Avenida");
            lst.Add("Avenida Contorno");
            lst.Add("Avenida Marginal");
            lst.Add("Avenida Marginal Direita");
            lst.Add("Avenida Marginal Esquerda");
            lst.Add("Avenida Marginal Norte");
            lst.Add("Avenida Perimetral");
            lst.Add("Baixa");
            lst.Add("Balão");
            lst.Add("Beco");
            lst.Add("Belvedere");
            lst.Add("Bloco");
            lst.Add("Blocos");
            lst.Add("Bosque");
            lst.Add("Boulevard");
            lst.Add("Bulevar");
            lst.Add("Buraco");
            lst.Add("Cais");
            lst.Add("Calçada");
            lst.Add("Calçadão");
            lst.Add("Caminho");
            lst.Add("Caminho de Servidão");
            lst.Add("Campo");
            lst.Add("Campus");
            lst.Add("Canal");
            lst.Add("Chácara");
            lst.Add("Ciclovia");
            lst.Add("Circular");
            lst.Add("Colônia");
            lst.Add("Complexo Viário");
            lst.Add("Comunidade");
            lst.Add("Condomínio");
            lst.Add("Condomínio Residencial");
            lst.Add("Conjunto");
            lst.Add("Conjunto Habitacional");
            lst.Add("Conjunto Mutirão");
            lst.Add("Conjunto Residencial");
            lst.Add("Contorno");
            lst.Add("Corredor");
            lst.Add("Córrego");
            lst.Add("Descida");
            lst.Add("Desvio");
            lst.Add("Distrito");
            lst.Add("Eixo");
            lst.Add("Eixo Industrial");
            lst.Add("Eixo Principal");
            lst.Add("Elevada");
            lst.Add("Entrada Particular");
            lst.Add("Entre Quadra");
            lst.Add("Escada");
            lst.Add("Escadaria");
            lst.Add("Esplanada");
            lst.Add("Estação");
            lst.Add("Estacionamento");
            lst.Add("Estádio");
            lst.Add("Estrada");
            lst.Add("Estrada Antiga");
            lst.Add("Estrada de Ferro");
            lst.Add("Estrada de Ligação");
            lst.Add("Estrada de Servidão");
            lst.Add("Estrada Estadual");
            lst.Add("Estrada Intermunicipal");
            lst.Add("Estrada Municipal");
            lst.Add("Estrada Nova");
            lst.Add("Estrada Particular");
            lst.Add("Estrada Velha");
            lst.Add("Estrada Vicinal");
            lst.Add("Favela");
            lst.Add("Fazenda");
            lst.Add("Feira");
            lst.Add("Ferrovia");
            lst.Add("Fonte");
            lst.Add("Forte");
            lst.Add("Galeria");
            lst.Add("Gleba");
            lst.Add("Granja");
            lst.Add("Ilha");
            lst.Add("Jardim");
            lst.Add("Jardim Residencial");
            lst.Add("Jardinete");
            lst.Add("Ladeira");
            lst.Add("Lago");
            lst.Add("Lagoa");
            lst.Add("Largo");
            lst.Add("Loteamento");
            lst.Add("Margem");
            lst.Add("Marina");
            lst.Add("Mercado");
            lst.Add("Módulo");
            lst.Add("Monte");
            lst.Add("Morro");
            lst.Add("Nova Avenida");
            lst.Add("Núcleo");
            lst.Add("Núcleo Habitacional");
            lst.Add("Núcleo Rural");
            lst.Add("Outeiro");
            lst.Add("Parada");
            lst.Add("Paralela");
            lst.Add("Parque");
            lst.Add("Parque Municipal");
            lst.Add("Parque Residencial");
            lst.Add("Passagem");
            lst.Add("Passagem de Pedestres");
            lst.Add("Passagem Subterrânea");
            lst.Add("Passarela");
            lst.Add("Passeio");
            lst.Add("Passeio Público");
            lst.Add("Pátio");
            lst.Add("Ponta");
            lst.Add("Ponte");
            lst.Add("Porto");
            lst.Add("Praça");
            lst.Add("Praça de Esportes");
            lst.Add("Praia");
            lst.Add("Prolongamento");
            lst.Add("Quadra");
            lst.Add("Quinta");
            lst.Add("Ramal");
            lst.Add("Rampa");
            lst.Add("Recanto");
            lst.Add("Residencial");
            lst.Add("Reta");
            lst.Add("Retiro");
            lst.Add("Retorno");
            lst.Add("Rodo Anel");
            lst.Add("Rodovia");
            lst.Add("Rotatória");
            lst.Add("Rótula");
            lst.Add("Rua");
            lst.Add("Rua de Ligação");
            lst.Add("Rua de Pedestre");
            lst.Add("Rua Particular");
            lst.Add("Rua Principal");
            lst.Add("Rua Projetada");
            lst.Add("Rua Velha");
            lst.Add("Ruela");
            lst.Add("Servidão");
            lst.Add("Servidão de Passagem");
            lst.Add("Setor");
            lst.Add("Sítio");
            lst.Add("Subida");
            lst.Add("Terceira Avenida");
            lst.Add("Terminal");
            lst.Add("Travessa");
            lst.Add("Travessa Particular");
            lst.Add("Trecho");
            lst.Add("Trevo");
            lst.Add("Trincheira");
            lst.Add("Túnel");
            lst.Add("Unidade");
            lst.Add("Vala");
            lst.Add("Vale");
            lst.Add("Variante da Estrada");
            lst.Add("Vereda");
            lst.Add("Via");
            lst.Add("Via Coletora");
            lst.Add("Via Costeira");
            lst.Add("Via de Acesso");
            lst.Add("Via de Pedestre");
            lst.Add("Via de Pedestres");
            lst.Add("Via Expressa");
            lst.Add("Via Lateral");
            lst.Add("Via Litoranea");
            lst.Add("Via Local");
            lst.Add("Via Marginal");
            lst.Add("Via Pedestre");
            lst.Add("Via Principal");
            lst.Add("Viaduto");
            lst.Add("Viela");
            lst.Add("Vila");
            lst.Add("Zigue-Zague");

            return lst;
        }
        public void SanearLogradouro(CepInfo _cepInfo)
        {
            try
            {
                string logradouro = string.Empty;
                foreach (string item in RetornarTiposDeLogradouro())
                {
                    logradouro = _cepInfo.Logradouro;
                    if (logradouro.StartsWith(item))
                    {
                        _cepInfo.LogradouroTipo = item.ToUpper();
                        _cepInfo.Logradouro = logradouro.Replace(item, "").TrimStart();
                    }
                }
            }
            catch { }
        }

        public void BloqueiaDigitacao(KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
                e.Handled = true;
        }

        public void ExcelExport(DataGridView dgView, string path, string name, string type, bool open)
        {
            if (type == ".xls")
            {
                System.IO.StreamWriter excelDoc;
                excelDoc = new System.IO.StreamWriter(path + "\\" + name + ".xls");
                const string startExcelXML =
                "<xml version>\r\n<Workbook " +
                  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n " +
                  "xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                  "xmlns:x=\"urn:schemas-microsoft-com:office: " +
                  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com: " +
                  "office:spreadsheet\">\r\n <Styles>\r\n " +
                  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                  "\r\n <Protection/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                  //"ss:Format=\"0.00\"/>\r\n </Style>\r\n " + //
                  "ss:Format=\"#,##0.00\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                  "ss:Format=\"dd/mm/yyyy;@\"/>\r\n </Style>\r\n " +
                  "</Styles>\r\n "; // <NumberFormat ss:Format=
                //"_(* #,##0.00_);_(* \(#,##0.00\);_(* &quot;-&quot;??_);_(@_)"/>

                const string endExcelXML = "</Workbook>";

                int rowCount = 0;
                int sheetCount = 1;

                excelDoc.Write(startExcelXML);
                excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                excelDoc.Write("<Table>");
                for (int w = 0; w < dgView.Columns.Count; w++)
                {
                    excelDoc.Write("<Column ss:Width=" + "\"" + dgView.Columns[w].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, false).ToString() + "\"" + "/>");
                }
                excelDoc.Write("<Row>");
                for (int x = 0; x < dgView.Columns.Count; x++)
                {
                    excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                    excelDoc.Write(dgView.Columns[x].HeaderText);
                    excelDoc.Write("</Data></Cell>");
                }
                excelDoc.Write("</Row>");

                foreach (DataGridViewRow x in dgView.Rows)
                {
                    rowCount++;
                    //if the number of rows is > 64000 create a new page to continue output
                    if (rowCount == 64000)
                    {
                        rowCount = 0;
                        sheetCount++;
                        excelDoc.Write("</Table>");
                        excelDoc.Write(" </Worksheet>");
                        excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                        excelDoc.Write("<Table>");
                    }
                    excelDoc.Write("<Row>");
                    for (int y = 0; y < dgView.Columns.Count; y++)
                    {

                        string rowType;
                        rowType = x.Cells[y].FormattedValue.GetType().ToString();

                        string rowType2;
                        try
                        {
                            rowType2 = x.Cells[y].Value.GetType().ToString();
                        }
                        catch
                        { rowType2 = "System.String"; }

                        if (rowType2 == "System.DateTime")
                            rowType = "System.DateTime";
                        else if (rowType2 == "System.Decimal")
                            rowType = "System.Decimal";
                        else if ((rowType2 == "System.Int32") && (x.Cells[y].FormattedValue.ToString() == x.Cells[y].Value.ToString()))
                            rowType = "System.Int32";

                        switch (rowType)
                        {
                            case "System.String":
                                string XMLstring = x.Cells[y].FormattedValue.ToString();
                                XMLstring = XMLstring.Trim();
                                XMLstring = XMLstring.Replace("&", "&");
                                XMLstring = XMLstring.Replace(">", ">");
                                XMLstring = XMLstring.Replace("<", "<");
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                               "<Data ss:Type=\"String\">");
                                excelDoc.Write(XMLstring);
                                excelDoc.Write("</Data></Cell>");
                                break;

                            case "System.DateTime":
                                DateTime XMLDate = (DateTime)x.Cells[y].Value;
                                string XMLDatetoString = ""; //Excel Converted Date

                                if (XMLDate.Year < 1000)
                                    XMLDatetoString = "";
                                else
                                {
                                    XMLDatetoString =
                                    (XMLDate.Year < 1000 ? "200" +
                                    XMLDate.Year.ToString() : XMLDate.Year.ToString()) +
                                    "-" +
                                    (XMLDate.Month < 10 ? "0" +
                                    XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                                    "-" +
                                    (XMLDate.Day < 10 ? "0" +
                                    XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                                    "T" +
                                    (XMLDate.Hour < 10 ? "0" +
                                    XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                                    ":" +
                                    (XMLDate.Minute < 10 ? "0" +
                                    XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                                    ":" +
                                    (XMLDate.Second < 10 ? "0" +
                                    XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                                    ".000";
                                }

                                excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                "<Data ss:Type=\"DateTime\">");
                                excelDoc.Write(XMLDatetoString);
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Boolean":
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                            "<Data ss:Type=\"String\">");
                                string velueBool = x.Cells[y].FormattedValue.ToString();
                                velueBool = velueBool.Replace("True", "S");
                                velueBool = velueBool.Replace("False", "N");
                                excelDoc.Write(velueBool);
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                "<Data ss:Type=\"Number\">");
                                excelDoc.Write(x.Cells[y].FormattedValue.ToString());
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                                      "<Data ss:Type=\"Number\">");
                                string valueFormat = x.Cells[y].FormattedValue.ToString();
                                valueFormat = valueFormat.Replace(".", "");
                                valueFormat = valueFormat.Replace(",", ".");
                                valueFormat = valueFormat.Replace("R$ ", "");
                                excelDoc.Write(valueFormat);
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.DBNull":
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                      "<Data ss:Type=\"String\">");
                                excelDoc.Write("");
                                excelDoc.Write("</Data></Cell>");
                                break;
                            default:
                                {
                                    XMLstring = x.Cells[y].FormattedValue.ToString();
                                    XMLstring = XMLstring.Trim();
                                    XMLstring = XMLstring.Replace("&", "&");
                                    XMLstring = XMLstring.Replace(">", ">");
                                    XMLstring = XMLstring.Replace("<", "<");
                                    excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                                   "<Data ss:Type=\"String\">");
                                    excelDoc.Write(XMLstring);
                                    excelDoc.Write("</Data></Cell>");
                                    break;
                                    //throw (new Exception(rowType.ToString() + " not handled."));
                                }
                        }
                    }
                    excelDoc.Write("</Row>");
                }
                excelDoc.Write("</Table>");
                excelDoc.Write(" </Worksheet>");
                excelDoc.Write(endExcelXML);
                excelDoc.Close();
            }

            if (open)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = path + "\\" + name + type;  ////= "c:" + "\\" + "temp_ora.xls";
                p.Start();
            }
        }

        public string AjustaCpfCnpjIe(object _value)
        {
            return _value.ToString().Replace(",", "").Replace(".", "").Replace("-", "").Replace("/", "");
        }
        public bool ValidCpfCnpj(string _valueString, ref string _msgErro)
        {
            if (_valueString.Length < 12)
                return ValidCpf(_valueString, ref _msgErro);
            else
                return ValidCnpj(_valueString, ref _msgErro);
        }
        bool ValidCpf(string _valueCpf, ref string _msgErro)
        {
            Int64 n = 0;
            if (!Int64.TryParse(_valueCpf, out n))
            {
                if (string.IsNullOrEmpty(_valueCpf) || (_valueCpf.Length > 0 && !_valueCpf.Substring(0, 1).Equals("E")))
                {
                    _msgErro = "Valor Inválido!";
                    return false;
                }
            }

            string valor = _valueCpf.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "");
            valor = Convert.ToInt64(valor).ToString().PadLeft(11, '0');

            if (valor.Length != 11)
            {
                _msgErro = "CPF deve possui 11 caracteres.";
                return false;
            }

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            _msgErro = "CPF inválido.";
            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                return false;

            _msgErro = "";
            return true;
        }
        bool ValidCnpj(string _valueCnpj, ref string _msgErro)
        {
            string cnpj = _valueCnpj.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "");

            Int64 n = 0;
            if (!Int64.TryParse(cnpj, out n))
            {
                if (string.IsNullOrEmpty(cnpj) || (cnpj.Length > 0 && !cnpj.Substring(0, 1).Equals("E")))
                {
                    _msgErro = "Valor Inválido!";
                    return false;
                }
            }

            // Esta alteração e adição da mensagem de 14 caracteres se deve ao fato de que deve ter sempre os 14 digitos, a pessoa deve digitar o digito zero caso seja o primeiro numero
            // Pois na NFe não é aceito cnpj com 13 digitos, e assim já evita problemas com o cnpj em outras areas fiscais que for utilizar.
            //cnpj = Convert.ToInt64(cnpj).ToString().PadLeft(14, '0');

            if (cnpj.Length != 14)
            {
                _msgErro = "CNPJ deve possui 14 caracteres.";
                return false;
            }

            int[] digitos, soma, resultado;
            int nrdig;
            string ftmt;
            bool[] cnpjok;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            cnpjok = new bool[2];
            cnpjok[0] = false;
            cnpjok[1] = false;

            try
            {
                for (nrdig = 0; nrdig < 14; nrdig++)
                {
                    digitos[nrdig] = int.Parse(cnpj.Substring(nrdig, 1));
                    if (nrdig <= 11)
                        soma[0] += (digitos[nrdig] * int.Parse(ftmt.Substring(nrdig + 1, 1)));
                    if (nrdig <= 12)
                        soma[1] += (digitos[nrdig] * int.Parse(ftmt.Substring(nrdig, 1)));
                }

                for (nrdig = 0; nrdig < 2; nrdig++)
                {
                    resultado[nrdig] = (soma[nrdig] % 11);
                    if ((resultado[nrdig] == 0) || (resultado[nrdig] == 1))
                        cnpjok[nrdig] = (digitos[12 + nrdig] == 0);
                    else
                        cnpjok[nrdig] = (digitos[12 + nrdig] == (11 - resultado[nrdig]));
                }

                bool valid = (cnpjok[0] && cnpjok[1]);
                if (!valid)
                    _msgErro = "CNPJ inválido.";
                return valid;
            }
            catch
            {
                _msgErro = "CNPJ inválido.";
                return false;
            }
        }

        public string Encrypt(string _valueEncrypt)
        {
            Byte[] b = ASCIIEncoding.ASCII.GetBytes(_valueEncrypt);
            string valueEncrypted = Convert.ToBase64String(b);
            return valueEncrypted;
        }
        public string Decrypt(string _valueDecrypt)
        {
            if (ConvertToInt32(_valueDecrypt) > 0)
                return "";

            Byte[] b = Convert.FromBase64String(_valueDecrypt);
            string valueDecrypted = ASCIIEncoding.ASCII.GetString(b);
            return valueDecrypted;
        }

        public DataTable ConvertListToDatatable<T>(IList<T> data)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var dataTable = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item);
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static XElement ToXElement(string pathToOfxFile)
        {
            if (!System.IO.File.Exists(pathToOfxFile))
                throw new FileNotFoundException();

            var tags = from line in File.ReadAllLines(pathToOfxFile)
                       where line.Contains("<STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<MEMO>")
                       select line;


            XElement el = new XElement("root");
            XElement son = null;
            foreach (var l in tags)
            {
                if (l.IndexOf("<STMTTRN>") != -1)
                {
                    son = new XElement("STMTTRN");
                    el.Add(son);
                    continue;
                }

                var tagName = GetTagName(l);
                var elSon = new XElement(tagName);
                elSon.Value = GetTagValue(l);
                son.Add(elSon);
            }
            return el;

        }
        /// <summary>
        /// Get the Tag name to create an Xelement
        /// </summary>
        /// <param name="line">One line from the file</param>
        /// <returns></returns>
        static string GetTagName(string line)
        {
            int pos_init = line.IndexOf("<") + 1;
            int pos_end = line.IndexOf(">");
            pos_end = pos_end - pos_init;
            return line.Substring(pos_init, pos_end);
        }
        /// <summary>
        /// Get the value of the tag to put on the Xelement
        /// </summary>
        /// <param name="line">The line</param>
        /// <returns></returns>
        static string GetTagValue(string line)
        {
            int pos_init = line.IndexOf(">") + 1;
            string retValue = line.Substring(pos_init).Trim();
            if (retValue.IndexOf("[") != -1)
            {
                //date--lets get only the 8 date digits
                retValue = retValue.Substring(0, 8);
            }
            return retValue;
        }
    }
}