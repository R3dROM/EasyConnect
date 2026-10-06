using EasyConnect.Controllers;
using EasyConnect.Handler;
using System.Diagnostics;

namespace EasyConnect
{
    public partial class LauncherView : Form
    {
        private readonly EasyLinkView _easyLinkView;
        private readonly LauncherController _launcherController;
        private readonly UiHandler _UiHandler;

        public LauncherView(
            LauncherController _launcherController,
            UiHandler _UiHandler,
            EasyLinkView _easyLinkView
            )
        {
            InitializeComponent();
            this._easyLinkView = _easyLinkView;
            this._launcherController = _launcherController;
            this._UiHandler = _UiHandler;

            _UiHandler.ListBoxLogs = listBoxStartingLogs;
            listBoxStartingLogs.DrawItem += _UiHandler.ListBoxLogs_DrawItem!;
        }

        private void buttonCaddyPath_Click(object sender, EventArgs e)
        {
            var selectedPath = _UiHandler.FolderBrowser();
            caddyPath.Text = selectedPath;
        }
        private void buttonManifestPath_Click(object sender, EventArgs e)
        {
            var selectedPath = _UiHandler.FolderBrowser();
            scriptsPath.Text = selectedPath;
        }
        private async void buttonContinue_Click(object sender, EventArgs e)
        {
            try
            {
                await _launcherController.StartEasyLink(
                    progressBarInitializer,
                    listBoxStartingLogs,
                    caddyPath.Text,
                    scriptsPath.Text);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR SETTING UP THE SERVICES!! {ex}");
                _launcherController.CurrentState = LauncherState.Failed;
            }
            finally
            {
                if (_launcherController.CurrentState == LauncherState.Initialized)
                {
                    _easyLinkView.Show();
                    Hide();
                    _easyLinkView.FormClosed += (s, args) => Close();
                }
            }
        }

        private void LauncherView_Load(object sender, EventArgs e)
        {
            var paths = _launcherController.StartPaths();
            if (paths == null) return;

            caddyPath.Text = paths.Value.caddyPath;
            scriptsPath.Text = paths.Value.scriptsPath;
        }
    }
}
