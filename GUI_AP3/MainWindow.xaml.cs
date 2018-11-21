using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Diagnostics;
using System.Net.Mail;
using System.ComponentModel;

using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Collections;

namespace GUI_AP3
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool bIfAP3Running=false;
        private delegate void ChangeStatusBar(String arg);
        private delegate void DelegateShowError();
        private delegate void ChangeParamIsEnable();

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        Process AP3Process = new Process();
        bool bIfRunAP3;
        public MainWindow()
        {
            InitializeComponent();
            bIfRunAP3 = false;
            SpeciesComboBox.SelectedIndex = 0;
            this.Closing += ClosingApplication;

        }
        private void ClosingApplication(object sender, CancelEventArgs e)
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to exit？", "Closing", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //Close the windown
                if (result == MessageBoxResult.Yes)
                {
                    KillProcesses();
                    e.Cancel = false;
                }
                //cancel the decision to close the window
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            catch (NotImplementedException nie)
            {
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

        }

        private void Load_paramters_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofile = new System.Windows.Forms.OpenFileDialog();
            ofile.Filter = "textfiles|*.params";
            if (ofile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadParameters(ofile.FileName);
                ShowInputSequences(InputFilePathTextBox.Text);
            }  
        }

        private void Parameters_Save_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog ifile = new System.Windows.Forms.SaveFileDialog();
            ifile.Filter = "text files|*.params";
            if (ifile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveParameters(ifile.FileName);
            }
            
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            KillProcesses();
            this.Close();
        }

        public int KillProcesses()
        {
            try
            {
                if (bIfRunAP3)
                {
                    if (!AP3Process.HasExited)
                        AP3Process.Kill();
                }

            }
            catch (NullReferenceException nre)
            {

            }

            return 0;
        }
        private void InitialTest_Click(object sender, RoutedEventArgs e)
        {

            if (!IfExistInEnvironmentPath("mclmcrrt8_3.dll"))
            {
                System.Windows.MessageBox.Show("Please intall and configurate the Matlab Runtime 8.3 before running AP3!");
            }
            else
            {
                System.Windows.MessageBox.Show("The Matlab Runtime 8.3 has been installed.");
            }

        }


        public bool IfExistInEnvironmentPath(string DllName)
        {
            if(File.Exists(DllName))
            {
                return true;
            }
            else
            {
                string pathlist=string.Empty;
                string DllFullPath;
                
                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                {
                    if (de.Key.ToString() == "PATH" || de.Key.ToString() == "Path")
                        pathlist = de.Value.ToString();
                }
                
                //检测是否以;结尾
                if (pathlist.Substring(pathlist.Length - 1, 1) != ";")
                {
                    pathlist = pathlist + ";";
                }
                string[] list = pathlist.Split(';');

                foreach (string item in list)
                {
                    DllFullPath = item + "\\" + DllName;
                    if (File.Exists(DllFullPath))
                        return true;
                }
            }
                
            return false;
        }
       

        private void About_Click(object sender, RoutedEventArgs e)
        {
            string strAboutAP3 = "AP3 Version: v1.0.0\nAP3 is an improved proteotypic peptide prediction tool by taking the protein proteolytic digestion process into consideration.";
            System.Windows.MessageBox.Show(strAboutAP3);
        }

        private void Online_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://fugroup.amss.ac.cn/software/AP3/AP3.html");
            }
            catch
            {
            }
     

        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            string strEmails = "Yan Fu: yfu@amss.ac.cn \nAcademy of Mathematics and Systems Science, Chinese Academy of Sciences, Beijing 100049, China.\n" +
                "Yunping Zhu: zhuyunping@gmail.com\nBeijing Proteome Research Center, National Center for Protein Sciences (Beijing), Beijing Institute of Lifeomics, Beijing 102206, China.";
            System.Windows.Forms.MessageBox.Show(strEmails);

        }
      
        

        public void LoadParameters(string parametersPath)
        {
            string strLine;
            string strTemp1, strTemp2;
            int iSplitLoc;
            int iStartLoc, iEndLoc;
           
            try
            {
                StreamReader sr = File.OpenText(parametersPath);
                while (sr.EndOfStream != true)
                {
                    strLine = sr.ReadLine();
                    iSplitLoc = strLine.IndexOf("=");
                    if (iSplitLoc == -1)
                    {
                        continue;
                    }
                    strTemp1 = strLine.Substring(0, iSplitLoc);
                    iStartLoc = strLine.IndexOf("\"");
                    iEndLoc = strLine.LastIndexOf("\"");
                    strTemp2 = strLine.Substring(iStartLoc + 1, iEndLoc - iStartLoc - 1);
                   if (strTemp1 == "InputFilePath")
                    {
                        InputFilePathTextBox.Text = strTemp2;
                    }
                    if (strTemp1 == "Model")
                    {
                        if (strTemp2 == "Saccharomyces cerevisiae" || strTemp2 == "Mus" || strTemp2 == "Homo sapiens" || strTemp2 == "E.coli" || strTemp2 == "Mix")
                            SpeciesComboBox.SelectedValue= strTemp2;
                        else
                        {
                            System.Windows.MessageBox.Show("Cannot read the \"Model\" from the parameter file.");
                        }
                    }
                    if (strTemp1 == "IdentifierParsingRule")
                    {
                        ParseRuleTextBox.Text = strTemp2;
                    }
                    if (strTemp1 == "ResultPath")
                    {
                        ResultPathTextBox.Text = strTemp2;
                    }

                    if (strTemp1 == "AllowMinPeptideLength")
                    {
                        MinPepLength.Text = strTemp2;
                    }
                    if (strTemp1 == "AllowMaxPeptideLength")
                    {
                        MaxPepLength.Text = strTemp2;
                    }
                    if (strTemp1 == "AllowMissingCutNumber")
                    {
                        MaxMissCleavage.Text = strTemp2;
                    }
                  }
                sr.Close();
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show("Cannot open " + parametersPath);
            }
            catch (ArgumentException ax)
            {
                System.Windows.MessageBox.Show("Cannot open " + parametersPath);
            }

        }

        public void ShowInputSequences(string InputFilePath)
        {
            const int iReadLines=50000;
            string[] InputContents = new string[iReadLines]; // assume the maxium number of lines of one protein's sequence is 50
            int iLine=0;
            
            try
            {
                StreamReader sReader = new StreamReader(InputFilePath, Encoding.Default);
                int iProteins = 0;
                string strLine = sReader.ReadLine();
                while (strLine!=null && iLine < iReadLines)
                {
                        
                    if (strLine!=""&& strLine[0] == '>')
                    {
                        iProteins++;
                        if (iLine > 0)
                            InputContents[iLine - 1] += "\n";
                        if (iProteins == 101)
                            break;
                    }
                         
                    InputContents[iLine] = strLine;
                    strLine = sReader.ReadLine(); 
                    iLine++;
                }


                InputTextBox.Text = "";
                for (int i = 0; i < iLine; i++)
                {
                    InputTextBox.Text += InputContents[i];
                }

                sReader.Close();
            }
            catch (ArgumentException ex)
            {
                System.Windows.MessageBox.Show("Cannot open the input directory: " + InputFilePath);
                return;
            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show("Cannot open the input directory: " + InputFilePath);
                return;
            }
        }

        public bool SaveParameters(string path)
        {
            CheckParameters();

            ComboBoxItem SpeciesItem = SpeciesComboBox.SelectedItem as ComboBoxItem;
            using (StreamWriter writer = File.CreateText(path))
            {
                if(FileInputCheckbox.IsChecked==true)
                {
                    writer.WriteLine("InputFilePath=\"" + InputFilePathTextBox.Text + "\"");
                }
                else
                {
                    writer.WriteLine("InputFilePath=\".\\ScreenInputProteins.fasta\"");
                    SaveScreenInputProteins("ScreenInputProteins.fasta");
                }
               
                writer.WriteLine("Model=\"" + SpeciesItem.Content.ToString() + "\"");
                writer.WriteLine("IdentifierParsingRule=\"" + ParseRuleTextBox.Text +"\"");
                writer.WriteLine("ResultPath=\"" +ResultPathTextBox.Text +"\"");
                writer.WriteLine("AllowMinPeptideLength=\"" + MinPepLength.Text +"\"");
                writer.WriteLine("AllowMaxPeptideLength=\"" + MaxPepLength.Text +"\"");
                writer.WriteLine("AllowMissingCutNumber=\"" +MaxMissCleavage.Text +"\"");
                writer.Close();
            }
      
            return true;
        }

        public void SaveScreenInputProteins(string strPath)
        {
            using (StreamWriter writer = File.CreateText(strPath))
            {
                writer.Write(InputTextBox.Text);
                writer.Close();
            }

        }
        public bool CheckParameters()
        {
            if(ParseRuleTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The regular expression for extract protein identifier from the protein fasta file is empty.");
                return false;
            }
            if(FileInputCheckbox.IsChecked==true&&InputFilePathTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The path of the input file is empty.");
                return false;
            }

            if (InputTextBox.Text == "")
            {
                System.Windows.MessageBox.Show("The input textbox is empty.");
                return false;
            }

            if(MaxMissCleavage.Text=="")
            {
                System.Windows.MessageBox.Show("The allowed maximum number of missed cleavage is empty.");
                return false;
            }

            if(MinPepLength.Text=="")
            {
                System.Windows.MessageBox.Show("The allowed minimum length of digested peptides is empty.");
                return false;
            }
            if (MaxPepLength.Text == "")
            {
                System.Windows.MessageBox.Show("The allowed maximum length of digested peptides is empty.");
                return false;
            }
            if(ResultPathTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The directory path of result files is empty.");
                return false;
            }

            ComboBoxItem SpeciesItem = SpeciesComboBox.SelectedItem as ComboBoxItem;
            if (SpeciesItem==null)
                SpeciesComboBox.SelectedIndex = 0;
            return true;
        }

        private void FollowSequencesCheckbox_click(object sender, RoutedEventArgs e)
        {
            InputDirectory_Browse.IsEnabled = false;
            FileInputCheckbox.IsChecked = false;
            InputFilePathTextBox.IsEnabled = false;
        }

        private void FileInputCheckbox_click(object sender, RoutedEventArgs e)
        {
            InputDirectory_Browse.IsEnabled = true;
            FollowSequencesCheckbox.IsChecked = false;
            InputFilePathTextBox.IsEnabled = true;
        }

        private void TestRuleButton_Click(object sender, RoutedEventArgs e)
        {
            TestRegularExpression re = new TestRegularExpression();
            re.RetureREEvent += new RetureREHandler(SetCorrectRE);
            re.Owner = this;
            re.ShowDialog();
        }

        void SetCorrectRE(string text)
        {
            this.ParseRuleTextBox.Text = text;
        }
        public string GetInputPathFunction()
        {
            if (this.InputFilePathTextBox.Text == "")
                return "";
            else
                return this.InputFilePathTextBox.Text;

        }
        private void AP3RunButton_click(object sender, RoutedEventArgs e)
        {
            if (!CheckParameters())
            {
                return;
            }
            System.DateTime data = System.DateTime.Now;
            string Now = data.ToString("yyyyMMdd-HH-mm-ss");
            string strParametersPath = ResultPathTextBox.Text + "\\parameters" + Now + ".params";
            if (!SaveParameters(strParametersPath))
            {
                return;
            }

            // set the parameter file for core program 
            string[] strParameters = new string[1];
            if ((strParametersPath.IndexOf(" ") != -1))
            {
                strParametersPath = "\"" + strParametersPath + "\"";
            }
            strParameters[0] = strParametersPath;
            ///*************
            StartProcess(strParameters);




   
        }

        public void StartProcess(string[] args)
        {

            try
            {
                if (bIfAP3Running)
                {
                    return;
                }

                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();

                
                AP3Process = new Process();
                string loadExeName = "PredictionOfRF31.exe";  //
                if(!File.Exists(loadExeName))
                {
                    System.Windows.MessageBox.Show("Can not find the excutable file PredictionOfRF31.exe.");
                    return;
                }

                ProcessStartInfo LoadStartInfo = new ProcessStartInfo(loadExeName, s);
                LoadStartInfo.CreateNoWindow = true;
                LoadStartInfo.UseShellExecute = false;
     

                DoEvents();
                AP3Process.EnableRaisingEvents = true;
                // LoadProcess.SynchronizingObject = this; 
                AP3Process.Exited += new EventHandler(AP3Process_Exited);
                AP3Process.StartInfo = LoadStartInfo;
                bIfAP3Running = true;
                 
                UpdateStatusBar("AP3 is running!");
                DoEvents();

                DisableParameters();
                AP3Process.Start();
                bIfRunAP3 = true;
               // AP3Process.WaitForExit();

 
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error when starting the program. The reason may be：" + ex.Message);
            }
        }
        private void DisableParameters()
        {
            MenuFile.IsEnabled = false;
            ParseRuleTextBox.IsEnabled = false;
            TestRuleButton.IsEnabled = false;
            InputTextBox.IsEnabled = false;
            InputFilePathTextBox.IsEnabled = false;
            MaxMissCleavage.IsEnabled = false;
            MinPepLength.IsEnabled = false;
            MaxPepLength.IsEnabled = false;
            ResultPathTextBox.IsEnabled = false;
            AP3RunButton.IsEnabled = false;
        }
        private void EnableParameters()
        {
            MenuFile.IsEnabled = true;
            ParseRuleTextBox.IsEnabled = true;
            TestRuleButton.IsEnabled = true;
            InputTextBox.IsEnabled = true;
            InputFilePathTextBox.IsEnabled = true;
            MaxMissCleavage.IsEnabled = true;
            MinPepLength.IsEnabled = true;
            MaxPepLength.IsEnabled = true;
            ResultPathTextBox.IsEnabled = true;
            AP3RunButton.IsEnabled = true;
        }
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate(object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);
        }

        private void AP3Process_Exited(object sender, EventArgs e)
        {
            int returnLoadValue = AP3Process.ExitCode;
            bIfAP3Running = false;
            if (returnLoadValue == 2)
            {
                System.Windows.MessageBox.Show("Cannot open the parameter file! (The file path cannot contain space.)");
            }
            else if (returnLoadValue != 0)
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
new ChangeStatusBar(UpdateStatusBar), "Ready");
                // UpdateStatusBar("Ready");
                DoEvents();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
