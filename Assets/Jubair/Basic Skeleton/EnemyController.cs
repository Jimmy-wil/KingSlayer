using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{

    private Animator myAnim;
    private Transform target;
    [SerializeField]
    
    private float speed;
    [SerializeField]
    private float maxRange;

    [SerializeField]
    private float minRange;
    [SerializeField]
    private float attackRange;

    public int damage;
    public PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void FindNearestPlayer()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        if (objects.Length == 0) return;

        Vector3 currentPosition = transform.position;

        float nearestDistance = (objects[0].transform.position - currentPosition).sqrMagnitude;

        target = objects[0].transform;

        for (int i = 1; i < objects.Length; i++)
        {
            Vector3 directionToTarget = objects[i].transform.position - currentPosition;
            float distanceToTarget = directionToTarget.sqrMagnitude;
            if (distanceToTarget < nearestDistance)
            {
                nearestDistance = distanceToTarget;
                target = objects[i].transform;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        FindNearestPlayer();
        if (target == null) { return; }

        if(Vector3.Distance(target.position, transform.position)<=maxRange && Vector3.Distance(target.position,transform.position)>=minRange)
        {
            FollowPlayer();
        }
        else 
        {
            myAnim.SetBool("IsMoving", false);
        }

       if(Vector3.Distance(target.position, transform.position)<=attackRange)
       {
          AttackPlayer();
       }
       else
       {
          myAnim.SetBool("InRange",false);
       }
        
    }
    public void FollowPlayer()
    {
        myAnim.SetBool("IsMoving",true);
        myAnim.SetFloat("moveX",(target.position.x-transform.position.x));
        myAnim.SetFloat("moveY",(target.position.y-transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,speed =Time.deltaTime);
    }

   public void AttackPlayer()
    {
        Debug.Log("Attacking");
        myAnim.SetBool("IsMoving", false);
        myAnim.SetBool("InRange", true);
        myAnim.SetFloat("moveX", (target.position.x - transform.position.x));
        myAnim.SetFloat("moveY", (target.position.y - transform.position.y));
        /*
        playerHealth.TakeDamage(damage);
        */

   }


}
