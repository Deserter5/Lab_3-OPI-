using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Lab_3
{
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currentFilePath = openFileDialog.FileName;
            FileInfo src = new FileInfo(currentFilePath);
            filename.Text = src.Name;
            source.Text = string.Empty;

            using (TextReader reader = new StreamReader(currentFilePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    source.AppendText(line + Environment.NewLine);
                }
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(source.Text))
            {
                MessageBox.Show("Please open a file or enter text first.");
                return;
            }

            double fileSizeKB = 0;
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                FileInfo fi = new FileInfo(currentFilePath);
                fileSizeKB = fi.Length / 1024.0;
            }

            string text = source.Text;
            int charCount = text.Length;
            double authorPages = charCount / 1800.0;

            string[] rawLines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int emptyLinesCount = 0;
            int paragraphsCount = 0;

            foreach (string line in rawLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    emptyLinesCount++;
                }
                else
                {
                    paragraphsCount++;
                }
            }

            int latinVowels = 0;
            int latinConsonants = 0;
            int cyrillicVowels = 0;
            int cyrillicConsonants = 0;
            int digitsCount = 0;
            int punctuationCount = 0;
            int specialSymbolsCount = 0;
            int latinTotal = 0;
            int cyrillicTotal = 0;

            string latVowelsList = "aeiouy";
            string cyrVowelsList = "аеєиіїоуюяэыё";

            foreach (char c in text)
            {
                char lower = char.ToLower(c);

                if (char.IsDigit(c))
                {
                    digitsCount++;
                }
                else if (char.IsPunctuation(c))
                {
                    punctuationCount++;
                }
                else if (char.IsSymbol(c))
                {
                    specialSymbolsCount++;
                }
                else if ((lower >= 'a' && lower <= 'z'))
                {
                    latinTotal++;
                    if (latVowelsList.IndexOf(lower) >= 0)
                    {
                        latinVowels++;
                    }
                    else
                    {
                        latinConsonants++;
                    }
                }
                else if ((lower >= 'а' && lower <= 'я') || lower == 'і' || lower == 'ї' || lower == 'є' || lower == 'ґ' || lower == 'ё' || lower == 'э' || lower == 'ы')
                {
                    cyrillicTotal++;
                    if (cyrVowelsList.IndexOf(lower) >= 0)
                    {
                        cyrillicVowels++;
                    }
                    else
                    {
                        cyrillicConsonants++;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FILE STATISTICS");
            sb.AppendLine($"File size: {fileSizeKB:F2} KB");
            sb.AppendLine($"Total characters: {charCount}");
            sb.AppendLine($"Paragraphs: {paragraphsCount}");
            sb.AppendLine($"Empty lines: {emptyLinesCount}");
            sb.AppendLine($"Author pages (1800 chars): {authorPages:F2}");
            sb.AppendLine();
            sb.AppendLine("LETTERS");
            sb.AppendLine($"Latin letters: {latinTotal} (Vowels: {latinVowels}, Consonants: {latinConsonants})");
            sb.AppendLine($"Cyrillic letters: {cyrillicTotal} (Vowels: {cyrillicVowels}, Consonants: {cyrillicConsonants})");
            sb.AppendLine();
            sb.AppendLine("OTHER SYMBOLS");
            sb.AppendLine($"Digits: {digitsCount}");
            sb.AppendLine($"Punctuation marks: {punctuationCount}");
            sb.AppendLine($"Special symbols: {specialSymbolsCount}");

            MessageBox.Show(sb.ToString(), "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(source.Text))
            {
                return;
            }

            string[] lines = source.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                string trimmed = Regex.Replace(line.Trim(), @"[ \t]+", " ");
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    sb.AppendLine(trimmed);
                }
            }

            source.Text = sb.ToString();
            MessageBox.Show("Unnecessary whitespaces, tabs, and empty lines have been removed.");
        }

        private void source_TextChanged(object sender, EventArgs e)
        {
        }
    }
}