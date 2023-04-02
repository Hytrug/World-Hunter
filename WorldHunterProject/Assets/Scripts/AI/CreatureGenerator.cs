using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    //gameobject onde ficao as creaturas geradas
    public GameObject creaturasInWorld;
    //objeto com a tag utilizada
    public GameObject creatura;
    //variavel para quardar o collider do objeto assima
    Collider creaturaCollider;
    //contador para o script saber quando n há mais árvores dentro da caixa
    int colliderCounter=0;
    //tamanho da caixa
    Vector3 size;
    //contador de creaturas
    int contadorCreaturas = 0;
    //lsita de objetos
    List <GameObject> creaturasList = new List<GameObject>();
    //guarda a informação do raycast
    RaycastHit hit;

    private void Start()
    {
        creaturaCollider = creatura.GetComponent<Collider>();
        size = transform.localScale;
    }

    private void Update()
    {
        Spawnar();
    }

    //quando algum objeto entrar dentro do trigger adicionar 1 ao contador
    private void OnTriggerEnter(Collider other)
    {
        creaturaCollider = creatura.GetComponent<Collider>();
        if(other.gameObject.tag == creaturaCollider.gameObject.tag)
        {
            colliderCounter++;
        }
    }

    //quando algum objeto sair de dentro do trigger retirar 1 ao contador
    private void OnTriggerExit(Collider other)
    {
        creaturaCollider = creatura.GetComponent<Collider>();
        if(other.gameObject.tag == creaturaCollider.gameObject.tag)
        {
            colliderCounter--;
        }
    }
    
    private void Spawnar()
    {
        if (colliderCounter==0)
        {
            int minRangeX=(int)(transform.position.x-(size.x/2));
            int maxRangeX=((int)(transform.position.x+(size.x/2)))+1;
            int minRangeZ=(int)(transform.position.z-(size.x/2));
            int maxRangeZ=((int)(transform.position.z+(size.x/2)))+1;

            int numeroDeTentaivas = Random.Range(1,101);

            for (int i = 0; i < numeroDeTentaivas; i++)
            {
                int chance = Random.Range(1,6);
                
                if (chance == 1)
                {
                    int randomX = Random.Range(minRangeX,maxRangeX);
                    int randomY = (1000-Random.Range(26,241));
                    int randomZ = Random.Range(minRangeZ,maxRangeZ);
                    int raycastDistance=860;
                    
                    if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
                    {
                        if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY))
                        {
                            creaturasList.Add((GameObject)Instantiate(creatura, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(0,0,0)));
                            creaturasList[contadorCreaturas].transform.parent = creaturasInWorld.transform;
                            contadorCreaturas++;
                        }
                    }
                }
            }
        }
    }
}
