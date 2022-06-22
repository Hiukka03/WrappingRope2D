using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_HiuKKa.InputSystem
{
    public class TouchSystem : MonoBehaviour
    {
        [SerializeField] Camera camera;
        [SerializeField] LayerMask layerMark;
        [SerializeField] float holdTime = 0f;
        IToucher toucher = null;
        Vector3 oldMousePos = Vector3.zero;
        Vector3 currentMousePos = Vector3.zero;
        bool isTouched = false;
        bool isHold = false;
        float timeHoldCurrent = 0;
        void Start()
        {
            if (camera == null) camera = Camera.main;
            timeHoldCurrent = holdTime;
        }

        void Update()
        {
            Vector2 ray = camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 5, layerMark.value);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider != null)
                {
                    var touch = hit.collider.GetComponentInParent<IToucher>();
                    if ( touch != null) 
                    {
                        isTouched = true;
                        oldMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                        toucher = touch;
                        toucher.Touched();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isTouched = false;
                isHold = false;
                timeHoldCurrent = holdTime;
                if(toucher != null) 
                {
                    toucher.TouchUp();
                    toucher = null;
                }
            }

            if (!isTouched) return;

            timeHoldCurrent -= Time.deltaTime;
            if (timeHoldCurrent < 0)
            {
                if (!isHold)
                {
                    isHold = true;
                    if (toucher != null) toucher.IsHold = true;
                }
            }

            if (isHold) 
            {
                currentMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                if (toucher == null) return;
                if (!toucher.IsHold) return;
                toucher.TouchDrag(currentMousePos);
            }

        }
    }
}

