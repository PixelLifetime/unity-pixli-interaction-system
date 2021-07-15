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
	/// A type of condition that requires to be referenced in different scenes or persist state between the scenes. Or any similar cases where modularity is required.
	/// </summary>
	[System.Serializable]
	[CreateAssetMenu(fileName = "s Condition", menuName = "Condition/Condition", order = 1)]
	public abstract class ScriptableObjectCondition : ScriptableObject, ICondition
	{
		public abstract bool _Satisfied { get; }

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(ScriptableObjectCondition))]
		[CanEditMultipleObjects]
		public class ScriptableObjectConditionEditor : Editor
		{
#pragma warning disable 0219, 414
			private ScriptableObjectCondition _sScriptableObjectCondition;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sScriptableObjectCondition = this.target as ScriptableObjectCondition;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}