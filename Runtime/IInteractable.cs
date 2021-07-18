using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Object = UnityEngine.Object;

namespace PixLi
{
	public interface IInteractable
	{
		void Interact();

		UnityEvent _OnInteract { get; }

#if UNITY_EDITOR
#endif
	}

	public interface IInteractable<TComponent>
		where TComponent : Component
	{
		void Interact(TComponent component);
		void Interact(Object @object);

#if UNITY_EDITOR
#endif
	}
}

namespace PixLi
{
#if UNITY_EDITOR
	[CustomEditor(typeof(IInteractable))]
	[CanEditMultipleObjects]
	public class IInteractableEditor : Editor
	{
#pragma warning disable 0219, 414
		private IInteractable _sIInteractable;
#pragma warning restore 0219, 414

		private void OnEnable()
		{
			this._sIInteractable = this.target as IInteractable;
		}

		public override void OnInspectorGUI()
		{
			this.DrawDefaultInspector();
		}
	}
#endif
}