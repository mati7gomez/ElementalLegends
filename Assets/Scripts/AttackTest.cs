using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    Vector2 a = new Vector2();
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            a = new Vector2(-2,0);
            transform.Translate(a * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            a = new Vector2(2, 0);
            transform.Translate(a * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<IDamagable>().ReceiveDamage(gameObject.transform, 10f);
        }
    }
}
