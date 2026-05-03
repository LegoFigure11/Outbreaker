namespace Outbreaker.WinForms;

public static class WinFormsUtil
{
    internal static string GetText(this Control c) => c.InvokeRequired ? c.Invoke(() => c.Text) : c.Text;
    internal static bool GetIsChecked(this CheckBox cb) => cb.InvokeRequired ? cb.Invoke(() => cb.Checked) : cb.Checked;
}
