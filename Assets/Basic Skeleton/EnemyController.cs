using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
        target =FindAnyObjectByType<PlayerControl>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(target.position, transform.position)<=maxRange && Vector3.Distance(target.position,transform.position)>=minRange)
        {
            FollowPlayer();
        }
        else 
        {
            myAnim.SetBool("IsMoving",false);
        }

       if(Vector3.Distance(target.position,transform.position)<=attackRange)
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
        myAnim.SetBool("IsMoving", false);
        myAnim.SetBool("InRange", true);
        myAnim.SetFloat("moveX", (target.position.x - transform.position.x));
        myAnim.SetFloat("moveY", (target.position.y - transform.position.y));
        playerHealth.TakeDamage(damage);

   }


}
