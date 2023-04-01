using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbrirDefinicoes : MonoBehaviour
{
    public int currentScene;
    public Loading loading;

    public void Entrar()
    {
        PlayerPrefs.SetInt("voltarScene", currentScene);
        loading.LoadScene(3);
    }

    public void Voltar()
    {
        loading.LoadScene(PlayerPrefs.GetInt("voltarScene"));
    }
}
