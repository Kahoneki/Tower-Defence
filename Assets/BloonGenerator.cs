using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonGenerator : MonoBehaviour
{

    public RoundManager roundManager;
    List<RoundManager.Round> rounds;
    int roundNum;
    int maxRoundNum;
    bool nextRound;
    float roundDelay;

    bool allBloonsSpawned;
    bool bloonsOnScreen; //stores whether there are bloons in the scene (true at the start of rounds and when all bloons have been destroyed)

    public List<GameObject> bloonTypes;
    public Transform parentBloon;

    void Start() {

        allBloonsSpawned = false;
        bloonsOnScreen = false;
        roundNum = 0;
        maxRoundNum = 1;
        rounds = roundManager.rounds;
        nextRound = false;
        roundDelay = 10f;

        StartRound();
    }


    void Update() {
        //Check if round is over
        if (allBloonsSpawned && !bloonsOnScreen && roundNum != maxRoundNum) {
            roundNum += 1;
            nextRound = true;
        }
        bloonsOnScreen = parentBloon.childCount != 0;
    }


    void StartRound() {
        List<int> bloons = rounds[roundNum].bloons;
        List<float> delays = rounds[roundNum].delays;

        StartCoroutine(SpawnBalloons(bloons, delays));
    }


    IEnumerator SpawnBalloons(List<int> bloons, List<float> delays) {
        bloonsOnScreen = true;
        for (int i=0; i<bloons.Count; i++) {
            Transform currentBloon = Instantiate(bloonTypes[bloons[i]], transform.position+Vector3.left*4, transform.rotation).transform;
            currentBloon.parent = parentBloon;
            yield return new WaitForSeconds(delays[i]);
        }
        allBloonsSpawned = true;
    }
}
