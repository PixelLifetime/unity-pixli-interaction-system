using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixLi
{
	public class Interactable : MonoBehaviour, IInteractable
	{
		[SerializeField] private Transform _interactionPoint;
		public Transform _InteractionPoint { get { return this._interactionPoint; } }

		[Header("Interactable Events")]

		[Tooltip("Called every time there is any interaction with this Interactable.")]
		[SerializeField] private UnityEvent _onInteract;
		public UnityEvent _OnInteract { get { return this._onInteract; } }

		[Tooltip("Called when no reaction was present when there is interaction with this Interactable.")]
		[SerializeField] private UnityEvent _onInteractionFail;
		public UnityEvent _OnInteractionFail { get { return this._onInteractionFail; } }

		[Header("Conditional Reaction Events")]

		[Tooltip("Reactions are called based on satisfaction of conditions.")]
		[SerializeField] private ConditionalEvent[] _conditionalEvents;

		public void Interact()
		{
			this._onInteract.Invoke();

			for (int i = 0; i < this._conditionalEvents.Length; i++)
			{
				if (this._conditionalEvents[i].Invoke())
					return;
			}

			this._onInteractionFail.Invoke();
		}

#if UNITY_EDITOR
		//protected virtual void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(Interactable))]
		[CanEditMultipleObjects]
		public class InteractableEditor : Editor
		{
			private void OnEnable()
			{
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();

#pragma warning disable 0219
				Interactable sInteractable = this.target as Interactable;
#pragma warning restore 0219
			}
		}
#endif
	}

	[System.Serializable]
	public struct ConditionalEvent
	{
		[SerializeField] private ConditionalCollection _conditionalCollection;
		public ConditionalCollection _ConditionalCollection => this._conditionalCollection;

		[SerializeField] private Reaction[] _reactions;
		public Reaction[] _Reactions => this._reactions;

		public bool Invoke()
		{
			if (this._conditionalCollection._Satisfied)
			{
				for (int i = 0; i < this._reactions.Length; i++)
				{
					this._reactions[i].React();
				}

				return true;
			}

			return false;
		}
	}
}