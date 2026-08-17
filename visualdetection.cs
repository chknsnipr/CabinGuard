using MimicSpace;
using UnityEngine;
using UnityEngine.AI;

public class visualdetection : MonoBehaviour
{
    public GameObject damageNumberPrefab;
    public float speed = 5.0f;
    private GameObject Target;

    private NavMeshAgent agent;

    
    [SerializeField] private float enemyhealth = 100f;

    public static bool inMotion = true;

    private float destinationUpdateRate = 0.2f;
    private float destinationTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        Target = GameObject.FindWithTag("Player");
        DiffScale();
        
    }

    void Update()
    {
        HealthGain();
        ChaseTarget();
        CheckIdle();
        Death();
    }

    void ChaseTarget()
    {
        if (Target == null) return;

        destinationTimer += Time.deltaTime;
        if (destinationTimer >= destinationUpdateRate)
        {
            agent.SetDestination(Target.transform.position);
            destinationTimer = 0f;
        }
    }

    void CheckIdle()
    {
        if (agent.pathPending)
        {
            
            inMotion = true;
            return;
        }

        bool hasVelocity = agent.velocity.sqrMagnitude > 0.01f;
        bool stillTraveling = agent.remainingDistance > agent.stoppingDistance;

        inMotion = hasVelocity && stillTraveling;
    }

    public void damagemech(float x)
    {
        enemyhealth -= x;
        Debug.Log(enemyhealth);


        SpawnDamageNumber(x);
    }

    void Death()
    {
        if (enemyhealth <= 0f)
        {
            GameManager.kills+=1;
            Destroy(gameObject, 0f);

        }
    }
    void DiffScale()
    {
        enemyhealth=enemyhealth*GameManager.WaveCount*.5f;
    }
    void HealthGain()
    {
        if(Movement.heal==true)
        {
            enemyhealth+=100f*GameManager.WaveCount*0.2f;
            Movement.heal=false;
        }
    }

    void SpawnDamageNumber(float damageAmount)
{
    if (damageNumberPrefab == null) return;

    Vector3 spawnPos = transform.position + Vector3.up * 2f; // adjust height to roughly head/chest level
    GameObject dmgObj = Instantiate(damageNumberPrefab, spawnPos, Quaternion.identity);
    dmgObj.GetComponent<DamageNumber>().Setup(damageAmount);
}
}