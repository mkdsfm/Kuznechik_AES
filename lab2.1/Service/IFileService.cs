namespace lab2._1
{
    public interface IFileService
    {
        string Open(string filename);
        void Save(string filename, string dataStringArrayOrigAndResult);
    }
}
