using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Existe Data. A eliminar o ficheiro antigo e a criar um novo");
                File.Delete(path);
            }
            else
            {
                Debug.Log("A criar um ficheiro pela primeira vez");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Impossivel salvar os dados devido a: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public bool ProcurarFile(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (File.Exists(path))
        {
            Debug.Log("Existe Data.");
            return true;
        }
        else
        {
            Debug.Log("Não existe data");
            return false;
        }
    }

    public void Delete(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;
        if (File.Exists(path))
        {
            Debug.Log("Existe Data. A eliminar o ficheiro");
            File.Delete(path);
        }
        else
        {
            Debug.Log("Não existe data para eliminar");
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        if(!File.Exists(path))
        {
            Debug.LogError($"Não é possivel iniciar o ficheiro em {path}. File não existe");
            throw new FileNotFoundException($"{path} não existe");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText (path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Impossivel iniciar os dados devido a: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
