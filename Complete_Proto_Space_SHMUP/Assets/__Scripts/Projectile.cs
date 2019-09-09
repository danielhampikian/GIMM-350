using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private BoundsCheck bndCheck;
    private Renderer rend;
    public Rigidbody rigid;
    [SerializeField]
    private Weapon.WeaponType _type;

        public Weapon.WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }
    public void SetType(Weapon.WeaponType eType)
    {
        _type = eType;
        Weapon.WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
	void Awake () {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
	}
}
