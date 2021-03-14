using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Procedure());
    }

    private IEnumerator Procedure()
    {
        while (enabled)
        {
            switch (GameManager.State)
            {
                case GameState.Menu:
                    if (Input.GetKeyDown(KeyCode.Space))
                        GameManager.GameStart();
                    break;

                case GameState.Ready:
                    break;

                case GameState.Run:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        GameManager.Restart();
                    break;

                case GameState.End:
                    if (Input.GetKeyDown(KeyCode.Space))
                        GameManager.Restart();
                    break;
            }

            yield return null;
        }
    }
}
