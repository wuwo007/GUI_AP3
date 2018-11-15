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

namespace GUI_AP3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
                ShowInputSequences(InputfileTextBox.Text);
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
                        InputfileTextBox.Text = strTemp2;
                    }
                    if (strTemp1 == "Species")
                    {
                        if(strTemp2=="Mouse"||strTemp2=="Human"||strTemp2=="Ecoli"||strTemp2=="Mix")
                            SpeciesComboBox.SelectedValue= strTemp2;
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
            string[] InputContents = new string[500]; // assume the maxium number of lines of one protein's sequence is 50
            int iLine=0;
            string strLine;
            try
            {
                StreamReader sReader = new StreamReader(InputFilePath, Encoding.Default);
                ComboBoxItem cbItem = (InputTypeComboBox.SelectedItem as ComboBoxItem);

                if (cbItem.Content.ToString() == "PeptideSequences")
                {
                    while (iLine < 10)
                    {
                        strLine = sReader.ReadLine();
                        InputContents[iLine] = strLine;
                        iLine++;
                    }
                }
                else if (cbItem.Content.ToString() == "ProteinsFasta")
                {
                    int iProteins = 0;
                    while (iLine<500)
                    {
                        strLine = sReader.ReadLine();
                        if (strLine!=""&& strLine[0] == '>')
                        {
                            iProteins++;
                            if (iLine > 0)
                                InputContents[iLine - 1] += "\n";
                            if (iProteins == 11)
                                break;
                        }                           
                        InputContents[iLine] = strLine;
                        iLine++;
                    }
                }

                InputContentTextBox.Text = "";
                for (int i = 0; i < iLine; i++)
                {
                    InputContentTextBox.Text += InputContents[i];
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
            if(CheckParameters()==false)
            {
                return false;
            }
            ComboBoxItem InputTypeItem=InputTypeComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem SpeciesItem = SpeciesComboBox.SelectedItem as ComboBoxItem;
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine("InputFileType=\""+ InputTypeItem.Content.ToString()+ "\"");
                writer.WriteLine("InputFilePath=\""+InputfileTextBox.Text+"\"");
                writer.WriteLine("Species=\"" + SpeciesItem.Content.ToString() + "\"");
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
                System.Windows.MessageBox.Show("The regular expression for extract protein identifier from the protein fasta file cannot be empty.");
                return false;
            }
            if(InputfileTextBox.Text==""&&InputContentTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The path of input file and the input sequences textbox cannot be both empty.");
                return false;
            }
            
            if(MaxMissCleavage.Text=="")
            {
                System.Windows.MessageBox.Show("The allowed maximum number of missed cleavage cannot be empty.");
                return false;
            }

            if(MinPepLength.Text=="")
            {
                System.Windows.MessageBox.Show("The allowed minimum length of digested peptides cannot be empty.");
                return false;
            }
            if (MaxPepLength.Text == "")
            {
                System.Windows.MessageBox.Show("The allowed maximum length of digested peptides cannot be empty.");
                return false;
            }
            if(ResultPathTextBox.Text=="")
            {
                System.Windows.MessageBox.Show("The directory path of result files cannot be empty.");
                return false;
            }

            return true;
        }

    }
}
