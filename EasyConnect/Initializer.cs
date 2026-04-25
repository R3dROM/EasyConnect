using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EasyConnect.Services;
using EasyConnect.Controllers;
using System.Diagnostics;
using EasyConnect.Models;

namespace EasyConnect
{
    enum States
    {
        NotInitialized,
        Running,
        Initialized
    }
    public partial class Initializer : Form
    {
        private States _currentState { get; set; }

        private readonly WINDOW _window;
        private readonly FolderBrowserDialog folderBrowserDialog;
        private readonly NetworkService _networkService;
        private readonly AppManager _appInitializer;
        private OpenFileDialog _openFileDialog = new();
        private string _mdmFilePath = string.Empty;
        public Initializer(
            NetworkService _networkService,
            NetworkConfigurationService _networkConfigurationService,
            HttpController _httpController,
            AppManager _appInitializer,
            WINDOW _window
            )
        {
            InitializeComponent();
            this._window = _window;
            this._networkService = _networkService;
            this._appInitializer = _appInitializer;

            folderBrowserDialog = new()
            {
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.Desktop
            };
            _currentState = States.NotInitialized;
            listBoxStartingLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxStartingLogs.ItemHeight = 20;
            listBoxStartingLogs.DrawItem += ListBoxStartingLogs_DrawItem!;

        }
        private void ListBoxStartingLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            if (listBoxStartingLogs.Items[e.Index] is ProgressStatus status)
            {
                // Selección
                Color bgColor = (e.State & DrawItemState.Selected) != 0
                    ? SystemColors.Highlight
                    : listBoxStartingLogs.BackColor;

                Color fgColor = status.IsCompleted
                    ? Color.DarkGreen
                    : status.Percent < 100
                        ? Color.Black
                        : Color.Black;

                using (var bgBrush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);

                using var fgBrush = new SolidBrush(fgColor);
                Font font = status.IsCompleted
                    ? new Font(e.Font!, FontStyle.Bold)
                    : e.Font!;
                e.Graphics.DrawString(status.ToString(), font, fgBrush, e.Bounds.X + 2, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }
        private void FolderBrowser(Action<string?> assing)
        {
            if (_currentState != States.NotInitialized)
                return;
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedPath = folderBrowserDialog.SelectedPath;
                if (selectedPath != null)
                {
                    assing(selectedPath);
                }
            }
        }
        private void buttonCaddyPath_Click(object sender, EventArgs e)
        {
            FolderBrowser(selectedPath =>
            {
                _networkService.CaddyPath = selectedPath!;
                caddyPath.Text = selectedPath;
            });
        }

        private void buttonManifestPath_Click(object sender, EventArgs e)
        {
            FolderBrowser(selectedPath =>
            {
                _networkService.ManifestScriptsPath = selectedPath!;
                manifestPath.Text = selectedPath;
            });
        }

        private void buttonDeployPath_Click(object sender, EventArgs e)
        {
            FolderBrowser(selectedPath =>
            {
                _networkService.DeployPath = selectedPath!;
                deployPath.Text = selectedPath;
            });
        }
        private async void buttonContinue_Click(object sender, EventArgs e)
        {
            try
            {
                var progress = new Progress<ProgressStatus>(p =>
                {
                    if (p.Percent >= 0)
                    {
                        progressBarInitializer.Value = Math.Max(
                            progressBarInitializer.Minimum,
                            Math.Min(progressBarInitializer.Maximum, p.Percent)
                        );
                    }

                    if (p.Stage != null)
                    {
                        listBoxStartingLogs.Items.Add(p);
                        listBoxStartingLogs.TopIndex = listBoxStartingLogs.Items.Count - 1;
                    }
                });
                if (_currentState == States.NotInitialized)
                {
                    _currentState = States.Running;
                    await _appInitializer.StartAsync(progress);
                    progressBarInitializer.Value = 100;
                    _currentState = States.Initialized;
                    await Task.Delay(3000);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (_currentState == States.Initialized)
                {
                    _window.Show();
                    this.Hide();
                    _window.FormClosed += (s, args) => this.Close();
                }
            }
        }

        private async void buttonMdmFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentState == States.NotInitialized)
                {
                    if (_openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _mdmFilePath = _openFileDialog.FileName;
                        if (!string.IsNullOrEmpty(_mdmFilePath))
                        {
                            devicesList.Clear();
                            devicesList.Text = _mdmFilePath;
                            _networkService.DeviceListPath = _mdmFilePath;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
