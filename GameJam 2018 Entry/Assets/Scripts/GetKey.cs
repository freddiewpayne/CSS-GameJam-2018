using UnityEngine.SceneManagement;
using UnityEngine;

public class GetKey : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("SampleScene");
            Destroy(gameObject);
        }
    }
}
