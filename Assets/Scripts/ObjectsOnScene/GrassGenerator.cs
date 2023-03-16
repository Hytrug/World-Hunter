using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    //gameobject para guardar a relva gerada
    public GameObject grassInWorld;
    //gameobject para a relva e o arbusto
    public GameObject grass;
    public GameObject bush;
    //gameobject para guardar o objeto que vai ser gerado
    GameObject spawn;
    //numero de relva
    public int grassAmount;
    //vetor para a rotação
    Vector3 rotationVector;
    //vetor para o tamanho
    //Vector3 size;
    //numero para o tamanho
    //int sizeNum;
    //numeros random para as coordenadas
    int randomX;
    int randomY;
    int randomZ;
    //distanci ado raycast
    int raycastDistance;
    //contador de relva
    int contadorGrassList = 0;
    //utilizar apenas a layer 3
    int grassLayer = (1 << 3) | (1 << 4);
    //numero para utilizar no switch
    int grassOption;
    //guarda a informação do raycast
    RaycastHit hit;
    //lsita de objetos
    List <GameObject> grassList = new List<GameObject>();

    public void GenerateGrass()
    {
        //inverte a variavel fazendo todos serem utilizados menos o 3
        int inverse = ~(grassLayer);
        //torna o vetor num vetor de rotação
        rotationVector = transform.rotation.eulerAngles;
        //torna o vetor num vertor de tamanho
        //size = transform.localScale;
        for (int z = 0; z < grassAmount ; z++)
        {
            //escolher o tipo de relva
            grassOption = Random.Range(1,3);
            //swicth para trocar os dados dependendo do tipo de relva
            switch (grassOption)
            {
                case 1:
                {
                    //poem um numero random nas variaveis das coordenadas
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=910;
                    //poem o obejto para spawnar dentro da variavel que vai ser utilizada
                    spawn = bush;
                    break;
                }

                case 2:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=910;
                    spawn = grass;
                    break;
                }

                default:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=910;
                    spawn = bush;
                    break;
                }
            }
            //define a rotação do objeto
            rotationVector.y = Random.Range(0,361);
            //define o tamanho
            /*sizeNum = Random.Range(1,501);
            size.x=sizeNum;
            size.y=sizeNum;
            size.z=sizeNum;*/
            //testa se o fim do raycast não está acima do tamnho permitido
            if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
            {
                //testa se encontra chão no local escolhido
                if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                {
                    //gera a relva no final do raycast
                    grassList.Add((GameObject)Instantiate(spawn, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                    //dá o tamanho correto à relva
                    //grassList[contadorGrassList].transform.localScale = new Vector3(size.x, size.y, size.z);
                    //poem a relva dentro do seu gameobject
                    grassList[contadorGrassList].transform.parent = grassInWorld.transform;
                    contadorGrassList++;
                }
            }
        }
    }
    //identico ao metodo acima, mas as suas coordenadas podem ser alteradas
    public void UpdateGrass(int minRangeX, int maxRangeX, int minRangeZ, int maxRangeZ)
    {
        int inverse = ~(grassLayer);
        rotationVector = transform.rotation.eulerAngles;
        //size = transform.localScale;
        for (int z = 0; z < (grassAmount/5) ; z++)
        {
            grassOption = Random.Range(1,3);
            switch (grassOption)
            {
                case 1:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=910;
                    spawn = bush;
                    break;
                }

                case 2:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=910;
                    spawn = grass;
                    break;
                }

                default:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(40,91));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=910;
                    spawn = bush;
                    break;
                }
            }
            rotationVector.y = Random.Range(0,361);
            /*sizeNum = Random.Range(1,2);
            size.x=sizeNum;
            size.y=sizeNum;
            size.z=sizeNum;*/
            if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
            {
                if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                {
                    grassList.Add((GameObject)Instantiate(spawn, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                    //grassList[contadorGrassList].transform.localScale = new Vector3(size.x, size.y, size.z);
                    grassList[contadorGrassList].transform.parent = grassInWorld.transform;
                    contadorGrassList++;
                }
            }
        }
    }
}
