using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCreatures : MonoBehaviour
{
    private int contaCreaturas;
    private int resetContaCreaturas=0;
    public GameObject creatura;
    public Transform player;
    RaycastHit hit;

    private void Start()
    {
        contaCreaturas = PlayerPrefs.GetInt("contaCreaturas");
        PlayerPrefs.SetInt("contaCreaturas", resetContaCreaturas);
        Spawn();
    }

    private void Update()
    {
        Debug.Log(contaCreaturas);
    }

    private void Spawn()
    {
        for (int i = 0; i < contaCreaturas; i++)
        {
            float randomX = Random.Range(player.position.x+50,player.position.x-50);
            float randomZ = Random.Range(player.position.z+50,player.position.z-50);
            if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, 1000))
            {
                Instantiate(creatura, hit.point, transform.rotation = Quaternion.Euler(new Vector3(0,0,0)));
            }
            else
            {
                contaCreaturas++;
            }
        }  
    }
}
