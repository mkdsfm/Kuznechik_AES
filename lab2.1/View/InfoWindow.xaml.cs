using System.Text;
using System.Windows;

namespace lab2._1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Информация о программе: ");
            sb.AppendLine("Автор: Тюлькина Ирина");
            sb.AppendLine("Группа: 494");
            sb.AppendLine("Дисциплина: Информационные безопасность");
            sb.AppendLine("Год: 2022");
            sb.AppendLine("Описание: Программа для шифрования и дешифрования текста ");
            sb.AppendLine("Поддерживаемые алгоритмы: Кузнечик (ГОСТ Р 34.12-2015)");
           
            this.text.Text = sb.ToString();
        }
    }
}
