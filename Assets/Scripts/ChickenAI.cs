using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ChickenAI : MonoBehaviour, IInteractable
{
    private bool isInteracting = false;
    private float nextWanderTime;
    public float minWanderInterval = 1f; 
    public float maxWanderInterval = 4f;

    public NavMeshAgent agent;
    public Transform player;
    public Animator animator; 
    public LayerMask whatIsGround, whatIsPlayer;

    //Patrulhamento
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Fuga
    public float fleeDistance = 5f;
    bool isFleeing = false;

    //Estados
    public float sightRange;
    public bool playerInSightRange;

    public bool IsIdle
    {
        get { return !agent.hasPath; }
    }

    public bool IsWalking
    {
        get { return agent.hasPath && agent.velocity.magnitude > 0.1f; }
    }

    public bool IsRunning
    {
        get { return isFleeing; }
    }

    public void Interact()
    {
        if (!isInteracting)
        {
            isInteracting = true; // Bloqueia o movimento durante a interação
            GoToCoop();
        }
    }
    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        // Verificar alcance de visão para o jogador
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInSightRange && !isInteracting)
        {
            if (Time.time >= nextWanderTime)
            {
                Wander();
                nextWanderTime = Time.time + Random.Range(minWanderInterval, maxWanderInterval);
            }
        }
        else if (!isInteracting)
        {
            Fleeing();
        }

        // Configurar os parâmetros do Animator com base nos estados
        animator.SetBool("IsIdle", IsIdle);
        animator.SetBool("IsWalking", IsWalking);
        animator.SetBool("IsRunning", IsRunning);
}

    public void Wander(){
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Verificar se o ponto de destino foi alcançado
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint(){
        // Calcular um ponto aleatório dentro da faixa
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Verificar se o ponto está no chão
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    public void Fleeing(){
        Vector3 distanceToPlayer = transform.position - player.position;

        if (distanceToPlayer.magnitude < fleeDistance){
            Vector3 fleeDirection = transform.position - player.position;
            Vector3 newFleePosition = transform.position + fleeDirection;

            agent.SetDestination(newFleePosition);
        }

    }

    public void GoToCoop(){
        // Encontrar um objeto com a tag "Coop"
        GameObject coop = GameObject.FindWithTag("Coop");
        // Definir o destino para o galinheiro
        agent.SetDestination(coop.transform.position);
    }

    // Verificar se a galinha está no galinheiro e fazê-la "dormir" (destruída)
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisão com " + other.name);
        if (other.CompareTag("Coop"))
        {
            Debug.Log("Galinha entrou no galinheiro");
            Destroy(gameObject);
        }
    }
}
