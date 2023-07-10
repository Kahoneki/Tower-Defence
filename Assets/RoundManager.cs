using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public List<Round> rounds = new List<Round>();


    public class Round {

        public List<int> bloons;
        //Note: final delay should always be 0.
        public List<float> delays;

        public Round(List<int> bloons, List<float> delays) {
            this.bloons = bloons;
            this.delays = delays;
        }
    }


    void Awake() {
        //lists are cleared after every round
        List<int> bloons;
        List<float> delays;


        //----Round 1----//
        bloons = new List<int> {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        delays = new List<float> {
            .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f,
            .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, 0f
        };

        rounds.Add(new Round(bloons,delays));

        //----Round 2----//
        bloons = new List<int> {
            1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
        };

        delays = new List<float> {
            .25f, .25f, .25f, .25f, 1f, .25f, .25f, .25f, .25f, 0f
        };

        rounds.Add(new Round(bloons,delays));
    }
}
