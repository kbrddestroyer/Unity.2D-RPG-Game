using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private Animator sceneTransition;

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(nextLevelName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LoadScene();
        }
    }
}
