using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

namespace PixLi
{
	public class InteractionTrigger2D : MonoBehaviour
	{
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider2D>>[] _onTriggerEnterData;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider2D>>[] _onTriggerStayData;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider2D>>[] _onTriggerExitData;

		private Coroutine _onTriggerStayProcess;

		private IEnumerator OnTriggerStayProcess2D(Collider2D other)
		{
			while (true)
			{
				for (int i = 0; i < this._onTriggerStayData.Length; i++)
				{
					if (this._onTriggerStayData[i]._LayerMask.Contains(other.gameObject))
						this._onTriggerStayData[i]._Event.Invoke(other);
				}

				yield return null;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			for (int i = 0; i < this._onTriggerEnterData.Length; i++)
			{
				if (this._onTriggerEnterData[i]._LayerMask.Contains(other.gameObject))
					this._onTriggerEnterData[i]._Event.Invoke(other);
			}

			this._onTriggerStayProcess = this.StartCoroutine(this.OnTriggerStayProcess2D(other));
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			for (int i = 0; i < this._onTriggerExitData.Length; i++)
			{
				if (this._onTriggerExitData[i]._LayerMask.Contains(other.gameObject))
					this._onTriggerExitData[i]._Event.Invoke(other);
			}

			this.StopCoroutine(this._onTriggerStayProcess);
		}

#if UNITY_EDITOR
		private const float GIZMO_SIZE_BIAS = 0.1f;

		private BoxCollider2D _defaultCollider;

		[ContextMenu("Reset Collision Data")]
		public void CheckForTriggerColliders()
		{
			foreach (Collider2D collider in this.GetComponents<Collider2D>())
			{
				if (collider.isTrigger)
				{
					this._defaultCollider = collider as BoxCollider2D;
					return;
				}
			}

			this._defaultCollider = this.gameObject.AddComponent<BoxCollider2D>();
			this._defaultCollider.isTrigger = true;
		}

		[Header("Unity Editor Only")]
		[SerializeField] private Vector2 _gizmoSize = Vector2.one;

		private bool _triedToFindDefaultCollider;

		protected virtual void OnDrawGizmos()
		{
			if (!this._triedToFindDefaultCollider)
			{
				this.CheckForTriggerColliders();

				this._triedToFindDefaultCollider = true;
			}

			Color color = Color.cyan;
			color.a = 0.2f;
			Gizmos.color = color;

			if (this._defaultCollider != null)
			{
				Gizmos.DrawCube(this.transform.position + Vector3.Scale(this._defaultCollider.offset, this.transform.localScale), Vector2.Scale(this._defaultCollider.size, this.transform.localScale) + Vector2.one * GIZMO_SIZE_BIAS);
			}
			else
			{
				Gizmos.DrawCube(this.transform.position, this._gizmoSize + Vector2.one * GIZMO_SIZE_BIAS);
			}
		}

		private void Reset()
		{
			InputActionExecutor inputActionExecutor = this.GetComponent<InputActionExecutor>();
			if (inputActionExecutor != null)
			{
				this._onTriggerEnterData = new LayeredEventTriggerData<UnityEvent<Collider2D>>[]
				{ new LayeredEventTriggerData<UnityEvent<Collider2D>>(new UnityEvent<Collider2D>(), new LayerMask()) };

				UnityEventToolsUtility.AddPersistentListener(this._onTriggerEnterData[0]._Event, inputActionExecutor.Activate);

				this._onTriggerExitData = new LayeredEventTriggerData<UnityEvent<Collider2D>>[]
				{ new LayeredEventTriggerData<UnityEvent<Collider2D>>(new UnityEvent<Collider2D>(), new LayerMask()) };

				UnityEventToolsUtility.AddPersistentListener(this._onTriggerExitData[0]._Event, inputActionExecutor.Deactivate);
			}
		}

		[CustomEditor(typeof(InteractionTrigger2D))]
		[CanEditMultipleObjects]
		public class InteractionTrigger2DEditor : Editor
		{
			private InteractionTrigger2D _sInteractionTrigger2D;

			private void OnEnable()
			{
				this._sInteractionTrigger2D = this.target as InteractionTrigger2D;

				this._sInteractionTrigger2D.CheckForTriggerColliders();
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();

				foreach (Collider2D collider in this._sInteractionTrigger2D.GetComponents<Collider2D>())
				{
					if (collider.isTrigger)
					{
						return;
					}
				}

				EditorGUILayout.HelpBox("There is no trigger collider attached. Events might not trigger", MessageType.Warning); //TODO: Search for colliders recursively
			}
		}
#endif
	}
}