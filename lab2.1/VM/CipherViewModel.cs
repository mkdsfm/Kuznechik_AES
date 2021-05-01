using lab2._1.View;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace lab2._1.VM
{
    //public enum Algoritmh { Kuznechik, Aes };
    class CipherViewModel : INotifyPropertyChanged
    {
        #region PropertyChange
        // через событие PropertyChanged извещает систему об изменении свойства. А система обновляет все привязанные объекты.
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Constructor
        private IDialogService dialogService;
        private IFileService fileService;
        public CipherViewModel(IDialogService dialogService, IFileService fileService)
        {
            this.dialogService = dialogService;
            this.fileService = fileService;
            Alg = Algoritmh.Kuznechik;
        }
        #endregion

        #region Fields
        private ICipher algoritmh;
        private Algoritmh _alg;
        private string _key = "";
        private string _textOrig = "";
        private string _textEncode;
        private string _textDecode = "";
        #endregion

        #region Properties
        // Выбор алгоритма с помощью radiobutton
        public Algoritmh Alg
        {
            get
            {
                return _alg;
            }
            set
            {
                _alg = value;
                OnPropertyChanged("Alg");
            }
        }

        // Ключ
        public string Key
        {
            get { return _key; }
            set
            {
                if (Alg == Algoritmh.Kuznechik && value.Length < 33)
                {
                    _key = value;
                }
                else if (Alg == Algoritmh.Aes && value.Length < 17)
                {
                    _key = value;
                }
                else if (Alg == Algoritmh.Aes && value.Length > 16)
                {
                    Key = _key.Substring(0, 16);
                }
                else if (Alg == Algoritmh.Kuznechik && value.Length > 32)
                {
                    Key = _key.Substring(0, 32);
                }
                OnPropertyChanged("Key");
            }
        }

        // Исходный текст
        public string TextOrig
        {
            get
            {
                return _textOrig;
            }
            set
            {
                _textOrig = value;
                OnPropertyChanged("TextOrig");
            }
        }

        // Текст после шифрования
        public string TextEncode
        {
            get
            {
                return _textEncode;
            }
            set
            {
                _textEncode = value;
                OnPropertyChanged("TextEncode");
            }
        }
        // Текст после дешифрования
        public string TextDecode
        {
            get
            {
                return _textDecode;
            }
            set
            {
                _textDecode = value;
                OnPropertyChanged("TextDecode");
            }
        }
        #endregion

        #region Command

        // Из файла
        private RelayCommand _openCommand;
        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ??
                  (_openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.OpenFileDialog() == true)
                          {
                              string text = fileService.Open(dialogService.FilePath);
                              TextOrig = text;
                          }
                      }
                      catch
                      {
                          dialogService.ShowMessage("Некорректные данные");
                      }
                  }));
            }
        }
        // команда для запуска алгоритма и выведения результата шифрования
        private RelayCommand _getEncodeResultCommand;
        public ICommand GetEncodeResultCommand
        {
            get
            {
                return _getEncodeResultCommand ??
                    (_getEncodeResultCommand = new RelayCommand(obj =>
                    {
                        if (Key == "")
                        {
                            dialogService.ShowMessage("Не введён ключ");
                            return;
                        }
                        if (TextOrig != "")
                        {
                            if (_alg == Algoritmh.Kuznechik)
                            {
                                algoritmh = new KuznechikCipher();
                            }
                            else
                            {

                                algoritmh = new AesCipher();
                            }
                            try
                            {
                                string result = algoritmh.Encode(TextOrig, Key);
                                TextEncode = result;
                            }
                            catch{
                                dialogService.ShowMessage("Ошибка");
                            }
                        }
                    }));
            }
        }

        // команда для запуска алгоритма и выведения результата дешифрования
        private RelayCommand _getDecodeResultCommand;
        public ICommand GetDecodeResultCommand
        {
            get
            {
                return _getDecodeResultCommand ??
                    (_getDecodeResultCommand = new RelayCommand(obj =>
                    {
                        if (Key == "")
                        {
                            dialogService.ShowMessage("Не введён ключ");
                            return;
                        }
                        if (TextOrig != "")
                        {
                            if (_alg == Algoritmh.Kuznechik)
                            {
                                algoritmh = new KuznechikCipher();
                            }
                            else
                            {
                                algoritmh = new AesCipher();
                            }
                            try
                            {
                                string result = algoritmh.Decode(TextOrig, Key);
                                TextDecode = result;
                            }
                            catch (Exception ex)
                            {
                                dialogService.ShowMessage(ex.Message);
                            }
                        }
                    }));
            }
        }

        private RelayCommand _getDecodeFromEncodeResultCommand;
        public ICommand GetDecodeFromEncodeResultCommand
        {
            get
            {
                return _getDecodeFromEncodeResultCommand ??
                    (_getDecodeFromEncodeResultCommand = new RelayCommand(obj =>
                    {
                        if (Key == "")
                        {
                            dialogService.ShowMessage("Не введён ключ");
                            return;
                        }
                        if (TextEncode != "")
                        {
                            if (_alg == Algoritmh.Kuznechik)
                            {
                                algoritmh = new KuznechikCipher();
                            }
                            else
                            {
                                algoritmh = new AesCipher();
                            }
                            try
                            {
                                string result = algoritmh.Decode(TextEncode, Key);
                                TextDecode = result;
                            }
                            catch (Exception ex)
                            {
                                dialogService.ShowMessage(ex.Message);
                            }
                        }
                    }));
            }
        }
        
        // Очистка экрана
        private RelayCommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ??
                    (_clearCommand = new RelayCommand(obj =>
                    {
                        Key = "";
                        TextOrig = "";
                        TextEncode = "";
                        TextDecode = "";
                    }));
            }
        }

        // мини - отчет
        private RelayCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                  (_saveCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              string dataArr = "Исходный текст: \n" + TextOrig + "\nРезультат шифрования: \n" +
                              TextEncode +
                              "\nРезультат Дешифрования:\n" + TextDecode;
                              fileService.Save(dialogService.FilePath, dataArr);
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch
                      {
                          dialogService.ShowMessage("Ошибка");
                      }
                  }));
            }
        }
        // сохранить исходник
        private RelayCommand _saveOrigCommand;
        public ICommand SaveOrigCommand
        {
            get
            {
                return _saveOrigCommand ??
                  (_saveOrigCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              fileService.Save(dialogService.FilePath, TextOrig);
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch
                      {
                          dialogService.ShowMessage("Ошибка");
                      }
                  }));
            }
        }

        // сохранить шифровку 
        private RelayCommand _saveResultEncodeCommand;
        public ICommand SaveResultEncodeCommand
        {
            get
            {
                return _saveResultEncodeCommand ??
                  (_saveResultEncodeCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              fileService.Save(dialogService.FilePath, TextEncode);
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch
                      {
                          dialogService.ShowMessage("Ошибка");
                      }
                  }));
            }
        }

        // Сохранить дешифровку
        private RelayCommand _saveResultDecodeCommand;
        public ICommand SaveResultDecodeCommand
        {
            get
            {
                return _saveResultDecodeCommand ??
                  (_saveResultDecodeCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.SaveFileDialog() == true)
                          {
                              string text = TextDecode;
                              fileService.Save(dialogService.FilePath, text);
                              dialogService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch
                      {
                          dialogService.ShowMessage("Ошибка");
                      }
                  }));
            }
        }

        // Открыть окно с информацией
        private RelayCommand _openWindowInformation;
        public ICommand OpenWindowInformation
        {
            get
            {
                return _openWindowInformation ??
                  (_openWindowInformation = new RelayCommand(obj =>
                  {
                      InfoWindow informWindow = new InfoWindow();
                      informWindow.Show();
                  }));
            }
        }

        #endregion
    }
}
