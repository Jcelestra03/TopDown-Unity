using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour


{   //Variables
    public int enemyHealth = 12;

    public float detectionRadius = 2;
    public float speed = 5;
    public bool canMove = false;
    public bool movementDirection = false; // false goes down, true goes up
    public bool isFollowing = false;

    public Transform playerTarget;
    private Rigidbody2D myRB;
    private CircleCollider2D detectionZone;
    private Vector2 up;
    private Vector2 down;
    private Vector2 zero;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        enemyHealth = 6;

        up = new Vector2(0, speed);
        down = new Vector2(0,-speed);

        playerTarget = GameObject.Find("Circle").transform;

        myRB = GetComponent<Rigidbody2D>();
        detectionZone = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            this.gameObject.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().playerKillCount++;

        }

        detectionZone.radius = detectionRadius;

        if (isFollowing == false)
            myRB.velocity = zero;

        else if (isFollowing == true)
        {
            Vector3 lookPos = playerTarget.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            myRB.rotation = angle;
            lookPos.Normalize();

            myRB.MovePosition(transform.position + (lookPos * speed * Time.deltaTime));
        }

        if (canMove == true)
        {

            myRB.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

            if (movementDirection == false)
                myRB.velocity = down;

            else if (movementDirection == true)
                myRB.velocity = up;

        }
        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("kbullet"))
        {
            Destroy(collision.gameObject);
            enemyHealth = enemyHealth - 1;
        }

        else if ((collision.gameObject.name.Contains("Sbullet")))
        {
            Destroy(collision.gameObject);
            enemyHealth = enemyHealth - 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Circle"))
            isFollowing = true;

       if(collision.gameObject.name == "trigger1" )
        {
            movementDirection = true;
        }

       else if (collision.gameObject.name == "trigger2")
        {
            movementDirection = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Circle"))
            isFollowing = false;
    }
}
