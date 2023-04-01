using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerEnergy : MonoBehaviour
{
    public PlayerCam playerCam;
    //receber o menu
    public GameObject menu;
    public Timer timer;
    private IDataService dataService = new JsonDataService();
    private Volume volume;
    private Vignette vignette;
    public float energia=100;
    private float energiaMax=0;
    private int contador=10;
    public float metadeDaEnergia;
    private bool podesEntrar=true;
    public float perda = 1;

    private void Start()
    {
        energiaMax = 100;
        metadeDaEnergia=50;
        volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vignette);
    }

    private void FixedUpdate()
    {
        Energia();
        Morrer();
    }

    private void Energia()
    {
        if(podesEntrar)
        {
            StartCoroutine(PerderEnergia());
        }
        if(energia<=50)
        {
            Debug.Log(energia);
            Debug.Log("Coisa");
            volume.priority = 1;
            vignette.intensity.value = ((contador*1)/metadeDaEnergia);
        }
    }

    public void ApanheiUm()
    {
        perda++;
        if(energia<=50)
        {
            energia = energiaMax;
            volume.priority = -1;
            vignette.intensity.value = 0;
            contador = 10;
        }else
        {
            energia = energia + energia;
        }
        PlayerPrefs.SetFloat("energia", energia);
        PlayerPrefs.SetFloat("perda", perda);
    }

    IEnumerator PerderEnergia()
    {
        podesEntrar=false;
        if(!podesEntrar)
        {
            yield return new WaitForSeconds(5);
            energia = energia - perda;
            podesEntrar=true;
            if(energia<=50)
            {
                contador++;
            }
        }
    }

    private void Morrer()
    {
        if (energia<=0)
        {
            Scene scene = SceneManager.GetActiveScene();
            Debug.Log("A apagar e desligar");
            timer.stop = true;
            //desprender o cursor e torna-lo visivel
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerCam.moveMouse=false;
            dataService.Delete ("/player"+scene.name+".json");
            dataService.Delete ("/objects"+scene.name+".json");
            PlayerPrefs.SetFloat("currentTime", 0);
            menu.SetActive(true);
        }
    }

}
