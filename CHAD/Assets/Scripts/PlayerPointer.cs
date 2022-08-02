using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerPointer : MonoBehaviour {
    public GameObject target;
    public float rotationOffset;
    private RectTransform pointerTransform;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Destroy(gameObject);
    }

    private void Start()
    {
        pointerTransform = gameObject.GetComponent<RectTransform>();
    }
    private void LateUpdate() {
        
        if (target != null) {
            Vector3 toPosition = target.transform.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = GetAngleFromVectorFloat(dir) + rotationOffset;
            //pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

            float borderSize = 40f;

            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(toPosition);
            bool isOffscreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if(isOffscreen) {
                gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,1);
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize) + 100;
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize - 40);
                cappedTargetScreenPosition.z = -10;

                Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointerTransform.position = pointerWorldPosition;
                pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0);
            
            }
            else{
                gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,0);
                // Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(targetPositionScreenPoint);
                // pointerTransform.position = pointerWorldPosition;
                // pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);

            }
        }

    }

    

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
