using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Script : MonoBehaviour
{
    public Vector3 lookDir;
    public GameObject target;
    [SerializeField] float speed = 1f;
    [SerializeField] float damageValue = 2f;
 
    private void Start()
    {
        lookDir = (target.transform.position - gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = lookDir;
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            collision.GetComponent<Combat_Script>().damageParticle.Play();
            collision.GetComponent<Combat_Script>().healthValue -= damageValue;
            Destroy(gameObject);
        }
    }
}
