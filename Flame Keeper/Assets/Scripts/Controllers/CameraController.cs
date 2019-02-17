using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> panList;

    private Vector3 offset;
    private bool inPanMode;

    private void Start()
    {
        offset = transform.position - player.transform.position;
        inPanMode = false;
    }

    void Update()
    {
        if (!inPanMode)
        {
            // Keep the Camera holder where the player is.
            this.transform.position = player.transform.position;
        }
    }

    public IEnumerator PanCamera(GameObject destination, float holdTime = 1)
    {
        Debug.Log("At Start of Method Call");
        inPanMode = true;

        Vector3 sourcePosition = player.transform.position + offset;
        Vector3 destinationPosition = destination.transform.position + offset;

        // Disable player controls some how

        transform.position = Vector3.Lerp(sourcePosition, destinationPosition, Time.deltaTime);
        yield return new WaitForSeconds(holdTime);
        transform.position = Vector3.Lerp(destinationPosition, sourcePosition, Time.deltaTime);

        inPanMode = false;
        Debug.Log("At end of Method Call");
    }

    public IEnumerator PanCamera2(GameObject destination, float holdTime = 1)
    {
        Debug.Log("At Start of Method Call");
        inPanMode = true;

        Vector3 sourcePosition = player.transform.position + offset;
        Vector3 destinationPosition = destination.transform.position + offset;
        
        while (Vector3.Distance(sourcePosition, destinationPosition) > 0)
        {
            transform.position = Vector3.Lerp(sourcePosition, destinationPosition, Time.deltaTime * 1.0f);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        inPanMode = false;
    }
}
