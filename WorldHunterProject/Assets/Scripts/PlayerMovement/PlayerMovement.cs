using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    [Header("Outro Script")]
    //script da camera
    public PlayerCam playerCam;

    [Header("Movement")]
    //tipos de velocidade
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float climbSpeed;
    public float dashSpeed;
    //variavel para fazer a velocidade descer aos poucos
    public float dashSpeedChangeFactor;
    //campo de visão
    public float runFov;
    public float climbFov;
    public float dashFov;
    //bool para saber se jogador está a clicar na tecla de correr
    bool aCorrer;

    //força de atração ao chão
    public float groundDrag;

    [Header("Jumping")]
    //força de salto cooldown e multiplicador
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    //bool para saber se o personagem já pode saltar
    bool readyToJump = true;
    //bool para saber se o jogador está a clicar na tecla de saltar
    bool aSaltar;

    [Header("Crouching")]
    //velocidade agachado
    public float crouchSpeed;
    //float para guardar a altura do player quando se agacha
    public float crouchYScale;
    //float que guarda a altura normal do personagem
    private float startYScale;
    //campo de visão agachado
    public float crounchFov;
    //bool para saber se o jogador está a clicar na tecla de agachar
    public bool agachado;
    //bool para caso o player esteja a clicar na tecla de agachar no teclado a do controlo não interromper
    bool limitarControlo;

    [Header("Swiming")]
    //velocidade do boost na agua
    public float waterMaxSpeed;
    //velocidade do recarregamento do boost
    public float waterMinSpeed;
    //float para a velocidade diminuir aos poucos
    public float waterSpeedChangeFactor;
    //tempo do boost
    public float timeOnMax;
    //tempo de recarga
    public float timeOnMin;
    //contador do tempo de boost
    private float timerMax;
    //contador do tempo de recarga
    private float timerMin;
    //campo de visão do boost
    public float waterFovMax;
    //campo de visão da recarga
    public float waterFovMin;
    //sencibilidade da camara normalmente
    private float normalSensX;
    private float normalSensY;
    //sencibilidade durante o boost
    public float speedSensX;
    public float speedSensY;
    //layer para a agua
    public LayerMask whatIsWater;
    //bool para saber se o personagem está em cima de agua
    bool swimming = false;

    [Header("Ground Check")]
    //altura do player
    public float playerHeight;
    //raio do spherecast
    public float sphereCastRadius;
    //layer para o que é chão
    public LayerMask whatIsGround;
    //bool para saber se o personagem está no chão
    public bool grounded;
    //dados guardados do raycast
    private RaycastHit hit;

    [Header("Slope Handling")]
    //angulo maximo das "rampas"
    public float maxSlopeAngle;
    //guardar os dados do raycast da "rampa"
    private RaycastHit slopeHit;
    //bool para saber se estamos a saltar numa "rampa"
    private bool exitingSlope;

    //orientação ou rotaçao do player
    public Transform rotacao;
    //inputs
    float horizontalInput;
    float verticalInput;
    //direção do movimento
    Vector3 moveDirection;
    //rigidbody
    Rigidbody rigidbody;
    //player inputsystem
    private InputSystem playerInputSystem;
    //vector2 para os dados do input system
    Vector2 inputPlayer;
    //informação do raycast
    private RaycastHit hit2;
    
    //guardar o estado do player
    public MovementState state;
    //alternar entre estes estados
    public enum MovementState
    {
        walking,
        sprinting,
        climbing,
        crouching,
        dashing,
        swimmingfast,
        swimming,
        air
    }

    //bool para saber se estamos a escalar ou a dar dash
    public bool climbing;
    public bool dashing;

    private void Start()
    {
        //receber o rigidbody e parar a sua rotação
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

        //receber o inputsystem e ativa-lo
        playerInputSystem = new InputSystem();
        playerInputSystem.Player.Enable();

        //altura inicial do player
        startYScale = transform.localScale.y;
        //saber a sencibilidade normal da camara
        normalSensX = playerCam.sensX;
        normalSensY = playerCam.sensY;

        cam.fieldOfView = (float)(PlayerPrefs.GetInt("Fov"));
        Debug.Log (PlayerPrefs.GetInt("Fov"));
        Debug.Log(cam.fieldOfView);

        if (PlayerPrefs.GetInt("Fov") == 0)
        {
            cam.fieldOfView= 60;
            PlayerPrefs.SetInt("Fov", (int)cam.fieldOfView);
        }

        runFov = cam.fieldOfView + 30;
        climbFov = cam.fieldOfView - 10;
        dashFov = cam.fieldOfView + 50;
        crounchFov = cam.fieldOfView - 20;
        waterFovMax = cam.fieldOfView - 20;
        waterFovMin = cam.fieldOfView - 30;

        cam.farClipPlane = PlayerPrefs.GetInt("Dis");
        if (PlayerPrefs.GetInt("Dis") == 0)
        {
            cam.farClipPlane = 1000;
            PlayerPrefs.SetInt("Dis", (int)cam.farClipPlane);
        }

        if (Physics.Raycast(new Vector3(transform.position.x, 1000, transform.position.z), Vector3.down, out hit2, 1500))
        {
            transform.position = new Vector3 (hit2.point.x, hit2.point.y+100 , hit2.point.z);
        }
    }

    private void Update()
    {
        //ground check
        grounded = Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsGround);
        //se o jogador não estiver no menu de pausa
        if (playerCam.moveMouse)
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }

        //ligar o drag
        if (state == MovementState.walking || state== MovementState.sprinting || state == MovementState.crouching)
        {
            rigidbody.drag = groundDrag;
        }else
        {
            rigidbody.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (playerCam.moveMouse)
        {
            MovePlayer();
        }
    }

    public void MyInput()
    {
        //receber os valores do input system
        inputPlayer = playerInputSystem.Player.Movement.ReadValue<Vector2>();
        horizontalInput = inputPlayer.x;
        verticalInput = inputPlayer.y;

        //mudar os Fov dependendo do estdo do player
        if (state == MovementState.swimming)
        {
            //se ele estiver no boost
            if(timerMax > 0)
            {
                playerCam.DoFov(waterFovMax);
            }//se ele estiver na recarga
            else if (timerMin > 0)
            {
                playerCam.DoFov(waterFovMin);
            }
        }
        else if (state == MovementState.dashing)
        {
            playerCam.DoFov(dashFov);
        }
        else if (state == MovementState.sprinting)
        {
            playerCam.DoFov(runFov);
        }
        else if (state == MovementState.climbing)
        {
            playerCam.DoFov(climbFov);
        }
        else if (state == MovementState.crouching)
        {
            playerCam.DoFov(crounchFov);
        }
        else if (state == MovementState.walking)
        {
            playerCam.DoFov((float)(PlayerPrefs.GetInt("Fov")));
        }

        //nadar
        if(Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsWater))
        {
            swimming = true;
        }
        else//se ele parar de nadar mudar a sencibilidade para o seu padrão
        {
            swimming = false;
            if (playerCam.sensX < normalSensX && playerCam.sensY < normalSensY)
            {
                playerCam.sensX = normalSensX;
                playerCam.sensY = normalSensY;
            }
        }

        //saltar
        if(((Input.GetAxis("JumpPad") != 0) || aSaltar) && readyToJump && grounded && !swimming)
        {
            readyToJump = false;

            Jump();
            //invocar a funçao para resetar o cooldown do jump
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //agachar
        if(Input.GetAxis("Crouch") != 0 && !swimming)
        {
            agachado = true;
        }
        if(((Input.GetAxis("Crouch") != 0) || agachado))
        {
            //diminuir o tamanho do player
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        //não deixar o controlo desligar o agachar se tiver sido o teclado a ligalo
        if (!limitarControlo)
        {
            if(Input.GetAxis("Crouch") == 0)
            {
                agachado = false;
            }
        }//se o jogador não estiver agachado, polo no seu tamanho padrao
        if(!agachado)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    //velocidade desejada
    private float desiredMoveSpeed;
    //a velocidade anterior
    private float lastDesiredMoveSpeed;
    //o ultimo estado
    private MovementState lastState;
    //manter a velocidade
    private bool keepMomentum;

    //escolher o estado
    private void StateHandler()
    {
        //swimming
        if (swimming)
        {
            //se o timer ainda tiver tempo
            if(timerMax > 0)
            {
                //mudar o estddo para swimmingfast
                state = MovementState.swimmingfast;
                //mudar o desiredmovespeed
                desiredMoveSpeed = waterMaxSpeed;
                //mudar a senc
                playerCam.sensX = speedSensX;
                playerCam.sensY = speedSensY;
                //passar o valor para descer a velocidade
                speedChangeFactor = waterSpeedChangeFactor;
            }
            else if (timerMin > 0)
            {
                state = MovementState.swimming;
                desiredMoveSpeed = waterMinSpeed;
                playerCam.sensX = normalSensX;
                playerCam.sensY = normalSensY;
            }//se os timer estiverem vazios, recomeça-los
            else
            {
                timerMax = timeOnMax;
                timerMin = timeOnMin;
            }
            //diminuir os timers
            if(timerMax > 0)
            {
                timerMax -=Time.deltaTime;
            }
            else if (timerMin > 0)
            {
                timerMin -=Time.deltaTime;
            }
        }

        //dashing
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        //climbing
        else if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        //crouching
        else if (((Input.GetAxis("Crouch") != 0) || agachado))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        //sprinting
        else if(grounded && ((Input.GetAxis("Run") != 0) || aCorrer))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //air
        else
        {
            state = MovementState.air;
            //se a velocidade não for a do sprint então será a de andar e vice-versa
            if(desiredMoveSpeed < sprintSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        //desiredMoveSpeedHasChanged é verdadeira se o desiredMoveSpeed for diferente de lastDesiredMoveSpeed
        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        //se o ultimo estado for um dos boost manter o momentum
        if (lastState == MovementState.dashing || lastState == MovementState.swimmingfast)
        {
            keepMomentum = true;
        }
        //se o desiredMoveSpeedHasChanged ==true
        if (desiredMoveSpeedHasChanged)
        {
            //se for para manter o momentum para todas as coroutines e começa a de descer o speed aos poucos
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else//se não o movespeed usado no resto do programa é igual ao desiredMoveSpeed;
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }
        //passar as variaveis atuais para as velhas
        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }
    //variavel que guarda o numero para descer o speed
    private float speedChangeFactor;
    //metodo para descer o speed
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //tempo
        float time = 0;
        //calculo matematico para obter a diferença entre o movespeed anterior e o desiredmovespeed atual
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        //valor inicial
        float startValue = moveSpeed;
        //variavel que guarda o numero para descer o speed
        float boostFactor = speedChangeFactor;
        //equanto o tempo for menor que a diferença
        while (time < difference)
        {
            //diminuir o movespeed aos poucos
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time/difference);
            //quanto tempo demora a descer
            time+= Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        //se o player estiver num dash n se podera mescer ent return
        if (state == MovementState.dashing)
        {
            return;
        }

        //calcular a direção do movimento
        moveDirection = rotacao.forward * verticalInput + rotacao.right * horizontalInput;

        //na "rampa"
        if (OnSlope() && !exitingSlope)
        {
            //andar na rampa
            rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            //manter o player no chão
            if (rigidbody.velocity.y > 0)
            {
                rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //fazer andar no chão
        else if (grounded)
        {
            rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }//fazer andar no ar
        else if (!grounded)
        {
            rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //desligar a gravidade quando o player está numa "rampa"
        rigidbody.useGravity = !OnSlope();
    }

    //não deixar a velocidade passar do seu limite
    private void SpeedControl()
    {
        //limitar a velocidade quando está numa "rampa"
        if (OnSlope() && !exitingSlope)
        {
            if (rigidbody.velocity.magnitude > moveSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * moveSpeed;
            }
        }

        //limitar a velocidade no chão ou no ar
        else
        {
            Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

            //controlar a velocidade
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
            }
        }
    }

    //saltar
    private void Jump()
    {
        exitingSlope = true;

        //resetar a velocidade do y
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        //saltar
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    //deixar o personagem saltar denovo
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    //descubrir o angulo da "rampa" se estiver numa
    private bool OnSlope()
    {
        if(Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    //Saber a direção do movimento na "rampa"
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
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
    public void Sprinting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aCorrer = true;
        }else
        {
            aCorrer = false;
        }
    }
    //saber se o botão para agachar está a ser premido
    public void Crouching(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            limitarControlo = true;
            agachado = true;
        }else
        {
            limitarControlo = false;
            agachado = false;
        }
    }
}
