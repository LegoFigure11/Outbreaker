namespace Outbreaker.WinForms.Subforms;

public partial class UpdateNotifPopup : Form
{
    private readonly Version cv;
    private readonly Version nv;
    private readonly bool topmost;
    public UpdateNotifPopup(Version currentVersion, Version newVersion, bool topMost = false)
    {
        cv = currentVersion;
        nv = newVersion;
        topmost = topMost;
        InitializeComponent();
    }

    private void UpdateNotifPopup_Load(object sender, EventArgs e)
    {
        L_Version.Text = $"Current: v{cv.Major}.{cv.Minor}.{cv.Build} | New: v{nv.Major}.{nv.Minor}.{nv.Build}";
        B_Download.Focus();
        CenterToScreen();
        TopMost = topmost;
    }
}
