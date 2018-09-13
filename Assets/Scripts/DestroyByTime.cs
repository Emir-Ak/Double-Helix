using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    [Header("Delay after which the object is destroyed")]
    public float destroyDelay;

    void Start () {
        Destroy(gameObject, destroyDelay);
	}
	
}
