using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    //objeto que vai guardar as àrvores na hierarquia
    public GameObject treesInWorld;

    //àrvores
    public GameObject pineTree1Tamanho2;
    public GameObject pineTree2Tamanho2;
    public GameObject pineTree3Tamanho2;
    public GameObject pineTree4Tamanho2;
    public GameObject pineTree5Tamanho2;
    public GameObject pineTree1Tamanho3;
    public GameObject pineTree2Tamanho3;
    public GameObject pineTree3Tamanho3;
    public GameObject pineTree4Tamanho3;
    public GameObject pineTree5Tamanho3;
    public GameObject pineTree1Tamanho4;
    public GameObject pineTree2Tamanho4;
    public GameObject pineTree3Tamanho4;
    public GameObject pineTree4Tamanho4;
    public GameObject pineTree5Tamanho4;
    
    public GameObject tree1Tamanho1;
    public GameObject tree2Tamanho1;
    public GameObject tree3Tamanho1;
    public GameObject tree4Tamanho1;
    public GameObject tree5Tamanho1;
    public GameObject tree1Tamanho2;
    public GameObject tree2Tamanho2;
    public GameObject tree3Tamanho2;
    public GameObject tree4Tamanho2;
    public GameObject tree5Tamanho2;
    public GameObject tree1Tamanho3;
    public GameObject tree2Tamanho3;
    public GameObject tree3Tamanho3;
    public GameObject tree4Tamanho3;
    public GameObject tree5Tamanho3;

    //variavel para definir a quantidade de arvores
    public int treeAmount;

    //vetor para definir a rotação dos objetos
    Vector3 rotationVector;

    //variaveis para obter numeros random
    int randomX;
    int randomY;
    int randomZ;
    int treeOption;

    //distancia que o raycast vai percorrer
    int raycastDistance;

    //variavel para todas as arvore | para ser utilizada no switch
    GameObject tree;

    //contador de àrvores
    int contadorTreeList = 0;

    //esta variavel diz só lá está a layer3
    int treeLayer = (1 << 3) | (1 << 4);

    //variavel que guarda os dados do raycast
    RaycastHit hit;

    //lista de arvores
    List <GameObject> treeList = new List<GameObject>();

    //metodo para criar as primeiras arvores
    public void GeneratePineTree1Tamanho2()
    {
        //isto inverte a variavel de layers acima, ou seja pertecem todas as layers exceto a 3
        int inverse = ~(treeLayer);
        //diz que o vector3 acima vai servir para rotação
        rotationVector = transform.rotation.eulerAngles;
        //for para spawnar o numero de arvores escolhido
        for (int z = 0; z < treeAmount ; z++)
        {
            //arvore que vai ser spawnada, random
            treeOption = Random.Range(1,31);
            //switch para trocar os valores das variaveis dependendo da arvor
            switch (treeOption)
            {
                case 1:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=880;
                    tree = pineTree1Tamanho2;
                    break;
                }

                case 2:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=880;
                    tree = pineTree2Tamanho2;
                    break;
                }

                case 3:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=880;
                    tree = pineTree3Tamanho2;
                    break;
                }

                case 4:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=880;
                    tree = pineTree4Tamanho2;
                    break;
                }
                
                case 5:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=880;
                    tree = pineTree5Tamanho2;
                    break;
                }

                case 6:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=860;
                    tree = pineTree1Tamanho3;
                    break;
                }

                case 7:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=860;
                    tree = pineTree2Tamanho3;
                    break;
                }

                case 8:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=860;
                    tree = pineTree3Tamanho3;
                    break;
                }

                case 9:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=860;
                    tree = pineTree4Tamanho3;
                    break;
                }

                case 10:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=860;
                    tree = pineTree5Tamanho3;
                    break;
                }

                case 11:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=843;
                    tree = pineTree1Tamanho4;
                    break;
                }
                
                case 12:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=843;
                    tree = pineTree2Tamanho4;
                    break;
                }

                case 13:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=843;
                    tree = pineTree3Tamanho4;
                    break;
                }

                case 14:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=843;
                    tree = pineTree4Tamanho4;
                    break;
                }

                case 15:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=843;
                    tree = pineTree5Tamanho4;
                    break;
                }

                case 16:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree1Tamanho1;
                    break;
                }

                case 17:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree2Tamanho1;
                    break;
                }

                case 18:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree3Tamanho1;
                    break;
                }

                case 19:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree4Tamanho1;
                    break;
                }

                case 20:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree5Tamanho1;
                    break;
                }

                case 21:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree1Tamanho2;
                    break;
                }

                case 22:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree2Tamanho2;
                    break;
                }

                case 23:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree3Tamanho2;
                    break;
                }

                case 24:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree4Tamanho2;
                    break;
                }

                case 25:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree5Tamanho2;
                    break;
                }

                case 26:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree1Tamanho3;
                    break;
                }

                case 27:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree2Tamanho3;
                    break;
                }

                case 28:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree3Tamanho3;
                    break;
                }

                case 29:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree4Tamanho3;
                    break;
                }

                case 30:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=905;
                    tree = tree5Tamanho3;
                    break;
                }
                
                default:
                {
                    randomX = Random.Range(-2500,2501);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(-2500,2501);
                    raycastDistance=935;
                    tree = tree1Tamanho1;
                    break;
                }
            }
            //escolher um numero random para a rotacao
            rotationVector.y = Random.Range(0,361);
            //verifica se esta arvore n vai spawnar acima do seu limite
            if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
            {
                //verifica se a arvor n vai ser criada em cima de algum objeto
                if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                {
                    //cria a arvor nas coordenadas do raycast
                    treeList.Add((GameObject)Instantiate(tree, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                    //poem as arvores da lista dentro do seu objeto na hierarquia
                    treeList[contadorTreeList].transform.parent = treesInWorld.transform;
                    contadorTreeList++;
                }
            }
        }
    }

    //criar o resto das arvores
    public void UpdateTree(int minRangeX, int maxRangeX, int minRangeZ, int maxRangeZ)
    {
        int inverse = ~(treeLayer);
        rotationVector = transform.rotation.eulerAngles;
        for (int z = 0; z < (treeAmount/5) ; z++)
        {
            treeOption = Random.Range(1,31);
            switch (treeOption)
            {
                case 1:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=880;
                    tree = pineTree1Tamanho2;
                    break;
                }

                case 2:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=880;
                    tree = pineTree2Tamanho2;
                    break;
                }

                case 3:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=880;
                    tree = pineTree3Tamanho2;
                    break;
                }

                case 4:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=880;
                    tree = pineTree4Tamanho2;
                    break;
                }
                
                case 5:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,121));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=880;
                    tree = pineTree5Tamanho2;
                    break;
                }

                case 6:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=860;
                    tree = pineTree1Tamanho3;
                    break;
                }

                case 7:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=860;
                    tree = pineTree2Tamanho3;
                    break;
                }

                case 8:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=860;
                    tree = pineTree3Tamanho3;
                    break;
                }

                case 9:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=860;
                    tree = pineTree4Tamanho3;
                    break;
                }

                case 10:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,141));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=860;
                    tree = pineTree5Tamanho3;
                    break;
                }

                case 11:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=843;
                    tree = pineTree1Tamanho4;
                    break;
                }
                
                case 12:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=843;
                    tree = pineTree2Tamanho4;
                    break;
                }

                case 13:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=843;
                    tree = pineTree3Tamanho4;
                    break;
                }

                case 14:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=843;
                    tree = pineTree4Tamanho4;
                    break;
                }

                case 15:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(90,158));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=843;
                    tree = pineTree5Tamanho4;
                    break;
                }

                case 16:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree1Tamanho1;
                    break;
                }

                case 17:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree2Tamanho1;
                    break;
                }

                case 18:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree3Tamanho1;
                    break;
                }

                case 19:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree4Tamanho1;
                    break;
                }

                case 20:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree5Tamanho1;
                    break;
                }

                case 21:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree1Tamanho2;
                    break;
                }

                case 22:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree2Tamanho2;
                    break;
                }

                case 23:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree3Tamanho2;
                    break;
                }

                case 24:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree4Tamanho2;
                    break;
                }

                case 25:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(60,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree5Tamanho2;
                    break;
                }

                case 26:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree1Tamanho3;
                    break;
                }

                case 27:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree2Tamanho3;
                    break;
                }

                case 28:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree3Tamanho3;
                    break;
                }

                case 29:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree4Tamanho3;
                    break;
                }

                case 30:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(66,96));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=905;
                    tree = tree5Tamanho3;
                    break;
                }
                
                default:
                {
                    randomX = Random.Range(minRangeX,maxRangeX);
                    randomY = (1000-Random.Range(26,66));
                    randomZ = Random.Range(minRangeZ,maxRangeZ);
                    raycastDistance=935;
                    tree = tree1Tamanho1;
                    break;
                }
            }
            rotationVector.y = Random.Range(0,361);
            if (!Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, raycastDistance))
            {
                if(Physics.Raycast(new Vector3 (randomX, 1000, randomZ), Vector3.down, out hit, randomY, inverse))
                {
                    treeList.Add((GameObject)Instantiate(tree, new Vector3(randomX, hit.point.y, randomZ), transform.rotation = Quaternion.Euler(rotationVector)));
                    treeList[contadorTreeList].transform.parent = treesInWorld.transform;
                    contadorTreeList++;
                }
            }
        }
    }
}
