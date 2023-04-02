using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableObjectLibrary : MonoBehaviour
{
    public static Dictionary<int, GameObject> SavableObjects;

    [SerializeField]
    private GameObject[] RegisteredObjects;

    private void Awake() 
    {
        SavableObjects = new Dictionary<int, GameObject>();

        for (int i = 0; i < RegisteredObjects.Length; i++)
        {
            int IdToRegister = RegisteredObjects[i].GetComponent<SavableObjectId>().ID;
            SavableObjects.Add(IdToRegister, RegisteredObjects[i]);
        }
    }
}
