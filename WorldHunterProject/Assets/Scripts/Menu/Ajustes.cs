using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Ajustes : MonoBehaviour
{
    [Header("Dados")]
    public TMP_InputField Semente;
    public TMP_InputField Nome;

    public void ReceberSemente()
    {
        PlayerPrefs.SetInt("Semente", int.Parse(Semente.text));
        Debug.Log(PlayerPrefs.GetInt("Semente"));
    }

    public void ReceberNome()
    {
        PlayerPrefs.SetString("Nome", Nome.text);
    }
}
