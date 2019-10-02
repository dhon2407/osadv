using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public ShipController ship;

    private void Start()
    {
        ship.AddOnDestroyAction(ShipDestroyed);
    }

    public void ShipDestroyed()
    {
        SceneManager.LoadScene(0);
    }
}
