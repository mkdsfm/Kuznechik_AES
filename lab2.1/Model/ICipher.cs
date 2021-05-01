namespace lab2._1
{
    public interface ICipher
    {
        string Encode(string TextO, string KeyO); // шифрование строк
        string Decode(string TextO, string KeyO); // дешифрование строк
    }
}
