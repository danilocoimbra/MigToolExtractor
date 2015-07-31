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

            _totalFileCount = 41;//FolderContentsCount(@"C:\Temp\Danilo\fastzip");

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
            UnZip(@"C:\temp\danilo.zip", @"C:\TEMP\Danilo\FastZip");

            //Thread myThread = new Thread(new ThreadStart(WorkThreadFunction));
            //myThread.Start();
            //myThread.Join();

        }

        public void WorkThreadFunction()
        {
            UnZip(@"C:\temp\danilo.zip", @"C:\TEMP\Danilo\FastZip");
        }

        public void UnZip(string zipName, string FolderDestination)
        {
            textBox1.Text = "Efetuando restore de " + FolderDestination + ". Aguarde!";
            textBox1.Update();

            try
            {
                int count = ZipContentsCount(zipName);

                textBox2.Text = count.ToString();
                textBox2.Update();

                initprogressbar(progressBar2, count);

                FastZipEvents zipevents = new FastZipEvents();
                //zipevents.ProcessDirectory = new ProcessDirectoryHandler(ProcessDirectory);
                zipevents.ProcessFile = new ProcessFileHandler(ProcessFile);

                //FastZip.ConfirmOverwriteDelegate objDelegate;
                //objDelegate = AddressOf NotifyClient

                FastZip fz = new FastZip(zipevents);
                fz.CreateEmptyDirectories = true;
                fz.ExtractZip(zipName, FolderDestination, "");
            }
            catch { }

        }

        private int ZipContentsCount(string zipfile)
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

        private void initprogressbar(ProgressBar progressBar, int cnt)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = cnt;
            progressBar.Value = 0;
        }

        private void changeprogressbar(int stepcnt)
        {
            progressBar2.Value = progressBar2.Value + stepcnt;
        }

        private void ProcessFile(Object sender, ScanEventArgs e)
        {
            textBox2.Text = e.Name;
            textBox2.Update();

            changeprogressbar(1);

            textBox3.Text = ((progressBar2.Value / progressBar2.Maximum) * 100) + "% concluído";
            textBox3.Update();
        }


        #region Backup
        int filecounts = 0;


        //Public Sub UnZip(ByVal zipName As String, ByVal FolderDestination As String)
        //    TextBox1.Text = "Efetuando restore de " & FolderDestination & ". Aguarde!"
        //    TextBox1.Update()
        //    Try
        //        Dim count As Integer = ZipContentsCount(zipName)
        //        initprogressbar(ProgressBar1, count)

        //        Dim zipevents As FastZipEvents = New FastZipEvents()
        //        zipevents.ProcessDirectory = New ProcessDirectoryHandler(AddressOf ProcessDirectory)
        //        zipevents.ProcessFile = New ProcessFileHandler(AddressOf ProcessFile)

        //        Dim objDelegate As FastZip.ConfirmOverwriteDelegate
        //        objDelegate = AddressOf NotifyClient

        //        Dim fz As FastZip = New FastZip(zipevents)
        //        fz.CreateEmptyDirectories = True
        //        fz.ExtractZip(zipName, FolderDestination, "")
        //    Catch e As Exception
        //        MsgBox(e.Message)
        //        Exit Sub
        //    End Try
        //    TextBox1.Text = ""
        //    TextBox1.Update()
        //    ProgressBar1.Value = 0
        //End Sub

        //Friend Sub initprogressbar(ByVal ProgressBar As ProgressBar, ByVal cnt As Integer)
        //    If Not ProgressBar1 Is Nothing Then
        //        ProgressBar1 = ProgressBar
        //        ProgressBar1.Minimum = 0
        //        ProgressBar1.Maximum = cnt
        //        ProgressBar1.Value = 0
        //    End If
        //End Sub

        //Friend Sub changeprogressbar(ByVal stepcnt As Integer)
        //    If Not ProgressBar1 Is Nothing Then
        //        'If ProgressBar1.Value < cnt Then
        //        ProgressBar1.Value = ProgressBar1.Value + stepcnt
        //        'End If
        //    End If
        //End Sub

        //Friend Sub ProcessFile(ByVal sender As Object, ByVal e As ScanEventArgs)
        //    TextBox2.Text = e.Name
        //    TextBox2.Update()
        //    changeprogressbar(1)
        //    TextBox3.Text = Int((ProgressBar1.Value / ProgressBar1.Maximum) * 100) & "% concluído"
        //    TextBox3.Update()
        //End Sub

        //Friend Sub ProcessDirectory(ByVal sender As Object, ByVal e As DirectoryEventArgs)
        //    If (e.HasMatchingFiles) Then
        //        'changeprogressbar(1)
        //    End If
        //End Sub

        //Friend Sub DirectoryFailure(ByVal sender As Object, ByVal e As ScanFailureEventArgs)
        //    Dim logFile As String = bkpFolder & "\BACKUPD.log"
        //    Dim objLogFile As StreamWriter = New StreamWriter(logFile, True)
        //    objLogFile.WriteLine(Now & " - " & e.Name & " - " & e.Exception.Message)
        //    objLogFile.Close()
        //    'changeprogressbar(1)
        //    e.ContinueRunning = True
        //End Sub

        //Friend Sub FileFailure(ByVal sender As Object, ByVal e As ScanFailureEventArgs)
        //    Dim logFile As String = bkpFolder & "\BACKUPD.log"
        //    Dim objLogFile As StreamWriter = New StreamWriter(logFile, True)
        //    objLogFile.WriteLine(Now & " - " & e.Name & " - " & e.Exception.Message)
        //    objLogFile.Close()
        //    'changeprogressbar(1)
        //    e.ContinueRunning = True
        //End Sub

        //Friend Function NotifyClient(ByVal filename As String) As Boolean
        //    Dim response As MsgBoxResult
        //    response = MsgBox("Replace the file " + filename, MsgBoxStyle.YesNo, "Replace File")
        //    'changeprogressbar(1)
        //    If response = MsgBoxResult.Yes Then
        //        Return True
        //    Else
        //        Return False
        //    End If
        //End Function

        //Friend Function ZipContentsCount(ByVal zipfile As String) As Integer
        //    Dim count As Integer
        //    Dim strmZipInputStream As ZipInputStream = New ZipInputStream(File.OpenRead(zipfile))
        //    Dim objEntry As ZipEntry

        //    objEntry = strmZipInputStream.GetNextEntry()
        //    count = 0
        //    While IsNothing(objEntry) = False
        //        count = count + 1
        //        objEntry = strmZipInputStream.GetNextEntry()
        //    End While
        //    strmZipInputStream.Close()

        //    Return count
        //End Function

        //'Private filecounts As Integer = 0
        //Friend Function FolderContentsCount(ByVal targetdir As String) As Integer
        //    Try
        //        Dim filestruc() As String = Directory.GetFiles(targetdir)
        //        filecounts = filecounts + filestruc.Length

        //        Dim dirstruc() As String = Directory.GetDirectories(targetdir)
        //        For Each nextdir As String In dirstruc
        //            FolderContentsCount(nextdir.ToString)
        //        Next

        //        FolderContentsCount = filecounts
        //        Return FolderContentsCount
        //    Catch ex As Exception
        //        'MsgBox(ex.Message)
        //    End Try
        //End Function
        #endregion;








    }
}