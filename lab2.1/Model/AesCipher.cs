using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lab2._1
{
    public class AesCipher : ICipher
    {
        #region PropertyChanged
        // через событие PropertyChanged извещает систему об изменении свойства.
        // А система обновляет все привязанные объекты.
        public event PropertyChangedEventHandler PropertyChanged;

        

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Константы
        public byte[,] SubTable = new byte[16, 16]
       {
             { 0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76},
             { 0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0},
             { 0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15},
             { 0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75},
             { 0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84},
             { 0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf},
             { 0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8},
             { 0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2},
             { 0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73},
             { 0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb},
             { 0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79},
             { 0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08},
             { 0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a},
             { 0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e},
             { 0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf},
             { 0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16}
       };
        public byte[,] InvSubTable = new byte[16, 16]
        {
             { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb},
             { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb},
             { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e},
             { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25},
             { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92},
             { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84},
             { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06},
             { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b},
             { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73},
             { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e},
             { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b},
             { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4},
             { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f},
             { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef},
             { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61},
             { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d}
        };

        private const int WORDINKEY = 4; // количество слов в ключе; 
        private const int WORDSIZE = 4; // количество слов в блоке;
        private const int WORDINSTATE = 4;
        //private byte[] word = new byte[WORDSIZE];
        private List<byte[][]> bText = new List<byte[][]>();   // массив блоков входного текста
        private byte[][] Key = new byte[4][];  // ключ
        
        private int Nr = 0; // количество раундов // в этом случае от 0 до 10
        private bool flag;  // метка шифровки или дешифровки
        public List<byte[][]> My_Text
        {
            get { return bText; }
            set { bText = value; }
        }
        public byte[][] My_Key
        {
            get { return Key; }
            set { Key = value; }
        }
        #endregion

        #region Math

        #region Массив констант
        // массив констант 
        public byte[] Rcon(int i)
        {
            // Умножение байта на массив
            byte[] rCon = new byte[4] { 0, 0, 0, 0 };
            byte temp = 0x00;
            switch (i)
            {
                case 0:
                    temp = 0x00;
                    break;
                case 1:
                    temp = 0x01;
                    break;
                case 2:
                    temp = 0x02;
                    break;
                case 3:
                    temp = 0x04;
                    break;
                case 4:
                    temp = 0x08;
                    break;
                case 5:
                    temp = 0x10;
                    break;
                case 6:
                    temp = 0x20;
                    break;
                case 7:
                    temp = 0x40;
                    break;
                case 8:
                    temp = 0x80;
                    break;
                case 9:
                    temp = 0x1b;
                    break;
                case 10:
                    temp = 0x36;
                    break;
                default:
                    //MessageBox.Show(i.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    throw new Exception("Ошибка");
            }
            rCon[0] = temp;
            return rCon;
        }
        #endregion

        #region Основные функции
        private byte[] Xor(byte[] Word1, byte[] Word2)
        {
            byte[] NewWord = new byte[WORDSIZE];
            for (int i = 0; i < WORDSIZE; i++ )
            {
                NewWord[i] = (byte)(Word1[i] ^ Word2[i]);
            }
            return NewWord;
        }

        private byte Xor(byte ByteT, byte ByteK)
        {
            return (byte)(ByteT ^ ByteK);
        }

        // Умножение на константы 
        private byte Multiplication09(byte Byte)
        {
            byte NewByte = Byte;
            Byte = Multiplication02(Multiplication02(Multiplication02(Byte)));
            NewByte = Xor(Byte, NewByte);
            return NewByte;
        }

        private byte Multiply0b(byte Byte)
        {
            byte NewByte = Multiplication09(Byte);
            Byte = Multiplication02(Byte);
            NewByte = Xor(NewByte, Byte);
            return NewByte;
        }
        private byte Multiplication0d(byte Byte)
        {
            byte NewByte = Multiplication09(Byte);
            Byte = Multiplication02(Multiplication02(Byte));
            NewByte = Xor(NewByte, Byte);
            return NewByte;
        }
        private byte Multiplication0e(byte Byte)
        {
            byte NewByte = Byte;
            Byte = Multiplication02(Multiplication02(Multiplication02(Byte)));
            byte ByteTwoTwo = Multiplication02(Multiplication02(NewByte));
            NewByte = Multiplication02(NewByte);
            Byte = Xor(Byte, ByteTwoTwo);
            NewByte = Xor(Byte, NewByte);
            return NewByte;
        }
        private byte Multiplication02(byte Byte)
        {
            if (Byte < 0x80)
            {
                Byte = (byte)(Byte << 1);
            }
            else
            {
                try
                {
                    Byte = (byte)(Byte << 1);
                    Byte = Xor(Byte, 0x1b);
                }
                catch
                {
                    int Num = Byte << 1;
                    Num = Num ^ 0x1b;
                    Byte = (byte)(Num % 0x100);
                }
            }
            return Byte;
        }
        private byte Multiplication03(byte Byte)
        {
            byte Num = Byte;
            Byte = Multiplication02(Byte);
            Byte = Xor(Byte, Num);
            return Byte;
        }

        #endregion


        // массив раундовых ключей
        private List<byte[][]> GetRkey(List<byte[]> RoundKey)
        {
            List<byte[][]> NewRoundKey = new List<byte[][]>();
            for (int i = 0; i < RoundKey.Count; i += 4)
            {
                byte[][] num = new byte[4][];
                num[0] = RoundKey[i];
                num[1] = RoundKey[i + 1];
                num[2] = RoundKey[i + 2];
                num[3] = RoundKey[i + 3];
                NewRoundKey.Add(num);
            }
            return NewRoundKey;
        }

        // Преобразование в шифре и в дешифре, в которых раундовый ключ
        // добавляется к блоку с помощью XOR 
        private void AddRoundKey(byte[][] RoundKey)
        {
            for (int j = 0; j < bText.Count; j++)
            {
                for (int x = 0; x < WORDINSTATE; x++)
                {
                    for (int i = 0; i < WORDSIZE; i++)
                    {
                        (bText[j])[x][i] = Xor(RoundKey[x][i], (bText[j])[x][i]);
                    }
                }
            }
        }

        // сложение раундового ключа и блока
        private byte[][] NewState(byte[][] State) // переворот блока
        {
            byte[][] NewState = new  byte[4][];

            for (int i = 0; i < WORDINSTATE; i++)
            {
                NewState[i] = new byte[WORDSIZE];
                for (int j = 0; j < WORDSIZE; j++)
                {
                    NewState[i][j] = State[i][j];
                }
               
            }
            return NewState;
        }

        // замена батов
        private void SubBytes()
        {
            for (int j = 0; j < bText.Count; j++)
            {
                for (int x = 0; x < WORDINSTATE; x++)
                {
                    if (flag)
                    {
                        (bText[j])[x] = SubWord((bText[j])[x]);       // заменяем байты
                    }
                    else
                    {
                        (bText[j])[x] = InvSubWord((bText[j])[x]);       // заменяем байты
                    }
                }
            }
        }
        // Преобразование в шифре, который обрабатвыет блок с использованием 
        // таблицы нелинейной замены байтов (S-блок), 
        // которая работает с каждым байтом состояния независимо.
        private void ShiftRows()
        {
            byte[][] State = new byte[WORDINSTATE][];
            for (int j = 0; j < bText.Count; j++)
            {
                State = NewState(bText[j]);
                for (int x = 1; x < WORDINSTATE; x++)
                {
                    int q = x;
                    while (q != 0)
                    {
                        if (flag)
                        {
                            State[x] = RotWord(State[x]);
                            (bText[j])[x] = RotWord((bText[j])[x]);
                        }
                        else
                        {
                            State[x] = InvRotWord(State[x]);
                            (bText[j])[x] = InvRotWord((bText[j])[x]);
                        }
                        q--;
                    }
                }
                (bText[j]) = NewState(State);
            }
        }

        // генерация раундовых ключей
        private List<byte[]> ExpendKey()
        {
            List<byte[]> NewKey = new List<byte[]>();
            //Nk = Key.Length;
            for (int i = 0; i < WORDINKEY; i++) { NewKey.Add(Key[i]); }
            int count = WORDINKEY;
            if (Nr == 0) { return NewKey; }
            while (count < WORDSIZE * (Nr + 1))
            {
                byte[] temp = NewKey[count - 1];
                if (count % WORDINKEY == 0)
                {
                    byte[] RWord = RotWord(temp);
                    byte[] Word1 = SubWord(RWord);
                    byte[] Word2 = Rcon((count / WORDINKEY)); // Умножение слова на слово
                    temp = Xor(Word1, Word2); // Формируем новое слово 
                }
                else
                {
                    if ((WORDINKEY > 6) && (count % WORDINKEY == 4))
                    {
                        temp = SubWord(temp);
                    }
                }
                byte[] Word = Xor(NewKey[count - WORDINKEY], temp);
                NewKey.Add(Word);
                count++;
            }
            return NewKey;
        }
        #region Encode

        private byte[] RotWord(byte[] MyWord)
        {
            byte Cup = MyWord[0];
            MyWord[0] = MyWord[1];
            MyWord[1] = MyWord[2];
            MyWord[2] = MyWord[3];
            MyWord[3] = Cup;
            ;
            return MyWord;
        }
        private byte[] SubWord(byte[] MyWord)
        {
            byte[] B_x = new byte[WORDSIZE];
            byte[] B_y = new byte[WORDSIZE];
            for (int i = 0; i<WORDSIZE; i++)
            {
                B_x[i] = (byte)(MyWord[i] / 16);
                B_y[i] = (byte)(MyWord[i] % 16);
                MyWord[i] = (byte)(SubTable[B_x[i], B_y[i]]);
            }
            return MyWord;
        }

        private void MixColumns()
        {
            byte[] NewWord = new byte[4];
            byte Cup1, Cup2, Xor1, Xor2;
            for (int i = 0; i < bText.Count; i++)
            {
                for (int j = 0; j < WORDINSTATE; j++)
                {
                    //---------------первый_байт_столбца -------------------------------
                    Cup1 = Multiplication02((bText[i])[j][0]);
                    Cup2 = Multiplication03((bText[i])[j][1]);
                    Xor1 = Xor(Cup1, Cup2); 
                    Xor2 = Xor(Xor1, (bText[i])[j][2]);
                    NewWord[0] = Xor(Xor2, (bText[i])[j][3]);
                    //---------------второй_байт_столбца -------------------------------
                    Cup1 = Multiplication02((bText[i])[j][1]);
                    Cup2 = Multiplication03((bText[i])[j][2]);
                    Xor1 = Xor((bText[i])[j][0], Cup1); 
                    Xor2 = Xor(Xor1, Cup2);
                    NewWord[1] = Xor(Xor2, (bText[i])[j][3]);
                    //---------------третий_байт_столбца -------------------------------
                    Cup1 = Multiplication02((bText[i])[j][2]);
                    Cup2 = Multiplication03((bText[i])[j][3]);
                    Xor1 = Xor((bText[i])[j][0], (bText[i])[j][1]);
                    Xor2 = Xor(Xor1, Cup1);
                    NewWord[2] = Xor(Xor2, Cup2);
                    //---------------четвертый_байт_столбца ----------------------------
                    Cup1 = Multiplication03((bText[i])[j][0]);
                    Cup2 = Multiplication02((bText[i])[j][3]);
                    Xor1 = Xor(Cup1, (bText[i])[j][1]);
                    Xor2 = Xor(Xor1, (bText[i])[j][2]);
                    NewWord[3] = Xor(Xor2, Cup2);
                    //-----------------заполнение---------------------------------------
                    for (int k =0; k< WORDSIZE; k++)
                    {
                        (bText[i])[j][k] = NewWord[k];
                    }
                }
            }
        }

        public List<byte[][]> bEncode()
        {
            flag = true;
            List<byte[]> RKey = new List<byte[]>();  // получили раундовый ключ
            List<byte[][]> RoundKey = new List<byte[][]>();
            Nr = 10;
            RKey = ExpendKey();  // получили раундовый ключ
            RoundKey = GetRkey(RKey);
            Nr = 0;
            AddRoundKey(RoundKey[Nr]);              // Xor`им раундовый ключь и state`ы
            Nr = 1;
            while (Nr < 10)
            {
                SubBytes();
                ShiftRows();                   // двигаем строки
                MixColumns();                  // миксуем столбцы
                AddRoundKey(RoundKey[Nr]);     // Xor`им раундовый ключь и state`ы
                Nr++;
            }
            SubBytes();
            ShiftRows();                   // двигаем строки
            AddRoundKey(RoundKey[Nr]);     // Xor`им раундовый ключь и state`ы
            return bText;
        }

        #endregion

        #region Decode

        private byte[] InvRotWord(byte[] MyWord)
        {
            byte Cup = MyWord[3];
            MyWord[3] = MyWord[2];
            MyWord[2] = MyWord[1];
            MyWord[1] = MyWord[0];
            MyWord[0] = Cup;
            ;
            return MyWord;
        }
        private byte[] InvSubWord(byte[] MyWord)
        {
            //---------------Byte_1---------------------------
            byte B1_x = (byte)(MyWord[0] / 16);
            byte B1_y = (byte)(MyWord[0] % 16);
            MyWord[0] = (byte)(InvSubTable[B1_x, B1_y]);
            //---------------Byte_2---------------------
            byte B2_x = (byte)(MyWord[1] / 16);
            byte B2_y = (byte)(MyWord[1] % 16);
            MyWord[1] = (byte)(InvSubTable[B2_x, B2_y]);
            //---------------Byte_3---------------------
            byte B3_x = (byte)(MyWord[2] / 16);
            byte B3_y = (byte)(MyWord[2] % 16);
            MyWord[2] = (byte)(InvSubTable[B3_x, B3_y]);
            //---------------Byte_4---------------------
            byte B4_x = (byte)(MyWord[3] / 16);
            byte B4_y = (byte)(MyWord[3] % 16);
            MyWord[3] = (byte)(InvSubTable[B4_x, B4_y]);
            return MyWord;
        }
        private void InvMixColumns()
        {
            byte[] NewWord = new byte[WORDSIZE];
            byte Cup1, Cup2, Cup3, Cup4, Xor1, Xor2;
            for (int i = 0; i < bText.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //---------------первый_байт_столбца -------------------------------
                    Cup1 = Multiplication0e((bText[i])[j][0]);
                    Cup2 = Multiply0b((bText[i])[j][1]);
                    Cup3 = Multiplication0d((bText[i])[j][2]);
                    Cup4 = Multiplication09((bText[i])[j][3]);
                    Xor1 = Xor(Cup1, Cup2); Xor2 = Xor(Xor1, Cup3);
                    NewWord[0] = Xor(Xor2, Cup4);
                    //---------------второй_байт_столбца -------------------------------
                    Cup1 = Multiplication09((bText[i])[j][0]);
                    Cup2 = Multiplication0e((bText[i])[j][1]);
                    Cup3 = Multiply0b((bText[i])[j][2]);
                    Cup4 = Multiplication0d((bText[i])[j][3]);
                    Xor1 = Xor(Cup1, Cup2); Xor2 = Xor(Xor1, Cup3);
                    NewWord[1] = Xor(Xor2, Cup4);
                    //---------------третий_байт_столбца -------------------------------
                    Cup1 = Multiplication0d((bText[i])[j][0]);
                    Cup2 = Multiplication09((bText[i])[j][1]);
                    Cup3 = Multiplication0e((bText[i])[j][2]);
                    Cup4 = Multiply0b((bText[i])[j][3]);
                    Xor1 = Xor(Cup1, Cup2); Xor2 = Xor(Xor1, Cup3);
                    NewWord[2] = Xor(Xor2, Cup4); ;
                    //---------------четвертый_байт_столбца ----------------------------
                    Cup1 = Multiply0b((bText[i])[j][0]);
                    Cup2 = Multiplication0d((bText[i])[j][1]);
                    Cup3 = Multiplication09((bText[i])[j][2]);
                    Cup4 = Multiplication0e((bText[i])[j][3]);
                    Xor1 = Xor(Cup1, Cup2); Xor2 = Xor(Xor1, Cup3);
                    NewWord[3] = Xor(Xor2, Cup4);
                    //-----------------заполнение---------------------------------------
                    (bText[i])[j][0] = NewWord[0];
                    (bText[i])[j][1] = NewWord[1];
                    (bText[i])[j][2] = NewWord[2];
                    (bText[i])[j][3] = NewWord[3];
                }
            }
        }
        public List<byte[][]> bDecode()
        {
            flag = false;
            List<byte[]> RKey = new List<byte[]>();  // получили раундовый ключ
            List<byte[][]> RoundKey = new List<byte[][]>();
            Nr = 10;
            RKey = ExpendKey();  // получили раундовый ключ
            RoundKey = GetRkey(RKey);
            AddRoundKey(RoundKey[Nr]);              // Xor`им раундовый ключь и state`
            Nr--;
            while (Nr > 0)
            {
                ShiftRows();                      // двигаем строки
                SubBytes();                       // замена байтов
                AddRoundKey(RoundKey[Nr]);        // Xor`им раундовый ключь и state`ы
                InvMixColumns();                  // миксуем столбцы
                Nr--;
            }
            ShiftRows();                      // двигаем строки
            SubBytes();                       // заменяем байты
            AddRoundKey(RoundKey[Nr]);        // Xor`им раундовый ключь и state`ы
            return bText;
        }
        #endregion
        #endregion

        #region Преобразование string в byte[][] и в List<byte[][]>
        private void KeyFromStringToByte(out byte[][] bKey, string KeyO)
        {
            bKey = new byte[WORDINKEY][];

            byte[] temp = System.Text.Encoding.Default.GetBytes(KeyO);
            for (int i = 0, z=0; i < WORDINKEY; i++)
            {
                bKey[i] = new byte[WORDSIZE];
                for (int j = 0; j < WORDSIZE; j++, z++)
                {
                    if (z < temp.Length) bKey[i][j] = temp[z];
                    else if (z == temp.Length) bKey[i][j] = (byte)1;
                    else bKey[i][j] = (byte)0;
                }
            }
        }
        private List<byte[][]> TextFromStringOfIntToByte(string text)
        {

            string[] temp = text.Split(' ');
            int countAll = temp.Length;
            while (countAll % 16 != 0)
            {
                countAll++;
            }
            int countBlocks = countAll / 16;
            List<byte[][]> bText = new List<byte[][]>();

            for (int i = 0, z = 0; i < countBlocks; i++)
            {
                byte[][] blocks = new byte[WORDINSTATE][];
                for (int j = 0; j < WORDINSTATE; j++)
                {
                    blocks[j] = new byte[WORDSIZE];
                    for (int k = 0; k < WORDSIZE; k++, z++)
                    {
                        if (z < temp.Length) blocks[j][k] = (byte)int.Parse(temp[z]);
                        else if (z == temp.Length) blocks[j][k] = (byte)1;
                        else blocks[j][k] = (byte)0;
                    }
                }
                bText.Add(blocks);
            }
            return bText;
        }
        private List<byte[][]> TextFromStringToByte(string text)
        {

            byte[] temp = System.Text.Encoding.Default.GetBytes(text);
            int countAll = temp.Length;
            while (countAll % 16 != 0)
            {
                countAll++;
            }
            int countBlocks = countAll / 16;
            List<byte[][]> bText = new List<byte[][]>();
            for (int i = 0, z = 0; i < countBlocks; i++)
            {
                byte[][] blocks = new byte[WORDINSTATE][];
                for (int j = 0; j < WORDINSTATE; j++)
                {
                    blocks[j] = new byte[WORDSIZE];
                    for (int k = 0; k < WORDSIZE; k++, z++)
                    {
                        if (z < temp.Length) blocks[j][k] = temp[z];
                        else if (z == temp.Length) blocks[j][k] = (byte)1;
                        else blocks[j][k] = (byte)0;
                    }
                }
                bText.Add(blocks);
            }    
            return bText;
        }

        #endregion

        public string Decode(string TextO, string KeyO)
        {
            KeyFromStringToByte(out Key, KeyO);
            bText = TextFromStringOfIntToByte(TextO);
            bDecode();
            string text = "";
            for (int i = 0; i < bText.Count; i++)
            {
                for (int j = 0; j < WORDINSTATE; j++)
                {
                    text += System.Text.Encoding.Default.GetString(bText[i][j]);
                }
            }
            return text;
        }

        public string Encode(string TextO, string KeyO)
        {
            KeyFromStringToByte(out Key, KeyO);
            bText = TextFromStringToByte(TextO);
            bEncode();
            string text = "";
            for (int i = 0, z=0; i < bText.Count; i++)
            {
                for (int j = 0; j < WORDINSTATE; j++)
                {
                    for (int k = 0; k < WORDSIZE; k++, z++)
                    {
                        // Для вывода строки в байтах
                        if (z == bText.Count * WORDINSTATE * WORDSIZE - 1)
                        {
                            text += bText[i][j][k].ToString();
                            return text;
                        }
                        else
                            text += bText[i][j][k].ToString() + " ";
                    }
                    //text += System.Text.Encoding.Default.GetString(bText[i][j]);
                }
            }
            return text;
        }

    }
}
