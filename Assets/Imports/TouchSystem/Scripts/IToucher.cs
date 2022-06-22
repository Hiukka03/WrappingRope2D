using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_HiuKKa.InputSystem 
{
    public interface IToucher
    {
        bool IsHold { set;  get; }
        void Touched();
        void TouchDrag(Vector3 posUpdate);
        void TouchUp();
    }
}

