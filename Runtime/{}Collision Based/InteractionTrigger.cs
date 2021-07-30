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
	public class InteractionTrigger : MonoBehaviour
	{
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider>>[] _onTriggerEnterData;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider>>[] _onTriggerStayData;
		[SerializeField] private LayeredEventTriggerData<UnityEvent<Collider>>[] _onTriggerExitData;

		private Coroutine _onTriggerStayProcess;

		private IEnumerator OnTriggerStayProcess(Collider other)
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

		private void OnTriggerEnter(Collider other)
		{
			for (int i = 0; i < this._onTriggerEnterData.Length; i++)
			{
				if (this._onTriggerEnterData[i]._LayerMask.Contains(other.gameObject))
					this._onTriggerEnterData[i]._Event.Invoke(other);
			}

			this._onTriggerStayProcess = this.StartCoroutine(this.OnTriggerStayProcess(other));
		}

		// Isn't called every frame, thus things like input aren't working properly.
		//private void OnTriggerStay(Collider other)
		//{
		//}

		private void OnTriggerExit(Collider other)
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

		private BoxCollider _defaultCollider;

		[ContextMenu("Reset Collision Data")]
		public void CheckForTriggerColliders()
		{
			foreach (Collider collider in this.GetComponents<Collider>())
			{
				if (collider.isTrigger)
				{
					this._defaultCollider = collider as BoxCollider;
					return;
				}
			}

			this._defaultCollider = this.gameObject.AddComponent<BoxCollider>();
			this._defaultCollider.isTrigger = true;
		}

		[Header("Unity Editor Only")]
		[SerializeField] private Vector3 _gizmoSize = Vector3.one;

		private bool _triedToFindDefaultCollider;

		[SerializeField] private bool _drawGizmosOnlyWhenSelected = true;
		public bool _DrawGizmosOnlyWhenSelected => this._drawGizmosOnlyWhenSelected;

		private void DrawGizmos()
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
				GizmosUtility.DrawCombinedCube(this.transform.position + Vector3.Scale(this._defaultCollider.center, this.transform.localScale), this._defaultCollider.size + Vector3.one * GIZMO_SIZE_BIAS, Color.cyan, this.transform, 0.2f);
			}
			else
			{
				GizmosUtility.DrawCombinedCube(this.transform.position, this._gizmoSize + Vector3.one * GIZMO_SIZE_BIAS, Color.cyan, this.transform, 0.2f);
			}
		}

		protected virtual void OnDrawGizmos()
		{
			if (!this._drawGizmosOnlyWhenSelected)
				this.DrawGizmos();
		}

		protected virtual void OnDrawGizmosSelected()
		{
			if (this._drawGizmosOnlyWhenSelected)
				this.DrawGizmos();
		}

		private void Reset()
		{
			InputActionExecutor inputActionExecutor = this.GetComponent<InputActionExecutor>();
			if (inputActionExecutor != null)
			{
				this._onTriggerEnterData = new LayeredEventTriggerData<UnityEvent<Collider>>[]
				{ new LayeredEventTriggerData<UnityEvent<Collider>>(new UnityEvent<Collider>(), new LayerMask()) };

				UnityEventToolsUtility.AddPersistentListener(this._onTriggerEnterData[0]._Event, inputActionExecutor.Activate);

				this._onTriggerExitData = new LayeredEventTriggerData<UnityEvent<Collider>>[]
				{ new LayeredEventTriggerData<UnityEvent<Collider>>(new UnityEvent<Collider>(), new LayerMask()) };

				UnityEventToolsUtility.AddPersistentListener(this._onTriggerExitData[0]._Event, inputActionExecutor.Deactivate);
			}

			this.CheckForTriggerColliders();
		}

		[CustomEditor(typeof(InteractionTrigger))]
		[CanEditMultipleObjects]
		public class InteractionTriggerEditor : Editor
		{
			private void OnEnable()
			{
				InteractionTrigger sInteractionTrigger = target as InteractionTrigger;

				sInteractionTrigger.CheckForTriggerColliders();
			}

			public override void OnInspectorGUI()
			{
				DrawDefaultInspector();

#pragma warning disable 0219
				InteractionTrigger sInteractionTrigger = target as InteractionTrigger;
#pragma warning restore 0219

				foreach (Collider collider in sInteractionTrigger.GetComponents<Collider>())
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