using System.IO;

namespace lab2._1
{
    public class TextFileService:  IFileService
    {
        public string Open(string filename)
        {
            string arr = "";
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                arr = sr.ReadToEnd();
            }
            return arr;
        }

        public void Save(string filename, string dataStringArrayOrigAndResult)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
            {
                sw.Write(dataStringArrayOrigAndResult);
            }
        }
    }
}
