using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_HiuKKa.InputSystem 
{
    public class TouchPoint : MonoBehaviour, IToucher
    {
        public Action<Vector3> ActionDragChange;
        public Action ActionDragPause;
        public bool IsHold { get; set; }

        public virtual void TouchDrag(Vector3 posUpdate)
        {
            this.transform.position = (Vector2)posUpdate;
            ActionDragChange?.Invoke(posUpdate);
        }

        public virtual void Touched()
        {
        }
        public virtual void TouchUp()
        {
            ActionDragPause?.Invoke();
        }
    }
}

