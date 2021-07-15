using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixLi
{
	public class Collider2DExecutor : MonoBehaviour
	{
		[SerializeField] private Collider2D _collider;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider2D>>[] _onOverlapColliderData;

		public void OverlapCollider()
		{
			for (int i = 0; i < this._onOverlapColliderData.Length; i++)
			{
				ContactFilter2D contactFilter2D = new ContactFilter2D();

				contactFilter2D.useLayerMask = true;
				contactFilter2D.SetLayerMask(this._onOverlapColliderData[i]._LayerMask);

				Collider2D[] collider2Ds = new Collider2D[1];

				this._collider.OverlapCollider(contactFilter2D, collider2Ds);

				this._onOverlapColliderData[i]._Event.Invoke(collider2Ds[i]);
			}
		}

#if UNITY_EDITOR
		private void Reset()
		{
			this._collider = this.GetComponent<Collider2D>();
		}

		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(Collider2DExecutor))]
		[CanEditMultipleObjects]
		public class Collider2DExecutorEditor : Editor
		{
#pragma warning disable 0219, 414
			private Collider2DExecutor _sCollider2DExecutor;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sCollider2DExecutor = this.target as Collider2DExecutor;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}