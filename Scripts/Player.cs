using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class Player : MonoBehaviour
{

    bool isEDITOR;
    [HideInInspector]
    public int direction;

    float sideMoveDistance;
    float downMoveDistance;

    int triggerCount = 0;

    bool isDead = false;

    public GameObject fx_Dead;

    StairsManager stairsManager;




    public GameObject FollowObj_01;
    public GameObject FollowObj_02;
    public GameObject FollowObj_03;
    public GameObject FollowObj_04;

    Vector3 velocity1 = Vector3.zero;
    Vector3 velocity2 = Vector3.zero;
    Vector3 velocity3 = Vector3.zero;
    Vector3 velocity4 = Vector3.zero;
    float smoothTime = 0.04F;

    public GameObject redMaskParent;
    public TextMeshPro timeText;

    float timeRemaining = 9.9f;


    public GameObject itemMagerObj;

    public GameObject fxItem_1;
    public GameObject fxItem_2;
    public GameObject fxItem_3;
    public GameObject fxItem_4;
    public GameObject fxItemText;

    bool isStairsReady = false;
    bool isFirstTouch = false;



    AudioSource source;
    [Space]
    public AudioClip moveClip;
    public AudioClip itemClip;
    public AudioClip deadClip;

    public GameObject instructionPanel;


    void Awake()
    {
#if UNITY_EDITOR
        isEDITOR = true;
#else
		isEDITOR = false;
#endif


        GetVariables();
    }

    void GetVariables()
    {
        source = GetComponent<AudioSource>();
        stairsManager = GameObject.Find("StairsManager").GetComponent<StairsManager>();
        sideMoveDistance = stairsManager.distanceX;
        downMoveDistance = stairsManager.distanceY;
    }


    void Update()
    {
        if (isDead || !isStairsReady) return;

        GetInput();
        FollowPlayer();
        ReduceTime();
        UpdateRedMask();

        DeadCheck();
    }



    void ReduceTime()
    {
        if (!isStairsReady || !isFirstTouch) return;

        timeRemaining -= Time.deltaTime;
        timeText.text = timeRemaining.ToString("F1");
        if (timeRemaining <= 0 && !isDead)
        {
            StartCoroutine(DeadEffect());
        }
    }


    void UpdateRedMask()
    {
        float maskV = Mathf.Clamp(timeRemaining, 0f, 9.9f);
        redMaskParent.transform.localScale = new Vector2(1, 1f - maskV / 9.9f);
    }


    void GetInput()
    {
        if (instructionPanel.activeInHierarchy == false)
        {
            if (isEDITOR == true)
            {
                getMouseClickPosition();
            }
            else
            {
                getTouchPosition();
            }
        }
    }


    void getMouseClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGame();

            if (Input.mousePosition.x < Screen.width / 2)
            {
                Move();
            }
            else
            {
                direction *= -1;
                Move();
            }
        }
    }


    void getTouchPosition()
    {
        foreach (Touch touch in Input.touches)
        {
            StartGame();

            if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2)
            {
                Move();
            }
            else if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
            {
                direction *= -1;
                Move();
            }
        }
    }


    void StartGame()
    {
        if (isFirstTouch == false)
        {
            isFirstTouch = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().StartGame();
        }
    }


    void Move()
    {
        transform.position = new Vector2(transform.position.x + direction * sideMoveDistance, transform.position.y - downMoveDistance);
        source.PlayOneShot(moveClip, 1);
        GameObject.Find("GameManager").GetComponent<GameManager>().AddScore(1);
        GameObject.Find("StairsManager").GetComponent<StairsManager>().StairsReposition();
    }


    void DeadCheck()
    {
        if (triggerCount == 0)
        {
            StartCoroutine(DeadEffect());
        }
    }


    IEnumerator DeadEffect()
    {
        isDead = true;
        redMaskParent.SetActive(false);

        CallCameraZoomIn();
        DeactiveFollowObj();


        // flicker effect ( Red And Black ) 
        int flickerCount = 4;
        while (flickerCount > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSecondsRealtime(0.08f);
            GetComponent<SpriteRenderer>().color = Color.black;
            yield return new WaitForSecondsRealtime(0.08f);
            flickerCount--;
        }

        source.PlayOneShot(deadClip, 1);
        HidePlayerObj();
        CallDeadEffect();

        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(GameObject.Find("GameManager").GetComponent<GameManager>().GameoverCoroutine());

        yield break;
    }


    void CallCameraZoomIn()
    {
        Camera.main.gameObject.GetComponent<CameraManager>().yOffset = 0;
        StartCoroutine(Camera.main.gameObject.GetComponent<CameraManager>().ZoomInToPlayerWhenGameOver());
    }


    void DeactiveFollowObj()
    {
        FollowObj_01.SetActive(false);
        FollowObj_02.SetActive(false);
        FollowObj_03.SetActive(false);
        FollowObj_04.SetActive(false);
    }


    void HidePlayerObj()
    {
        timeText.text = "";
        GetComponent<SpriteRenderer>().enabled = false;
    }


    void CallDeadEffect()
    {
        Destroy(Instantiate(fx_Dead, transform.position, Quaternion.identity), 1.0f);
    }


    void FollowPlayer()
    {
        FollowObj_01.transform.position = Vector3.SmoothDamp(FollowObj_01.transform.position, transform.position, ref velocity1, smoothTime);
        FollowObj_02.transform.position = Vector3.SmoothDamp(FollowObj_02.transform.position, FollowObj_01.transform.position, ref velocity2, smoothTime);
        FollowObj_03.transform.position = Vector3.SmoothDamp(FollowObj_03.transform.position, FollowObj_02.transform.position, ref velocity3, smoothTime);
        FollowObj_04.transform.position = Vector3.SmoothDamp(FollowObj_04.transform.position, FollowObj_03.transform.position, ref velocity4, smoothTime);
    }


    void AddTime(int time)
    {
        timeRemaining += time;
    }


    IEnumerator WhiteEffect()
    {
        float colorValue = 1.0f;

        while (colorValue >= 0.0f)
        {
            Color color = new Color(colorValue, colorValue, colorValue);
            GetComponent<SpriteRenderer>().color = color;

            colorValue -= 0.05f;
            yield return 0;
        }
        yield break;
    }


    void ItemEffect(int time)
    {

        if (time == 1)
        {
            Destroy(Instantiate(fxItem_1, transform.position, Quaternion.identity), 1.0f);
        }
        else if (time == 2)
        {
            Destroy(Instantiate(fxItem_2, transform.position, Quaternion.identity), 1.0f);
        }
        else if (time == 3)
        {
            Destroy(Instantiate(fxItem_3, transform.position, Quaternion.identity), 1.0f);
        }
        else
        {
            Destroy(Instantiate(fxItem_4, transform.position, Quaternion.identity), 1.0f);
        }
    }


    void DisplayText(int time)
    {
        GameObject fxText = Instantiate(fxItemText, transform.position, Quaternion.identity);

        string tempText = "Секунда";
        if (time != 1) tempText = "Секунды";

        fxText.transform.Find("timeText").GetComponent<TextMeshPro>().text = "+" + time + " " + tempText;
        Destroy(fxText, 1.0f);
    }


    void MakeNewItem()
    {
        itemMagerObj.GetComponent<ItemManager>().MakeNewItem();
    }


    public void SetToReady()
    {
        isStairsReady = true;
    }


    void DestroyItem(Collider2D other)
    {
        Destroy(other.gameObject.transform.parent.gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            source.PlayOneShot(itemClip, 1);
            StartCoroutine(WhiteEffect());
            AddTime(other.gameObject.GetComponent<Item>().time);
            ItemEffect(other.gameObject.GetComponent<Item>().time);
            DisplayText(other.gameObject.GetComponent<Item>().time);
            DestroyItem(other);
            MakeNewItem();
            return;
        }

        if (other.gameObject.tag == "Stairs")
        {
            StartCoroutine(other.gameObject.GetComponent<Stairs>().StairsEffect());
            triggerCount++;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Stairs")
        {
            triggerCount--;
        }
    }




}
