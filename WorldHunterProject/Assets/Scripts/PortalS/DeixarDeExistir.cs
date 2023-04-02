using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeixarDeExistir : MonoBehaviour
{
    public GameObject portalDeleter;

    private void Start()
    {
        StartCoroutine(Destruir());
    }

    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(5);
        Destroy(portalDeleter);
    }
}
