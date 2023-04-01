using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveObjects : MonoBehaviour
{
    public PlayerEnergy energia;
    public Timer timer;
    private IDataService dataService = new JsonDataService();
    public Transform player;

    [System.Serializable]
    public class SavableObjectsInScene
    {
        public SavableObject[] SavableObjects;
    }

    [System.Serializable]
    public class SavableObject
    {
        /*public Vector3 WorldPosition;
        public Quaternion WorldRotation;*/
        public float ObjectWorldPositionX;
        public float ObjectWorldPositionY;
        public float ObjectWorldPositionZ;
        public float ObjectWorldRotationX;
        public float ObjectWorldRotationY;
        public float ObjectWorldRotationZ;
        public float ObjectWorldSizeX;
        public float ObjectWorldSizeY;
        public float ObjectWorldSizeZ;
        public int Id;
    }

    [System.Serializable]
    public class PlayerData
    {
        /*public Vector3 WorldPosition;
        public Quaternion WorldRotation;*/
        public float WorldPositionX;
        public float WorldPositionY;
        public float WorldPositionZ;
        public float WorldRotationX;
        public float WorldRotationY;
        public float WorldRotationZ;
        public float WorldSizeX;
        public float WorldSizeY;
        public float WorldSizeZ;
    }

    public void Save()
    {
        SaveObjectsMethod();
        SavePlayerMethod();
    }

    private void SavePlayerMethod()
    {
        Scene scene = SceneManager.GetActiveScene();

        PlayerData data;

        //vetor para a rotação
        Vector3 rotationVector;

        rotationVector.x=player.eulerAngles.x;
        rotationVector.y=player.eulerAngles.y;
        rotationVector.z=player.eulerAngles.z;
        data = new PlayerData()
        {
            /*WorldPosition = ObjectsInScene[i].transform.position,
            WorldRotation = ObjectsInScene[i].transform.rotation,*/
            WorldPositionX = player.position.x,
            WorldPositionY = player.position.y,
            WorldPositionZ = player.position.z,
            WorldRotationX = rotationVector.x,
            WorldRotationY = rotationVector.y,
            WorldRotationZ = rotationVector.z,
            WorldSizeX = player.localScale.x,
            WorldSizeY = player.localScale.y,
            WorldSizeZ = player.localScale.z, 
        };

        if (dataService.SaveData ("/player"+scene.name+".json", data))
        {
            Debug.Log ("És lindo");
        }
        else
        {
            Debug.LogError("Não possivel salvar o ficheiro");
        }

        PlayerPrefs.SetFloat("currentTime", timer.currentTime);
    }

    private void SaveObjectsMethod()
    {
        Scene scene = SceneManager.GetActiveScene();
        SavableObjectId[] ObjectsInScene = FindObjectsOfType<SavableObjectId>();
        //vetor para a rotação
        Vector3 rotationVector;

        SavableObjectsInScene ObjectData = new SavableObjectsInScene()
        {
            SavableObjects = new SavableObject[ObjectsInScene.Length]
        };

        for (int i = 0; i < ObjectData.SavableObjects.Length; i++)
        {
            rotationVector.x=ObjectsInScene[i].transform.eulerAngles.x;
            rotationVector.y=ObjectsInScene[i].transform.eulerAngles.y;
            rotationVector.z=ObjectsInScene[i].transform.eulerAngles.z;
            //Debug.Log(LoadedObjectData.SavableObjects[i].Id + " "+ Quaternion.Euler(rotationVector));

            ObjectData.SavableObjects[i] = new SavableObject
            {
                /*WorldPosition = ObjectsInScene[i].transform.position,
                WorldRotation = ObjectsInScene[i].transform.rotation,*/
                ObjectWorldPositionX = ObjectsInScene[i].transform.position.x,
                ObjectWorldPositionY = ObjectsInScene[i].transform.position.y,
                ObjectWorldPositionZ = ObjectsInScene[i].transform.position.z,
                ObjectWorldRotationX = rotationVector.x,
                ObjectWorldRotationY = rotationVector.y,
                ObjectWorldRotationZ = rotationVector.z,
                ObjectWorldSizeX = ObjectsInScene[i].transform.localScale.x,
                ObjectWorldSizeY = ObjectsInScene[i].transform.localScale.y,
                ObjectWorldSizeZ = ObjectsInScene[i].transform.localScale.z,
                Id = ObjectsInScene[i].ID
            };
        }

        dataService.SaveData ("/objects"+scene.name+".json", ObjectData);
    }

    public void Carregar()
    {
        CarregarObjects();
        CarregarPlayer();
    }

    public void CarregarPlayer()
    {
        //vetor para a rotação
        Vector3 rotationVector;
        //torna o vetor num vetor de rotação
        rotationVector = transform.rotation.eulerAngles;

        Scene scene = SceneManager.GetActiveScene();
        PlayerData LoadedObjectData = dataService.LoadData<PlayerData>("/player"+scene.name+".json");

        rotationVector.x=LoadedObjectData.WorldRotationX;
        rotationVector.y=LoadedObjectData.WorldRotationY;
        rotationVector.z=LoadedObjectData.WorldRotationZ;

        player.position = new Vector3((LoadedObjectData.WorldPositionX+5),(LoadedObjectData.WorldPositionY+100),LoadedObjectData.WorldPositionZ);
        player.rotation = Quaternion.Euler(rotationVector);
        player.localScale = new Vector3(LoadedObjectData.WorldSizeX,LoadedObjectData.WorldSizeY,LoadedObjectData.WorldSizeZ);
        energia.energia = PlayerPrefs.GetFloat("energia");
        energia.perda = PlayerPrefs.GetFloat("perda");
    }

    public void CarregarObjects()
    {
        //vetor para a rotação
        Vector3 rotationVector;
        //torna o vetor num vetor de rotação
        rotationVector = transform.rotation.eulerAngles;

        Scene scene = SceneManager.GetActiveScene();
        SavableObjectsInScene LoadedObjectData = dataService.LoadData<SavableObjectsInScene>("/objects"+scene.name+".json");

        SavableObjectId[] ObjectsInScene = FindObjectsOfType<SavableObjectId>();

        for(int i = 0; i < ObjectsInScene.Length; i++)
        {
            Destroy(ObjectsInScene[i].gameObject);
        }

        for (int i=0; i<LoadedObjectData.SavableObjects.Length; i++)
        {
            rotationVector.x=LoadedObjectData.SavableObjects[i].ObjectWorldRotationX;
            rotationVector.y=LoadedObjectData.SavableObjects[i].ObjectWorldRotationY;
            rotationVector.z=LoadedObjectData.SavableObjects[i].ObjectWorldRotationZ;

            GameObject justAnObject = Instantiate(SavableObjectLibrary.SavableObjects[LoadedObjectData.SavableObjects[i].Id], new Vector3 (LoadedObjectData.SavableObjects[i].ObjectWorldPositionX, LoadedObjectData.SavableObjects[i].ObjectWorldPositionY, LoadedObjectData.SavableObjects[i].ObjectWorldPositionZ), transform.rotation = Quaternion.Euler(rotationVector));
            justAnObject.transform.rotation = Quaternion.Euler(rotationVector);
            justAnObject.transform.localScale = new Vector3(LoadedObjectData.SavableObjects[i].ObjectWorldSizeX, LoadedObjectData.SavableObjects[i].ObjectWorldSizeY, LoadedObjectData.SavableObjects[i].ObjectWorldSizeZ);
        }
    }
}
