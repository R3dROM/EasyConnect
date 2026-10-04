using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Progress;
using EasyConnect.Models.Status;

namespace EasyConnect.Handler
{
    public class UiHandler
    {
        private ListBox? _listBoxLogs;
        public ListBox? ListBoxLogs
        {
            get => _listBoxLogs;
            set => _listBoxLogs = value;
        }
        private readonly OpenFileDialog _openFileDialog = new();
        private readonly FolderBrowserDialog _folderBrowserDialog = new();
        public string? FolderBrowser()
        {
            DialogResult dialogResult = _folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return null;
            return string.IsNullOrEmpty(_folderBrowserDialog.SelectedPath)
                ? null : _folderBrowserDialog.SelectedPath;
        }
        public void FileBrowser(Action<string?> assing)
        {
            DialogResult dialogResult = _openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedPath = _openFileDialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    assing(selectedPath);
                }
            }
        }
        public void ListBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || _listBoxLogs == null) return;

            e.DrawBackground();

            if (_listBoxLogs.Items[e.Index] is ProgressStatus<ProgressStatusStage> status)
            {
                // Selección
                Color bgColor = (e.State & DrawItemState.Selected) != 0
                    ? SystemColors.Highlight
                    : _listBoxLogs.BackColor;

                Color fgColor = status.IsCompleted
                    ? Color.DarkGreen
                    : status.Percent < 100
                        ? Color.Black
                        : Color.Black;

                using (var bgBrush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);

                using var fgBrush = new SolidBrush(fgColor);
                Font font = status.IsCompleted
                    ? new Font(e.Font!, FontStyle.Regular)
                    : e.Font!;
                e.Graphics.DrawString(status.ToString(), font, fgBrush, e.Bounds.X + 2, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }
    }
}
