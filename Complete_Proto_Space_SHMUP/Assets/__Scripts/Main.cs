using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    static public Main S; //singleton for main
    static Dictionary<Weapon.WeaponType, Weapon.WeaponDefinition> WEAPON_DICT; //static dictionary
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public Weapon.WeaponDefinition[] weaponDefinitions; //error in book
    public GameObject prefabPowerUp;
    public Weapon.WeaponType[] powerUpFrequency = new Weapon.WeaponType[]
    {
        Weapon.WeaponType.blaster, Weapon.WeaponType.blaster,
        Weapon.WeaponType.spread, Weapon.WeaponType.shield
    };
    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, powerUpFrequency.Length);
            Weapon.WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }
	void Awake () {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        WEAPON_DICT = new Dictionary<Weapon.WeaponType, Weapon.WeaponDefinition>();
        foreach(Weapon.WeaponDefinition def in weaponDefinitions)
        {
            WEAPON_DICT[def.type] = def;
        }
	}
	public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);//if we dynamically adjust enemySpawnPerSecond, we can adjust the rate of spawning in this recursive method call
    }
	public void DelayRestart(float delay)
    {
        //waits for delay seconds to invoke the restart loader
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
    static public Weapon.WeaponDefinition GetWeaponDefinition(Weapon.WeaponType wt)
    {
        if (WEAPON_DICT.ContainsKey(wt))
        {
            return (WEAPON_DICT[wt]);

        }
        return (new Weapon.WeaponDefinition());
    }
}
