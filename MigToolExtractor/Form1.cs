using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using System.IO;

namespace MigToolExtractor
{
    public partial class Form1 : Form
    {
        private int _uptoFileCount;
        private int _totalFileCount;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        public void TestFastZipUnpack(string zipFileName, string targetDir)
        {

            _totalFileCount = 1000;//FolderContentsCount(@"C:\Temp\Danilo\fastzip");

            FastZipEvents events = new FastZipEvents();
            events.ProcessFile = ProcessFileMethod;
            FastZip fastZip = new FastZip(events);

            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);
        }


        private void ProcessFileMethod(object sender, ScanEventArgs args)
        {
            _uptoFileCount++;
            int percentCompleted = _uptoFileCount * 100 / _totalFileCount;
            label1.Text = percentCompleted.ToString();
            label1.Update();

            // do something here with a progress bar
            // file counts are easier as sizes take more work to calculate, and compression levels vary by file type

            string fileName = args.Name;
            // To terminate the process, set args.ContinueRunning = false
            if (fileName == "stop on this file")
                args.ContinueRunning = false;
        }

        // Returns the number of files in this and all subdirectories
        private int FolderContentsCount(string path)
        {
            int result = Directory.GetFiles(path).Length;
            string[] subFolders = Directory.GetDirectories(path);
            foreach (string subFolder in subFolders)
            {
                result += FolderContentsCount(subFolder);
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestFastZipUnpack(@"C:\temp\danilo.zip", @"C:\temp\danilo\fastzip");
        }


    }
}
