using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPortal : MonoBehaviour
{
    [Header ("Receive Information")]
    public GameObject portal;
    public LayerMask whatIsGround;
    private GameObject portalDeleter;

    private RaycastHit hit;

    [SerializeField]
    private bool clicking;
    private bool clicked = false;

    private void Update()
    {
        Creating();
    }

    private void Creating()
    {
        if ((clicking || (Input.GetAxis("SpawnPortal") != 0)) && !clicked)
        {
            if(!Physics.Raycast(transform.position, transform.forward, out hit, 5f, whatIsGround))
            {
                if(Physics.Raycast(transform.position, transform.forward, out hit, 10f, whatIsGround))
                {
                    portalDeleter=Instantiate(portal, new Vector3(hit.point.x,(hit.point.y+1),hit.point.z), transform.rotation = Quaternion.Euler(new Vector3(0,0,0)));
                    clicked = true;
                    StartCoroutine(Destruir());
                    StartCoroutine(EsperarParaClicar());
                }
            }
        }
    }

    IEnumerator EsperarParaClicar()
    {
        yield return new WaitForSeconds(300);
        clicked = false;
    }
    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(5);
        Destroy(portalDeleter);
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
