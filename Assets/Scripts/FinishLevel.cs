using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{

    [SerializeField] bool goNextLevel;
    [SerializeField] string levelName;
    [SerializeField] int nextLevel;
    private void OnTriggerEnter2D(Collider2D collition)
    {
        if (collition.CompareTag("Player"))
        {
            if (goNextLevel)
            {
                SoundManager.Instance.PlaySound2D("teleport");
                SaveManager.Instance.Save(nextLevel);
                ScenesController.instance.NextLevel();
            }
            else
            {
                if (levelName == "end")
                    MusicManager.Instance.PlayMusic("Main Menu");
                ScenesController.instance.LoadScene(levelName);

            }

        }
    }
}
