using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hunt : MonoBehaviour
{
    [Header ("Receive Information")]
    public Transform cameraPosition;
    public LayerMask creature;
    public PlayerEnergy energia;

    private RaycastHit hit;
    private RaycastHit hit2;

    [SerializeField]
    private bool clicking;

    private int contaCreaturas=0;
    
    private void Update()
    {
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
        Physics.Raycast(transform.position, transform.forward, out hit2, Mathf.Infinity);
        Debug.DrawLine(transform.position, transform.forward * 360f, Color.blue, 0);
        Debug.DrawLine(transform.position, hit2.point, Color.yellow, 0);
        Hunting();
    }

    private void Hunting()
    {
        if (clicking || Input.GetButtonDown("Capture"))
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f, creature))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 0);
                Destroy(hit.transform.gameObject);
                contaCreaturas = PlayerPrefs.GetInt("contaCreaturas");
                contaCreaturas++;
                Debug.Log(contaCreaturas);
                PlayerPrefs.SetInt("contaCreaturas", contaCreaturas);
                Debug.Log(PlayerPrefs.GetInt("contaCreaturas"));
                energia.ApanheiUm();
            }
        }
    }

    public void Clicking(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            clicking = true;
        }else
        {
            clicking = false;
        }
    }
}
