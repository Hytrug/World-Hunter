using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    //receber o menu
    public GameObject menu;
    //sencibilidades para a camara
    public float sensX;
    public float sensY;
    //orientação do player
    public Transform playerRotation;
    //rotação
    float xRotation;
    float yRotation;
    //bollean para saber se o rato está ativo ou desativo
    public bool moveMouse = true;
    //variaveis para o controlo
    //zona morta do controlo
    public float inputDeadZone;

    private void Start()
    {
        //não prender o cursor e não torna-lo invisivel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Menu();
        if (moveMouse)
        {
            float JoyX = Input.GetAxis("HorizontalJoy") * Time.fixedDeltaTime * sensX;
            float JoyY = Input.GetAxis("VerticalJoy") * Time.fixedDeltaTime * (sensY-20f);
            Vector2 leftStickInput= new Vector2(JoyX, JoyY);
            if (leftStickInput.magnitude < inputDeadZone)
            {
                leftStickInput = Vector2.zero;
            }

            //a rotação atual mais a que fazemos com o rato
            yRotation += leftStickInput.x;
            xRotation -= leftStickInput.y;
            //não deixar a camara ir muito para cima nem para baixo
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            //rodar a camara e o player respetivamente
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerRotation.rotation = Quaternion.Euler(0, yRotation, 0);
            
            //receber o input do rato
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

            //a rotação atual mais a que fazemos com o rato
            yRotation += mouseX;
            xRotation -= mouseY;
            //não deixar a camara ir muito para cima nem para baixo
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rodar a camara e o player respetivamente
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerRotation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void Prender()
    {
        //prender o cursor e torna-lo invisivel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Menu()
    {
        if (moveMouse && (Input.GetButtonDown("Pause")))
        {  
            Debug.Log(moveMouse);
            //desprender o cursor e torna-lo visivel
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.SetActive(true);
            moveMouse=false;
        }
        else if (!moveMouse && (Input.GetButtonDown("Pause")))
        {
            Debug.Log(moveMouse);
            //prender o cursor e torna-lo invisivel
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menu.SetActive(false);
            moveMouse=true;
        }
    }

    public void Menu2(InputAction.CallbackContext context)
    {
        if (moveMouse && (context.performed /*|| Input.GetButtonDown("Pause")*/))
        {
            Debug.Log(moveMouse);
            //desprender o cursor e torna-lo visivel
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menu.SetActive(true);
            moveMouse=false;
        }
        else if (!moveMouse && (context.performed /*|| Input.GetButtonDown("Pause")*/))
        {
            Debug.Log(moveMouse);
            //prender o cursor e torna-lo invisivel
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menu.SetActive(false);
            moveMouse=true;
        }
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }
}
