using DG.Tweening;
using Gamee_HiuKKa.InputSystem;
using UnityEngine;

namespace Gamee_Hiukka.WrappingRope2D
{
    public class HandDrag : TouchPoint
    {
        public Rope2D rope2D;
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoldTarget = false;
        [SerializeField] Collision2DComponent collision2d;
        [SerializeField] GameObject boxMove;

        Rigidbody2D rig;
        bool isMoving = false;
        bool isLock = false;
        IHander hander = null;
        Vector3 posUpdate = Vector3.zero;

        public bool IsLock { set => isLock = value; get => isLock; }
        public void Awake()
        {
            rig = this.GetComponentInChildren<Rigidbody2D>();
            collision2d.actionTriggerEnter = OnTriggerEnterEvent;
            boxMove.gameObject.SetActive(true);
        }
        private void OnEnable()
        {
            rope2D.CreateLine("line");
            rope2D.EnableRender();
            rope2D.SetCameraCanvasLine();
        }

        public override void Touched()
        {
            base.Touched();
        }
        public override void TouchDrag(Vector3 posUpdate)
        {
            if (rig == null) return;
            if (isMoving) return;
            this.posUpdate = posUpdate;
            var direction = posUpdate - this.transform.position;
            if(direction.magnitude > .1f) 
            {
/*                Quaternion toQuaterion = Quaternion.LookRotation(this.transform.forward, direction);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, toQuaterion, 150f * Time.deltaTime);*/

                /*                UnityEngine.Vector3 val_3 = this.transform.position;
                                UnityEngine.Vector3 val_4 = UnityEngine.Camera.main.WorldToScreenPoint(this.transform.position);
                                UnityEngine.Vector3 val_6 = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3() { x = posUpdate.x, y = val_4.y,z = 0f });
                                if ((UnityEngine.Vector2.Distance(a: val_4, val_6)) <= 0.2f)
                                {
                                    return;
                                }

                                float val_11 = val_6.x - val_4.x;
                                UnityEngine.Quaternion val_13 = this.transform.rotation;
                                UnityEngine.Vector3 val_14 = UnityEngine.Vector3.forward;
                                UnityEngine.Quaternion val_15 = UnityEngine.Quaternion.AngleAxis((val_6.y - val_4.y) * 57.29578f, axis: val_14);
                                float t = Time.deltaTime * 2f;
                                UnityEngine.Quaternion val_17 = UnityEngine.Quaternion.Lerp(val_13, val_15, t);
                                this.transform.rotation = val_17;*/
            }
            rig.MovePosition((Vector2)posUpdate);
        }

        public override void TouchUp ()
        {
            base.TouchUp();
        }

        public void MoveBack() 
        {
            if (isLock) return;
            rope2D.UpdateLineCollor();
            boxMove.gameObject.SetActive(false);
            this.IsHold = false;
            this.transform.DOKill();
            isMoving = true;
            this.transform.DOPath(rope2D.WayPoints.ToArray(), rope2D.Length / speed).SetEase(Ease.InSine).OnComplete(() => 
            {
                isMoving = false;
                boxMove.gameObject.SetActive(true);
                rope2D.DefautLineCollor();
                if (!isHoldTarget) 
                {
                    hander.OntriggerHand(null);
                    hander = null;
                }
            });
        }

        void OnTriggerEnterEvent(GameObject go) 
        {
            var hander = go.GetComponentInParent<IHander>();
            if (hander == null) return;
            this.hander = hander;
            hander.OntriggerHand(this);
            MoveBack();
        }
    }
}

