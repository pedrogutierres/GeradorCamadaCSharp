namespace GeradorCamadaCSharp
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
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFechar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(698, 50);
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
            this.btnFechar.Location = new System.Drawing.Point(657, 9);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(35, 35);
            this.btnFechar.TabIndex = 2;
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
            this.groupBox2.Size = new System.Drawing.Size(698, 339);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // tabAtualiza
            // 
            this.tabAtualiza.Controls.Add(this.tabGeral);
            this.tabAtualiza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAtualiza.Location = new System.Drawing.Point(3, 16);
            this.tabAtualiza.Name = "tabAtualiza";
            this.tabAtualiza.SelectedIndex = 0;
            this.tabAtualiza.Size = new System.Drawing.Size(692, 320);
            this.tabAtualiza.TabIndex = 0;
            // 
            // tabGeral
            // 
            this.tabGeral.Controls.Add(this.groupBox4);
            this.tabGeral.Controls.Add(this.groupBox3);
            this.tabGeral.Location = new System.Drawing.Point(4, 22);
            this.tabGeral.Name = "tabGeral";
            this.tabGeral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeral.Size = new System.Drawing.Size(684, 294);
            this.tabGeral.TabIndex = 0;
            this.tabGeral.Text = "Geral";
            this.tabGeral.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkTodos);
            this.groupBox4.Controls.Add(this.dgTabelasAlteradas);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox4.Location = new System.Drawing.Point(353, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(328, 288);
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
            this.dgTabelasAlteradas.Size = new System.Drawing.Size(322, 269);
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
            this.groupBox3.Size = new System.Drawing.Size(344, 288);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Pacote";
            // 
            // txtPacote
            // 
            this.txtPacote.Location = new System.Drawing.Point(79, 70);
            this.txtPacote.Name = "txtPacote";
            this.txtPacote.Size = new System.Drawing.Size(222, 20);
            this.txtPacote.TabIndex = 11;
            this.txtPacote.Text = "WebServiceSales";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Enabled = false;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Image = global::GeradorCamadaCSharp.Properties.Resources.Cancel_Ret_32;
            this.btnCancelar.Location = new System.Drawing.Point(297, 247);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(35, 35);
            this.btnCancelar.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnCancelar, "Cancelar");
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnConfirmarProcesso
            // 
            this.btnConfirmarProcesso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarProcesso.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmarProcesso.Enabled = false;
            this.btnConfirmarProcesso.FlatAppearance.BorderSize = 0;
            this.btnConfirmarProcesso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarProcesso.Image = global::GeradorCamadaCSharp.Properties.Resources.Save_Ret_32;
            this.btnConfirmarProcesso.Location = new System.Drawing.Point(256, 247);
            this.btnConfirmarProcesso.Name = "btnConfirmarProcesso";
            this.btnConfirmarProcesso.Size = new System.Drawing.Size(35, 35);
            this.btnConfirmarProcesso.TabIndex = 9;
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
            this.btnAtualizaDatabases.Location = new System.Drawing.Point(307, 25);
            this.btnAtualizaDatabases.Name = "btnAtualizaDatabases";
            this.btnAtualizaDatabases.Size = new System.Drawing.Size(25, 25);
            this.btnAtualizaDatabases.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnAtualizaDatabases, "Atualizar banco de dados");
            this.btnAtualizaDatabases.UseVisualStyleBackColor = true;
            this.btnAtualizaDatabases.Click += new System.EventHandler(this.btnAtualizaDatabases_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(79, 29);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(222, 21);
            this.cmbDatabase.TabIndex = 6;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(698, 389);
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
    }
}

