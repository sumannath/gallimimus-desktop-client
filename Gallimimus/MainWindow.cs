using Gallimimus.Models;
using Gallimimus.Models.Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace Gallimimus
{
    public partial class frmMainWindow : Form
    {
        int index = 0, dtGridHeight = 0;
        bool isHidden = true;

        public frmMainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            dtGridHeight = dtGridApps.Height;
            if (isHidden)
            {
                dtGridApps.Hide();
                Size = new Size(Size.Width, this.Size.Height - dtGridHeight);
            }

            BackgroundWorker bwApplicationLoad = new BackgroundWorker();
            bwApplicationLoad.WorkerReportsProgress = true;

            bwApplicationLoad.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;

                    try
                    {
                        ApplicationList appList = FetchApplicationList();
                        foreach (ApplicationInstall app in appList.Applications)
                        {
                            app.Status = "Waiting...";
                        }
                        args.Result = appList;
                    }
                    catch(WebException)
                    {
                        args.Cancel = true;
                        return;
                    }
                });

            bwApplicationLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                if (args.Cancelled)
                {
                    lblProgress.Text = "Cannot progress. Do you have a working internet connection?";
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    pbProgress.Value = 0;
                }
                else
                {
                    ApplicationList appList = (ApplicationList)args.Result;
                    dtGridApps.DataSource = appList.Applications;
                    dtGridApps.Refresh();
                    lblProgress.Text = "Initialize complete!";
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    pbProgress.Value = 0;

                    ProcessApplication(index);
                }
            });

            lblProgress.Text = string.Format("Initializing...");
            pbProgress.Style = ProgressBarStyle.Marquee;
            dtGridApps.AutoGenerateColumns = false;

            bwApplicationLoad.RunWorkerAsync();
        }

        private void ProcessApplication(int index)
        {
            List<ApplicationInstall> apps = (List<ApplicationInstall>) dtGridApps.DataSource;

            BackgroundWorker bwInstallerDownloader = new BackgroundWorker();
            BackgroundWorker bwInstallation = new BackgroundWorker();

            bwInstallerDownloader.WorkerReportsProgress = false;
            bwInstallation.WorkerReportsProgress = false;

            bwInstallerDownloader.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;

                    ApplicationInstall app = apps[index];
                    string installer = null;
                    try
                    {
                        installer = DownloadInstaller(app.DownloadLink);
                    }
                    catch(WebException)
                    {
                        args.Cancel = true;
                        return;
                    }
                    args.Result = installer;
                });

            bwInstallerDownloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                if(args.Cancelled)
                {
                    lblProgress.Text = "Cannot progress. Do you have a working internet connection?";
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    pbProgress.Value = 0;
                    apps[index].Status = "Error downloading installer";
                    dtGridApps.Refresh();
                }
                else
                {
                    lblProgress.Text = string.Format(String.Format("Running installer for {0}...", apps[index].Name));
                    apps[index].Status = "Installing...";
                    pbProgress.Style = ProgressBarStyle.Marquee;
                    dtGridApps.Refresh();

                    string installer = (string)args.Result;
                    bwInstallation.RunWorkerAsync(installer);
                }                
            });

            bwInstallation.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    string installerPath = (string) args.Argument;

                    ApplicationInstall app = apps[index];

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = installerPath;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.Arguments = apps[index].SilentInstallArgs;

                    try
                    {
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();
                            File.Delete(installerPath);
                        }
                    }
                    catch
                    {
                        // Log error.
                    }                    
                });

            bwInstallation.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                lblProgress.Text = string.Format(String.Format("Installed {0}", apps[index].Name));
                apps[index].Status = "Complete";
                pbProgress.Style = ProgressBarStyle.Continuous;
                pbProgress.Value = 0;
                dtGridApps.Refresh();

                if (++index < apps.Count)
                {
                    ProcessApplication(index);
                }
                else
                {
                    lblProgress.Text = string.Format(String.Format("Installation complete!"));
                    pbProgress.Style = ProgressBarStyle.Continuous;
                    pbProgress.Value = 0;
                    dtGridApps.Refresh();
                }
            });

            lblProgress.Text = string.Format(String.Format("Downloading installer for {0}...", apps[index].Name));
            apps[index].Status = "Downloading installer...";
            pbProgress.Style = ProgressBarStyle.Marquee;
            dtGridApps.Refresh();

            bwInstallerDownloader.RunWorkerAsync();
        }

        private string DownloadInstaller(string installerUrl)
        {
            var result = Downloader.Download(installerUrl, Path.GetTempPath(), Properties.Settings.Default.ParallelDownloads);

            Debug.WriteLine($"Location: {result.FilePath}");
            Debug.WriteLine($"Size: {result.Size}bytes");
            Debug.WriteLine($"Time taken: {result.TimeTaken.Milliseconds}ms");
            Debug.WriteLine($"Parallel: {result.ParallelDownloads}");

            return result.FilePath;
        }

        private ApplicationList FetchApplicationList()
        {
            GetApplicationJson();
            string json = File.ReadAllText("DataGal1.json");
            ApplicationList appList = JsonConvert.DeserializeObject<ApplicationList>(json);
            return appList;
        }

        private void GetApplicationJson()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("http://localhost/api/v1/gals/1", "DataGal1.json");
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblFeedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string mailto = string.Format("mailto:{0}?Subject={1}", Properties.Settings.Default.FeedbackEmail, Properties.Settings.Default.FeedbackSubject);
            mailto = Uri.EscapeUriString(mailto);
            Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
        }

        private void lblShowHide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (isHidden)
            {
                dtGridApps.Show();
                lblShowHide.Text = "Hide Details";
                Size = new Size(Size.Width, this.Size.Height + dtGridHeight);
                isHidden = false;
            }
            else
            {
                dtGridApps.Hide();
                lblShowHide.Text = "Show Details";
                Size = new Size(Size.Width, this.Size.Height - dtGridHeight);
                isHidden = true;
            }
        }
    }
}
