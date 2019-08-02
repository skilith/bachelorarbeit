using UnityEngine;
using UnityEngine.SceneManagement;

public class BuzzerController : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Main Room");
        }
    }
}
