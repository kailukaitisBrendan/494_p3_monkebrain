﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    // Represents state of level at its end
    public enum LevelEndState {Won, Lost, Quit}

    private LevelEndState state = LevelEndState.Quit;
    private Scene thisScene;

    private GameObject player, goldenPackage;

    private Vector3 playerLocation {
        get {
            return player ? player.transform.position : Vector3.zero;
        }
        set { }
    }
    private Vector3 goldenPackageLocation {
        get {
            return goldenPackage ? goldenPackage.transform.position : Vector3.zero;
        }
        set { }
    }

    private float secondsElapsed = 0;


    // EventBus Subscriptions
    private Subscription<LevelClearEvent> levelClearSubscription;
    private Subscription<LevelFailEvent> levelFailSubscription;


    void Awake () {
        thisScene = SceneManager.GetActiveScene();
        AnalyticsEvent.LevelStart(thisScene.name, thisScene.buildIndex);
    }

    void Start () {
        GameObject[] results = GameObject.FindGameObjectsWithTag("Player");
        if (results.Length > 0) {
            player = results[0];
        }

        results = GameObject.FindGameObjectsWithTag("GoldenPackage");
        if (results.Length > 0) {
            goldenPackage = results[0];
        }

        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(LevelComplete);
        levelFailSubscription = EventBus.Subscribe<LevelFailEvent>(LevelFail);
    }

    private void LevelComplete(LevelClearEvent _event){
        state = LevelEndState.Won;
    }

    private void LevelFail(LevelFailEvent _event){
        state = LevelEndState.Lost;
    }

    void Update(){
        secondsElapsed += Time.deltaTime;
    }

    void OnDestroy(){
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("player_location", playerLocation);
        customParams.Add("golden_package_location", goldenPackageLocation);
        AnalyticsResult res;

        switch(state){
        case LevelEndState.Won:
            res = AnalyticsEvent.LevelComplete(thisScene.name,
                                             thisScene.buildIndex, 
                                             customParams);
            break;
        case LevelEndState.Lost:
            res = AnalyticsEvent.LevelFail(thisScene.name,
                                         thisScene.buildIndex, 
                                         customParams);
            break;
        case LevelEndState.Quit:
        default:
            res = AnalyticsEvent.LevelQuit(thisScene.name,
                                         thisScene.buildIndex, 
                                         customParams);
            break;
        }

    }


}
