﻿namespace GeradorCamadaCSharp
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFechar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabAtualiza = new System.Windows.Forms.TabControl();
            this.tabGeral = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkTodos = new System.Windows.Forms.CheckBox();
            this.dgTabelasAlteradas = new System.Windows.Forms.DataGridView();
            this.seleciona = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabela = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arquivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordenar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.novaTabela = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRemoveLetrasIniciais = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTabelasIniciam = new System.Windows.Forms.TextBox();
            this.chkConsiderarRelacionamentosSetIdPorInfo = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFiltroList = new System.Windows.Forms.TextBox();
            this.txtFiltroInfo = new System.Windows.Forms.TextBox();
            this.chkConsiderarRelacionamentos = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rdbMySQL = new System.Windows.Forms.RadioButton();
            this.rdbADO_Net = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.rdbFuncoesWebService = new System.Windows.Forms.RadioButton();
            this.rdbFuncoesForm = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdbREST = new System.Windows.Forms.RadioButton();
            this.rdbSOAP = new System.Windows.Forms.RadioButton();
            this.chkValidacoesColuna = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSiglaInicial = new System.Windows.Forms.TextBox();
            this.chkStringConexao = new System.Windows.Forms.CheckBox();
            this.chkComunicadorSemNewInstance = new System.Windows.Forms.CheckBox();
            this.lblPacoteWebService = new System.Windows.Forms.Label();
            this.txtPacoteWebService = new System.Windows.Forms.TextBox();
            this.chkWebService = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPacote = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnConfirmarProcesso = new System.Windows.Forms.Button();
            this.btnAtualizaDatabases = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabAtualiza.SuspendLayout();
            this.tabGeral.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTabelasAlteradas)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFechar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(746, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatAppearance.BorderSize = 0;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Image = global::GeradorCamadaCSharp.Properties.Resources.Exit_Ret_32;
            this.btnFechar.Location = new System.Drawing.Point(705, 9);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(35, 35);
            this.btnFechar.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnFechar, "Sair");
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabAtualiza);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(746, 579);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // tabAtualiza
            // 
            this.tabAtualiza.Controls.Add(this.tabGeral);
            this.tabAtualiza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAtualiza.Location = new System.Drawing.Point(3, 16);
            this.tabAtualiza.Name = "tabAtualiza";
            this.tabAtualiza.SelectedIndex = 0;
            this.tabAtualiza.Size = new System.Drawing.Size(740, 560);
            this.tabAtualiza.TabIndex = 0;
            // 
            // tabGeral
            // 
            this.tabGeral.Controls.Add(this.groupBox4);
            this.tabGeral.Controls.Add(this.groupBox3);
            this.tabGeral.Location = new System.Drawing.Point(4, 22);
            this.tabGeral.Name = "tabGeral";
            this.tabGeral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeral.Size = new System.Drawing.Size(732, 534);
            this.tabGeral.TabIndex = 0;
            this.tabGeral.Text = "Geral";
            this.tabGeral.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkTodos);
            this.groupBox4.Controls.Add(this.dgTabelasAlteradas);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox4.Location = new System.Drawing.Point(401, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(328, 528);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // chkTodos
            // 
            this.chkTodos.AutoSize = true;
            this.chkTodos.Location = new System.Drawing.Point(12, 20);
            this.chkTodos.Name = "chkTodos";
            this.chkTodos.Size = new System.Drawing.Size(15, 14);
            this.chkTodos.TabIndex = 1;
            this.chkTodos.UseVisualStyleBackColor = true;
            this.chkTodos.Click += new System.EventHandler(this.chkTodos_CheckedChanged);
            // 
            // dgTabelasAlteradas
            // 
            this.dgTabelasAlteradas.AllowUserToAddRows = false;
            this.dgTabelasAlteradas.AllowUserToDeleteRows = false;
            this.dgTabelasAlteradas.AllowUserToResizeColumns = false;
            this.dgTabelasAlteradas.AllowUserToResizeRows = false;
            this.dgTabelasAlteradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTabelasAlteradas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.seleciona,
            this.tabela,
            this.arquivo,
            this.ordenar,
            this.novaTabela});
            this.dgTabelasAlteradas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTabelasAlteradas.Location = new System.Drawing.Point(3, 16);
            this.dgTabelasAlteradas.MultiSelect = false;
            this.dgTabelasAlteradas.Name = "dgTabelasAlteradas";
            this.dgTabelasAlteradas.RowHeadersVisible = false;
            this.dgTabelasAlteradas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTabelasAlteradas.Size = new System.Drawing.Size(322, 509);
            this.dgTabelasAlteradas.TabIndex = 0;
            // 
            // seleciona
            // 
            this.seleciona.HeaderText = "";
            this.seleciona.Name = "seleciona";
            this.seleciona.Width = 30;
            // 
            // tabela
            // 
            this.tabela.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tabela.DataPropertyName = "table_name";
            this.tabela.HeaderText = "Tabela";
            this.tabela.Name = "tabela";
            this.tabela.ReadOnly = true;
            // 
            // arquivo
            // 
            this.arquivo.DataPropertyName = "arquivo";
            this.arquivo.HeaderText = "Caminho";
            this.arquivo.Name = "arquivo";
            this.arquivo.ReadOnly = true;
            this.arquivo.Visible = false;
            // 
            // ordenar
            // 
            this.ordenar.DataPropertyName = "ordenar";
            this.ordenar.HeaderText = "ordenar";
            this.ordenar.Name = "ordenar";
            this.ordenar.ReadOnly = true;
            this.ordenar.Visible = false;
            // 
            // novaTabela
            // 
            this.novaTabela.DataPropertyName = "novaTabela";
            this.novaTabela.HeaderText = "novaTabela";
            this.novaTabela.Name = "novaTabela";
            this.novaTabela.ReadOnly = true;
            this.novaTabela.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtRemoveLetrasIniciais);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtTabelasIniciam);
            this.groupBox3.Controls.Add(this.chkConsiderarRelacionamentosSetIdPorInfo);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtFiltroList);
            this.groupBox3.Controls.Add(this.txtFiltroInfo);
            this.groupBox3.Controls.Add(this.chkConsiderarRelacionamentos);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.chkValidacoesColuna);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtSiglaInicial);
            this.groupBox3.Controls.Add(this.chkStringConexao);
            this.groupBox3.Controls.Add(this.chkComunicadorSemNewInstance);
            this.groupBox3.Controls.Add(this.lblPacoteWebService);
            this.groupBox3.Controls.Add(this.txtPacoteWebService);
            this.groupBox3.Controls.Add(this.chkWebService);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtPacote);
            this.groupBox3.Controls.Add(this.btnCancelar);
            this.groupBox3.Controls.Add(this.btnConfirmarProcesso);
            this.groupBox3.Controls.Add(this.btnAtualizaDatabases);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.cmbDatabase);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(392, 528);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Remover letras inicias:";
            // 
            // txtRemoveLetrasIniciais
            // 
            this.txtRemoveLetrasIniciais.Location = new System.Drawing.Point(122, 153);
            this.txtRemoveLetrasIniciais.Name = "txtRemoveLetrasIniciais";
            this.txtRemoveLetrasIniciais.Size = new System.Drawing.Size(223, 20);
            this.txtRemoveLetrasIniciais.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 26);
            this.label7.TabIndex = 7;
            this.label7.Text = "Apenas tabelas \r\nque iniciam com:";
            // 
            // txtTabelasIniciam
            // 
            this.txtTabelasIniciam.Location = new System.Drawing.Point(122, 127);
            this.txtTabelasIniciam.Name = "txtTabelasIniciam";
            this.txtTabelasIniciam.Size = new System.Drawing.Size(223, 20);
            this.txtTabelasIniciam.TabIndex = 8;
            // 
            // chkConsiderarRelacionamentosSetIdPorInfo
            // 
            this.chkConsiderarRelacionamentosSetIdPorInfo.AutoSize = true;
            this.chkConsiderarRelacionamentosSetIdPorInfo.Location = new System.Drawing.Point(122, 229);
            this.chkConsiderarRelacionamentosSetIdPorInfo.Name = "chkConsiderarRelacionamentosSetIdPorInfo";
            this.chkConsiderarRelacionamentosSetIdPorInfo.Size = new System.Drawing.Size(232, 17);
            this.chkConsiderarRelacionamentosSetIdPorInfo.TabIndex = 13;
            this.chkConsiderarRelacionamentosSetIdPorInfo.Text = "Considerar Relacionamentos Set Id Por Info";
            this.chkConsiderarRelacionamentosSetIdPorInfo.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 380);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Filtro List";
            this.label5.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 354);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Filtro Info";
            this.label3.Visible = false;
            // 
            // txtFiltroList
            // 
            this.txtFiltroList.Location = new System.Drawing.Point(123, 377);
            this.txtFiltroList.Name = "txtFiltroList";
            this.txtFiltroList.Size = new System.Drawing.Size(222, 20);
            this.txtFiltroList.TabIndex = 19;
            this.txtFiltroList.Visible = false;
            // 
            // txtFiltroInfo
            // 
            this.txtFiltroInfo.Location = new System.Drawing.Point(123, 351);
            this.txtFiltroInfo.Name = "txtFiltroInfo";
            this.txtFiltroInfo.Size = new System.Drawing.Size(222, 20);
            this.txtFiltroInfo.TabIndex = 17;
            this.txtFiltroInfo.Visible = false;
            // 
            // chkConsiderarRelacionamentos
            // 
            this.chkConsiderarRelacionamentos.AutoSize = true;
            this.chkConsiderarRelacionamentos.Checked = true;
            this.chkConsiderarRelacionamentos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConsiderarRelacionamentos.Location = new System.Drawing.Point(123, 206);
            this.chkConsiderarRelacionamentos.Name = "chkConsiderarRelacionamentos";
            this.chkConsiderarRelacionamentos.Size = new System.Drawing.Size(161, 17);
            this.chkConsiderarRelacionamentos.TabIndex = 12;
            this.chkConsiderarRelacionamentos.Text = "Considerar Relacionamentos";
            this.chkConsiderarRelacionamentos.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rdbMySQL);
            this.groupBox7.Controls.Add(this.rdbADO_Net);
            this.groupBox7.Location = new System.Drawing.Point(246, 279);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(117, 66);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Banco de Dados ?";
            // 
            // rdbMySQL
            // 
            this.rdbMySQL.AutoSize = true;
            this.rdbMySQL.Checked = true;
            this.rdbMySQL.Location = new System.Drawing.Point(10, 19);
            this.rdbMySQL.Name = "rdbMySQL";
            this.rdbMySQL.Size = new System.Drawing.Size(60, 17);
            this.rdbMySQL.TabIndex = 0;
            this.rdbMySQL.TabStop = true;
            this.rdbMySQL.Text = "MySQL";
            this.rdbMySQL.UseVisualStyleBackColor = true;
            // 
            // rdbADO_Net
            // 
            this.rdbADO_Net.AutoSize = true;
            this.rdbADO_Net.Location = new System.Drawing.Point(10, 42);
            this.rdbADO_Net.Name = "rdbADO_Net";
            this.rdbADO_Net.Size = new System.Drawing.Size(73, 17);
            this.rdbADO_Net.TabIndex = 1;
            this.rdbADO_Net.Text = "ADO.NET";
            this.rdbADO_Net.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.rdbFuncoesWebService);
            this.groupBox6.Controls.Add(this.rdbFuncoesForm);
            this.groupBox6.Location = new System.Drawing.Point(123, 279);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(117, 66);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Funções ?";
            // 
            // rdbFuncoesWebService
            // 
            this.rdbFuncoesWebService.AutoSize = true;
            this.rdbFuncoesWebService.Checked = true;
            this.rdbFuncoesWebService.Location = new System.Drawing.Point(10, 19);
            this.rdbFuncoesWebService.Name = "rdbFuncoesWebService";
            this.rdbFuncoesWebService.Size = new System.Drawing.Size(87, 17);
            this.rdbFuncoesWebService.TabIndex = 0;
            this.rdbFuncoesWebService.TabStop = true;
            this.rdbFuncoesWebService.Text = "Web Service";
            this.rdbFuncoesWebService.UseVisualStyleBackColor = true;
            // 
            // rdbFuncoesForm
            // 
            this.rdbFuncoesForm.AutoSize = true;
            this.rdbFuncoesForm.Location = new System.Drawing.Point(10, 42);
            this.rdbFuncoesForm.Name = "rdbFuncoesForm";
            this.rdbFuncoesForm.Size = new System.Drawing.Size(48, 17);
            this.rdbFuncoesForm.TabIndex = 1;
            this.rdbFuncoesForm.Text = "Form";
            this.rdbFuncoesForm.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdbREST);
            this.groupBox5.Controls.Add(this.rdbSOAP);
            this.groupBox5.Location = new System.Drawing.Point(122, 472);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(179, 47);
            this.groupBox5.TabIndex = 24;
            this.groupBox5.TabStop = false;
            // 
            // rdbREST
            // 
            this.rdbREST.AutoSize = true;
            this.rdbREST.Checked = true;
            this.rdbREST.Location = new System.Drawing.Point(11, 19);
            this.rdbREST.Name = "rdbREST";
            this.rdbREST.Size = new System.Drawing.Size(54, 17);
            this.rdbREST.TabIndex = 0;
            this.rdbREST.TabStop = true;
            this.rdbREST.Text = "REST";
            this.rdbREST.UseVisualStyleBackColor = true;
            this.rdbREST.Visible = false;
            // 
            // rdbSOAP
            // 
            this.rdbSOAP.AutoSize = true;
            this.rdbSOAP.Location = new System.Drawing.Point(108, 19);
            this.rdbSOAP.Name = "rdbSOAP";
            this.rdbSOAP.Size = new System.Drawing.Size(54, 17);
            this.rdbSOAP.TabIndex = 1;
            this.rdbSOAP.Text = "SOAP";
            this.rdbSOAP.UseVisualStyleBackColor = true;
            this.rdbSOAP.Visible = false;
            // 
            // chkValidacoesColuna
            // 
            this.chkValidacoesColuna.AutoSize = true;
            this.chkValidacoesColuna.Location = new System.Drawing.Point(123, 186);
            this.chkValidacoesColuna.Name = "chkValidacoesColuna";
            this.chkValidacoesColuna.Size = new System.Drawing.Size(262, 17);
            this.chkValidacoesColuna.TabIndex = 11;
            this.chkValidacoesColuna.Text = "Considerar NULL / Not NULL / Tamanho Varchar";
            this.chkValidacoesColuna.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sigla Inicial";
            // 
            // txtSiglaInicial
            // 
            this.txtSiglaInicial.Location = new System.Drawing.Point(122, 101);
            this.txtSiglaInicial.Name = "txtSiglaInicial";
            this.txtSiglaInicial.Size = new System.Drawing.Size(223, 20);
            this.txtSiglaInicial.TabIndex = 6;
            // 
            // chkStringConexao
            // 
            this.chkStringConexao.AutoSize = true;
            this.chkStringConexao.Location = new System.Drawing.Point(122, 44);
            this.chkStringConexao.Name = "chkStringConexao";
            this.chkStringConexao.Size = new System.Drawing.Size(179, 17);
            this.chkStringConexao.TabIndex = 2;
            this.chkStringConexao.Text = "Deve informar stirng de conexão";
            this.chkStringConexao.UseVisualStyleBackColor = true;
            // 
            // chkComunicadorSemNewInstance
            // 
            this.chkComunicadorSemNewInstance.AutoSize = true;
            this.chkComunicadorSemNewInstance.Location = new System.Drawing.Point(122, 451);
            this.chkComunicadorSemNewInstance.Name = "chkComunicadorSemNewInstance";
            this.chkComunicadorSemNewInstance.Size = new System.Drawing.Size(174, 17);
            this.chkComunicadorSemNewInstance.TabIndex = 23;
            this.chkComunicadorSemNewInstance.Text = "Comunicador sem newInstance";
            this.chkComunicadorSemNewInstance.UseVisualStyleBackColor = true;
            this.chkComunicadorSemNewInstance.Visible = false;
            // 
            // lblPacoteWebService
            // 
            this.lblPacoteWebService.AutoSize = true;
            this.lblPacoteWebService.Location = new System.Drawing.Point(50, 419);
            this.lblPacoteWebService.Name = "lblPacoteWebService";
            this.lblPacoteWebService.Size = new System.Drawing.Size(66, 26);
            this.lblPacoteWebService.TabIndex = 20;
            this.lblPacoteWebService.Text = "Pacote\r\nWebService";
            this.lblPacoteWebService.Visible = false;
            // 
            // txtPacoteWebService
            // 
            this.txtPacoteWebService.Location = new System.Drawing.Point(122, 425);
            this.txtPacoteWebService.Name = "txtPacoteWebService";
            this.txtPacoteWebService.Size = new System.Drawing.Size(222, 20);
            this.txtPacoteWebService.TabIndex = 22;
            this.txtPacoteWebService.Visible = false;
            // 
            // chkWebService
            // 
            this.chkWebService.AutoSize = true;
            this.chkWebService.Location = new System.Drawing.Point(122, 402);
            this.chkWebService.Name = "chkWebService";
            this.chkWebService.Size = new System.Drawing.Size(117, 17);
            this.chkWebService.TabIndex = 21;
            this.chkWebService.Text = "Gerar Web Service";
            this.chkWebService.UseVisualStyleBackColor = true;
            this.chkWebService.CheckedChanged += new System.EventHandler(this.chkWebService_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pacote";
            // 
            // txtPacote
            // 
            this.txtPacote.Location = new System.Drawing.Point(122, 75);
            this.txtPacote.Name = "txtPacote";
            this.txtPacote.Size = new System.Drawing.Size(223, 20);
            this.txtPacote.TabIndex = 4;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Enabled = false;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Image = global::GeradorCamadaCSharp.Properties.Resources.Cancel_Ret_32;
            this.btnCancelar.Location = new System.Drawing.Point(346, 484);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(35, 35);
            this.btnCancelar.TabIndex = 26;
            this.toolTip1.SetToolTip(this.btnCancelar, "Cancelar");
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnConfirmarProcesso
            // 
            this.btnConfirmarProcesso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarProcesso.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmarProcesso.Enabled = false;
            this.btnConfirmarProcesso.FlatAppearance.BorderSize = 0;
            this.btnConfirmarProcesso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarProcesso.Image = global::GeradorCamadaCSharp.Properties.Resources.Save_Ret_32;
            this.btnConfirmarProcesso.Location = new System.Drawing.Point(305, 484);
            this.btnConfirmarProcesso.Name = "btnConfirmarProcesso";
            this.btnConfirmarProcesso.Size = new System.Drawing.Size(35, 35);
            this.btnConfirmarProcesso.TabIndex = 25;
            this.toolTip1.SetToolTip(this.btnConfirmarProcesso, "Processar");
            this.btnConfirmarProcesso.UseVisualStyleBackColor = true;
            this.btnConfirmarProcesso.Click += new System.EventHandler(this.btnConfirmarProcesso_Click);
            // 
            // btnAtualizaDatabases
            // 
            this.btnAtualizaDatabases.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAtualizaDatabases.FlatAppearance.BorderSize = 0;
            this.btnAtualizaDatabases.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtualizaDatabases.Image = global::GeradorCamadaCSharp.Properties.Resources.Command_Refresh_24;
            this.btnAtualizaDatabases.Location = new System.Drawing.Point(356, 14);
            this.btnAtualizaDatabases.Name = "btnAtualizaDatabases";
            this.btnAtualizaDatabases.Size = new System.Drawing.Size(25, 25);
            this.btnAtualizaDatabases.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnAtualizaDatabases, "Atualizar banco de dados");
            this.btnAtualizaDatabases.UseVisualStyleBackColor = true;
            this.btnAtualizaDatabases.Click += new System.EventHandler(this.btnAtualizaDatabases_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(122, 16);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(223, 21);
            this.cmbDatabase.TabIndex = 0;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(746, 629);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPrincipal";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmCriaAtualiza_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabAtualiza.ResumeLayout(false);
            this.tabGeral.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTabelasAlteradas)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabAtualiza;
        private System.Windows.Forms.TabPage tabGeral;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkTodos;
        private System.Windows.Forms.DataGridView dgTabelasAlteradas;
        private System.Windows.Forms.DataGridViewCheckBoxColumn seleciona;
        private System.Windows.Forms.DataGridViewTextBoxColumn tabela;
        private System.Windows.Forms.DataGridViewTextBoxColumn arquivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordenar;
        private System.Windows.Forms.DataGridViewTextBoxColumn novaTabela;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnConfirmarProcesso;
        private System.Windows.Forms.Button btnAtualizaDatabases;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPacote;
        private System.Windows.Forms.CheckBox chkWebService;
        private System.Windows.Forms.Label lblPacoteWebService;
        private System.Windows.Forms.TextBox txtPacoteWebService;
        private System.Windows.Forms.CheckBox chkComunicadorSemNewInstance;
        private System.Windows.Forms.RadioButton rdbREST;
        private System.Windows.Forms.RadioButton rdbSOAP;
        private System.Windows.Forms.CheckBox chkStringConexao;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSiglaInicial;
        private System.Windows.Forms.CheckBox chkValidacoesColuna;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton rdbFuncoesWebService;
        private System.Windows.Forms.RadioButton rdbFuncoesForm;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rdbMySQL;
        private System.Windows.Forms.RadioButton rdbADO_Net;
        private System.Windows.Forms.CheckBox chkConsiderarRelacionamentos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFiltroList;
        private System.Windows.Forms.TextBox txtFiltroInfo;
        private System.Windows.Forms.CheckBox chkConsiderarRelacionamentosSetIdPorInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRemoveLetrasIniciais;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTabelasIniciam;
    }
}

