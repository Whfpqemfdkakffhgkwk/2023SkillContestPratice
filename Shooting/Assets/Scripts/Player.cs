using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Image Hpbar, FadeImg;

    [SerializeField] private GameObject Boss, bullet, Tapwindow, EndWindow;

    private int hp = 250;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;

            Hpbar.fillAmount = (float)hp / 250;
            Hpbar.color = new Color(1, (float)Hp / 250, (float)Hp / 250, 1);
            if (hp <= 0)
            {
                EndWindow.transform.GetComponentInParent<TabUI>().PlayerDead = true;
                EndWindow.SetActive(true);
            }

        }
    }

    [SerializeField] private Transform LimitFocusTR;
    private Vector3 LimitFocusPos => LimitFocusTR.position;

    private bool isBossFight = true;
    public bool IsBossFight
    {
        get { return isBossFight; }
        set
        {
            isBossFight = value;

            if (isBossFight)
            {
                StartCoroutine(MoveLimit());
            }
        }
    }

    [SerializeField] float speed;

    private bool isLimitOver = false;
    public bool IsLimitOver
    {
        get { return isLimitOver; }
        set
        {
            isLimitOver = value;
        }
    }

    private bool isLimitOvering = false;

    void Start()
    {
        Hpbar.gameObject.SetActive(true);
        StartCoroutine(MoveLimit());
        SectorFormDelayShot(5, 30, 10, 5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            Tapwindow.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            Tapwindow.SetActive(false);

        Move();
        Hpbar.gameObject.transform.position = transform.position + new Vector3(0, 0.3f);
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 pos = new Vector3(x, y, 0);
        gameObject.transform.Translate(pos * Time.deltaTime * speed);

    }

    IEnumerator MoveLimit()
    {
        if (!isBossFight) yield break;

        isLimitOver = transform.position.y > LimitFocusTR.position.y + 3.2f || transform.position.y < LimitFocusTR.position.y - 2.5f ||
                        transform.position.x > LimitFocusTR.position.x + 5.3f || transform.position.x < LimitFocusTR.position.x - 5.3f;

        if (!isLimitOvering && isLimitOver)
        {
            isLimitOvering = true;
            StartCoroutine(Fade(0, 0.4f));
        }
        else if (!isLimitOver && isLimitOvering)
        {
            StartCoroutine(FadeOut(0.4f));
        }
        if (isLimitOver)
            Hp -= 3;
        else
        {
            isLimitOvering = false;
        }


        yield return new WaitForSeconds(0.15f);
        StartCoroutine(MoveLimit());
    }

    IEnumerator Fade(float start, float end)
    {
        float cur = 0;
        float per = 0;
        while (per < 1)
        {
            cur += Time.deltaTime;
            per = cur / 0.4f;

            Color color = FadeImg.color;
            color.a = Mathf.Lerp(start, end, per);
            FadeImg.color = color;

            yield return null;
        }
    }
    IEnumerator FadeOut(float start)
    {
        float cur = 0;
        float per = 0;
        while (per < 1)
        {
            cur += Time.deltaTime;
            per = cur / 0.4f;

            Color color = FadeImg.color;
            color.a = Mathf.Lerp(start, 0, per);
            FadeImg.color = color;

            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bulletCount">ÇÑ¹ø¿¡ ¸î¹ßÀ» ½ò °ÍÀÎ°¡?</param>
    /// <param name="central">ÇÑ¹ø ½ò¶§ °¢µµ ÆÛÁü</param>
    /// <param name="rotate"></param>
    /// <param name="rotateCount"></param>
    /// <param name="entireCount">ÃÑ ½î´Â È½¼ö</param>
    void SectorFormDelayShot(int bulletCount, int central, int rotate, int rotateCount, int entireCount)
    {
        StartCoroutine(shotRoutine());
        IEnumerator shotRoutine()
        {
            float z = -rotate;
            float amount = (rotate * 2f) / rotateCount;

            for (int i = 0; i < entireCount; i++)
            {
                Vector2 nor = ((Boss.transform.position + new Vector3(0.88f, 0)) - transform.position).normalized;
                float tarZ = Mathf.Atan2(nor.y, nor.x) * Mathf.Rad2Deg;

                z += amount;
                if (z == rotate || z == -rotate)
                {
                    amount *= -1;
                }

                shotBullet(central, z + tarZ);

                yield return new WaitForSeconds(0.1f);
            }

            StartCoroutine(shotRoutine());
        }

        void shotBullet(float central, float startRot)
        {
            float amount = central / (bulletCount - 1);
            float z = central / -2f;

            for (int i = 0; i < bulletCount; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, z + startRot);
                Instantiate(bullet, transform.position, rot);
                z += amount;
            }
        }

    }
        IEnumerator HitMotion()
        {
            for (int i = 0; i < 7; i++)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(0.08f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.08f);
            }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Enemy"))
        {
            StartCoroutine(HitMotion());
        }
    }
}
