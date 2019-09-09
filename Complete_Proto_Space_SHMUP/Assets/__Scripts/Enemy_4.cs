using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part
{
    public string name;
    public float health;
    public string[] protectedBy;
    public GameObject go;
    public Material mat; //material to show damage
}
public class Enemy_4 : Enemy {

    public Part[] parts;
    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4;

	void Start () {
        p0 = p1 = pos;
        InitMovement();
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
	}
	
    void InitMovement()
    {
        //print("In init movement");
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camWidth - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        
        timeStart = Time.time;
    }
    public override void Move()
    {
        //print("In move");
        float u = (Time.time - timeStart) / duration;
        //print("float u is " + u + " and Time.time is: " + Time.time + " and timeStart is: " + timeStart);
        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }
        else
        {
            u = 1 - Mathf.Pow(1 - u, 2);//apply ease out easing to u - begins quickly, slows as approaches
            pos = (1 - u) * p0 + u * p1; //interpolate
            //print("Moving from " + p0.x + " x " + p0.y + " y \n to " + p1.x + " x " + p1.y + " y ");
           
        }
    }

    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if(prt.name == n)
            {
                return (prt);
            }
        }
        return null;
    }
    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go == go)
            {
                return prt;
            }
        }
        return null;
    }
    Part FindPartRandomly()
    {
        if (parts.Length <= 0)
        {
            return null;
        }
        else
        {
            int ndx = parts.Length;
            return parts[Random.Range(0, ndx)];
        }
    }
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if(prt == null)
        {
            return (true);
        }
        return (prt.health <= 0);
    }
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    private void OnTriggerEnter(Collider collision)
    {
        //bug in the book code, got around by randomizing which part gets damaged and not damaging protected parts, makes killing them a lot harder 
        Transform rootT = collision.gameObject.transform.root;
        GameObject other = rootT.gameObject;
        //print("In collision for enemy 4");
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                
                Part prtHit = FindPartRandomly();
                if (prtHit == null)
                {
                    //there's no parts left, destroy the object - first double check:
                    bool allDestroyed = true;
                    foreach (Part prt in parts)
                    {
                        if (!Destroyed(prt))
                        {
                            allDestroyed = false;
                            break;
                        }
                    }
                    if (allDestroyed)
                    {
                        Main.S.ShipDestroyed(this);
                        Destroy(this.gameObject);

                        Destroy(other);
                        break;
                    }

                }
                if (prtHit.protectedBy != null)
                {
                    foreach (string s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            //then don't damage this part yet
                            Destroy(other);
                            return;
                        }
                    }
                }
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    prtHit.go.SetActive(false);
                }
                
                break;
        }
    }
    // Update is called once per frame
    void Update () {
        Move();
        //TODO: Figure out why this doesn't get inherited automatically
        if (showingDamage && Time.time > damageDoneTime)
        {
            base.UnShowDamage();
        }
    }
}
