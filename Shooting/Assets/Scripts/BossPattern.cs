using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    private int hp = 10000;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;

            if (hp <= 0)
            {
                Death();
            }

        }
    }

    [SerializeField] private GameObject bullet, rotateBullet, PlayerObj, Drone, cam, NormalEnemy, DeathEff, TapWindow, EndWindow;

    [SerializeField] private Transform LeftTR, RightTR, UpTR, DownTR;
    [SerializeField] private Transform SectorForm1TR, SectorForm2TR;
    [SerializeField] private Transform CircleTR1, CircleTR2;
    [SerializeField] private Transform Drone1TR, Drone2TR;
    [SerializeField] private Transform BossAppearTR;

    [SerializeField] private Transform[] NormalAssemblyPoints;

    [SerializeField] private Transform[] NormalSpawnerTR;
    [SerializeField] private GameObject[] NormalSpawner;

    Vector3 FirstPos;

    Vector3 Drone1Pos => Drone1TR.position;
    Vector3 Drone2Pos => Drone2TR.position;
    Vector3 SectorForm1Pos => SectorForm1TR.position;
    Vector3 SectorForm2Pos => SectorForm2TR.position;
    Vector3 LeftPos => LeftTR.position;
    Vector3 RightPos => RightTR.position;
    Vector3 UpPos => UpTR.position;
    Vector3 DownPos => DownTR.position;
    Vector3 CirclePos1 => CircleTR1.position;
    Vector3 CirclePos2 => CircleTR2.position;
    Vector3 BossAppearPos => BossAppearTR.position;

    private void Start()
    {
        FirstPos = transform.position;
        //NormalEnemyPattern(0);
        //NormalEnemyPattern(1);
        //NormalEnemyPattern(2);
        //NormalEnemyPattern(3);
        StartCoroutine(AppearShow());
        StartCoroutine(CameraShake());
    }

    IEnumerator Shoot()
    {
        StartCoroutine(NormalEnemyPattern());
        yield return new WaitForSeconds(25);
        LinearTargetDelayShot();
        yield return new WaitForSeconds(4.8f);
        SectorFormShot(45);
        yield return new WaitForSeconds(2.4f);
        CircleShot(8);
        yield return new WaitForSeconds(12f);
        WiperShot();
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(LinearAndCircle());
        yield return new WaitForSeconds(8f);
        StartCoroutine(SectorAndCircle());
        yield return new WaitForSeconds(8f);
        StartCoroutine(Shoot());
    }

    IEnumerator LinearAndCircle()
    {
        CircleShot(3);
        yield return new WaitForSeconds(2f);
        LinearTargetDelayShot();
    }
    IEnumerator SectorAndCircle()
    {
        CircleShot(3);
        yield return new WaitForSeconds(2f);
        SectorFormShot(45);
    }

    void WiperShot()
    {
        StartCoroutine(NextShotDelay());
        IEnumerator NextShotDelay()
        {
            for (int z = 0; z < 5; z++)
            {
                for (int i = 0; i < 14; i++)
                {
                    GameObject temp = Instantiate(rotateBullet);
                    temp.GetComponent<RotateBullet>().InitBullet(CirclePos1, 2 + i, 20, -5);

                    GameObject temp2 = Instantiate(rotateBullet);
                    temp2.GetComponent<RotateBullet>().InitBullet(CirclePos2, 2 + i, -20, 185);
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1);
        }
    }


    void LinearTargetDelayShot()
    {
        List<Bullet> bullets = new List<Bullet>();

        StartCoroutine(shotBullet(11, true, false)); //윗쪽
        StartCoroutine(shotBullet(5, false, false)); //왼쪽
        StartCoroutine(shotBullet(11, true, true));
        StartCoroutine(shotBullet(5, false, true));
        StartCoroutine(EveryShot());

        IEnumerator shotBullet(int count, bool isWidth, bool ShotType)
        {

            Vector3 TypeToPos()
            {
                if (isWidth == true && ShotType == false)
                {
                    return UpPos;
                }
                else if (isWidth == false && ShotType == false)
                {
                    return LeftPos;
                }
                else if (isWidth == true && ShotType == true)
                {
                    return DownPos;
                }
                else
                {
                    return RightPos;
                }
            }


            for (int i = 0; i < count; i++)
            {
                Vector3 pos;

                if (isWidth)
                {
                    pos = TypeToPos() + new Vector3(-count / 2 + i, 0);
                }
                else
                    pos = TypeToPos() + new Vector3(0, -count / 2 + i);

                var temp = Instantiate(bullet, pos, Quaternion.identity).GetComponent<Bullet>();
                temp.isRun = false;
                bullets.Add(temp);
                yield return new WaitForSeconds(0.1f);
            }

            yield break;
        }
        IEnumerator EveryShot()
        {
            yield return new WaitForSeconds(2);

            foreach (var item in bullets)
            {
                if (item == null) continue;
                var dir = PlayerObj.transform.position - item.transform.position;
                float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, 0, z);

                item.transform.rotation = rot;
                item.isRun = true;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="central">부채꼴 각도</param>
    void SectorFormShot(float central)
    {
        StartCoroutine(RelayShot());

        IEnumerator RelayShot()
        {
            for (int q = 0; q < 3; q++)
            {
                Vector2 nor1 = (PlayerObj.transform.position - SectorForm1Pos).normalized;
                Vector2 nor2 = (PlayerObj.transform.position - SectorForm2Pos).normalized;

                float tarZ1 = Mathf.Atan2(nor1.y, nor1.x) * Mathf.Rad2Deg;
                float tarZ2 = Mathf.Atan2(nor2.y, nor2.x) * Mathf.Rad2Deg;

                float amount = central / (5 - 1);
                float z1 = central / -2f + tarZ1;
                float z2 = central / -2f + tarZ2;

                for (int i = 0; i < 5; i++)
                {
                    Quaternion rot1 = Quaternion.Euler(0, 0, z1);
                    Quaternion rot2 = Quaternion.Euler(0, 0, z2);

                    Instantiate(bullet, SectorForm1Pos, rot1);
                    Instantiate(bullet, SectorForm2Pos, rot2);
                    z1 += amount;
                    z2 += amount;
                }
                yield return new WaitForSeconds(0.8f);
            }
        }
    }

    void CircleShot(int count)
    {
        StartCoroutine(DelayShot());
        IEnumerator DelayShot()
        {
            for (int z = 0; z < count; z++)
            {
                for (int i = 0; i < 360; i += Random.Range(7, 15))
                {
                    Vector3 pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 1.5f, Mathf.Sin(i * Mathf.Deg2Rad) * 1.5f);
                    GameObject bull = Instantiate(bullet, pos, Quaternion.Euler(0, 0, i));
                    bull.GetComponent<Bullet>().moveSpeed = 0.7f;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }

    void Death()
    {
        TapWindow.SetActive(false);
        PlayerObj.GetComponent<Player>().enabled = false;
        PlayerObj.SetActive(false);
        PlayerObj.SetActive(true);
        DeathEff.SetActive(true);
        StartCoroutine(Dead());
        IEnumerator Dead()
        {
            float duration = 0f;

            while (5 > duration)
            {
                duration += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(BossAppearPos, FirstPos, duration / 5);

                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            EndWindow.SetActive(true);
        }
    }

    IEnumerator DroneAttack()
    {
        yield return new WaitForSeconds(Random.Range(35, 50));
        GameObject temp1 = Instantiate(Drone, Drone1Pos, Quaternion.Euler(0, 0, 0));
        GameObject temp2 = Instantiate(Drone, Drone2Pos, Quaternion.Euler(0, 0, 0));
        temp1.GetComponent<Drone>().DroneTpye = 1;
        temp2.GetComponent<Drone>().DroneTpye = 2;
        StartCoroutine(DroneAttack());
    }
    
    IEnumerator NormalEnemyPattern()
    {
        Vector3[] EnemysPos = new Vector3[4]; 
        for (int i = 0; i < 4; i++)
        {
            EnemysPos[i] = NormalSpawner[i].transform.position;
            StartCoroutine(NormalmobAppearShow(i));
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 4; i++)
        {
            NormalEnemyPattern(i);
        }
        yield return new WaitForSeconds(23);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(NormalmobExitAppearShow(i, EnemysPos[i]));
        }
    }


    IEnumerator NormalmobAppearShow(int num)
    {
            float duration = 0f;
            Vector3 pos = NormalSpawner[num].transform.position;
            while (0.5f > duration)
            {
                duration += Time.deltaTime;
                NormalSpawner[num].transform.position = Vector3.Lerp(pos, NormalSpawnerTR[num].position, duration / 0.5f);

                yield return null;
            }  
    }
    IEnumerator NormalmobExitAppearShow(int num, Vector3 end)
    {
        float duration = 0f;
        Vector3 pos = NormalSpawner[num].transform.position;
        while (0.5f > duration)
        {
            duration += Time.deltaTime;
            NormalSpawner[num].transform.position = Vector3.Lerp(pos, end, duration / 0.5f);

            yield return null;
        }
    }

    IEnumerator AppearShow()
    {
        float duration = 0f;

        Vector3 curPos = gameObject.transform.position;

        while (5 > duration)
        {
            duration += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(curPos, BossAppearPos, duration / 5);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        PlayerObj.GetComponent<Player>().enabled = true;
        StartCoroutine(DroneAttack());
        StartCoroutine(Shoot());
    }
    IEnumerator CameraShake()
    {
        float duration = 0;

        while (5 > duration)
        {
            cam.transform.localPosition = Random.insideUnitSphere * 0.3f + new Vector3(0, 0, -10);

            duration += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = new Vector3(0, 0, -10);
    }

    void NormalEnemyPattern(int index)
    {
        Vector3 RangeMin()
        {
            if (index == 0)
            {
                return new Vector3(-9f, -5f, 0);
            }
            else if (index == 1)
            {
                return new Vector3(-12f, -5f, 0);
            }
            else if (index == 2)
            {
                return new Vector3(-9f, -7f, 0);
            }
            else if (index == 3)
            {
                return new Vector3(-12f, -7f, 0);
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }
        Vector3 RangeMax()
        {
            if (index == 0)
            {
                return new Vector3(12f, 7f, 0);
            }
            else if (index == 1)
            {
                return new Vector3(9f, 7f, 0);
            }
            else if (index == 2)
            {
                return new Vector3(12f, 5f, 0);
            }
            else if (index == 3)
            {
                return new Vector3(9f, 5f, 0);
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
        }

        bool ComparePos(Vector3 pos)
        {
            if (index == 0)
            {
                return pos.x < 9 && pos.y < 5;
            }
            else if (index == 1)
            {
                return pos.x > -9 && pos.y < 5;
            }
            else if (index == 2)
            {
                return pos.x < 9 && pos.y > -5;
            }
            else if (index == 3)
            {
                return pos.x > -9 && pos.y > -5;
            }
            else
            {
                return false;
            }
        }
        StartCoroutine(Spawn());
        IEnumerator Spawn()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 RandomSpawnPos = new Vector3(Random.Range(RangeMin().x, RangeMax().y), Random.Range(RangeMin().y, RangeMax().y));
                if (ComparePos(RandomSpawnPos))
                {
                    i--;
                    continue;
                }

                GameObject Enemy = Instantiate(NormalEnemy, RandomSpawnPos, Quaternion.Euler(0, 0, 0));
                #region 쳐다보기
                Vector3 temp = NormalAssemblyPoints[index].position - RandomSpawnPos;
                var rot = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
                Enemy.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
                #endregion
                #region 움직이기
                float cur = 0f;

                while (Enemy.transform.position != NormalAssemblyPoints[index].position)
                {
                    Enemy.transform.position = Vector3.Lerp(RandomSpawnPos, NormalAssemblyPoints[index].position, cur / Vector2.Distance(RandomSpawnPos, NormalAssemblyPoints[index].position));
                    cur += Time.deltaTime * 15;
                    yield return null;
                }
                Destroy(Enemy);
                #endregion

            }
        }
    }
}

