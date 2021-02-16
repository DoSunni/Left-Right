using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemPrefab;

    public StairsManager stairsManager;


    public void MakeNewItem()
    {
        Vector2 pos = stairsManager.GetNextStairsForItem() + new Vector3(0, stairsManager.stairsHeight/2+1f, 0);
        GameObject itemObj =  Instantiate(itemPrefab, pos, Quaternion.identity);
        itemObj.transform.SetParent(transform);
    }

}
