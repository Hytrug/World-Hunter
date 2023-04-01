public interface IDataService
{
    bool SaveData<T>(string relativePath, T data);

    bool ProcurarFile(string relativePath);

    void Delete(string relativePath);

    T LoadData<T>(string relativePath);
}
