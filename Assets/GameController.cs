using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] asteroids = new GameObject[3];

    public Text dnaText;

    public int dna = 0;

    public int hazardCount;

    public float startWait, spawnWait, waveWait;
    [Header("Random spawn coordinates")]
    public float minX, maxX, minY, maxY;
   

    public PlayerController playerOne;
    public PlayerController playerTwo;

    public bool restartGame = false;

    private void Start()
    {
    
        dna = PlayerPrefs.GetInt("DNA");
        dnaText.text = "DNA: " + dna.ToString();

        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if(playerTwo.isRestoring == true && playerOne.isRestoring == true)
        {
            Camera.main.backgroundColor = Color.red;
            StartCoroutine(Restart());
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        float[] astSpeeds = new float[asteroids.Length];
        float[] astHealths = new float[asteroids.Length];

        for (int i = 0; i < 3; i++)
        {
            Asteroid currentAsteroid = asteroids[i].GetComponent<Asteroid>();

            astSpeeds[i] = currentAsteroid.speed;
            astHealths[i] = currentAsteroid.standardHealth;
        }

        while (true)
        {


            yield return new WaitForSeconds(waveWait);
            for (int i = 0; i <= hazardCount; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    Vector3 spawnposition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
                    GameObject astInstance = Instantiate(asteroids[j], spawnposition, Quaternion.identity);
                    float rndScaleFactor = Random.Range(0.1f, 0.75f);
                    astInstance.transform.localScale = new Vector3(astInstance.transform.localScale.x * rndScaleFactor, astInstance.transform.localScale.y * rndScaleFactor, astInstance.transform.localScale.z);
                    Asteroid currentAsteroid = astInstance.GetComponent<Asteroid>();

                    currentAsteroid.speed = astSpeeds[j];
                    currentAsteroid.standardHealth = astHealths[j]; //Not needed rn but may be in future
                    currentAsteroid.currentHealth = astHealths[j];
                }

                yield return new WaitForSeconds(spawnWait);
                if (spawnWait > 0.1f)
                    spawnWait -= 0.25f;
                else
                    spawnWait = 0.1f;
            }

            hazardCount++;
            if (waveWait > 0.5f)
                waveWait -= 0.1f;
            else
                waveWait = 0.5f;


            for (int i = 0; i < 3; i++)
            {
                astSpeeds[i] *= 1.05f;
                astHealths[i] *= 1.05f;
            }
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2.5f);
        restartGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddDNA()
    {
        dna += Random.Range(100, 350);
        PlayerPrefs.SetInt("DNA", dna);
        dnaText.text = "DNA: " + dna.ToString();
    }
}
