using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f; 
    public float fadeTime = 4f;
    public Weapon.WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;
    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;
    private Material[] materials;

	void Awake () {
        cube = transform.Find("Cube").gameObject; //find a reference by transform
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = GetComponent<Renderer>();
        materials = Utils.GetAllMaterials(cube);
        Vector3 vel = Random.onUnitSphere; //Random xyz velocity
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;
        transform.rotation = Quaternion.identity;
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y), 
            Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime; //power up will exist for 10 secoinds then fade out over 4 seconds
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        if (u > 0)
        {
            Color c = new Color();
            //TODO: Figure out why material of cube alpha is not fading
            foreach (Material m in materials)
            {
                c = m.color; 
                c.a = 1f - u;
            }

            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (u * .5f);
            letter.color = c;
        }
        if(!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(Weapon.WeaponType wt)
    {
        Weapon.WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }
    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
