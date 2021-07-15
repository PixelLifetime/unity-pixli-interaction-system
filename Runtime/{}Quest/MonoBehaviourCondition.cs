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
	/// Conditions are used when certain calculation need to be done.
	/// You override the _Satisfied and return the result of those calculations.
	/// For things that don't require any specific calculations and require data based conditioning - use Tasks.
	/// 
	/// A type of condition that can reference scene objects. And does specific per scene calculations.
	/// </summary>
	public abstract class MonoBehaviourCondition : MonoBehaviour, ICondition
	{
		public abstract bool _Satisfied { get; }

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(MonoBehaviourCondition))]
		[CanEditMultipleObjects]
		public class MonoBehaviourConditionEditor : Editor
		{
#pragma warning disable 0219, 414
			private MonoBehaviourCondition _sMonoBehaviourCondition;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sMonoBehaviourCondition = this.target as MonoBehaviourCondition;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}