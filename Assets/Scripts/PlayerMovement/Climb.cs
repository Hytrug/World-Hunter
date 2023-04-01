using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climb : MonoBehaviour
{
    [Header("Outro Script")]
    //script da camera
    public PlayerCam playerCam;

    [Header ("References")]
    public Transform rotacao;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    private float climbTimer;
    public float maxClimbTime;

    private bool climbing;

    [Header ("ClimbJumping")]
    //força do salto na parede
    public float climbJumpUpForce;
    //força com que o player vai para trás quando salta na parede
    public float climbJumpBackForce;

    [Header("CameraEffects")]
    public PlayerCam cam;

    [Header ("Detection")]
    //campo de deteção
    public float detectionLength;
    public float sphereCastRadius;
    //anngulo maximo para poder esclar quando a olhar para a parede
    public float maxWallLookAngle;
    //angulo em que o player está a olhar
    private float wallLookAngle;
    //bool para saber se o player está a clicar no w
    public bool clickingW;

    private RaycastHit frontWallHit;
    //bool para saber se o player tem uma parede à frente
    private bool wallFront;

    bool aSaltar;

    private void Update()
    {
        if (playerCam.moveMouse)
        {
            WallCheck();
            StateMachine();
            //se o player estiver a escalar fazer o movimento de escalada
            if(climbing)
            {
                ClimbingMovement();
            }
        }
    }

    private void StateMachine()
    {
        //se o player estiver a clicar no w, tem uma parede à frente e está dentro do angulo limite pode escalar
        if(clickingW && wallFront && wallLookAngle < maxWallLookAngle)
        {
            //se o player não tiver passado o limite de escalada pode escalar se não cai
            if(!climbing && climbTimer > 0)
            {
                StartClimbing();
            }

            if(climbTimer > 0)
            {
                climbTimer -= Time.deltaTime;
            }
            if(climbTimer < 0)
            {
                StopClimbing();
            }
        }else
        {
            if(climbing)
            {
                StopClimbing();
            }
        }
        //se o player tiver uma parede à frente e saltar
        if (wallFront && ((Input.GetAxis("JumpPad") != 0) || aSaltar))
        {
            ClimbJump();
        }
    }

    private void WallCheck()
    {
        //saber se tem uma parede à frente e se está dentro do angulo limite
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, rotacao.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(rotacao.forward, -frontWallHit.normal);
        //se a variavle grounded do PlayerMovement for verdadeira
        if (pm.grounded)
        {
            climbTimer= maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;
    }

    private void ClimbingMovement()
    {
        //aplicar força para cima
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;
    }

    private void ClimbJump()
    {//calculos para o salto da parede e mudança de fov
        cam.DoFov(65f);
        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;
        //aplicar os dados e a força
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    //saber se o botão para saltar está a ser premido
    public void Jumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aSaltar = true;
        }else
        {
            aSaltar = false;
        }
    }
    //saber se o botão para correr está a ser premido
    public void ClickingW(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            clickingW = true;
        }else
        {
            clickingW = false;
        }
    }
}
