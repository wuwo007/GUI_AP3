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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace GUI_AP3
{
    public delegate void RetureREHandler(string text);


    /// <summary>
    /// TestRegularExpression.xaml 的交互逻辑
    /// </summary>
    public partial class TestRegularExpression : Window
    {
        public event RetureREHandler RetureREEvent;

        public ObservableCollection<RegularExpressionExample> exams { get; set; }
        public ObservableCollection<ProteinName> ProteinNames { get; set; }


        public TestRegularExpression()
        {
            InitializeComponent();

             try
            {
                exams = new ObservableCollection<RegularExpressionExample>();
                exams.Add(new RegularExpressionExample { header = ">IPI:IPI00107908.1|TREMBL:Q9D5E3|ENSEMBL:ENSMUSP00000106228|REFSEQ:NP_080412", re = ">(.*?)\\|", Identifier = "IPI:IPI00107908.1" });
                exams.Add(new RegularExpressionExample { header = ">sp|P02768ups|ALBU_HUMAN_UPS Serum albumin (Chain 26-609) - Homo sapiens (Human)", re = ">sp\\|(.*?)\\|", Identifier = "P02768ups" });
                exams.Add(new RegularExpressionExample { header = ">ENSP00000381386 pep:known chromosome:GRCh37:22:24313554:24316773:-1", re = ">(.*?)\\s", Identifier = "ENSP00000381386" });
                examples.DataContext = exams;
 
           }
            catch (IOException ex)
            { 
                System.Windows.MessageBox.Show("An IOException has been thrown!"+ex.Message);
                return;
            }
             catch (ArgumentException ex)
             {
                 System.Windows.MessageBox.Show("Error:\t" + ex.Message);
                 return;
             }

        }


        private void fasta_browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog();
            openDlg.Filter = "fasta Files| *.fasta";
            string strLine;
            string strProteinIdentifier;
            List<string> ProteinsInFasta = new List<string>();

            if (true == openDlg.ShowDialog()) //get protein names from fasta file
            {
                string FastaFilePath = openDlg.FileName;
                txtfasta.Text = FastaFilePath;
                int iProteins = 0;
                int iFastaProteinsNum = 999;
                try
                {
                    FileStream aFile = new FileStream(txtfasta.Text, FileMode.Open);
                    StreamReader streamReader = new StreamReader(aFile);
                    ProteinNames = new ObservableCollection<ProteinName>();
                    strLine = streamReader.ReadLine();
                    while (strLine != null && iProteins <= iFastaProteinsNum)
                    {
                        while (strLine == "")
                        {
                            strLine = streamReader.ReadLine();
                        }
                        if (strLine[0] == '>')
                        {
                            ProteinsInFasta.Add(strLine);
                            iProteins++;
                        }
                        strLine = streamReader.ReadLine();
                    }
                    fasta_status.Content = "Load protein headers from the fasta file (show the first 1000 proteins)";
                    streamReader.Close();

                    foreach (string item in ProteinsInFasta)
                    {
                            strProteinIdentifier = "";
                            ProteinNames.Add(new ProteinName { ProteinHeader = item, ProteinIdentifier = strProteinIdentifier });
                    }
                    fasta_grid.DataContext = ProteinNames;
                }
                catch (ArgumentException)
                {
                    System.Windows.MessageBox.Show("Cannot open "+txtfasta.Text);
                }
                catch (IOException ex)
                {
                    System.Windows.MessageBox.Show("An IOException has been thrown!" + ex.Message);
                    return;
                }
 
            }
        }
        private void Test_button_Click(object sender, RoutedEventArgs e)
        {
            string strLine;
            List<string> ProteinsInFasta = new List<string>();
            Match match;
            int iProteins = 0;
            try
            {
                if (!File.Exists(txtfasta.Text))
                {
                    System.Windows.MessageBox.Show("Cannot open the fasta file: " + txtfasta.Text);
                    return;
                }
                FileStream aFile = new FileStream(txtfasta.Text, FileMode.Open);
                StreamReader streamReader = new StreamReader(aFile);
                string strProteinIdentifier;
                ProteinNames = new ObservableCollection<ProteinName>();

                strLine = streamReader.ReadLine();
                iProteins = 0;
                while (strLine != null)
                {
                    while (strLine == "")
                    {
                        strLine = streamReader.ReadLine();
                    }
                    if (strLine[0] == '>')
                    {
                        ProteinsInFasta.Add(strLine);
                        iProteins++;
                        try
                        {
                            match = Regex.Match(strLine, txtRegularExpression.Text);
                            if (!match.Success)
                            {
                                System.Windows.MessageBox.Show("Cannot parse " + strLine + " by " + txtRegularExpression.Text);
                                streamReader.Close();
                                return;
                            }
                            else
                            {
                                strProteinIdentifier = match.Groups[1].Value;
                                ProteinNames.Add(new ProteinName { ProteinHeader = strLine, ProteinIdentifier = strProteinIdentifier });
                            }
                        }
                        catch (ArgumentException)
                        {
                            System.Windows.MessageBox.Show("Seting a wrong regular expression.");
                            return;
                        }

                    }
                    strLine = streamReader.ReadLine();
                }
                fasta_status.Content ="Load protein headers from the fasta file (show the first 1000 proteins)";
                fasta_grid.DataContext = ProteinNames;
                streamReader.Close();
            }
            catch (ArgumentException){               
                System.Windows.MessageBox.Show("Please set the fasta file first!");
                return;
                }
            catch (IOException ex)
            {
                System.Windows.MessageBox.Show("An IOException has been thrown!" + ex.Message);
                return;
            }

                   
        }

        private void Parse_OK_button_Click(object sender, RoutedEventArgs e)
        {
            if(RetureREEvent!=null)
            {
                RetureREEvent.Invoke(txtRegularExpression.Text);
                this.Close();
            }
        }
        public class RegularExpressionExample : INotifyPropertyChanged
        {

            public event PropertyChangedEventHandler PropertyChanged;
            private string m_header;
            private string m_re;
            private string m_Identifier;
            public string header
            {
                get { return m_header; }
                set
                {
                    m_header = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("header"));
                    }
                }
            }
            public string re
            {
                get { return m_re; }
                set
                {
                    m_re = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("re"));
                    }
                }
            }
            public string Identifier
            {
                get { return m_Identifier; }
                set
                {
                    m_Identifier = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Identifier"));
                    }
                }
            }
        }

        public class ProteinName : INotifyPropertyChanged
        {
            private string m_ProteinHeader;
            private string m_ProteinIdentifier;
            public event PropertyChangedEventHandler PropertyChanged;
            public string ProteinHeader
            {
                get
                {
                    return m_ProteinHeader;
                }
                set
                {
                    m_ProteinHeader = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ProteinHeader"));
                    }
                }
            }
            public string ProteinIdentifier
            {
                get
                {
                    return m_ProteinIdentifier;
                }
                set
                {
                    m_ProteinIdentifier = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ProteinIdentifier"));
                    }
                }
            }
        }

        private void txtRegularExpression_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
