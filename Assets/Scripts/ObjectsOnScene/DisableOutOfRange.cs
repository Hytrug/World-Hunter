using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutOfRange : MonoBehaviour
{
    public float distanceMultiplier = 5f;
    private float fov; // Campo de visão vertical da câmera em graus
    private float distance; // Distância máxima que os objetos podem estar da câmera

    private Collider[] colliders;
    private Vector3 cameraPosition;

    private void Start()
    {
        fov = Camera.main.fieldOfView;
        // Calcula a distância máxima com base no campo de visão vertical e na altura do frustum
        distance = (Camera.main.farClipPlane * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad))*5;
        colliders = GetComponentsInChildren<Collider>();
        cameraPosition = Camera.main.transform.position;
        DisableObjects();
    }

    private void Update()
    {    
        if (Vector3.Distance(Camera.main.transform.position, cameraPosition) < distance)
        {
            EnableObjects();// Ativa todos os objetos
            cameraPosition = Camera.main.transform.position;
            DisableObjects();// Desativa todos os objetos que estão fora da distância máxima
        }
    }

    private void EnableObjects()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.SetActive(true);// Ativa o objeto correspondente ao collider
        }
    }

    private void DisableObjects()
    {

        foreach (Collider collider in colliders)
        {
            if (Vector3.Distance(collider.transform.position, cameraPosition) > distance)// Verifica se o objeto está fora da distância máxima
            {
                collider.gameObject.SetActive(false);// Desativa o objeto correspondente ao collider
            }
        }
    }
}
