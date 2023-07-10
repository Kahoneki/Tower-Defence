using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloonGenerator : MonoBehaviour
{

    public RoundManager roundManager;
    List<RoundManager.Round> rounds; //Stores bloons and delay between bloons for each round
    int roundNum;
    int numOfRounds;
    bool nextRound;
    bool startNextRound;
    float startDelay; //Time before first wave
    float roundDelay; //Time between waves

    bool allBloonsSpawned;
    bool bloonsOnScreen; //stores whether there are bloons in the scene (true at the start of rounds and when all bloons have been destroyed)

    public List<GameObject> bloonTypes;
    public Transform parentBloon;

    void Start() {

        allBloonsSpawned = false;
        bloonsOnScreen = false;
        roundNum = 0;
        numOfRounds = 2;
        rounds = roundManager.rounds;
        nextRound = false;
        startNextRound = false;
        startDelay = 2f;
        roundDelay = 2f;

        Invoke("StartGame",startDelay);
    }


    void StartGame() {
        StartCoroutine(StartRound());
    }


    void Update() {
        //Check if round is over
        if (allBloonsSpawned && !bloonsOnScreen && roundNum != numOfRounds-1) {
            roundNum += 1;
            nextRound = true;
        }
        bloonsOnScreen = parentBloon.childCount != 0;
    }


    IEnumerator StartRound() {
        List<int> bloons = rounds[roundNum].bloons;
        List<float> delays = rounds[roundNum].delays;
        StartCoroutine(SpawnBalloons(bloons, delays));    

        for (int i = 0; i < numOfRounds;) {
            if (startNextRound) {
                startNextRound = false;
                bloons = rounds[roundNum].bloons;
                delays = rounds[roundNum].delays;
                StartCoroutine(SpawnBalloons(bloons, delays));
            }

            if (nextRound) {
                startNextRound = true;
                nextRound = false;
                i++;
                yield return new WaitForSeconds(roundDelay);
            }
            else
                yield return null;
        }
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
