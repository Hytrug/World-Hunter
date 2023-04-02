using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SapoDouradoAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform water;
    public Transform grass;

    public LayerMask whatIsGround, whatIsPlayer, whatIsFood, whatIsDrink;

    public PlayerMovement pm;

    [Header("Andar")]
    public float tempoPreso;
    private float tempoPresoCompare;
    public Vector3 destino;
    bool buscarDestino;
    public float tamanhoBusca;

    [Header("Cancaco")]
    private float maxCancaco;
    [SerializeField]
    private float cancaco;
    private bool aDescancar = false;

    [Header("Fuga")]
    [SerializeField]
    private bool emFuga = false;

    [Header("FomeESede")]
    private float maxSede;
    [SerializeField]
    private float sede;
    private bool aBeber = false;
    private float maxFome;
    [SerializeField]
    private float fome;
    private bool aComer = false;

    [Header("Ranges")]
    public float verPlayer, ouvirPlayer, cheirarComida, cheirarAgua;
    public bool playerASerVisto, playerASerOuvido, aCheirarComida, aCheirarAgua;

    [Header("Sound")]
    public AudioSource patrollingSoundEffect;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        water = GameObject.Find("Waterplane").transform;
        grass = GameObject.Find("Bush").transform;
        agent = GetComponent<NavMeshAgent>();
        maxCancaco = cancaco;
        maxSede = sede;
        maxFome = fome;
        tempoPresoCompare = tempoPreso;
    }

    private void Update()
    {
        playerASerVisto = Physics.CheckSphere(transform.position, verPlayer, whatIsPlayer);
        playerASerOuvido = Physics.CheckSphere(transform.position, ouvirPlayer, whatIsPlayer);
        aCheirarComida = Physics.CheckSphere(transform.position, cheirarComida, whatIsFood);
        aCheirarAgua = Physics.CheckSphere(transform.position, cheirarAgua, whatIsDrink);
        
        Descancar();
        Foge();
        Beber();
        Comer();
        if (!aDescancar && !emFuga && !aBeber && !aComer)
        {
            Patroling();
        }
    }

    IEnumerator EsperarBeber()
    {
        yield return new WaitForSeconds(maxSede/3);
        sede=maxSede;
    }
    IEnumerator EsperarDescancar()
    {
        yield return new WaitForSeconds(maxCancaco/3);
        cancaco=maxCancaco;
    }
    IEnumerator EsperarComer()
    {
        yield return new WaitForSeconds(maxFome/3);
        fome=maxFome;
    }

    private void Descancar()
    {
        if(cancaco < 1)
        {
            aDescancar=true;
            if(cancaco<2)
            {
                StartCoroutine(EsperarDescancar());
                patrollingSoundEffect.Stop();
            }
            if(!aBeber)
            {
                if (sede > 0)
                {
                    sede-= Time.deltaTime;
                }
            }
            if(!aComer)
            {
                if (fome > 0)
                {
                    fome-= Time.deltaTime;
                }
            }
        }
        else if(cancaco >= maxCancaco)
        {
            aDescancar=false;
            StopCoroutine(EsperarDescancar());
        }
    }

    private void Foge()
    {
        if(!aDescancar)
        {
            if (playerASerOuvido && !pm.agachado)
            {
                //patrollingSoundEffect.Play();
                float distance = Vector3.Distance(transform.position, player.position);

                Vector3 dirtoPlayer = transform.position - player.position;
                Vector3 newPos = transform.position + dirtoPlayer;

                agent.SetDestination(newPos);
                buscarDestino = false;
                emFuga = true;
                cancaco-= Time.deltaTime;
            }
            else if (playerASerVisto)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                Vector3 dirtoPlayer = transform.position - player.position;
                Vector3 newPos = transform.position + dirtoPlayer;

                agent.SetDestination(newPos);
                buscarDestino = false;
                emFuga = true;
                if(!aDescancar)
                {
                    if (cancaco > 0)
                    {
                        cancaco-= Time.deltaTime;
                    }
                }
            }else
            {
                emFuga = false;
            }
        }
    }

    private void Beber()
    {
        if(sede < 1 && !emFuga && !aDescancar)
        {
            if(aCheirarAgua)
            {
                aBeber=true;
                agent.SetDestination(water.position);
                buscarDestino = false;
                
                if(sede<2)
                {
                    StartCoroutine(EsperarBeber());
                }
                if(!aDescancar)
                {
                    if (cancaco > 0)
                    {
                        cancaco-= Time.deltaTime;
                    }
                }
                if(!aComer)
                {
                    if (fome > 0)
                    {
                        fome-= Time.deltaTime;
                    }
                }
            }
        }
        else if(sede>=maxSede)
        {
            aBeber=false;
            StopCoroutine(EsperarBeber());
        }
    }

    private void Comer()
    {
        if(fome < 1 && !emFuga && !aDescancar && !aBeber)
        {
            if(aCheirarComida)
            {
                aComer=true;
                agent.SetDestination(grass.position);
                buscarDestino = false;
                
                if(fome<2)
                {
                    StartCoroutine(EsperarComer());
                }
                if(!aDescancar)
                {
                    if (cancaco > 0)
                    {
                        cancaco-= Time.deltaTime;
                    }
                }
                if(!aBeber)
                {
                    if (sede > 0)
                    {
                        sede-= Time.deltaTime;
                    }
                }
                
            }
        }
        else if (fome >= maxFome)
        {
            aComer=false;
            StopCoroutine(EsperarComer());
        }
    }

    private void Patroling()
    {
        
            if (!buscarDestino)
            {
                Debug.Log("Search");
                Searchdestino();
            }

            if(buscarDestino)
            {
                Debug.Log("Patrolling");
                agent.SetDestination(destino);
            }

            Vector3 distanceTodestino = transform.position - destino;

            if (distanceTodestino.magnitude < 1f)
            {
                buscarDestino = false;
                patrollingSoundEffect.Play();
            }
            if(!aBeber)
            {
                if (sede > 0)
                {
                    sede-= Time.deltaTime;
                }
            }
            if(!aDescancar)
            {
                if (cancaco > 0)
                {
                    cancaco-= Time.deltaTime;
                }
            }
            if(!aComer)
            {
                if (fome > 0)
                {
                    fome-= Time.deltaTime;
                }
            }
            tempoPreso-= Time.deltaTime;
            if (tempoPreso <= 1)
            {
                Debug.Log("Search tempoPreso");
                Searchdestino();
            }
    }
    private void Searchdestino()
    {
        tempoPreso = tempoPresoCompare;
        float randomZ = Random.Range(-tamanhoBusca, tamanhoBusca);
        float randomX = Random.Range(-tamanhoBusca, tamanhoBusca);

        destino = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(destino, -transform.up, 2f, whatIsGround));
        {
            buscarDestino= true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, cheirarAgua);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, cheirarComida);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, verPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ouvirPlayer);
    }
}