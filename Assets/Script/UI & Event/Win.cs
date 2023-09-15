using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Win : MonoBehaviour
{
   [SerializeField] private GameObject winUI;
   [SerializeField] private TextMeshProUGUI totalDiamondCount;
   [SerializeField] private ItemCollector itemCollector;

   public void IsWon()
   {
      Time.timeScale = 0f;
      winUI.SetActive(true);
      totalDiamondCount.text = itemCollector.gem + "/12 Collected";
   }

   public void nextLevel()
   {
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      int nextSceneIndex = currentSceneIndex + 1;

      if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
      {
         SceneManager.LoadScene(nextSceneIndex);
         Time.timeScale = 1f;
      }
      else
      {
         Debug.LogWarning("No next scene found. Make sure your scene order is correct.");
      }
   }

   public void ExitStage()
   {
      SceneManager.LoadScene("StartMenu");
   }
}
