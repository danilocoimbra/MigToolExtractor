using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using System.IO;


namespace SimpleZIPxTractor
{
    public partial class Form1 : Form
    {
        public string zipName = @"C:\temp\danilo.zip";
        public string strFolderDestination = @"C:\TEMP\Danilo\FastZip";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text) && Directory.Exists(textBox2.Text))
            {
                Thread thread = new Thread(new ThreadStart(DoWork));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Check the path!");
            }
        }

        public void DoWork()
        {
            button1.SafeInvoke(d => d.Enabled = false);
            initprogressbar(progressBar2, ZipContentsCount(zipName) - 1);
            UnZip(textBox1.Text, textBox2.Text);
            label1.SafeInvoke(d => d.Text = string.Empty);
            label2.SafeInvoke(d => d.Text = string.Empty);
            textBox4.SafeInvoke(d => d.Text = string.Empty);
            MessageBox.Show("Finished!");
            button1.SafeInvoke(d => d.Enabled = true);
        }

        public void UnZip(string zipName, string FolderDestination)
        {
            label1.SafeInvoke(d => d.Text = "Extracting files to folder " + FolderDestination + ". Wait!");
            try
            {
                FastZipEvents zipevents = new FastZipEvents();
                zipevents.ProcessFile = new ProcessFileHandler(ProcessFile);

                FastZip fz = new FastZip(zipevents);
                fz.CreateEmptyDirectories = true;
                fz.ExtractZip(zipName, FolderDestination, "");
            }
            catch { }

        }

        public int ZipContentsCount(string zipfile)
        {
            int count = 0;
            ZipInputStream strmZipInputStream = new ZipInputStream(File.OpenRead(zipfile));
            ZipEntry objEntry = strmZipInputStream.GetNextEntry();

            while (objEntry != null)
            {
                count++;
                objEntry = strmZipInputStream.GetNextEntry();
            }
            strmZipInputStream.Close();

            return count;
        }

        public void initprogressbar(ProgressBar progressBar, int cnt)
        {
            progressBar.SafeInvoke(d => d.Minimum = 0);
            progressBar.SafeInvoke(d => d.Maximum = cnt);
            progressBar.SafeInvoke(d => d.Value = 0);
        }

        public void changeprogressbar(ProgressBar progressBar, int stepcnt)
        {
            if ((progressBar.Value + stepcnt > progressBar.Minimum) && (progressBar.Value + stepcnt <= progressBar.Maximum))
            {
                progressBar.SafeInvoke(d => d.Value = progressBar.Value + stepcnt);
            }

        }

        public void ProcessFile(Object sender, ScanEventArgs e)
        {
            label2.SafeInvoke(d => d.Text = e.Name);
            changeprogressbar(progressBar2, 1);
            textBox4.SafeInvoke(d => d.Text = Convert.ToInt32(((Convert.ToDecimal(progressBar2.Value) / Convert.ToDecimal(progressBar2.Maximum)) * 100)).ToString() + "% concluído");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Zip File";
            theDialog.Filter = "ZIP files|*.zip";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.SafeInvoke(d => d.Text = theDialog.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            textBox2.SafeInvoke(d => d.Text = fbd.SelectedPath);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}