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
	public interface ITask
	{
		bool _Active { get; }

		bool _Completed { get; }
		void Complete(bool includeSubTasks = false);

		bool TryToCompleteDependingOnSubTasks();
	}
}

namespace PixLi
{
#if UNITY_EDITOR
	[CustomEditor(typeof(ITask))]
	[CanEditMultipleObjects]
	public class ITaskEditor : Editor
	{
#pragma warning disable 0219, 414
		private ITask _sITask;
#pragma warning restore 0219, 414

		private void OnEnable()
		{
			this._sITask = this.target as ITask;
		}

		public override void OnInspectorGUI()
		{
			this.DrawDefaultInspector();
		}
	}
#endif
}