using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    static public Hero S;//Singleton - there's only one hero and we might want to access from other classes
    
    public GameObject weapon;
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public Transform shootPoint;
    public Weapon[] weapons;
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;

    //Declare a delegate like so for firing many weapons or one:
    public delegate void WeaponFireDelegate();
    //create a feild for this delegate:
    public WeaponFireDelegate fireDelegate;

    private void Start()
    {
        if (S == null)
        {
            S = this;//setting the singleton
        }
        else
        {
            Debug.Log("Hero.Awake() attmpted to assign a second Hero");
        }
        ClearWeapons();
        weapons[0].SetType(Weapon.WeaponType.blaster);
        //fireDelegate += weapon.GetComponent<Weapon>().Fire; //We're adding a method to a delegate here
    }
    // Update is called once per frame
    void Update () {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //must ensure that fireDelegate isn't null to avoid an error
        if(Input.GetAxis("Jump")==1 && fireDelegate != null)
        {
            fireDelegate();
        }
	}

    public float shieldLevel
    {
        get { return _shieldLevel; }
        set { _shieldLevel = Mathf.Min(value, 4); //can never be higher than 4
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayRestart(gameRestartDelay);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Tiggered: " + go.name);

        if(go == lastTriggerGo)
        {
            return;
        }
        if(go.tag == "Enemy")
        {
            shieldLevel--; //this will use both the get and set methods of sheild level above
            Destroy(go);
        }
        else if(go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("tiggered by non-Enemy: " + go.name);
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        print("weapon type: " + pu.type);

        switch (pu.type)

        {
            case Weapon.WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i< weapons.Length; i++)
        {
            if(weapons[i].type == Weapon.WeaponType.none)
            return (weapons[i]);
        }
        return null;
    }
    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(Weapon.WeaponType.none);
        }
    }
}
