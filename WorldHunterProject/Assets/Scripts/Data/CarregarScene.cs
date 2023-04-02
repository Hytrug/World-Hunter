using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarScene : MonoBehaviour
{
    public SaveObjects salvar;
    private void Start()
    {
        salvar.Carregar();
    }
}
