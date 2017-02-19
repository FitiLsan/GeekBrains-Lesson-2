using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{
    public GameObject Bullet, StartBullet; // шаблон пули
    public Rigidbody MyBody;  // Ссылка на свой Rigidboody
    public float Speed; // Скорость игрока
    private Vector3 Movement; 

    public bool CanAttack = true, Reload = false;

    public const int Magazine = 30;
    public int Ammo = 90, CurMagazine = 30;

    public int Health;

    // Use this for initialization
    void Start()
    {
        MyBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            StartCoroutine(Fire());

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(StartReload());
    }

    void FixedUpdate()
    {
        float Right = Input.GetAxisRaw("Horizontal");
        float Forward = Input.GetAxisRaw("Vertical");

        Movement.Set(Forward, 0f, Right);

        MyBody.AddForce(transform.forward * Forward * Speed, ForceMode.Impulse);
        MyBody.AddForce(transform.right * Right * Speed, ForceMode.Impulse);
    }

    IEnumerator Fire()
    {
        if (CanAttack && CurMagazine > 0 && !Reload)
        {
            CanAttack = false;

            CurMagazine--;

            Instantiate(Bullet, StartBullet.transform.position, StartBullet.transform.rotation);

            if (CurMagazine <= 0)
            {
                StartCoroutine(StartReload());
                Reload = true;
            }

            yield return new WaitForSeconds(0.05f);

            CanAttack = true;
        }
    }

    IEnumerator StartReload()
    {        
        yield return new WaitForSeconds(1f);

        if(Ammo > Magazine)
        {
            int Num = Magazine;
            Num = Num - CurMagazine;
            Ammo -= Num;
            CurMagazine = Magazine;
        }

        else
        {
            CurMagazine = Ammo;
            Ammo = 0;
        }

        Reload = false;
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            //Условно смерть
        }
    }
}
