using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBullet : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    Vector3 centerPos;
    //돌아가는 속도
    float rotateSpd = 10;
    //돌아간 각도
    float angle;
    //거리
    float radius = 100;

    void Start()
    {
       // Draw();
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Hp -= 2;
            Destroy(gameObject);
        }
    }

    public void InitBullet(Vector3 _startPos, float _radius, float _rotateSpd, float _startAngle)
    {
        centerPos = _startPos;
        rotateSpd = _rotateSpd;
        radius = _radius;
        angle = _startAngle;
    }

    private void Update()
    {
        angle += rotateSpd * Time.deltaTime;

        Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
        pos += centerPos;

        transform.position = pos;
    }

    void Draw()
    {
        for (int i = 0; i < 360; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            lineRenderer.SetPosition(i, pos);
            angle += 1;
        }
    }
}
