using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Definicoes : MonoBehaviour
{
    [Header("Vsync")]
    public Toggle vsyncTog;

    [Header("Fov e Distância")]
    //public TMP_Dropdown dropdownFov;
    public TMP_Text textFov;
    //public TMP_Dropdown dropdownDis;
    public TMP_Text textDis;

    [Header("Resolução")]
    //public TMP_Dropdown dropdownRes;
    public TMP_Text textRes;

    [Header("Qualidade")]
    public TMP_Dropdown dropdownQua;
    public RenderPipelineAsset[] qualityLevels;

    void Start()
    {
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }else
        {
            vsyncTog.isOn = true;
        }

        dropdownQua.value = QualitySettings.GetQualityLevel();
        textRes.text = Screen.height.ToString();
        textDis.text = PlayerPrefs.GetInt("Dis").ToString();
        textFov.text = PlayerPrefs.GetInt("Fov").ToString();
    }

    public void ChangeLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }

    public void Resolucao()
    {
        /*if (dropdownRes.value == 1080)
        {
            Debug.Log(dropdownRes.value);
            Screen.SetResolution(1920, 1080, true);
        }
        else if (dropdownRes.value == 720)
        {
            Debug.Log(dropdownRes.value);
            Screen.SetResolution(1280, 720, true);
        }
        else if (dropdownRes.value == 480)
        {
            Debug.Log(dropdownRes.value);
            Screen.SetResolution(720, 480, true);
        }*/
        int numeroInt = int.Parse(textRes.text);
        Debug.Log(numeroInt);
        if (numeroInt == 1080)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (numeroInt == 720)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else if (numeroInt == 480)
        {
            Screen.SetResolution(720, 480, true);
        }
    }

    public void Distancia()
    {
        PlayerPrefs.SetInt("Dis", int.Parse(textDis.text));
        /*int numeroInt = int.Parse(numero.text);
        Debug.Log(numeroInt);
        if (numeroInt == 1000)
        {
            PlayerPrefs.SetInt("Dis", 1000);
            guardarDis =1000;
            Debug.Log(guardarDis);
        }
        else if (numeroInt == 2000)
        {
            PlayerPrefs.SetInt("Dis", 2000);
            guardarDis =2000;
            Debug.Log(guardarDis);
        }
        else if (numeroInt == 3000)
        {
            PlayerPrefs.SetInt("Dis", 3000);
            guardarDis =3000;
            Debug.Log(guardarDis);
        }*/
    }

    public void Fov()
    {
        PlayerPrefs.SetInt("Fov", int.Parse(textFov.text));
        /*int numeroInt = int.Parse(numero.text);
        Debug.Log(numeroInt);
        if (numeroInt == 50)
        {
            PlayerPrefs.SetInt("Fov", 50);
            guardarFov =50;
            Debug.Log(guardarFov);
        }
        else if (numeroInt == 60)
        {
            PlayerPrefs.SetInt("Fov", 60);
            guardarFov =60;
            Debug.Log(guardarFov);
        }
        else if (numeroInt == 70)
        {
            PlayerPrefs.SetInt("Fov", 70);
            guardarFov =70;
            Debug.Log(guardarFov);
        }*/
    }

    public void Vsync()
    {
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    /*public void Salvar()
    {
        if (PlayerPrefs.GetInt("res") == 1080)
        {
            Screen.SetResolution(1920, 1080, true); 
        }
        else if (PlayerPrefs.GetInt("res") == 720)
        {
            Screen.SetResolution(1280, 720, true); 
        }
        else if (PlayerPrefs.GetInt("res") == 480)
        {
            Screen.SetResolution(720, 480, true); 
        }

        if (guardarDis == 1000)
        {
            PlayerPrefs.SetInt("Dis", 1000);
            Debug.Log(guardarDis);
        }
        else if (guardarDis == 2000)
        {
            PlayerPrefs.SetInt("Dis", 2000);
            Debug.Log(guardarDis);
        }
        else if (guardarDis == 3000)
        {
            PlayerPrefs.SetInt("Dis", 3000);
            Debug.Log(guardarDis);
        }

        if (guardarFov == 50)
        {
            PlayerPrefs.SetInt("Fov", 50);
            Debug.Log(guardarFov);
        }
        else if (guardarFov == 60)
        {
            PlayerPrefs.SetInt("Fov", 60);
            Debug.Log(guardarFov);
        }
        else if (guardarFov == 70)
        {
            PlayerPrefs.SetInt("Fov", 70);
            Debug.Log(guardarFov);
        }
    }*/
}
