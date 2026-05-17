using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnim;
    public float waitTime = 1f;
    public Vector2 newPlayerPosition;
    public int faceDirection = 1;
    private Player_movement playerMovement;


    private Transform player;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            playerMovement = collision.GetComponent<Player_movement>();
            fadeAnim.Play("FadeToBlack");
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(waitTime/2);
        player.position = newPlayerPosition;
        playerMovement.FaceDirection(faceDirection);
        yield return new WaitForSeconds(waitTime/2);
        SceneManager.LoadScene(sceneToLoad);
    }
}
