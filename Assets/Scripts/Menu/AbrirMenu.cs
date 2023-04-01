using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirMenu : MonoBehaviour
{
    public Loading loading;

    public void Entrar()
    {
       loading.LoadScene(0);
    }
}
