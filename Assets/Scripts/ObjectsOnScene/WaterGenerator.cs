using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public GameObject Waterplane;

    public Transform viewer;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;

    //variaveis da criação da água
    RaycastHit hit;
    int waterPositionX=0;
    int waterPositionZ=0;
    List <GameObject> waterListUniversal = new List<GameObject>(); 
    List <GameObject> waterListCorrect = new List<GameObject>(); 
    List <GameObject> waterListEsq = new List<GameObject>();
    List <GameObject> waterListDir = new List<GameObject>();
    List <GameObject> waterListCima = new List<GameObject>();
    List <GameObject> waterListBaix = new List<GameObject>();

    //variaveis para atualizar a água
    int waterPositionZUpdateSaveEsq=-1490;
    int waterPositionZUpdateSaveDir=1500;
    int waterPositionXUpdateSaveBaix=-1500;
    int waterPositionXUpdateSaveCima=1500;
    int waterPositionXUpdateEsq=1500;
    int waterPositionXUpdateDir=-1500;
    int waterPositionXUpdateBaix=0;
    int waterPositionXUpdateCima=0;
    int waterPositionZUpdateEsq=0;
    int waterPositionZUpdateDir=-0;
    int waterPositionZUpdateCima=-1500;
    int waterPositionZUpdateBaix=1500;
    int waterLayer = 1 << 4;
    Vector2 viewerPositionOldForWater;
    Vector2 viewerPositionOldForWaterNum;

    //Gerar objetos
    public TreeGenerator treeGenerator;
    public RockGenerator rockGenerator;
    public GrassGenerator grassGenerator;

    //esperar para dar update na água
    bool esperar = false;

    private void Start()
    {
        //StartCoroutine serve para começar a contar
        StartCoroutine(CreateWater());
        viewerPositionOldForWater= new Vector2(viewer.position.x, viewer.position.z);
        viewerPositionOldForWaterNum= new Vector2(viewer.position.x, viewer.position.z);
        waterPositionZUpdateSaveEsq=(int)viewerPosition.y-1490;
        waterPositionZUpdateSaveDir=(int)viewerPosition.y+1500;
        waterPositionXUpdateSaveBaix=(int)viewerPosition.x-1500;
        waterPositionXUpdateSaveCima=(int)viewerPosition.x+1500;
        waterPositionXUpdateEsq=(int)viewerPosition.x+1500;
        waterPositionXUpdateDir=(int)viewerPosition.x-1500;
        waterPositionZUpdateCima=(int)viewerPosition.y-1500;
        waterPositionZUpdateBaix=(int)viewerPosition.y+1500;
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
        }

        if (esperar)
        {
            VerificarPlayerPositionForWaterPosition();
        }
    }

    IEnumerator CreateWater()
    {
        //esperar 5 segundos
        yield return new WaitForSeconds(5f);
        waterPositionX=((int)viewerPosition.x-1500);
        Debug.Log(waterPositionX);
        Debug.Log(waterPositionZ);
        esperar = true;
        for(int x = 0; x < 300 ; x++)
        {
            //se o raycast não atingir nada ent...
            if (!Physics.Raycast(new Vector3 (waterPositionX, 1000, -2200), Vector3.down, out hit, 970))
            {
                waterListUniversal.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionX, 26, (viewerPosition.y-1500)), Quaternion.identity));
            }else
            {
                waterListUniversal.Add(null);
            }
            //waterPositionZ=-1490;
            waterPositionZ=((int)viewerPosition.y-1490);
            for(int z = 0; z < 300 ; z++)
            {
                if (!Physics.Raycast(new Vector3 (waterPositionX, 1000, waterPositionZ), Vector3.down, out hit, 970))
                {
                    waterListUniversal.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionX, 26, waterPositionZ), Quaternion.identity));
                }else
                {
                    waterListUniversal.Add(null);
                }
                waterPositionZ=waterPositionZ+10;
            }
            waterPositionX=waterPositionX+10;
        }

        IniciarWaterList();
        treeGenerator.GeneratePineTree1Tamanho2();
        rockGenerator.GenerateRock();
        grassGenerator.GenerateGrass();
    }

    //Iniciar as waterList
    void IniciarWaterList()
    {
        //Iniciar uma linha em waterListCorrect
        for(int i=0; i<=300; i++)
        {
            waterListCorrect.Add(null);
        }
        for(int i=0; i<=300; i++)
        {
            waterListCorrect[i]=waterListUniversal[i];
        }

        int count = waterListUniversal.Count;

        //Passar para waterListEsq
        for(int i=0; i<=count-1; i++)
        {
            waterListEsq.Add(null);
        }
        for(int i=0; i<=count-1; i++)
        {
            waterListEsq[i]=waterListUniversal[i];
        }

        //Passar para waterListDir
        for(int i=0; i<=count-1; i++)
        {
            waterListDir.Add(null);
        }
        int countForDir = count;
        for(int i=0; i<=count-1; i++)
        {
            waterListDir[i]=waterListUniversal[countForDir-1];
            countForDir--;
        }

        //Passar para waterListBaix
        for(int i=0; i<=count-1; i++)
        {
            waterListBaix.Add(null);
        }
        int countForBaix = 0;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListBaix[countForBaix]=waterListUniversal[i+j];
                countForBaix++;
            }
        }

        //Passar para waterListCima
        for(int i=0; i<=count-1; i++)
        {
            waterListCima.Add(null);
        }
        int countForCima = 0;
        for(int i=300; i >= 0; i--)
        {
            for(int j = 90000-1; j >=0 ; j=j-301)
            {
                waterListCima[countForCima]=waterListUniversal[i+j];
                countForCima++;
            }
        }
        /*int countForCima = count;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListCima[countForCima-1]=waterListUniversal[i+j];
                countForCima--;
            }
        }*/

        /*Debug.Log("waterListUniversal de IniciarWaterList"+waterListUniversal.Count);
        Debug.Log("waterListDir de IniciarWaterList"+waterListDir.Count);
        Debug.Log("waterListEsq de IniciarWaterList"+waterListEsq.Count);
        Debug.Log("waterListBaix de IniciarWaterList"+waterListBaix.Count);*/
    }

    //Verificar a posição do player
    void VerificarPlayerPositionForWaterPosition()
    {
        //player vai para a esq
        if (viewerPosition.x > viewerPositionOldForWaterNum.x+(((20/*velocidade maxima do player*//2)*100)*0.8)-50)
        {
            UpdateWaterPositionEsqNum();
            viewerPositionOldForWaterNum.x=viewerPosition.x;
            viewerPositionOldForWater.x=viewerPosition.x;
        }
        else if (viewerPosition.x > viewerPositionOldForWater.x+10)
        {
            UpdateWaterPositionEsq();
            viewerPositionOldForWater.x=viewerPosition.x;
        }
        //player vai para a dir
        if (viewerPosition.x < viewerPositionOldForWaterNum.x-(((20/*velocidade maxima do player*//2)*100)*0.8)-50)
        {
            UpdateWaterPositionDirNum();
            viewerPositionOldForWaterNum.x=viewerPosition.x;
            viewerPositionOldForWater.x=viewerPosition.x;
        }
        else if (viewerPosition.x < viewerPositionOldForWater.x-10)
        {
            UpdateWaterPositionDir();
            viewerPositionOldForWater.x=viewerPosition.x;
        }
        //player vai para baixo
        if (viewerPosition.y > viewerPositionOldForWaterNum.y+(((20/*velocidade maxima do player*//2)*100)*0.8)-50)
        {
            UpdateWaterPositionBaixNum();
            viewerPositionOldForWaterNum.y=viewerPosition.y;
            viewerPositionOldForWater.y=viewerPosition.y;
        }
        else if (viewerPosition.y > viewerPositionOldForWater.y+10)
        {
            UpdateWaterPositionBaix();
            viewerPositionOldForWater.y=viewerPosition.y;
        }
        //player vai para cima
        if (viewerPosition.y < viewerPositionOldForWaterNum.y-(((20/*velocidade maxima do player*//2)*100)*0.8)-50)
        {
            UpdateWaterPositionCimaNum();
            viewerPositionOldForWaterNum.y=viewerPosition.y;
            viewerPositionOldForWater.y=viewerPosition.y;
        }
        else if (viewerPosition.y < viewerPositionOldForWater.y-10)
        {
            UpdateWaterPositionCima();
            viewerPositionOldForWater.y=viewerPosition.y;
            /*Debug.Log("viewerPositionOldForWaterNum.y"+viewerPositionOldForWaterNum.y);
            Debug.Log("viewerPosition.y"+viewerPosition.y);
            Debug.Log("viewerPositionOldForWater.y"+viewerPositionOldForWater.y);*/
        }
    }

    //Atualizar todas as waterList
    void UpdateWaterList()
    {
        int count = waterListUniversal.Count;

        //Passar para waterListEsq
        for(int i=0; i<=count-1; i++)
        {
            waterListEsq[i]=waterListUniversal[i];
        }

        //Passar para waterListDir
        int countForDir = count;
        for(int i=0; i<=count-1; i++)
        {
            waterListDir[i]=waterListUniversal[countForDir-1];
            countForDir--;
        }

        //Passar para waterListBaix
        int countForBaix = 0;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListBaix[countForBaix]=waterListUniversal[i+j];
                countForBaix++;
            }
        }

        //Passar para waterListCima
        int countForCima = 0;
        for(int i=300; i >= 0; i--)
        {
            for(int j = 90000-1; j >=0 ; j=j-301)
            {
                waterListCima[countForCima]=waterListUniversal[i+j];
                countForCima++;
            }
        }
        /*int countForCima = count;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListCima[countForCima-1]=waterListUniversal[i+j];
                countForCima--;
            }
        }*/
        /*Debug.Log("waterListUniversal de UpdateWaterList"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterList"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterList"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterList"+waterListBaix.Count);*/
    }

    //ter a certesa que não ficao espaços de agua por prenxer 
    void UpdateWaterCorrect(int waterPositionXCorrect, int waterPositionZCorrect)
    {
        for(int j=0; j<301 ; j++)
        {  
            //if para ter a certesa que não ficao espaços de agua por prenxer 
            if (!Physics.Raycast(new Vector3 (waterPositionXCorrect, 1000, waterPositionZCorrect), Vector3.down, out hit, 970))
            {
                //este raycast verifica quais espaços faltao prenxer na agua
                if (!Physics.Raycast(new Vector3 (waterPositionXCorrect, 1000, waterPositionZCorrect), Vector3.down, out hit, 980, waterLayer))
                {
                    //Debug.Log(waterListCorrect.Count+"aaaaa");
                    waterListCorrect.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXCorrect, 26, waterPositionZCorrect), Quaternion.identity));
                    if (waterListCorrect.Count>100300)
                    {
                        Destroy(waterListCorrect[j]);
                        waterListCorrect.RemoveAt(j);
                    }
                }
            }
            waterPositionZCorrect=waterPositionZCorrect+10;
        }
    }

    void UpdateWaterCorrectZ(int waterPositionXCorrect, int waterPositionZCorrect)
    {
        for(int j=0; j<301 ; j++)
        {  
            //if para ter a certesa que não ficao espaços de agua por prenxer 
            if (!Physics.Raycast(new Vector3 (waterPositionXCorrect, 1000, waterPositionZCorrect), Vector3.down, out hit, 970))
            {
                //este raycast verifica quais espaços faltao prenxer na agua
                if (!Physics.Raycast(new Vector3 (waterPositionXCorrect, 1000, waterPositionZCorrect), Vector3.down, out hit, 980, waterLayer))
                {
                    //Debug.Log(waterListCorrect.Count+"aaaaa");
                    waterListCorrect.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXCorrect, 26, waterPositionZCorrect), Quaternion.identity));
                    if (waterListCorrect.Count>100300)
                    {
                        Destroy(waterListCorrect[j]);
                        waterListCorrect.RemoveAt(j);
                    }
                }
            }
            waterPositionXCorrect=waterPositionXCorrect+10;
        }
    }

    //atualizar o agua quando o player vai para a esquerda
    void UpdateWaterPositionEsq()
    {
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListEsq[z]);
            waterListEsq.RemoveAt(z);
        }

        waterPositionZUpdateEsq=waterPositionZUpdateSaveEsq;

        for(int j=0; j<301 ; j++)
        {  
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListEsq.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateEsq, 26, waterPositionZUpdateEsq), Quaternion.identity));
                }else
                {
                    waterListEsq.Add(null);
                }
                //if para ter a certesa que não ficao espaços de agua por prenxer 
                /*if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq-10, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 970))
                {
                    //este array verifica quais espaços faltao prenxer na agua
                    if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq-10, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 980, waterLayer))
                    {
                        Debug.Log(waterListDir.Count+"aaaaa");
                        Debug.Log(waterListEsq.Count+"aaaaa");
                        waterListEsq.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateEsq-10, 26, waterPositionZUpdateEsq), Quaternion.identity));
                        waterListDir.Add(null);
                        waterListBaix.Add(null);
                        waterListUniversal.Add(null);
                    }
                }*/
            }else
            {
                waterListEsq.Add(null);
            }
            waterPositionZUpdateEsq=waterPositionZUpdateEsq+10;
        }
        waterPositionXUpdateEsq=waterPositionXUpdateEsq+10;
        waterPositionXUpdateDir=waterPositionXUpdateEsq-3000;
        waterPositionXUpdateSaveBaix=waterPositionXUpdateEsq-3000;
        waterPositionXUpdateSaveCima=waterPositionXUpdateEsq;

        int count = waterListEsq.Count;

        //Passar para waterListUniversal
        for(int i=0; i<=count-1; i++)
        {
            waterListUniversal[i]=waterListEsq[i];
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionEsq"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionEsq"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionEsq"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionEsq"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrect(waterPositionXUpdateEsq-1000,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1200,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1485,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1499,waterPositionZUpdateSaveEsq);
    }

    //a cada x numero de espaço andado atualizar o agua quando o player vai para a esquerda
    void UpdateWaterPositionEsqNum()
    {
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListEsq[z]);
            waterListEsq.RemoveAt(z);
        }
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListEsq[z]);
            waterListEsq.RemoveAt(z);
        }

        waterPositionZUpdateEsq=waterPositionZUpdateSaveEsq;

        for(int i=0; i<301 ; i++)
        {  
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListEsq.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateEsq, 26, waterPositionZUpdateEsq), Quaternion.identity));
                }else
                {
                    waterListEsq.Add(null);
                }
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateEsq+10, 1000, waterPositionZUpdateEsq), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListEsq.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateEsq+10, 26, waterPositionZUpdateEsq), Quaternion.identity)); 
                }else
                {
                    waterListEsq.Add(null);
                }
            }else
            {
                waterListEsq.Add(null);
                waterListEsq.Add(null);
            }
            waterPositionZUpdateEsq=waterPositionZUpdateEsq+20;
        }

        waterPositionXUpdateEsq=waterPositionXUpdateEsq+20;
        waterPositionXUpdateDir=waterPositionXUpdateEsq-3000;
        waterPositionXUpdateSaveBaix=waterPositionXUpdateEsq-3000;
        waterPositionXUpdateSaveCima=waterPositionXUpdateEsq;

        int count = waterListEsq.Count;

        //Passar para waterListUniversal
        for(int i=0; i<=count-1; i++)
        {
            waterListUniversal[i]=waterListEsq[i];
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionEsqNum"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionEsqNum"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionEsqNum"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionEsqNum"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrect(waterPositionXUpdateEsq-1000,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1200,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1485,waterPositionZUpdateSaveEsq);
        UpdateWaterCorrect(waterPositionXUpdateEsq-1499,waterPositionZUpdateSaveEsq);
    }

    //atualizar o agua quando o player vai para a direita
    void UpdateWaterPositionDir()
    {
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListDir[z]);
            waterListDir.RemoveAt(z);
        }

        waterPositionZUpdateDir=waterPositionZUpdateSaveDir;
        
        for(int j=0; j<301 ; j++)
        {  
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListDir.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateDir, 26, waterPositionZUpdateDir), Quaternion.identity)); 
                }else
                {
                    waterListDir.Add(null);
                }
                //if para ter a certesa que não ficao espaços de agua por prenxer 
                /*if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir+10, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 970))
                {
                    //este array verifica quais espaços faltao prenxer na agua
                    if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir+10, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 980, waterLayer))
                    {
                        //Debug.Log(waterListDir.Count+"aaaaa");
                        //Debug.Log(waterListEsq.Count+"aaaaa");
                        waterListDir.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateDir+10, 26, waterPositionZUpdateDir), Quaternion.identity)); 
                        waterListEsq.Add(null);
                        waterListBaix.Add(null);
                        waterListUniversal.Add(null);
                    }
                }*/
            }else
            {
                waterListDir.Add(null);
            }
            waterPositionZUpdateDir=waterPositionZUpdateDir-10;
        }
        waterPositionXUpdateDir=waterPositionXUpdateDir-10;
        waterPositionXUpdateEsq=waterPositionXUpdateDir+3000;
        waterPositionXUpdateSaveBaix=waterPositionXUpdateDir;
        waterPositionXUpdateSaveCima=waterPositionXUpdateDir+3000;

        int count = waterListEsq.Count;
        int countForDir = waterListEsq.Count;

        //Passar para waterListUniversal
        for(int i=0; i<=count-1; i++)
        {
            waterListUniversal[i]=waterListDir[countForDir-1];
            countForDir--;
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionDir"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionDir"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionDir"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionDir"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrect(waterPositionXUpdateDir+1000,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1200,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1485,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1499,waterPositionZUpdateSaveDir-3000);
    }

    //a cada x numero de espaço andado atualizar o agua quando o player vai para a direita
    void UpdateWaterPositionDirNum()
    {
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListDir[z]);
            waterListDir.RemoveAt(z);
        }
        for(int z = 0; z < 301; z++)
        {
            Destroy(waterListDir[z]);
            waterListDir.RemoveAt(z);
        }

        waterPositionZUpdateDir=waterPositionZUpdateSaveDir;

        for(int i=0; i<301 ; i++)
        {  
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListDir.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateDir, 26, waterPositionZUpdateDir), Quaternion.identity)); 
                }else
                {
                    waterListDir.Add(null);
                }
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateDir-10, 1000, waterPositionZUpdateDir), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListDir.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateDir-10, 26, waterPositionZUpdateDir), Quaternion.identity)); 
                }else
                {
                    waterListDir.Add(null);
                }
            }
            else
            {
                waterListDir.Add(null);
                waterListDir.Add(null);
            }
            waterPositionZUpdateDir=waterPositionZUpdateDir-20;
        }

        waterPositionXUpdateDir=waterPositionXUpdateDir-20;
        waterPositionXUpdateEsq=waterPositionXUpdateDir+3000;
        waterPositionXUpdateSaveBaix=waterPositionXUpdateDir;
        waterPositionXUpdateSaveCima=waterPositionXUpdateDir+3000;

        int count = waterListEsq.Count;
        int countForDir = waterListEsq.Count;

        //Passar para waterListUniversal
        for(int i=0; i<=count-1; i++)
        {
            waterListUniversal[i]=waterListDir[countForDir-1];
            countForDir--;
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionDirNum"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionDirNum"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionDirNum"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionDirNum"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrect(waterPositionXUpdateDir+1000,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1200,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1485,waterPositionZUpdateSaveDir-3000);
        UpdateWaterCorrect(waterPositionXUpdateDir+1499,waterPositionZUpdateSaveDir-3000);
    }

    //atualizar o agua quando o player vai para baixo
    void UpdateWaterPositionBaix()
    {
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListBaix.Count+"aaaaa");
            Destroy(waterListBaix[z]);
            waterListBaix.RemoveAt(z);
        }
        
        waterPositionXUpdateBaix=waterPositionXUpdateSaveBaix;

        for(int j=0; j<300 ; j++)
        {  
            //Debug.Log(j);
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000, waterPositionZUpdateBaix), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListBaix.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateBaix, 26,  waterPositionZUpdateBaix), Quaternion.identity));
                }else
                {
                    waterListBaix.Add(null);
                } 
                //if para ter a certesa que não ficao espaços de agua por prenxer 
                /*if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix-10), Vector3.down, out hit, 970))
                {
                    //este array verifica quais espaços faltao prenxer na agua
                    if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix-10), Vector3.down, out hit, 980, waterLayer))
                    {
                        //Debug.Log(waterListBaix.Count+"aaaaa");
                        //Debug.Log(waterListEsq.Count+"aaaaa");
                        //Debug.Log(waterListBaix.Count+"aaaaa");
                        waterListBaix.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateBaix, 26,  waterPositionZUpdateBaix-10), Quaternion.identity)); 
                        waterListEsq.Add(null);
                        waterListDir.Add(null);
                        waterListUniversal.Add(null);
                    }
                }*/
            }else
            {
                waterListBaix.Add(null);
            }
            waterPositionXUpdateBaix=waterPositionXUpdateBaix+10;
        }
        waterPositionZUpdateBaix= waterPositionZUpdateBaix+10;
        waterPositionZUpdateSaveDir = waterPositionZUpdateBaix;
        waterPositionZUpdateSaveEsq = waterPositionZUpdateBaix-3000;
        waterPositionZUpdateCima = waterPositionZUpdateBaix-3000;

        int count = waterListEsq.Count;
        int countForBaix = 0;

        //Passar para waterListUniversal
        for(int i= 0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListUniversal[i+j]=waterListBaix[countForBaix];
                countForBaix++;
            }
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionBaix"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionBaix"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionBaix"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionBaix"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1000);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1200);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1485);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1499);
    }

    //a cada x numero de espaço andado atualizar o agua quando o player vai para Baixo
    void UpdateWaterPositionBaixNum()
    {
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListBaix.Count+"aaaaa");
            Destroy(waterListBaix[z]);
            waterListBaix.RemoveAt(z);
        }
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListBaix.Count+"aaaaa");
            Destroy(waterListBaix[z]);
            waterListBaix.RemoveAt(z);
        }

        waterPositionXUpdateBaix=waterPositionXUpdateSaveBaix;

        for(int j=0; j<300 ; j++)
        {  
            //Debug.Log(j);
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000, waterPositionZUpdateBaix), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListBaix.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateBaix, 26,  waterPositionZUpdateBaix), Quaternion.identity));
                }else
                {
                    waterListBaix.Add(null);
                }
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000, waterPositionZUpdateBaix+10), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListBaix.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateBaix, 26,  waterPositionZUpdateBaix+10), Quaternion.identity));
                }else
                {
                    waterListBaix.Add(null);
                }
            }else
            {
                waterListBaix.Add(null);
                waterListBaix.Add(null);
            }
            waterPositionXUpdateBaix=waterPositionXUpdateBaix+20;
        }

        waterPositionZUpdateBaix= waterPositionZUpdateBaix+20;
        waterPositionZUpdateSaveDir = waterPositionZUpdateBaix;
        waterPositionZUpdateSaveEsq = waterPositionZUpdateBaix-3000;
        waterPositionZUpdateCima = waterPositionZUpdateBaix-3000;

        int count = waterListEsq.Count;
        int countForBaix = 0;

        //Passar para waterListUniversal
        for(int i= 0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListUniversal[i+j]=waterListBaix[countForBaix];
                countForBaix++;
            }
        }
        /*Debug.Log("waterListUniversal de UpdateWaterPositionBaixNum"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionBaixNum"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionBaixNum"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionBaixNum"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1000);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1200);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1485);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveBaix,waterPositionZUpdateBaix-1499);
    }

    //atualizar o agua quando o player vai para cima
    void UpdateWaterPositionCima()
    {
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListCima.Count+"aaaaa");
            Destroy(waterListCima[z]);
            waterListCima.RemoveAt(z);
        }
        
        waterPositionXUpdateCima=waterPositionXUpdateSaveCima;

        for(int j=0; j<300 ; j++)
        {  
            //Debug.Log(j);
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateCima, 1000,  waterPositionZUpdateCima), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateCima, 1000, waterPositionZUpdateCima), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListCima.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateCima, 26,  waterPositionZUpdateCima), Quaternion.identity)); 
                }else
                {
                    waterListCima.Add(null);
                }
                //if para ter a certesa que não ficao espaços de agua por prenxer 
                /*if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix-10), Vector3.down, out hit, 970))
                {
                    //este array verifica quais espaços faltao prenxer na agua
                    if (!Physics.Raycast(new Vector3 (waterPositionXUpdateBaix, 1000,  waterPositionZUpdateBaix-10), Vector3.down, out hit, 980, waterLayer))
                    {
                        //Debug.Log(waterListBaix.Count+"aaaaa");
                        //Debug.Log(waterListEsq.Count+"aaaaa");
                        //Debug.Log(waterListBaix.Count+"aaaaa");
                        waterListBaix.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateBaix, 26,  waterPositionZUpdateBaix-10), Quaternion.identity)); 
                        waterListEsq.Add(null);
                        waterListDir.Add(null);
                        waterListUniversal.Add(null);
                    }
                }*/
            }else
            {
                waterListCima.Add(null);
            }
            waterPositionXUpdateCima=waterPositionXUpdateCima-10;
        }
        waterPositionZUpdateCima= waterPositionZUpdateCima-10;
        waterPositionZUpdateSaveDir = waterPositionZUpdateCima+3000;
        waterPositionZUpdateSaveEsq = waterPositionZUpdateCima;
        waterPositionZUpdateBaix = waterPositionZUpdateCima+3000;

        int count = waterListEsq.Count;
        int countForCima = 0;

        //Passar para waterListUniversal
        for(int i=300; i >= 0; i--)
        {
            for(int j = 90000-1; j >=0 ; j=j-301)
            {
                waterListUniversal[i+j]=waterListCima[countForCima];
                countForCima++;
            }
        }
        /*int countForCima = count;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListUniversal[i+j]=waterListCima[countForCima-1];
                countForCima--;
            }
        }*/
        /*Debug.Log("waterListUniversal de UpdateWaterPositionBaix"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionBaix"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionBaix"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionBaix"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1000);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1200);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1485);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1499);
    }

    //a cada x numero de espaço andado atualizar o agua quando o player vai para Cima
    void UpdateWaterPositionCimaNum()
    {
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListCima.Count+"aaaaa");
            Destroy(waterListCima[z]);
            waterListCima.RemoveAt(z);
        }
        for(int z = 0; z < 300; z++)
        {
            //Debug.Log(waterListCima.Count+"aaaaa");
            Destroy(waterListCima[z]);
            waterListCima.RemoveAt(z);
        }

        waterPositionXUpdateCima=waterPositionXUpdateSaveCima;

        for(int j=0; j<300 ; j++)
        {  
            //Debug.Log(j);
            if (!Physics.Raycast(new Vector3 (waterPositionXUpdateCima, 1000,  waterPositionZUpdateCima), Vector3.down, out hit, 970))
            {
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateCima, 1000, waterPositionZUpdateCima), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListCima.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateCima, 26,  waterPositionZUpdateCima), Quaternion.identity)); 
                }else
                {
                    waterListCima.Add(null);
                }
                if (!Physics.Raycast(new Vector3 (waterPositionXUpdateCima, 1000, waterPositionZUpdateCima-10), Vector3.down, out hit, 980, waterLayer))
                {
                    waterListCima.Add((GameObject)Instantiate(Waterplane, new Vector3(waterPositionXUpdateCima, 26,  waterPositionZUpdateCima-10), Quaternion.identity)); 
                }else
                {
                    waterListCima.Add(null);
                }
            }else
            {
                waterListCima.Add(null);
                waterListCima.Add(null);
            }
            waterPositionXUpdateCima=waterPositionXUpdateCima-20;
        }

        waterPositionZUpdateCima= waterPositionZUpdateCima-20;
        waterPositionZUpdateSaveDir = waterPositionZUpdateCima+3000;
        waterPositionZUpdateSaveEsq = waterPositionZUpdateCima;
        waterPositionZUpdateBaix = waterPositionZUpdateCima+3000;

        int count = waterListEsq.Count;
        int countForCima = 0;

        //Passar para waterListUniversal
        for(int i=300; i >= 0; i--)
        {
            for(int j = 90000-1; j >=0 ; j=j-301)
            {
                waterListUniversal[i+j]=waterListCima[countForCima];
                countForCima++;
            }
        }
        /*int countForCima = count;
        for(int i=0; i <= 300; i++)
        {
            for(int j = 0; j <= 90000-1; j=j+301)
            {
                waterListUniversal[i+j]=waterListCima[countForCima-1];
                countForCima--;
            }
        }*/
        /*Debug.Log("waterListUniversal de UpdateWaterPositionBaixNum"+waterListUniversal.Count);
        Debug.Log("waterListDir de UpdateWaterPositionBaixNum"+waterListDir.Count);
        Debug.Log("waterListEsq de UpdateWaterPositionBaixNum"+waterListEsq.Count);
        Debug.Log("waterListBaix de UpdateWaterPositionBaixNum"+waterListBaix.Count);*/

        UpdateWaterList();
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1000);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1200);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1485);
        UpdateWaterCorrectZ(waterPositionXUpdateSaveCima-3000,waterPositionZUpdateCima+1499);
    }
}