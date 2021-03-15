using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    private int scoreValue = 0;

    public Text lives;

    private int livesValue = 3;

    public Text winText;

    public Text loseText;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    private bool facingRight = true;

    Animator anim;

    //the music used for clip 1 can be found here: https://opengameart.org/content/little-town
    //the music used for clip 2 can be found here: https://opengameart.org/content/fun-is-infinite-at-agm

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: 0";
        lives.text = "Lives: 3";
        winText.text = "";
        loseText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (vertMovement == 0 && hozMovement == 0 || (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
        {
            anim.SetInteger("State", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: "+scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            
            if (scoreValue == 4) 
            {
                livesValue = 3;
                lives.text = "Lives: "+livesValue.ToString();
                transform.position = new Vector2(25.0f, 0.5f);
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: "+livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (livesValue == 0)
        {
            loseText.text = "You Lose! Game created by Aaron Rose.";
            Destroy(gameObject);
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
        
        if (scoreValue == 8)
        {
            Destroy(gameObject);
            winText.text = "You Win! Game created by Aaron Rose.";
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            anim.SetInteger("State", 0);
            
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
        
    }

   void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}