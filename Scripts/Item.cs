using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [HideInInspector]
    public int time;

    public TextMeshPro timeText;


    void Start()
    {
        SetTimeOfItem();
    }

    void SetTimeOfItem()
    {
        int randomInt = Random.Range(0, 10);

        if (randomInt >= 0 && randomInt < 4) time = 1;
        if (randomInt >= 4 && randomInt < 7) time = 2;
        if (randomInt >= 7 && randomInt < 9) time = 3;
        if (randomInt >= 9) time = 4;

        timeText.text = time.ToString();
    }



}
