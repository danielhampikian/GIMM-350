using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum WeaponType //this class can be seen and used by other classes in the project
    {
        none,
        blaster,
        spread,
        phaser,
        missile,
        laser,
        shield
    }

    [System.Serializable]
    public class WeaponDefinition
    {
        public WeaponType type = WeaponType.none;
        public string letter;
        public Color color = Color.white;
        public GameObject projectilePrefab;
        public Color projectileColor = Color.white;
        public float damageOnHit = 10;
        public float continuousDamage = 0; //laser has dps
        public float delayBetweenShots = 0.2f;
        public float velocity = 20;
    }

    static public Transform PROJECTILE_ANCHOR;
    public WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //last time shot was fired
    private Renderer collarRend;

	
	void Start () {
        collar = transform.Find("Collar").gameObject;//finding a subchild by name
        collarRend = collar.GetComponent<Renderer>();
        SetType(_type);
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        GameObject rootGo = transform.root.gameObject;
        if (rootGo.GetComponent<Hero>() != null)
        {
            rootGo.GetComponent<Hero>().fireDelegate += Fire;
        }
	}
	public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }
    public void Fire()
    {
       
        //if game object isn't active, return
        if (!gameObject.activeInHierarchy) return;
        //if enough time hasn't passed between shots, return

        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y<0)
        {
            vel.y = -vel.y; //if enemy shooting, we need to switch the sign of the velocity
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }
	public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        //Bug in the books code: The next line sets projectilehero to projectile_anchor, making collisions impossible
        //go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
