using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [SerializeField] private GameObject Explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hp -= 10;
            GameObject temp = Instantiate(Explosion, transform.position, Quaternion.Euler(0, 0, 0));
            temp.transform.localScale = new Vector3(20, 20, 20);
            temp.GetComponent<SpriteRenderer>().sortingOrder = 6;
            gameObject.SetActive(false);
        }
    }
}
