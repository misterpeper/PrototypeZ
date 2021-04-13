using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class SC_NPCEnemy : MonoBehaviour, IEntity
{
    public float attackDistance = 2f;
    public float movementSpeed = 5f;
    public float npcHP = 180;

    //How much damage will npc deal to the player
    public float npcDamage = 5;
    public float attackRate = 1f;
    public Transform firePoint;
    public GameObject npcDeadPrefab;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public SC_EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    Rigidbody r;
    private Animator animator;

    // VFX Sounds
    AudioSource randomSound;
    [SerializeField] private AudioClip[] audioSources;
    [SerializeField] private AudioClip enemyDie;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
        animator = GetComponent<Animator>();
        randomSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (agent.remainingDistance - attackDistance < 0.1f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);
                        IEntity player = hit.transform.GetComponent<IEntity>();
                        player.ApplyDamage(npcDamage);
                        VFXSounds();
                    }
                }
            }

            animator.SetInteger("condition", 1);
        }
        else
        {
            animator.SetInteger("condition", 0);
        }
        //Move towardst he player
        agent.destination = playerTransform.position;
        //Always look at player
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
        //Gradually reduce rigidbody velocity if the force was applied by the bullet
        r.velocity *= 0.99f;
    }

    void VFXSounds()
    {
        Invoke("RandomSounds", 0);
    }

    void RandomSounds()
    {
        if (!randomSound.isPlaying)
        {
            randomSound.spatialBlend = Random.Range(0.4f, 0.6f);
            randomSound.volume = 0.65f;
            randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
            randomSound.Play();
        }

        else
            randomSound.Stop();
    }
    public void ApplyDamage(float points)
    {
        npcHP -= points;

        if (npcHP <= 0)
        {   
            //Destroy the NPC
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
            SoundManager.Instance.Play(enemyDie);
            Destroy(npcDead, 5f);
            Score.scoreValue += 17;
            es.EnemyEliminated(this);
            Destroy(gameObject);
        }
    }
}