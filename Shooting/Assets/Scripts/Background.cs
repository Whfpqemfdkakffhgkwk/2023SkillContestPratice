using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    void Update()
    {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime);
        if (gameObject.transform.localPosition.y >= 1600)
        {
            gameObject.transform.localPosition = new Vector3(0, transform.position.y - 1600, 0);
        }
    }
}
