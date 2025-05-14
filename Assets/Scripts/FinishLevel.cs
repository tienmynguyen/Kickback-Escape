using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] bool goNextLevel;
    [SerializeField] string levelName;
    private void OnTriggerEnter2D(Collider2D collition)
    {
        if (collition.CompareTag("Player"))
        {
            if (goNextLevel)
            {
                ScenesController.instance.NextLevel();
            }
            else
            {
                ScenesController.instance.LoadScene(levelName);
            }
        }
    }
}
