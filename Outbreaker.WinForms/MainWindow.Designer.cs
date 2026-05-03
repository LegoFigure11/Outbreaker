namespace Outbreaker.WinForms;

    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        GB_Connection = new GroupBox();
        TB_Status = new TextBox();
        L_Status = new Label();
        B_Disconnect = new Button();
        B_Connect = new Button();
        L_SwitchIP = new Label();
        TB_SwitchIP = new TextBox();
        GB_SAVInfo = new GroupBox();
        L_Theme = new Label();
        CB_Theme = new ComboBox();
        B_SearchOutbreaks = new Button();
        TT_PinToTop = new ToolTip(components);
        CB_Species = new ComboBox();
        L_Species = new Label();
        CB_Paldea = new CheckBox();
        CB_Kitakami = new CheckBox();
        CB_Blueberry = new CheckBox();
        CB_BlueberryBCAT = new CheckBox();
        CB_KitakamiBCAT = new CheckBox();
        CB_PaldeaBCAT = new CheckBox();
        GB_Connection.SuspendLayout();
        GB_SAVInfo.SuspendLayout();
        SuspendLayout();
        // 
        // GB_Connection
        // 
        GB_Connection.Controls.Add(TB_Status);
        GB_Connection.Controls.Add(L_Status);
        GB_Connection.Controls.Add(B_Disconnect);
        GB_Connection.Controls.Add(B_Connect);
        GB_Connection.Controls.Add(L_SwitchIP);
        GB_Connection.Controls.Add(TB_SwitchIP);
        GB_Connection.Location = new Point(0, -8);
        GB_Connection.Margin = new Padding(3, 0, 3, 3);
        GB_Connection.Name = "GB_Connection";
        GB_Connection.RightToLeft = RightToLeft.No;
        GB_Connection.Size = new Size(212, 83);
        GB_Connection.TabIndex = 2;
        GB_Connection.TabStop = false;
        // 
        // TB_Status
        // 
        TB_Status.BackColor = SystemColors.Control;
        TB_Status.BorderStyle = BorderStyle.None;
        TB_Status.Location = new Point(74, 64);
        TB_Status.Name = "TB_Status";
        TB_Status.ReadOnly = true;
        TB_Status.RightToLeft = RightToLeft.No;
        TB_Status.Size = new Size(132, 16);
        TB_Status.TabIndex = 18;
        TB_Status.TabStop = false;
        TB_Status.Text = "wwwwwwwwwwwwww";
        TB_Status.TextAlign = HorizontalAlignment.Right;
        // 
        // L_Status
        // 
        L_Status.AutoSize = true;
        L_Status.Location = new Point(11, 64);
        L_Status.Name = "L_Status";
        L_Status.Size = new Size(42, 15);
        L_Status.TabIndex = 17;
        L_Status.Text = "Status:";
        // 
        // B_Disconnect
        // 
        B_Disconnect.Enabled = false;
        B_Disconnect.Location = new Point(109, 36);
        B_Disconnect.Name = "B_Disconnect";
        B_Disconnect.Size = new Size(97, 25);
        B_Disconnect.TabIndex = 2;
        B_Disconnect.Text = "Disconnect";
        B_Disconnect.UseVisualStyleBackColor = true;
        B_Disconnect.Click += B_Disconnect_Click;
        // 
        // B_Connect
        // 
        B_Connect.Location = new Point(11, 36);
        B_Connect.Name = "B_Connect";
        B_Connect.Size = new Size(97, 25);
        B_Connect.TabIndex = 1;
        B_Connect.Text = "Connect";
        B_Connect.UseVisualStyleBackColor = true;
        B_Connect.Click += B_Connect_Click;
        // 
        // L_SwitchIP
        // 
        L_SwitchIP.AutoSize = true;
        L_SwitchIP.Location = new Point(11, 14);
        L_SwitchIP.Name = "L_SwitchIP";
        L_SwitchIP.Size = new Size(58, 15);
        L_SwitchIP.TabIndex = 12;
        L_SwitchIP.Text = "Switch IP:";
        // 
        // TB_SwitchIP
        // 
        TB_SwitchIP.CharacterCasing = CharacterCasing.Lower;
        TB_SwitchIP.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        TB_SwitchIP.Location = new Point(95, 12);
        TB_SwitchIP.MaxLength = 15;
        TB_SwitchIP.Name = "TB_SwitchIP";
        TB_SwitchIP.Size = new Size(111, 22);
        TB_SwitchIP.TabIndex = 0;
        TB_SwitchIP.Text = "123.123.123.123";
        TB_SwitchIP.TextChanged += TB_SwitchIP_TextChanged;
        // 
        // GB_SAVInfo
        // 
        GB_SAVInfo.Controls.Add(L_Theme);
        GB_SAVInfo.Controls.Add(CB_Theme);
        GB_SAVInfo.Location = new Point(0, 65);
        GB_SAVInfo.Name = "GB_SAVInfo";
        GB_SAVInfo.Size = new Size(212, 48);
        GB_SAVInfo.TabIndex = 4;
        GB_SAVInfo.TabStop = false;
        // 
        // L_Theme
        // 
        L_Theme.AutoSize = true;
        L_Theme.Location = new Point(12, 19);
        L_Theme.Name = "L_Theme";
        L_Theme.Size = new Size(46, 15);
        L_Theme.TabIndex = 179;
        L_Theme.Text = "Theme:";
        // 
        // CB_Theme
        // 
        CB_Theme.FormattingEnabled = true;
        CB_Theme.Items.AddRange(new object[] { "Light", "System", "Dark" });
        CB_Theme.Location = new Point(95, 16);
        CB_Theme.Name = "CB_Theme";
        CB_Theme.Size = new Size(111, 23);
        CB_Theme.TabIndex = 178;
        CB_Theme.SelectedIndexChanged += CB_Theme_SelectedIndexChanged;
        // 
        // B_SearchOutbreaks
        // 
        B_SearchOutbreaks.Location = new Point(218, 81);
        B_SearchOutbreaks.Name = "B_SearchOutbreaks";
        B_SearchOutbreaks.Size = new Size(240, 25);
        B_SearchOutbreaks.TabIndex = 7;
        B_SearchOutbreaks.Text = "Search Outbreaks";
        B_SearchOutbreaks.UseVisualStyleBackColor = true;
        B_SearchOutbreaks.Click += B_SearchOutbreaks_Click;
        // 
        // CB_Species
        // 
        CB_Species.FormattingEnabled = true;
        CB_Species.Items.AddRange(new object[] { "Light", "System", "Dark" });
        CB_Species.Location = new Point(315, 3);
        CB_Species.Name = "CB_Species";
        CB_Species.Size = new Size(143, 23);
        CB_Species.TabIndex = 180;
        // 
        // L_Species
        // 
        L_Species.AutoSize = true;
        L_Species.Location = new Point(218, 6);
        L_Species.Name = "L_Species";
        L_Species.Size = new Size(91, 15);
        L_Species.TabIndex = 181;
        L_Species.Text = "Desired Species:";
        // 
        // CB_Paldea
        // 
        CB_Paldea.AutoSize = true;
        CB_Paldea.Checked = true;
        CB_Paldea.CheckState = CheckState.Checked;
        CB_Paldea.Location = new Point(218, 26);
        CB_Paldea.Name = "CB_Paldea";
        CB_Paldea.Size = new Size(61, 19);
        CB_Paldea.TabIndex = 182;
        CB_Paldea.Text = "Paldea";
        CB_Paldea.UseVisualStyleBackColor = true;
        // 
        // CB_Kitakami
        // 
        CB_Kitakami.AutoSize = true;
        CB_Kitakami.Checked = true;
        CB_Kitakami.CheckState = CheckState.Checked;
        CB_Kitakami.Location = new Point(218, 44);
        CB_Kitakami.Name = "CB_Kitakami";
        CB_Kitakami.Size = new Size(72, 19);
        CB_Kitakami.TabIndex = 183;
        CB_Kitakami.Text = "Kitakami";
        CB_Kitakami.UseVisualStyleBackColor = true;
        // 
        // CB_Blueberry
        // 
        CB_Blueberry.AutoSize = true;
        CB_Blueberry.Checked = true;
        CB_Blueberry.CheckState = CheckState.Checked;
        CB_Blueberry.Location = new Point(218, 62);
        CB_Blueberry.Name = "CB_Blueberry";
        CB_Blueberry.Size = new Size(76, 19);
        CB_Blueberry.TabIndex = 184;
        CB_Blueberry.Text = "Blueberry";
        CB_Blueberry.UseVisualStyleBackColor = true;
        // 
        // CB_BlueberryBCAT
        // 
        CB_BlueberryBCAT.AutoSize = true;
        CB_BlueberryBCAT.Location = new Point(315, 62);
        CB_BlueberryBCAT.Name = "CB_BlueberryBCAT";
        CB_BlueberryBCAT.Size = new Size(107, 19);
        CB_BlueberryBCAT.TabIndex = 187;
        CB_BlueberryBCAT.Text = "Blueberry BCAT";
        CB_BlueberryBCAT.UseVisualStyleBackColor = true;
        // 
        // CB_KitakamiBCAT
        // 
        CB_KitakamiBCAT.AutoSize = true;
        CB_KitakamiBCAT.Location = new Point(315, 44);
        CB_KitakamiBCAT.Name = "CB_KitakamiBCAT";
        CB_KitakamiBCAT.Size = new Size(103, 19);
        CB_KitakamiBCAT.TabIndex = 186;
        CB_KitakamiBCAT.Text = "Kitakami BCAT";
        CB_KitakamiBCAT.UseVisualStyleBackColor = true;
        // 
        // CB_PaldeaBCAT
        // 
        CB_PaldeaBCAT.AutoSize = true;
        CB_PaldeaBCAT.Location = new Point(315, 26);
        CB_PaldeaBCAT.Name = "CB_PaldeaBCAT";
        CB_PaldeaBCAT.Size = new Size(92, 19);
        CB_PaldeaBCAT.TabIndex = 185;
        CB_PaldeaBCAT.Text = "Paldea BCAT";
        CB_PaldeaBCAT.UseVisualStyleBackColor = true;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(470, 118);
        Controls.Add(CB_BlueberryBCAT);
        Controls.Add(CB_KitakamiBCAT);
        Controls.Add(CB_PaldeaBCAT);
        Controls.Add(CB_Blueberry);
        Controls.Add(CB_Kitakami);
        Controls.Add(CB_Paldea);
        Controls.Add(L_Species);
        Controls.Add(CB_Species);
        Controls.Add(B_SearchOutbreaks);
        Controls.Add(GB_Connection);
        Controls.Add(GB_SAVInfo);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "MainWindow";
        FormClosing += MainWindow_FormClosing;
        Load += MainWindow_Load;
        GB_Connection.ResumeLayout(false);
        GB_Connection.PerformLayout();
        GB_SAVInfo.ResumeLayout(false);
        GB_SAVInfo.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox GB_Connection;
    private TextBox TB_Status;
    private Label L_Status;
    private Button B_Disconnect;
    private Button B_Connect;
    private Label L_SwitchIP;
    private TextBox TB_SwitchIP;
    private GroupBox GB_SAVInfo;
    private Button B_SearchOutbreaks;
    private Label L_Theme;
    private ComboBox CB_Theme;
    private ToolTip TT_PinToTop;
    private ComboBox CB_Species;
    private Label L_Species;
    private CheckBox CB_Paldea;
    private CheckBox CB_Kitakami;
    private CheckBox CB_Blueberry;
    private CheckBox CB_BlueberryBCAT;
    private CheckBox CB_KitakamiBCAT;
    private CheckBox CB_PaldeaBCAT;
}

