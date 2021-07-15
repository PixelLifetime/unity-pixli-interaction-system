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
	public class TestTaskEventListener : MonoBehaviour, ITaskCompletioner
	{
		[SerializeField] private float someFloat;
		[SerializeField] private string someString;

		public int GetTasksCompletionHashCode()
		{
			return this.gameObject.name.GetHashCode();
		}

		private void OnDestroy()
		{
			this.PassDataToTask(new TaskIntCompletionData(1));
		}

		private void Awake()
		{
			Object.Destroy(this.gameObject, 5f);
		}

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(TestTaskEventListener))]
		[CanEditMultipleObjects]
		public class TestTaskEventListenerEditor : Editor
		{
#pragma warning disable 0219, 414
			private TestTaskEventListener _sTestTaskEventListener;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sTestTaskEventListener = this.target as TestTaskEventListener;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}