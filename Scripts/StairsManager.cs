using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsManager : MonoBehaviour
{


    GameObject PlayerObj;

    public GameObject stairsPrefab;

    [Range(2, 15)]
    public int numberOfStairs = 15;

    public float stairsWidth = 3f;
    public float stairsHeight = 0.5f;

    public float distanceX = 1.5f;
    public float distanceY = 2.5f;

    List<Transform> childTransformList;


    void Awake()
    {
        PlayerObj = GameObject.Find("Player Parent/Player");
    }


    void Start()
    {
        MakeStairs();
        StartCoroutine(SetStairsPosition());
    }


    void MakeStairs()
    {
        childTransformList = new List<Transform>();

        for (int i = 0; i < numberOfStairs; i++)
        {
            GameObject stairsObj = Instantiate(stairsPrefab, Vector2.zero, Quaternion.identity);
            stairsObj.transform.SetParent(transform);
            stairsObj.transform.position = new Vector2(-99999, -99999);
            childTransformList.Add(stairsObj.transform);
        }

    }


    IEnumerator SetStairsPosition()
    {
        childTransformList[0].position = new Vector3(0, -PlayerObj.transform.localScale.y / 2f - stairsHeight / 2f);
        childTransformList[0].localScale = new Vector2(stairsWidth, stairsHeight);

        for (int i = 1; i < transform.childCount; i++)
        {
            childTransformList[i].localScale = new Vector2(stairsWidth, stairsHeight);
            childTransformList[i].position = childTransformList[i - 1].transform.position + new Vector3(distanceX - Random.Range(0, 2) * distanceX * 2, -distanceY);
            yield return new WaitForSecondsRealtime(0.03f);
        }

        PlayerObj.GetComponent<Player>().SetToReady();
        SetFirstDirection();
        GameObject.Find("ItemManager").GetComponent<ItemManager>().MakeNewItem();

        yield break;
    }


    public void StairsReposition()
    {
        Transform firstTransform = childTransformList[0];
        firstTransform.position = childTransformList[transform.childCount - 1].transform.position + new Vector3(distanceX - Random.Range(0, 2) * distanceX * 2, -distanceY);

        childTransformList.RemoveAt(0);
        childTransformList.Add(firstTransform);
    }


    void SetFirstDirection()
    {
        if (childTransformList[0].position.x > childTransformList[1].position.x)
        {
             PlayerObj.GetComponent<Player>().direction = -1;
        }
        else
        {
            PlayerObj.GetComponent<Player>().direction = 1;
        }
    }


    public Vector3 GetNextStairsForItem()
    {
        int scoreValue = GameObject.Find("GameManager").GetComponent<GameManager>().GetCurrentScore() / 15;
        int randomInt = Random.Range(2, 6 + scoreValue);
        return childTransformList[randomInt].position;
    }




}
