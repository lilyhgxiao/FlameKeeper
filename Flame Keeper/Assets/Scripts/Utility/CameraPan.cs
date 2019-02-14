using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraHolder;
    //public List<GameObject> panList;

    // Start is called before the first frame update
    void Start()
    {
        //panList = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PanCameraFromPlayerToo(GameObject gameobject, float holdSpeed)
    {
        PanFromToo(player, gameobject, holdSpeed);
    }

    public IEnumerator PanFromToo(GameObject source, GameObject destination, float holdSpeed)
    {
        Vector3 sourcePosition = source.transform.position;
        Vector3 destinationPosition = destination.transform.position;

        // Disable player controls some how

        cameraHolder.transform.position = Vector3.Lerp(sourcePosition, destinationPosition, Time.deltaTime);
        yield return new WaitForSeconds(holdSpeed);
        cameraHolder.transform.position = Vector3.Lerp(destinationPosition, sourcePosition, Time.deltaTime);
    }

    public IEnumerator PanFromToo(GameObject source, List<GameObject> destinations, float holdSpeed)
    {
        Vector3 sourcePosition = source.transform.position;
        Vector3 currentPosition = sourcePosition;
        Vector3 destinationPosition;

        // Disable player controls some how


        foreach (GameObject destination in destinations)
        {
            destinationPosition = destination.transform.position;
            cameraHolder.transform.position = Vector3.Lerp(currentPosition, destinationPosition, Time.deltaTime);
            yield return new WaitForSeconds(holdSpeed);
            // tbc...

        }


    }
}
