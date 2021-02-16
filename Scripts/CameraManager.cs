using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    GameObject PlayerObj;

    Vector3 velocity = Vector3.zero;

    public int yOffset = -3;
    public float followTime = 0.3F;

    int ZoomInStart = 25;
    int ZoomInEnd = 13;
    float smoothTime = 0.5F;
    float velocityForZoomInWhenStart = 0.0f;

    float smoothTimeDead = 0.1F;
    float velocityForZoomInWhenDead = 0.0f;



    void Awake()
    {
        PlayerObj = GameObject.Find("Player Parent/Player");
    }


    void Start()
    {
        InitOrthographicSize();
        StartCoroutine(ZoomInToPlayerWhenStartGame());
    }


    void Update()
    {
        FollowThePlayer();
    }


    void FollowThePlayer()
    {
        Vector3 targetPosition = PlayerObj.transform.TransformPoint(new Vector3(0, yOffset, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followTime);
    }


    void InitOrthographicSize()
    {
        Camera.main.orthographicSize = ZoomInStart;
    }


    public IEnumerator ZoomInToPlayerWhenStartGame()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        while (Camera.main.orthographicSize > ZoomInEnd + 0.1f)
        {
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, ZoomInEnd, ref velocityForZoomInWhenStart, smoothTime);
            yield return 0;
        }
        yield break;
    }


    public IEnumerator ZoomInToPlayerWhenGameOver()
    {
        while (Camera.main.orthographicSize > 10)
        {
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, 10, ref velocityForZoomInWhenDead, smoothTimeDead);
            yield return 0;
        }
        yield break;
    }


}
