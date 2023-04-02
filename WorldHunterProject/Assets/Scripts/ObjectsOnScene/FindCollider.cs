using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCollider : MonoBehaviour
{
    //utiliza o script TreeGenerator
    public TreeGenerator treeGenerator;
    public RockGenerator rockGenerator;
    public GrassGenerator grassGenerator;

    //objeto com a tag utilizada
    public GameObject tree;
    //variavel para quardar o collider do objeto assima
    Collider treeCollider;
    //contador para o script saber quando n há mais árvores dentro da caixa
    int colliderCounter=0;
    //variavel para permitir o update iniciar
    bool comecarUpdate = false;
    //localização da caixa
    Vector3 posicaoDoObjeto;
    //tamanho da caixa
    Vector3 size;
    //portal
    public GameObject portal;
    RaycastHit hit;


    void Start()
    {
        //StartCoroutine serve para começar a contar
        StartCoroutine(Esperar());
        //definir o que fica dentro da variavel
        treeCollider = tree.GetComponent<Collider>();
        size = transform.localScale;
    }

    void Update()
    {
        //so comeca quando isto se definir true
        if(comecarUpdate)
        {
            posicaoDoObjeto = transform.position;
            //quando o contador for 0 criar mais objetos
            if (colliderCounter==0)
            {
                //apartir da posição da caixa e do seu tamanho sabemos onde spawnar os objetos
                treeGenerator.UpdateTree((int)(posicaoDoObjeto.x-(size.x/2)), ((int)(posicaoDoObjeto.x+(size.x/2)))+1, (int)(posicaoDoObjeto.z-(size.z/2)), ((int)(posicaoDoObjeto.z+(size.z/2)))+1);
                rockGenerator.UpdateRock((int)(posicaoDoObjeto.x-(size.x/2)), ((int)(posicaoDoObjeto.x+(size.x/2)))+1, (int)(posicaoDoObjeto.z-(size.z/2)), ((int)(posicaoDoObjeto.z+(size.z/2)))+1);
                grassGenerator.UpdateGrass((int)(posicaoDoObjeto.x-(size.x/2)), ((int)(posicaoDoObjeto.x+(size.x/2)))+1, (int)(posicaoDoObjeto.z-(size.z/2)), ((int)(posicaoDoObjeto.z+(size.z/2)))+1);
                int minRangeX = (int)(transform.position.x - (size.x / 2));
                int maxRangeX = ((int)(transform.position.x + (size.x / 2))) + 1;
                int minRangeZ = (int)(transform.position.z - (size.x / 2));
                int maxRangeZ = ((int)(transform.position.z + (size.x / 2))) + 1;

                int numeroDeTentaivas = Random.Range(1, 10);

                for (int i = 0; i < numeroDeTentaivas; i++)
                {
                    int chance = Random.Range(1, 6);

                    if (chance == 1)
                    {
                        int randomX = Random.Range(minRangeX, maxRangeX);
                        int randomY = (1000 - Random.Range(26, 241));
                        int randomZ = Random.Range(minRangeZ, maxRangeZ);
                        int raycastDistance = 860;

                        if (!Physics.Raycast(new Vector3(randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
                        {
                            if (Physics.Raycast(new Vector3(randomX, 1000, randomZ), Vector3.down, out hit, randomY))
                            {
                                Instantiate(portal, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(0, 0, 0));
                            }
                        }
                    }
                }
            }
        }
    }

    //esperar
    IEnumerator Esperar()
    {
        //esperar 6 segundos
        yield return new WaitForSeconds(6);
        //tornar a variavel que permite o update comecar true
        comecarUpdate=true;
    }

    //quando algum objeto entrar dentro do trigger adicionar 1 ao contador
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == treeCollider.gameObject.tag)
        {
            colliderCounter++;
        }
    }

    //quando algum objeto sair de dentro do trigger retirar 1 ao contador
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == treeCollider.gameObject.tag)
        {
            colliderCounter--;
        }
    }
}
