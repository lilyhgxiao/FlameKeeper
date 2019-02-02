using UnityEngine;

public class ConnectLinks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<CharacterJoint>().connectedBody = this.transform.parent.GetComponent<Rigidbody>();
    }
}
