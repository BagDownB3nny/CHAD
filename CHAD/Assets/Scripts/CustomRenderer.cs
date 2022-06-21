using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRenderer : MonoBehaviour
{
    [SerializeField]
    private int sortingBase;
    [SerializeField]
    private float offset;
    [SerializeField]
    private bool runOnce;
    [SerializeField]
    private float updateInterval;
    private float timeToNextUpdate;
    private Renderer myRenderer;

    private int sortingOffset;

    private void Start()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
        sortingOffset = myRenderer.sortingOrder;
    }

    private void LateUpdate() {
        timeToNextUpdate -= Time.deltaTime;
        if (timeToNextUpdate < 0) {
            timeToNextUpdate = updateInterval;
            myRenderer.sortingOrder = sortingOffset + (int) (sortingBase - transform.position.y - offset);
            if (runOnce) {
                Destroy(this);
            }
        }
    }
}
