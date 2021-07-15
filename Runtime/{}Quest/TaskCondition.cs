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
	[System.Serializable]
	public class TaskCondition : ICondition
	{
		[SerializeField] private Task _task;

		[SerializeField] private bool _shouldBeActive = true;
		[SerializeField] private bool _shouldBeCompleted = true;

		public bool _Satisfied { get { return !(this._task._Active ^ this._shouldBeActive) && !(this._task._Completed ^ this._shouldBeCompleted); } }

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(TaskCondition))]
		[CanEditMultipleObjects]
		public class TaskConditionEditor : Editor
		{
#pragma warning disable 0219, 414
			private TaskCondition _sTaskCondition;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				//this._sTaskCondition = this.target as TaskCondition;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}