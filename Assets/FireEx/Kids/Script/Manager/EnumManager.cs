using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
    public enum BilliardType
    {
        threeBall,
        fourBall,
        poketBall,
        non,
    }

    public enum GameType
    {
        tutorialMode,
        normalMode,
        challengeMode,
        non,
    }


    public enum BallType
    {
        white,
        yellow,
        red,
        color,
        black,
        line,
        non,
    }


}