new DelegateShowError(ShowError));
            }
            else
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
new ChangeParamIsEnable(EnableParameters));

                System.Windows.MessageBox.Show("AP3 has finished successfully!");
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
new ChangeStatusBar(UpdateStatusBar), "Finished!");
                DoEvents();
            }
            //throw new NotImplementedException();
        }

        private void UpdateStatusBar(String args)
        {
            statBarText.Text = args;  
        }
        private void ShowError()
        {
            string logPath = ResultPathTextBox.Text + "\\log.txt";
            if (!File.Exists(logPath))
            {
                System.Windows.MessageBox.Show("Cannot find file: " + logPath);
                return;
            }
            try
            {
                using (StreamReader sr = new StreamReader(logPath, Encoding.Default))
                {
                    string Line;
                    string strTemp;
                    int iEnd;
                    while ((Line = sr.ReadLine()) != null)
                    {
                        iEnd = Line.IndexOf("\t");
                        if (iEnd != -1)
                        {
                            strTemp = Line.Substring(0, iEnd);
                            if (strTemp == "Error:")
                            {
                                System.Windows.MessageBox.Show(Line);
                                break;
                            }
                        }
                    }
                }

            }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show("An IOException has been thrown!" + ex.Message);
                return;
            }
        }

        private void InputDirectory_Browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
            openDlg.Filter = "fasta files| *.fasta";
            if (true == openDlg.ShowDialog())
            {
                string DatafromFile = openDlg.FileName;
                InputFilePathTextBox.Text = DatafromFile;
            }

            ShowInputSequences(InputFilePathTextBox.Text);
        }

        private void MaxMissCleavage_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strTemp = MaxMissCleavage.Text;
            if (!string.IsNullOrEmpty(strTemp))
            {
                try
                {
                    Int32 dTemp = Convert.ToInt32(strTemp);
                    if (dTemp < 0 || dTemp > 5)
                    {
                        System.Windows.MessageBox.Show("The allowed maximum missing cleavages of one peptide in theoretic digestion should be in the 0-5 range");
                        e.Handled = false;
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("The allowed maximum missing cleavages of one peptide in theoretic digestion should be a integer.");
                }
            }

        }

    }
}
