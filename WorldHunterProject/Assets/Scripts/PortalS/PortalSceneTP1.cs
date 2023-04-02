using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneTP1 : MonoBehaviour
{
    public SaveObjects salvar;
    public string sceneName;
    //posso usa o nome da scene ou o index dela que se ve nas "build Settings"
    public int sceneIndex;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sceneIndex=Random.Range(4, 5);
            if (PlayerPrefs.GetInt("jaPassou") != 1)
            {
                PlayerPrefs.SetInt("jaPassou", 1);
                PlayerPrefs.SetInt("estaScene", SceneManager.GetActiveScene().buildIndex);
                Debug.Log(PlayerPrefs.GetInt("jaPassou"));
                //entrar na outra cena
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                salvar.Save();
            }
            else if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("estaScene") && PlayerPrefs.GetInt("jaPassou") != 0)
            {
                PlayerPrefs.SetInt("jaPassou", 0);
                Debug.Log(PlayerPrefs.GetInt("jaPassou"));
                Debug.Log(PlayerPrefs.GetInt("estaScene"));
                SceneManager.LoadScene(PlayerPrefs.GetInt("estaScene"), LoadSceneMode.Single);
                salvar.Save();
            }
            else
            {
                PlayerPrefs.SetInt("jaPassou", 1);
                PlayerPrefs.SetInt("estaScene", SceneManager.GetActiveScene().buildIndex);
                Debug.Log(PlayerPrefs.GetInt("jaPassou"));
                //entrar na outra cena
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                salvar.Save();
            }
        }
    }
}
