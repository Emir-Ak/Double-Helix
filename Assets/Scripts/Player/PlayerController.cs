using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerController : Damageable
{

    //[Tooltip("Which player instance is it, is it player number one?")]
    //public int playerInt = 1;
    public Text hpText;

    Rigidbody2D rb; //Player's rigidbody
    SpriteRenderer spriteRenderer;
    [SerializeField]
    GameObject bulletPrefab;

    public float standardSpeed = 3.5f; //Standard speed
    [HideInInspector]
    public float currentSpeed; //Current speed for speed buffs or defects

    public float rotationSpeed = 5f; //Speed of rotation to sides
    public float minRot = -30f;
    public float maxRot = 30f;

    public float restoreTime = 30f;

    public float bulletRechargeTime = 0.5f;

    public float damage = 10f;

    public int bulletNum = 1;

    [HideInInspector]
    public bool isRestoring = false;

    private bool isShooting = false;

    public Sprite invertedShip;

    [Header("Boundary which can not be exceeded")]
    [SerializeField]
    MovementBoundary movementBoundary;

    //Movement keys
    [Header("Relative keys")]
    public KeyCode right;
    public KeyCode left;
    public KeyCode up;
    public KeyCode down;
    public KeyCode shoot;

    private int firstRun;
    // Use this for initialization
    void Start()
    {
        firstRun = PlayerPrefs.GetInt("savedFirstRun");


        if (firstRun == 0) 
        {
            firstRun = 1;
            PlayerPrefs.SetInt("savedFirstRun", firstRun);
            PlayerPrefs.SetFloat("Health", standardHealth);
            PlayerPrefs.SetFloat("Speed", standardSpeed);
            PlayerPrefs.SetFloat("RechargeTime", bulletRechargeTime);
            PlayerPrefs.SetFloat("RestoreTime", restoreTime);
            PlayerPrefs.SetInt("BulletNum", bulletNum);
            PlayerPrefs.SetInt("UpgradeCost", 1000);
        }
        else
        {
            standardHealth = PlayerPrefs.GetFloat("Health");
            standardSpeed = PlayerPrefs.GetFloat("Speed");
            bulletRechargeTime = PlayerPrefs.GetFloat("RechargeTime");
            restoreTime = PlayerPrefs.GetFloat("RestoreTime");
            bulletNum = PlayerPrefs.GetInt("BulletNum");
        }



        currentHealth = standardHealth;
        currentSpeed = standardSpeed; //Set up current speed
        rb = GetComponent<Rigidbody2D>(); //Reference the rigidbody
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hpText.text = "HP: " + ((int)currentHealth).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 0 && isRestoring == false)
        {


            Vector3 movDir = Vector3.zero;
            float rotDir = 0;

            //Adjust movement and rotation values if input keys are pressed
            if (Input.GetKey(left))
                movDir += transform.right * -1;

            if (Input.GetKey(right))
                movDir += transform.right;

            if (Input.GetKey(up))
            {
                movDir += transform.up;

                //Adjust rotation value based on whether player moves left or right and if its moving ups
                if (Input.GetKey(left))
                    rotDir = minRot;
                else
                    rotDir = maxRot;
            }

            if (Input.GetKey(down))
            {
                movDir += transform.up * -1;

                //Adjust rotation value based on whether player moves left or right and if its moving down
                if (Input.GetKey(left))
                    rotDir = maxRot;
                else
                    rotDir = minRot;
            }

            //Set up boundary values
            float minX = movementBoundary.minX, minY = movementBoundary.minY, maxX = movementBoundary.maxX, maxY = movementBoundary.maxY;

            //Apply movement
            rb.position = Vector3.Lerp(transform.position, transform.position + movDir, currentSpeed * Time.deltaTime);

            //Nullify rotation if boundary reached
            if (transform.position.y == minY || transform.position.y == maxY)
            {
                rb.rotation = Mathf.Lerp(rb.rotation, 0, rotationSpeed * Time.deltaTime);
            }
            //Apply rotation
            else
            {
                rb.rotation = Mathf.Lerp(rb.rotation, rotDir, rotationSpeed * Time.deltaTime);
            }

            //Restrict position to boundary values
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minX, maxX), Mathf.Clamp(rb.position.y, minY, maxY), 0.0f);
            //Restrict rotation to boundary values
            rb.rotation = Mathf.Clamp(rb.rotation, minRot, maxRot);

            if (Input.GetKey(shoot) && isShooting != true)
            {

                Shoot();
                Invoke("StopShooting", bulletRechargeTime);
                
            }

            if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.T))
            {
                spriteRenderer.sprite = invertedShip;
                if (Input.GetKeyDown(KeyCode.D)) { FindObjectOfType<GameController>().dna += 1000000; }
                if (Input.GetKeyDown(KeyCode.N)) {
                    PlayerPrefs.SetFloat("Health", 100f);
                    PlayerPrefs.SetFloat("Speed", 10f);
                    PlayerPrefs.SetFloat("RechargeTime", 0.75f);
                    PlayerPrefs.SetFloat("RestoreTime", 7.5f);
                    PlayerPrefs.SetInt("BulletNum", 1);
                    PlayerPrefs.SetInt("DNA", 0);
                    PlayerPrefs.SetInt("UpgradeCost", 1000);

                }
            }
        }
        else
        {
            if (isRestoring == false)
            {
                isRestoring = true;
                spriteRenderer.color = new Color32(255, 255, 255, 125);
                Invoke("Restore", restoreTime);
            }
        }

    }

    void Restore()
    {
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        currentHealth = standardHealth;
        isRestoring = false;
        hpText.text = "HP: " + ((int)currentHealth).ToString() ;
        Debug.Log("End");
    }
    void Shoot()
    {
        isShooting = true;
        
        for (int i = 0; i < bulletNum; i++)
        {

            GameObject bulletInstance = Instantiate(bulletPrefab);
            if (i == 0)
            {
                bulletInstance.transform.position = new Vector3(transform.position.x + 3f, transform.position.y, 0);
            }
            else {
                bulletInstance.transform.position = new Vector3(transform.position.x + 3f + ((i + 1) % 2 != 0 ? (i/6) * (i/6) : ((-i - 1)/6) * ((-i - 1) / 6)), transform.position.y + ((i + 1) % 2 != 0 ? i / 2f : (-i - 1) / 2f), 0);
            }
            if (i > 0)
            {
                bulletInstance.transform.rotation = Quaternion.Euler(0, 0, ((i + 1) % 2 != 0 ? i * 25f : (-i - 1) * 25f));
            }
            bulletInstance.GetComponent<Bullet>().damage = damage;
        }
       
        
    }

    void StopShooting()
    {
        isShooting = false;
    }

}
