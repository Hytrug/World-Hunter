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
