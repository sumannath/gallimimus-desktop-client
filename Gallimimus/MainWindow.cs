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
                            ApplicationVersion version = GetPlatformVersion(app);
                            app.Version = version.Version;
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
                    ApplicationVersion version = null;
                    try
                    {
                        version = GetPlatformVersion(apps, index);
                        installer = DownloadInstaller(version.DownloadLink);
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
                    ApplicationVersion version = GetPlatformVersion(apps, index);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = installerPath;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.Arguments = version.SilentInstallArgs;

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

        private ApplicationVersion GetPlatformVersion(List<ApplicationInstall> apps, int index)
        {
            ApplicationInstall app = apps[index];
            return GetPlatformVersion(app);
        }

        private ApplicationVersion GetPlatformVersion(ApplicationInstall app)
        {
            if (app.VersionCount == 1)
            {
                return app.Versions[0];
            }
            else
            {
                foreach (ApplicationVersion version in app.Versions)
                {
                    if (version.Architechture == "x64" && Environment.Is64BitOperatingSystem)
                    {
                        return version;
                    }
                    if (version.Architechture == "x86" && !Environment.Is64BitOperatingSystem)
                    {
                        return version;
                    }
                }
            }
            return null;
        }

        private string DownloadInstaller(string installerUrl)
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

            var result = Downloader.Download(installerUrl, strWorkPath, Properties.Settings.Default.ParallelDownloads);

            Debug.WriteLine($"Location: {result.FilePath}");
            Debug.WriteLine($"Size: {result.Size}bytes");
            Debug.WriteLine($"Time taken: {result.TimeTaken.Milliseconds}ms");
            Debug.WriteLine($"Parallel: {result.ParallelDownloads}");

            return result.FilePath;
        }

        private ApplicationList FetchApplicationList()
        {
            string fileName = GetApplicationJson();
            string json = File.ReadAllText(fileName);
            ApplicationList appList = JsonConvert.DeserializeObject<ApplicationList>(json);
            return appList;
        }

        private string GetApplicationJson()
        {
            string json = File.ReadAllText("appsettings.json");
            string galFileName = String.Empty;
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(json);
            using (var client = new WebClient())
            {
                int unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                galFileName = String.Format("GalData{0}.json", unixTimestamp);
                client.DownloadFile(settings.Url, galFileName);
            }
            return galFileName;
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
