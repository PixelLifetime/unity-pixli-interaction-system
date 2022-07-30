using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixLi
{
	public class TaskCoordinator : MonoBehaviourSingleton<TaskCoordinator>
	{
		[SerializeField] private Task _task;
		public Task _Task => this._task;

		private Dictionary<IdTag, Task> _activeTasks;

		public void BridgeCompletionAttempt(IdTag idTag, TaskCompletionData taskCompletionData)
		{
			if (this._activeTasks.TryGetValue(key: idTag, out Task task))
			{
				task.TryToComplete(taskCompletionData: taskCompletionData);
			}
		}

		private void Register(Task task)
		{
			this._activeTasks[task._IdTag] = task;

			for (int a = 0; a < task._Subtasks.Length; a++)
			{
				this.Register(task: task._Subtasks[a]);
			}
		}

		public void Initialize(Task task)
		{
			this._activeTasks = new Dictionary<IdTag, Task>();

			this.Register(task: task);
		}

		protected override void Awake()
		{
			base.Awake();

			this.Initialize(task: this._task);
		}
	}
}