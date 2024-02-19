using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    static bool tutorialDone = false;
    public GameObject tutorialUI;

    private void Awake()
    {
        if (tutorialDone) return;
        tutorialUI.SetActive(true);
        tutorialDone = true;
    }
}
