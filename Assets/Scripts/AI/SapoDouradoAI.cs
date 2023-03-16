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

    [Header("Patrulhar")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Fuga")]
    public float tempoFuga;
    bool emFuga;

    [Header("States")]
    public float sightRange, hearRange, foodRange, drinkRange;
    public bool playerInSightRange, playerInHearRange, foodInSmellRange, drinkInSmellRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        water = GameObject.Find("Waterplane").transform;
        grass = GameObject.Find("Bush").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInHearRange = Physics.CheckSphere(transform.position, hearRange, whatIsPlayer);
        foodInSmellRange = Physics.CheckSphere(transform.position, foodRange, whatIsFood);
        drinkInSmellRange = Physics.CheckSphere(transform.position, drinkRange, whatIsDrink);
        Patroling();
        if (playerInSightRange)
        {
            Running();
            Debug.Log("Running");
        }
        else if (playerInHearRange && !pm.agachado)
        {
            Running();
            Debug.Log("Hearyou");
        }
        else if (foodInSmellRange)
        {
            Eating();
            Debug.Log("WannaEat");
        }
        else if (drinkInSmellRange)
        {
            Drinking();
            Debug.Log("WannaDrink");
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround));
        {
            walkPointSet= true;
        }
    }
    private void Running()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 dirtoPlayer = transform.position - player.position;
        Vector3 newPos = transform.position + dirtoPlayer;

        agent.SetDestination(newPos);
    }
    private void Drinking()
    {
        agent.SetDestination(water.position);
    }
    private void Eating()
    {
        agent.SetDestination(grass.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, drinkRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, foodRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearRange);
    }
}
