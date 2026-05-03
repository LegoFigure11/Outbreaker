namespace Outbreaker.WinForms;

public static class ErrorHandler
{
    public static void DisplayMessageBox(this Form form, string msg, string caption = "")
    {
        caption = caption.Length == 0 ? "Outbreaker Error" : caption;
        if (form.InvokeRequired)
            form.Invoke(() => MessageBox.Show(msg, caption, MessageBoxButtons.OK));
        else
            MessageBox.Show(msg, caption, MessageBoxButtons.OK);
    }
}
