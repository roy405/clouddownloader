using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections;
using Microsoft.Win32;
using System.Diagnostics;

namespace CloudDownloader
{
    public partial class DownloadForm : Form
    {
        //Declaring global variables
        private string Url;
        private string fextension;
        private string fName;
        private long fSize;
        private string sPrefix;
        private string Slocation = getDownloadFolderPath() + "/";
        private string size;
        private bool existenceCheck;
        private string status;
        private int currDownloads;
        private int maxDownloads = 4;
        Stopwatch sw = new Stopwatch();
        Thread thread;

        public string url { get => Url; set => Url = value; }
        public long FSize { get => fSize; set => fSize = value; }
        public string FName { get => fName; set => fName = value; }

        public DownloadForm()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {    
            thread = new Thread(new ThreadStart(DownloadsInfo));
            if (currDownloads < maxDownloads)
            {
                ++currDownloads;

                //Inititating timer and starting the thread
                timer1.Start();
                thread.Start();
            }
            else
            {
                MessageBox.Show("You have reached the maximum number of downloads, Please wait to finish or about a download.");
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (existenceCheck)
            {

                string message = "Would you like to adort the current download?";

                var result = MessageBox.Show(message, "File name conflict: " + (@Slocation + fName + fextension), MessageBoxButtons.OKCancel);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    thread.Abort();
                    existenceCheck = false;
                    MessageBox.Show("Download Cancelled");
                }
                else
                {
                    //Exiting the method
                    return;
                }
            }
            else
            {
                MessageBox.Show("Currenly, you have no active downloads");
            }

        }

        private void DownloadsInfo() {
            Url = textBox1.Text;

           
            if (Url == null || Url == "")
            {
                MessageBox.Show("INVALID URL, PLEASE ENTER VALID URL");
            }

            else
            {
                
                int index = Url.LastIndexOf(".");

                
                fextension = ".";

                
                for (int x = 1; x < 4; x++)
                {
                    fextension = fextension + string.Concat(Url[index + x]);
                }

                
                if (fextension == ".exe" || fextension == ".jpg" || fextension == ".png" || fextension == ".mp3" || fextension == ".mp4" || fextension == ".ico")
                {
                    
                    index = Url.LastIndexOf("/");

                    
                    FName = "";

                   
                    for (int x = 1; x < 6; x++)
                    {
                       
                        if (Url[index + x] != '.')
                            FName = FName + string.Concat(Url[index + x]);
                        
                        else
                        {
                            FName = "Downloaded_";
                        }
                    }

                    try
                    {
                        
                        if (File.Exists(@Slocation + FName + fextension))
                        {

                            string message = "A file with the same name already exists, " + "Would you like to redownload it? ";

                            var result = MessageBox.Show(message, "File name conflict: " + (@Slocation + FName + fextension), MessageBoxButtons.OKCancel);

                            if (result == System.Windows.Forms.DialogResult.OK)
                            {
                                File.Delete((@Slocation + FName + fextension));
                            }
                            else
                            {
                                //exits the execution
                                return;
                            }
                        }
                    }
                    
                    catch (Exception)
                    {
                        MessageBox.Show("File is already being downloaded");
                    }

                    
                    existenceCheck = true;

                    try
                    {
                        using (WebClient Client = new WebClient())
                        {

                            status = "Downloading";

                            Client.DownloadFile(url, @Slocation + FName + fextension);

                           
                            Int64 fileSize = Convert.ToInt64(Client.ResponseHeaders["Content-Length"]);

                           
                            if (fileSize > 1099510579199)
                            {
                                fileSize = ((((fileSize / 1024) / 1024) / 1024) / 1024);
                                sPrefix = "TB";
                            }
                            if (fileSize > 1073740800)
                            {
                                fileSize = ((fileSize / 1024) / 1024) / 1024;
                                sPrefix = "GB";
                            }
                            if (fileSize > 1048575)
                            {
                                fileSize = (fileSize / 1024) / 1024;
                                sPrefix = "MB";
                            }
                            else if (fileSize > 1023)
                            {
                                fileSize = fileSize / 1024;
                                sPrefix = "KB";
                            }
                            else
                            {
                                sPrefix = "Bytes";
                            }

                            size = fileSize.ToString() + sPrefix;

                            status = "Completed";
                            MessageBox.Show("COMPLETED Download File: " + FName + fextension);
                        }
                    }
                    catch (Exception e)
                    {
                        
                     
                    }

                    
                    existenceCheck = false;

                    --currDownloads;

                    thread.Join();
                }
                else
                {
                    
                    MessageBox.Show("ERROR 404:  URL NOT SUPPORTED!");
                }
            }
        }

        //Used to handle the list view
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = listView1.SelectedItems; //A variable is created in order to store the list view items

            System.Diagnostics.Process.Start(@Slocation + FName + fextension);
        }

        private void BtnSaveLocation_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                Slocation = dialog.SelectedPath + "/";
        }

        //The users current directory is returned in this method
        static string getDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Timer to refresh the list view every 100ms
            if (thread != null)
            {
                
                if (existenceCheck == true) //Checking if the boolean which shows the existence of the file is true or not before updating
                {
                    //Appending data to the list view
                    String[] row = { FName, fextension, size, status, Slocation };//Adding data to the list
                    var listViewItem = new ListViewItem(row);

                    var downloadingListView = new ListViewItem("Downloading");

                    if (status == "Downloading")
                    {
                        label2.Text = "Downloading " + currDownloads + " Files";

                    }

                    if (status != "Downloading")
                    {
                        listView1.Items.Add(listViewItem);
                        timer1.Stop();
                    }
                }
            }

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}