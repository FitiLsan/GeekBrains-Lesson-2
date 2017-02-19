using UnityEngine;
using System.Collections;

public class Enemy2 : MonoBehaviour
{
    public Transform Target;

    public float Speed, TimeCast;
    public float MaxDistance, SpeedRotation, MinDistance, AttackDistance;

    public int Health;
    public int Damage;
    Rigidbody MyBody;
    Transform MyTransform;

    public bool Couldown = false;

    // Use this for initialization
    void Start ()
    {
        MyBody = GetComponent<Rigidbody>();
        MyTransform = transform;

        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Debug.Log("MaxDistance " + MaxDistance);

        if (Vector3.Distance(MyTransform.position, Target.position) < MaxDistance)
        {
           // Debug.Log("Дистанция меньше MaxDistance " + MaxDistance);
            Vector3 rot = Target.position - MyTransform.position;
            
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, Quaternion.LookRotation(rot), SpeedRotation * Time.deltaTime);

            if (Vector3.Distance(MyTransform.position, Target.position) > MinDistance)
                MyTransform.position += MyTransform.forward * Speed * Time.deltaTime;

            if (Vector3.Distance(MyTransform.position, Target.position) < AttackDistance)
                if (!Couldown)
                {
                    Couldown = true;
                    StartCoroutine(Attack());
                }
        }
	}

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(TimeCast);

        if (Vector3.Distance(MyTransform.position, Target.position) < AttackDistance)
            Target.GetComponent<Player>().TakeDamage(Damage);

        Couldown = false;
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            MyBody.constraints = RigidbodyConstraints.None;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
