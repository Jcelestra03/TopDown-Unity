using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public bool canShoot = true;
    public float Scooldown = 5;
    private float timeDifference;

    public AudioClip SlashsoundEffect;
    public AudioClip ShootsoundEffect;
    public AudioClip HitsoundEffect;
    public AudioClip HealthsoundEffect;
    public AudioClip PowersoundEffect;
    private AudioSource speaker;

    private Animator myAnimator;
    public bool flip;

    public bool canTele = false;
    public float TeleAmount = 0;

    private Rigidbody2D myRB;
    public float speed = 10;
    public int playerHealth = 3;

    public GameObject bullet;
    public float bulletSpeed = 15;
        public float bulletLifeSpan = 2;
    public GameObject bullet2;
    public float bullet2Speed = 15;
    public float bullet2LifeSpan = 1;
    public GameObject bullet3;
    public float bullet3Speed = 15;
    public float bullet3LifeSpan = 1;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();

        speaker = GetComponent<AudioSource>();

        playerHealth = 3;
        myAnimator = GetComponent<Animator>();

        flip = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (TeleAmount > 0)
        {
            canTele = true;
        }

        else if (TeleAmount == 0)
        {
            canTele = false;
        }

        if (playerHealth <= 0)
        {
            transform.SetPositionAndRotation(new Vector2(), new Quaternion());
            playerHealth = 3;
        }

        Vector2 velocity = myRB.velocity;

        velocity.x = (Input.GetAxisRaw("Horizontal")) * speed;
        velocity.y = (Input.GetAxisRaw("Vertical")) * speed;

        myRB.velocity = velocity;

        if(myRB.velocity.x == 0 && myRB.velocity.y == 0)
        {
            myAnimator.SetBool("IsWalking", false);

        }

        else if (myRB.velocity.x > 0 || myRB.velocity.y > 0 || myRB.velocity.x < 0 || myRB.velocity.y < 0)
        {
            myAnimator.SetBool("IsWalking", true);
        }

        if(myRB.velocity.x < 0)
        {
            flip = true;
        }
        
        else if (myRB.velocity.x > 0)
        {
            flip = false;
        }

        if(flip == true)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        else if (flip == false)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) && canShoot)
        {
            timeDifference = Time.deltaTime;

            GameObject Hbullet = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + 1), transform.rotation);
            Hbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);

            speaker.clip = ShootsoundEffect;
            speaker.Play();

            Destroy(Hbullet, bulletLifeSpan);

            canShoot = false;
        }
        
        else if (Input.GetKeyDown(KeyCode.DownArrow) && canShoot)
        {
            GameObject Hbullet = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y - 1), transform.rotation);
            Hbullet.GetComponent<SpriteRenderer>().flipY = true;
            Hbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);

            speaker.clip = ShootsoundEffect;
            speaker.Play();

            Destroy(Hbullet, bulletLifeSpan);

            canShoot = false;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject Hbullet = Instantiate(bullet3, new Vector2(transform.position.x-1, transform.position.y), transform.rotation);
            Hbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bullet3Speed, 0);

            speaker.clip = SlashsoundEffect;
            speaker.Play();

          Destroy(Hbullet, bullet3LifeSpan);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject Hbullet = Instantiate(bullet2, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
            Hbullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bullet2Speed, 0);

            speaker.clip = SlashsoundEffect;
            speaker.Play();

          Destroy(Hbullet, bullet2LifeSpan);
        }
        if (canTele == true)
        {
            if (Input.GetKeyDown(KeyCode.Space) && GameObject.Find("kbullet(Clone)"))
            {
                transform.SetPositionAndRotation(GameObject.Find("kbullet(Clone)").transform.position, new Quaternion());
                Destroy(GameObject.Find("kbullet(Clone)"));
                TeleAmount--;
            }
        }

        if (canShoot == false)
        {
            timeDifference += Time.deltaTime;

            if (timeDifference >= Scooldown)
            {
                canShoot = true;
                timeDifference = 0;
            }
                
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.name.Contains("enemy"))
        {
            // This also means playerHealth = playerHealth - 1;

            speaker.clip = HitsoundEffect;
            speaker.Play();

            playerHealth--;
        }

       else if((collision.gameObject.name.Contains("health")) && (playerHealth <3))
        {
            // This also means playerHealth = playerHealth + 1;


            playerHealth++;
            collision.gameObject.SetActive(false);
        }

       else if((collision.gameObject.name.Contains("pickup")) && (TeleAmount <= 5))
        {
            canTele = true;
            TeleAmount++;
            collision.gameObject.SetActive(false);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "enemyTrigger")
        {
            GameObject.Find("enemy").GetComponent<enemyMovement>().canMove = true;
            GameObject.Find("enemy").GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            Destroy(collision.gameObject);
        }
                
    }
}
