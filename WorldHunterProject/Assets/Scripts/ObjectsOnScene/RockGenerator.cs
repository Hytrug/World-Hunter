using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    //gameobject onde ficao as pedras geradas
    public GameObject rocksInWorld;
    //gameobject para por a pedra
    public GameObject rock;
    //numero de pedras geradas
    public int rockAmount;
    //vetor para a rotação
    Vector3 rotationVector;
    //vetor para o tamanho
    Vector3 size;
    //numero para o tamanho
    int sizeNum;
    //numeros random para as coordenadas
    int randomX;
    int randomY;
    int randomZ;
    //distanci ado raycast
    int raycastDistance;
    //contador de pedras
    int contadorRockList = 0;
    //utilizar apenas a layer 3
    int rockLayer = (1 << 3) | (1 << 4);
    //guarda a informação do raycast
    RaycastHit hit;
    //lsita de objetos
    List <GameObject> rockList = new List<GameObject>();
    //objeto com a tag utilizada
    public GameObject treeObjectCollider;
    //variavel para quardar o collider do objeto assima
    Collider treeCollider;
    //contador para o script saber quando n há mais árvores dentro da caixa
    int colliderCounter=0;

    //quando algum objeto entrar dentro do trigger adicionar 1 ao contador
    private void OnTriggerEnter(Collider other)
    {
        treeCollider = treeObjectCollider.GetComponent<Collider>();
        if(other.gameObject.tag == treeCollider.gameObject.tag)
        {
            colliderCounter++;
        }
    }

    //quando algum objeto sair de dentro do trigger retirar 1 ao contador
    private void OnTriggerExit(Collider other)
    {
        treeCollider = treeObjectCollider.GetComponent<Collider>();
        if(other.gameObject.tag == treeCollider.gameObject.tag)
        {
            colliderCounter--;
        }
    }

    public void GenerateRock()
    {
        if (colliderCounter==0)
        {
            //inverte a rocklayer tornando as layers afetadas todas menos a 3
            int inverse = ~(rockLayer);
            //torna o vetor num vetor de rotação
            rotationVector = transform.rotation.eulerAngles;
            //torna o vetor num vertor de tamanho
            size = transform.localScale;
            //for para gerar as pedras utilizando o rockamount
            for (int z = 0; z < rockAmount ; z++)
            {
                //poem um numero random nas variaveis das coordenadas
                randomX = Random.Range(-2500,2501);
                randomY = (1000-Random.Range(1,351));
                randomZ = Random.Range(-2500,2501);
                raycastDistance=650;
                //dá um numero random ao vetor de rotação
                rotationVector.y = Random.Range(0,361);
                //o modelo da pedra estava a ser gerado de lado, por isso foi preciso giralo
                rotationVector.x = -90;
                //define o tamanho
                sizeNum = Random.Range(1,501);
                size.x=sizeNum;
                size.y=sizeNum;
                size.z=sizeNum;
                //testa se o fim do raycast não está acima do tamnho permitido
                if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
                {
                    //testa se encontra chão no local escolhido
                    if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                    {
                        //gera a pedra no final do raycast
                        rockList.Add((GameObject)Instantiate(rock, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                        //dá o tamanho correto à pedra
                        rockList[contadorRockList].transform.localScale = new Vector3(size.x, size.y, size.z);
                        //poem as pedras dentro do seu gameobject
                        rockList[contadorRockList].transform.parent = rocksInWorld.transform;
                        contadorRockList++;
                    }
                }
            }
        }
    }
    //identico ao metodo acima, mas as suas coordenadas podem ser alteradas
    public void UpdateRock(int minRangeX, int maxRangeX, int minRangeZ, int maxRangeZ)
    {
        int inverse = ~(rockLayer);
        rotationVector = transform.rotation.eulerAngles;
        size = transform.localScale;
        for (int z = 0; z < (rockAmount/5) ; z++)
        {
            randomX = Random.Range(minRangeX,maxRangeX);
            randomY = (1000-Random.Range(1,351));
            randomZ = Random.Range(minRangeZ,maxRangeZ);
            raycastDistance=650;
            
            rotationVector.y = Random.Range(0,361);
            rotationVector.x = -90;
            sizeNum = Random.Range(1,501);
            size.x=sizeNum;
            size.y=sizeNum;
            size.z=sizeNum;
            if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
            {
                if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                {
                    rockList.Add((GameObject)Instantiate(rock, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                    rockList[contadorRockList].transform.localScale = new Vector3(size.x, size.y, size.z);
                    rockList[contadorRockList].transform.parent = rocksInWorld.transform;
                    contadorRockList++;
                }
            }
        }
    }
}
