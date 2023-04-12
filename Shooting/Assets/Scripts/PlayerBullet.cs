using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool isRun = true;

    [SerializeField] private GameObject Explosion;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        if (!isRun) return;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossPattern>().Hp -= 2;
            Instantiate(Explosion, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
