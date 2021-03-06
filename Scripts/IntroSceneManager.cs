﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    int loadManager;

    void Start()
    {
        loadManager = 0;
        StartCoroutine(GoToMainScene());
    }


    IEnumerator GoToMainScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("loadManager", loadManager);
        yield break;
    }
}
