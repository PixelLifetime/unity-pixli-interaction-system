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
#if UNITY_EDITOR
	[CanEditMultipleObjects]
	public class TaskEditor<TTask> : Editor
		where TTask : Task
	{
#pragma warning disable 0219, 414
		protected TTask sTask;
#pragma warning restore 0219, 414

		private void OnEnable()
		{
			this.sTask = this.target as TTask;
		}

		public override void OnInspectorGUI()
		{
			this.DrawDefaultInspector();

			if (GUILayout.Button("Reset State"))
			{
				this.sTask.ResetState();
			}

			if (GUILayout.Button("Initialize"))
			{
				this.sTask.Initialize();
			}

			if (GUILayout.Button("Reset/Init"))
			{
				this.sTask.ResetState();
				this.sTask.Initialize();
			}
		}
	}
#endif
}