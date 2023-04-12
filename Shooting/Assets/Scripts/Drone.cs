using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameObject bullet;

    Vector3 Drone1Pos => GameObject.Find("DronePos1").transform.position;
    Vector3 Drone2Pos => GameObject.Find("DronePos2").transform.position;

    public int DroneTpye;

    bool IsShotLaser = false;

    private void Start()
    {
        StartMove();
    }

    Vector3 DronePosType()
    {
        if (DroneTpye == 1)
            return Drone1Pos;

        else
            return Drone2Pos;
    }

    void StartMove()
    {
        StartCoroutine(Move());
        IEnumerator Move()
        {
            float duration = 0f;
            float maxDur = 2;
            Vector3 StartPos = gameObject.transform.position;


            while (maxDur > duration)
            {
                duration += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(StartPos, DronePosType(), duration / maxDur);

                yield return null;
            }

            yield return new WaitForSeconds(1.5f);
            IngMove(StartPos);
        }
    }

    void IngMove(Vector3 Pos)
    {
        StartCoroutine(Move());

        IEnumerator Laser()
        {
            GameObject temp = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(0, 0, -90));
            temp.GetComponent<Bullet>().DestoryDelayTime = 3;
            yield return new WaitForSeconds(0.005f);
            if (IsShotLaser)
                StartCoroutine(Laser());
        }

        IEnumerator Move()
        {
            float duration = 0f;
            float firstdur = 0f;
            float lastdur = 0f;
            float maxDur = 2;
            float dir = 1;
            int count = 0;
            Vector3 StartPos = gameObject.transform.position;
            Vector3 TargetPos1 = StartPos + new Vector3(3, 0);
            Vector3 TargetPos2 = StartPos + new Vector3(-3, 0);

            IsShotLaser = true;
            StartCoroutine(Laser());

            while (1 > firstdur)
            {
                firstdur += Time.deltaTime;

                gameObject.transform.position = Vector3.Lerp(StartPos, TargetPos1, firstdur / 1);

                yield return null;
            }

            while (count < 5)
            {
                duration += dir * Time.deltaTime;

                print(count);

                if (duration > maxDur || duration < 0)
                {
                    dir *= -1;
                    count++;
                }

                gameObject.transform.position = Vector3.Lerp(TargetPos1, TargetPos2, duration / maxDur);

                yield return null;
            }
            while (1 > lastdur)
            {
                lastdur += Time.deltaTime;

                gameObject.transform.position = Vector3.Lerp(TargetPos2, StartPos, lastdur / 1);

                yield return null;
            }

            IsShotLaser = false;
            yield return new WaitForSeconds(1.5f);
            LastMove(Pos);
        }
    }

    void LastMove(Vector3 Pos)
    {
        StartCoroutine(Move());

        IEnumerator Move()
        {
            float duration = 0f;
            float maxDur = 2;

            while (maxDur > duration)
            {
                duration += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(DronePosType(), Pos, duration / maxDur);

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
