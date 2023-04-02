using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jogar : MonoBehaviour
{
    private IDataService dataService = new JsonDataService();
    public Loading loading;
    public GameObject canvas;

    public void Continuar()
    {
        if(dataService.ProcurarFile("/playerProcedural_landmass.json"))
        {
            canvas.SetActive(true);
            loading.LoadScene(1);
        }
    }
}
