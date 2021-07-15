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
	public static class TaskExtensions
	{
		public static void PassDataToTask(this ITaskCompletioner taskCompletioner, TaskCompletionData taskCompletionData)
		{
			TaskEventListener._Instance.PassDataToTask(taskCompletioner.GetTasksCompletionHashCode(), taskCompletionData);
		}
	}
}