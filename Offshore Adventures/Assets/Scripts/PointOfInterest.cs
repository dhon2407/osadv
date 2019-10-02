using UnityEngine;
using System.Collections;

public class PointOfInterest : MonoBehaviour
{
    private PointOfInterestSelection selection;
    private int triggeringLayers;

    private void Awake()
    {
        triggeringLayers |= (1 << 8); //Player Layer
        selection = GetComponentInChildren<PointOfInterestSelection>();
    }

    public void ShownOnCamera()
    {
        selection.Show();
    }

    public void NotOnCamera()
    {
        selection.Hide();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnTriggeringLayers(other))
            ScreenPopUpController.Show("Package delivered! Well done.");
    }

    private bool OnTriggeringLayers(Collider2D other)
    {
        return (triggeringLayers == (triggeringLayers | (1 << other.gameObject.layer)));
    }
}
