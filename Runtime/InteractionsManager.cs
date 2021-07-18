using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixLi
{
	public class InteractionsManager : MonoBehaviourSingleton<InteractionsManager>
	{
		[SerializeField] private UnityEvent _onAnyInteraction;
		public UnityEvent _OnAnyInteraction => this._onAnyInteraction;

		private List<IInteractable> _interactables = new List<IInteractable>();

		public void Register(IInteractable interactable)
		{
			interactable._OnInteract.AddListener(this._onAnyInteraction.Invoke);

			this._interactables.Add(interactable);
		}
	}
}