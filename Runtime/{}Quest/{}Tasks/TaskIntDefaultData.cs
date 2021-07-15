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
	[CreateAssetMenu(fileName = "[Task Int Data][Default]", menuName = "Task/Data/[Task Int Data][Default]", order = 1)]
	public class TaskIntDefaultData : TaskData
	{
		[SerializeField] private int _defaultCountValue;
		public int _DefaultValue { get { return this._defaultCountValue; } }

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(TaskIntDefaultData))]
		[CanEditMultipleObjects]
		public class TaskIntDefaultDataEditor : Editor
		{
#pragma warning disable 0219, 414
			private TaskIntDefaultData _sTaskIntDefaultData;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sTaskIntDefaultData = this.target as TaskIntDefaultData;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}