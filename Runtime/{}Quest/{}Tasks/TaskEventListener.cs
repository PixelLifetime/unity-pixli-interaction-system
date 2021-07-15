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
	//TODO: make this a ScO - because then you would be able to add tasks to listeners during runtime via that ScO - maybe if that makes sense later
	[System.Serializable]
	public struct TaskTaskCompletionerPair
	{
		[SerializeField] private Task _task;
		public Task _Task => this._task;

		[SerializeField] private GameObject _taskCompletioner;
		public GameObject _TaskCompletioner => this._taskCompletioner;
	}

	public class TaskEventListener : MonoBehaviourSingleton<TaskEventListener>
	{
		[SerializeField] private TaskTaskCompletionerPair[] _taskTaskCompletionerPair;

		private Dictionary<int, List<Task>> _taskHash_tasksRelation;

		public void PassDataToTask<TTaskCompletionData>(int tasksCompletionHashCode, TTaskCompletionData taskCompletionData)
			where TTaskCompletionData : TaskCompletionData
		{
			if (this._taskHash_tasksRelation.TryGetValue(tasksCompletionHashCode, out List<Task> tasks))
			{
				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].TryToComplete(taskCompletionData);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();

			this._taskHash_tasksRelation = new Dictionary<int, List<Task>>(this._taskTaskCompletionerPair.Length);

			for (int i = 0; i < this._taskTaskCompletionerPair.Length; i++)
			{
				if (this._taskHash_tasksRelation.TryGetValue(this._taskTaskCompletionerPair[i]._TaskCompletioner.GetComponent<ITaskCompletioner>().GetTasksCompletionHashCode(),
					out List<Task> tasks))
				{
					tasks.Add(this._taskTaskCompletionerPair[i]._Task);
				}
				else
				{
					tasks = new List<Task>();
					tasks.Add(this._taskTaskCompletionerPair[i]._Task);

					this._taskHash_tasksRelation.Add(this._taskTaskCompletionerPair[i]._TaskCompletioner.GetComponent<ITaskCompletioner>().GetTasksCompletionHashCode(), tasks);
				}
			}
		}

#if UNITY_EDITOR
		//protected override void OnDrawGizmos()
		//{
		//}

		[CustomEditor(typeof(TaskEventListener))]
		[CanEditMultipleObjects]
		public class TaskEventListenerEditor : Editor
		{
#pragma warning disable 0219, 414
			private TaskEventListener _sTaskEventListener;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				this._sTaskEventListener = this.target as TaskEventListener;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}