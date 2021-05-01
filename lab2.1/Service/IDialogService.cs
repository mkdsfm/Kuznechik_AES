namespace lab2._1
{
    // Интерфейс - набор методов и свойств без реализации. 
    // Затем этот функционал реализуют классы и структуры, которые применяют данные интерфейсы.
    public interface IDialogService
    {
        void ShowMessage(string message);   // показ сообщения
        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFileDialog();  // открытие файла
        bool SaveFileDialog();  // сохранение файла
    }
}
