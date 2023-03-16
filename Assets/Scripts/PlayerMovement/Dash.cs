using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("References")]
    public Transform rotacao;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header ("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("CameraEffects")]
    public PlayerCam cam;
    //public float dashFov;

    [Header("Settings")]
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    //reiniciar a velocidade
    public bool resetVel = true;

    [Header("Cooldown")]
    //cooldown do dash
    public float dashCd;
    //timer do cooldown
    private float dashCdTimer;
    //bool para saber se o player está a premir o botao de dash
    bool dashbool;

    private InputSystem playerInputSystem;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        pm=GetComponent<PlayerMovement>();

        playerInputSystem = new InputSystem();
        playerInputSystem.Player.Enable();
    }

    private void Update()
    {
        if ((Input.GetAxis("Dash") != 0) || dashbool)
        {
            Dash2();
        }//diminuir o timer
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash2()
    {
        //se o player estiver no cooldown n podrá usar o dash então retorna
        if (dashCdTimer > 0)
        {
            return;
        }else
        {
            dashCdTimer = dashCd;
        }
        //passar a bool do script do playermovement para true
        pm.dashing = true;

        //cam.DoFov(dashFov);

        Transform forwardT;

        forwardT = rotacao;

        Vector3 direction = GetDirection(forwardT);
        //aplicar a força na direção para qual o player está a ir
        Vector3 forceToApply = direction * dashForce + rotacao.up * dashUpwardForce;
        //desligar a gravidade durante o dash
        if(disableGravity)
        {
            rb.useGravity = false;
        }
        //aplicar a força durante x segundos
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        //reniciar o dashDuration
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        if (resetVel)
        {
            rb.velocity = Vector3.zero;
        }
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    //o dash acabou
    private void ResetDash()
    {
        //cam.DoFov(60f);

        pm.dashing = false;
        //ligar a gravidade
        if(disableGravity)
        {
            rb.useGravity = true;
        }
    }
    //pegar a direção da movimentação
    private Vector3 GetDirection(Transform forwardT)
    {
        Vector2 inputPlayer = playerInputSystem.Player.Movement.ReadValue<Vector2>();
        float horizontalInput = inputPlayer.x;
        float verticalInput = inputPlayer.y;

        Vector3 direction = new Vector3();
        //mover-se em todas as direções, só para frente ou se parado para a frente
        if (allowAllDirections)
        {
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        }
        else
        {
            direction = forwardT.forward;
        }
        if(verticalInput == 0 && horizontalInput == 0)
        {
            direction = forwardT.forward;
        }
        return direction.normalized;
    }

    //saber se o botão para dar dash está a ser premido
    public void Dashing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dashbool = true;
        }else
        {
            dashbool = false;
        }
    }
}
