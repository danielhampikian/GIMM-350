using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject player; //prefab had no reference on load scene
    public GameObject[] spawnPoints;
    public GameObject alien;
    public GameObject upgradePrefab;
    public Gun gun; //prefab has no reference on load scene
    public int level;
    public int score;
    public float upgradeMaxTimeSpawn = 7.5f;
    public int maxAliensOnScreen;
    public int initialAliensOnLoad;
    public int totalAliens;
    public int difficulty = 5;
    public int initialTotalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;
    public GameObject deathFloor; //prefab had no reference on load scene
    public Animator arenaAnimator;
    public Vector3 scaleParams = new Vector3(3f,3f,3f);
    private int aliensOnScreen = 0;
    private float generatedSpawnTime = 0;
    private float currentSpawnTime = 0;
    public bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0f;
    private float currentUpgradeTime = 0f;

    public Vector3 getRandomScaleVector()
    {
        return new Vector3(Random.Range(0, scaleParams.x), 
            Random.Range(0, scaleParams.y), 
            Random.Range(0, scaleParams.z));
    }


    public static GameManager instance = null;
    // Use this for initialization
    void Start()
    {

        if (instance == null)
        {
            instance = this; //provide global reference to single script instance
        }
        else if (instance != this)
        {
            Destroy(gameObject); //enforce singleton pattern
        }

        DontDestroyOnLoad(gameObject); //persist throughout game
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        player = GameObject.FindGameObjectWithTag("Player");

        PlayerController pc = player.GetComponentInChildren<PlayerController>();
        deathFloor = GameObject.FindGameObjectWithTag("deathfloor");

    }
    private void endGame()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.elevatorArrived);
        arenaAnimator.SetTrigger("PlayerWon");
        StartCoroutine("waitAndLoad");
        

    }
    IEnumerator waitAndLoad()
    {
        SceneManager.LoadScene("final"); //load scene

        yield return new WaitForSeconds(.02f); //wait for load to happen

        //grab all references
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        player = GameObject.FindGameObjectWithTag("Player");
        deathFloor = GameObject.FindGameObjectWithTag("deathfloor");
        initialTotalAliens += 10;
        totalAliens = initialTotalAliens;
        maxAliensOnScreen = totalAliens;
        
    }
    public void AlienDestroyed() //add this as an event listener in the inspector under alien's ondestroyed, or since it's a prefab add it to the alien when you instantiate it below
    {
        aliensOnScreen -= 1;
        totalAliens -= 1;
        if(totalAliens <= 0)
        {
            Invoke("endGame", 2.0f);
        }
    }
    
    public void setUpgradeReady()
    {
        Physics.gravity = new Vector3(0, -40, 0);
        spawnedUpgrade = false;
    }


	public void updateScore()
    {
        score++;
        Debug.Log(score);
    }
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
        {
            return;
        }
        if (currentUpgradeTime < upgradeMaxTimeSpawn)
        {
            currentUpgradeTime += Time.deltaTime;
        }

        currentSpawnTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime)
        {
            if (!spawnedUpgrade)
            {

                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                spawnedUpgrade = true;
                currentUpgradeTime = 0;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                List<int> previousSpawnLocations = new List<int>();
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        aliensOnScreen += 1;
                        int spawnPoint = -1;
                        while (spawnPoint == -1)
                        {
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        GameObject newAlien = Instantiate(alien) as GameObject;
                        newAlien.transform.position = spawnLocation.transform.position;
                        Alien alienScript = newAlien.GetComponent<Alien>();
                        alienScript.target = player.transform;
                        Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                        newAlien.transform.localScale = getRandomScaleVector();
                        alienScript.OnDestroy.AddListener(AlienDestroyed); //this is adding the game manager itself as a listener for Alien events
                        alienScript.GetDeathParticles().SetDeathFloor(deathFloor);
                    }
                }
            }
        }
    }
}
