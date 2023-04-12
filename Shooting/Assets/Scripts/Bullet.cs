using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool isRun = true;
    public float DestoryDelayTime = 15f;
    void Start()
    {
        Destroy(gameObject, DestoryDelayTime);
    }

    void Update()
    {
        if (!isRun) return;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hp -= 2;
            Destroy(gameObject);
        }
    }
}
