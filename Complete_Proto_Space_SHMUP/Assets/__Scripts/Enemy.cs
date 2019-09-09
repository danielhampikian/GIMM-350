using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f; //seconds to show damage
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime; //time to stop showing damage
    public bool notifiedOfDestruction = false;
    public float powerUpDropChance = 1f;
    //this is a property: A method/function that acts like a field so we can get and set pos as if it were a class variable of enemy
    public Vector3 pos {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    //Here's how you access a script from another component in the same class:
    protected BoundsCheck bndCheck; //we need protected to inherit in subclasses

    private void Awake() //before start, the instant game object is instantiated
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    void Update () {
        Move();
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck!=null && !bndCheck.isOnScreen)
        {
            if(pos.y<bndCheck.camHeight - bndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
	}
    public virtual void Move() //written as virtual here so we can override in subclasses without declaring it virtual in all of those
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Transform rootT = collision.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Enenmy name: " + this.gameObject.name + " Triggered by: " + go.name);

        //if(this.gameObject.name == "Enemy_4(Clone)")
        //{
            //print("Enemy 4 detected, handle collision in script");
        //    return;
       // }
        switch (go.tag)
        {
            case "ProjectileHero":
            Projectile p = go.GetComponent<Projectile>(); //we need to know the damage, so we need a reference to projectile
                //only damage enemies currently on screen
                if (!bndCheck.isOnScreen)
                {
                    Destroy(go);
                    break;
                }
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowDamage();
                if (health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(go);
                break;
            default:
                //print("Enemy hit by non-projectilehero: " + go.name);
                break;
        }
    }
    protected void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
    protected void UnShowDamage()
    {
        for (int i = 0; i< materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
