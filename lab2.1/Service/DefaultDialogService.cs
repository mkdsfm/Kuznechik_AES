using Microsoft.Win32;
using System.Windows;

namespace lab2._1
{
    public class DefaultDialogService: IDialogService
    {
        public string FilePath { get; set; }
       
        public bool OpenFileDialog()
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*(.txt)|*.txt|*(.doc)|*.doc";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*(.txt)|*.txt|*(.doc)|*.doc";
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
