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

namespace GUI_AP3
{


    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool bIfAP3Running;
        private delegate void ChangeStatusBar(String arg);
        private delegate void DelegateShowError();

        Process AP3Process = new Process();

        public MainWindow()
        {
            InitializeComponent();

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

        }

        private void InitialTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Online_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem InputTypeItem = ((sender as System.Windows.Controls.ComboBox).SelectedItem as ComboBoxItem);

            if(InputTypeItem.Content==null){}
            else if(InputTypeItem.Content.ToString()=="PeptideSequences")
            {
                ParseRuleTextBox.IsEnabled = false;
                TestRuleButton.IsEnabled = false;
                MaxMissCleavage.IsEnabled = false;
                MinPepLength.IsEnabled = false;
                MaxPepLength.IsEnabled = false;
            }
            else if (InputTypeItem.Content.ToString() == "ProteinsFasta")
            {
                ParseRuleTextBox.IsEnabled = true;
                TestRuleButton.IsEnabled = true;
                MaxMissCleavage.IsEnabled = true;
                MinPepLength.IsEnabled = true;
                MaxPepLength.IsEnabled = true;
            }

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
                    if (strTemp1 == "InputFileType")
                    {
                        if (strTemp2 == "ProteinsFasta")
                            InputTypeComboBox.SelectedIndex = 0;
                        else if (strTemp2 == "PeptideSequences")
                            InputTypeComboBox.SelectedIndex = 1;
                        else
                            System.Windows.MessageBox.Show("Cannot read the InputFileType in the parameter file.");
                    }
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
                ComboBoxItem cbItem = (InputTypeComboBox.SelectedItem as ComboBoxItem);

                if (cbItem.Content.ToString() == "PeptideSequences")
                {
                    string strLine=sReader.ReadLine();
                    while (strLine!=null&& iLine < 10)
                    {                     
                        InputContents[iLine] = strLine;
                        strLine = sReader.ReadLine();
                        iLine++;
                    }
                }
                else if (cbItem.Content.ToString() == "ProteinsFasta")
                {
                    int iProteins = 0;
                    string strLine = sReader.ReadLine();
                    while (strLine!=null && iLine < iReadLines)
                    {
                        
                        if (strLine!=""&& strLine[0] == '>')
                        {
                            iProteins++;
                            if (iLine > 0)
                                InputContents[iLine - 1] += "\n";
                            if (iProteins == 11)
                                break;
                        }
                         
                        InputContents[iLine] = strLine;
                        strLine = sReader.ReadLine(); 
                        iLine++;
                    }
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

            ComboBoxItem InputTypeItem=InputTypeComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem SpeciesItem = SpeciesComboBox.SelectedItem as ComboBoxItem;
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine("InputFileType=\""+ InputTypeItem.Content.ToString()+ "\"");
                writer.WriteLine("InputFilePath=\"" + InputFilePathTextBox.Text + "\"");
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

        public bool CheckParameters()
        {
            if(ParseRuleTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The regular expression for extract protein identifier from the protein fasta file is empty.");
                return false;
            }
            if (InputFilePathTextBox.Text == "")
            {
                System.Windows.MessageBox.Show("The path of input file and the input sequences textbox is empty.");
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
        public string GetInputType()
        {
            ComboBoxItem InputTypeItem = InputTypeComboBox.SelectedItem as ComboBoxItem;
            if (InputTypeItem.Content.ToString() == "ProteinsFasta")
            {
                return "ProteinsFasta";
            }
            else if(InputTypeItem.Content.ToString()=="PeptideSequences")
            {
                return "PeptideSequences";
            }
            else
            {
                return "null";
            }
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

                DisableParameters();
                AP3Process = new Process();
                string loadExeName = " PredictionOfRF31.exe";  //

                ProcessStartInfo LoadStartInfo = new ProcessStartInfo(loadExeName, s);
                LoadStartInfo.CreateNoWindow = true;
                LoadStartInfo.UseShellExecute = false;
     

                DoEvents();
                AP3Process.EnableRaisingEvents = true;
                // LoadProcess.SynchronizingObject = this; 
                AP3Process.Exited += new EventHandler(AP3Process_Exited);
                AP3Process.StartInfo = LoadStartInfo;
                bIfAP3Running = true;
                AP3Process.Start();
                AP3Process.WaitForExit();


                if (AP3Process.ExitCode != 0)
                {
                    EnableParameters();
                    return;
                }



 
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
                //System.Windows.MessageBox.Show("Have loaded the input files!");
            }
            //throw new NotImplementedException();
        }

        private void UpdateStatusBar(String args)
        {
            //statBarText.Text = args;  TODO
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

    }
}
