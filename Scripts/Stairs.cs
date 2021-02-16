using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{


    public IEnumerator StairsEffect()
    {
        float xValue = transform.localScale.x / 20f;
        float yValue = transform.localScale.y / 20f;

        Vector2 originalScale = transform.localScale;

        transform.localScale = new Vector2(transform.localScale.x + transform.localScale.x / 4f, transform.localScale.y + transform.localScale.y / 4f);

        while (transform.localScale.x > originalScale.x)
        {
            transform.localScale = new Vector2(transform.localScale.x - xValue, transform.localScale.y - yValue);
            yield return 0;
        }




        yield break;

    }


}
