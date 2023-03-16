using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneTP : MonoBehaviour
{
    public string sceneName;
    //posso usa o nome da scene ou o index dela que se ve nas "build Settings"
    public int sceneIndex;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //entrar na outra cena
            SceneManager.LoadScene(sceneName);
        }
    }
}
