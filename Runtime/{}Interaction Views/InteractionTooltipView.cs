//using System.Collections;
//using System.Collections.Generic;

//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

//namespace PixLi
//{
//    public class InteractionTooltipUserInterfaceView : UserInterfaceView<InteractionTooltipUserInterfaceView.InteractionTooltipUserInterfaceViewDisplayData, InteractionTooltipUserInterfaceView.InteractionTooltipUserInterfaceViewOutput>
//    {
//        [System.Serializable]
//        public class InteractionTooltipUserInterfaceViewDisplayData : UserInterfaceViewDisplayData
//        {
//        }

//        [System.Serializable]
//        public class InteractionTooltipUserInterfaceViewOutput : UserInterfaceViewOutput
//        {
//        }

//        public override void Display(InteractionTooltipUserInterfaceViewDisplayData displayData)
//        {
//            throw new System.NotImplementedException();
//        }

//        [SerializeField] private Transform _target;

//        [SerializeField] private CanvasScaler _canvasScaler;
//        [SerializeField] private Vector2 _offset = new Vector3(0f, 100f);

//        private void FixedUpdate()
//        {
//            Vector2 targetViewportPoint = Camera.main.WorldToViewportPoint(this._target.position);

//            this.rectTransform.anchoredPosition =
//                new Vector2(
//                    this._canvasScaler.referenceResolution.x * targetViewportPoint.x,
//                    this._canvasScaler.referenceResolution.y * targetViewportPoint.y
//                ) + this._offset;
//        }

//#if UNITY_EDITOR
//        //protected override void OnDrawGizmos()
//        //{
//        //}

//        [CustomEditor(typeof(InteractionTooltipUserInterfaceView))]
//        [CanEditMultipleObjects]
//        public class InteractionTooltipUserInterfaceViewEditor : UserInterfaceViewEditor<InteractionTooltipUserInterfaceView>
//        {
//#pragma warning disable 0219, 414
//            private InteractionTooltipUserInterfaceView _sInteractionTooltipUserInterfaceView;
//#pragma warning restore 0219, 414

//            protected override void OnEnable()
//            {
//                base.OnEnable();

//                this._sInteractionTooltipUserInterfaceView = this.target as InteractionTooltipUserInterfaceView;
//            }
//        }
//#endif
//    }
//}