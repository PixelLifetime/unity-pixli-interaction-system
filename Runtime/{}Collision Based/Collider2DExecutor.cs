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
	/// <summary>
	/// Gets each first object colliding with this collider based on layer mask.
	/// Better approach would be to loop each distinct layer mask but I guess by design you would actually use distinct layer masks instead of having events with same layers selected in layer masks.
	/// </summary>
	public class Collider2DExecutor : MonoBehaviour
	{
		[SerializeField] private Collider2D _collider;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider2D>>[] _onOverlapColliderData;

		public void OverlapCollider()
		{
			for (int a = 0; a < this._onOverlapColliderData.Length; a++)
			{
				ContactFilter2D contactFilter2D = new ContactFilter2D
				{
					useLayerMask = true
				};
				contactFilter2D.SetLayerMask(this._onOverlapColliderData[a]._LayerMask);

				Collider2D[] collider2Ds = new Collider2D[1];

				this._collider.OverlapCollider(contactFilter2D, collider2Ds);

				this._onOverlapColliderData[a]._Event.Invoke(collider2Ds[0]);
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