using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;

namespace PixLi
{
	[System.Serializable]
	//[CreateAssetMenu(fileName = "[Task]", menuName = "Task/Task", order = 1)]
	public class Task : ScriptableObject, ITask
	{
		[SerializeField] private IdTag _idTag;
		public IdTag _IdTag => this._idTag;

		[SerializeField] protected Task[] subTasks = new Task[0];
		public Task[] _Subtasks => this.subTasks;

		public Task ParentTask { get; set; }

		[SerializeField] protected bool active = true;
		public bool _Active => this.active;

		public void Activate()
		{
			this.active = true;
		}

		[SerializeField] protected bool completed;
		public bool _Completed => this.completed;

		[Expose]
		[SerializeField] private UnityEventReference _onCompleted;
		public UnityEventReference _OnCompleted => this._onCompleted;

		public void Complete(bool includeSubTasks = false)
		{
			if (includeSubTasks)
			{
				for (int i = 0; i < this.subTasks.Length; i++)
				{
					this.subTasks[i].Complete(true);
				}
			}

			this.completed = true;

			this._onCompleted._UnityEvent.Invoke();

			this.ParentTask?.TryToCompleteDependingOnSubTasks();
		}

		public virtual bool TryToCompleteDependingOnSubTasks()
		{
			if (this.completed)
				return true;
			else if (!this.active)
				return false;

			if (this.subTasks.Length > 0)
			{
				for (int i = 0; i < this.subTasks.Length; i++)
				{
					if (!this.subTasks[i].completed)
						return false;
				}
			}

			this.Complete();

			return true;
		}

		public virtual bool TryToComplete<TTaskCompletionData>(TTaskCompletionData taskCompletionData)
			where TTaskCompletionData : TaskCompletionData
		{
			return this.TryToCompleteDependingOnSubTasks();
		}

		public virtual void ResetState()
		{
			this.completed = false;

			for (int i = 0; i < this.subTasks.Length; i++)
			{
				this.subTasks[i].ResetState();
			}
		}

		public virtual void Initialize()
		{
			for (int i = 0; i < this.subTasks.Length; i++)
			{
				this.subTasks[i].Initialize();

				this.subTasks[i].ParentTask = this;
			}
		}

		protected virtual void Awake()
		{
			this.Initialize();
		}


#if UNITY_EDITOR
		public static void CreateAsset<TTask>()
			where TTask : Task
		{
			string selectionPath = AssetDatabase.GetAssetPath(assetObject: Selection.activeObject);

			TTask task = ScriptableObjectUtility.CreateAsset<TTask>(
				path: Directory.Exists(path: selectionPath) ? selectionPath : Path.GetDirectoryName(path: selectionPath),
				name: "[Task]"
			);

			task._onCompleted = ScriptableObjectUtility.CreateAssetAsChild<UnityEventReference>(
				scriptableObject: task,
				name: "[Unity Event Reference] On Completed"
			);

			EditorUtility.SetDirty(target: task);
		}

		[MenuItem(itemName: "Assets/Create/Task/[Task]")]
		public static void CreateAsset() => Task.CreateAsset<Task>();

		//private void OnValidate()
		//{
		//	if (this._OnCompleted == null)
		//	{
		//		this._onCompleted = ScriptableObjectUtility.CreateAssetAsChild<UnityEventReference>(
		//			scriptableObject: this,
		//			name: "[Unity Event Reference] On Completed"
		//		);
		//	}
		//}

		public static Task S_GlobalTaskEO;

		[UnityEngine.HideInInspector]
		[SerializeField] private bool _globalEO;

		[Header("Editor Only")]

		[SerializeField] private bool _eoResetOnEnteringPlayMode = true;
		public bool _EOResetOnEnteringPlayMode => this._eoResetOnEnteringPlayMode;

		public void OnEnable()
		{
			if (this._eoResetOnEnteringPlayMode)
			{
				this.ResetState();
			}
		}
#endif
	}

	[System.Serializable]
	public abstract class Task<TDefaultTaskData, TTaskCompletionDataInternal> : Task
		where TDefaultTaskData : TaskData
		where TTaskCompletionDataInternal : TaskCompletionData
	{
		[Expose]
		[SerializeField] protected TDefaultTaskData defaultTaskData;

		protected abstract bool TryToCompleteInternal(TTaskCompletionDataInternal taskCompletionData);

		public sealed override bool TryToComplete<TTaskCompletionData>(TTaskCompletionData taskCompletionData)
		{
			if (this.TryToCompleteInternal(taskCompletionData as TTaskCompletionDataInternal))
			{
				this.Complete();

				return true;
			}

			return false;
		}

		protected abstract void ResetStateInternal();

		public sealed override void ResetState()
		{
			base.ResetState();

			this.ResetStateInternal();
		}
	}
}

namespace PixLi
{
#if UNITY_EDITOR
	[CustomEditor(typeof(Task))]
	[CanEditMultipleObjects]
	public class TaskEditor : TaskEditor<Task>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (Task.S_GlobalTaskEO == null || (this.sTask == Task.S_GlobalTaskEO))
			{
				EditorGUILayout.Space();

				SerializedProperty globalProp = this.serializedObject.FindProperty("_globalEO");

				EditorGUI.BeginChangeCheck();
				{
					EditorGUILayout.PropertyField(globalProp);
				}
				if (EditorGUI.EndChangeCheck())
					Task.S_GlobalTaskEO = globalProp.boolValue ? this.sTask : null;

				if (globalProp.boolValue)
				{
					if (GUILayout.Button("Find And Assign All Tasks As Sub Tasks"))
					{
						SerializedProperty subTasksProp = this.serializedObject.FindProperty("subTasks");
						subTasksProp.ClearObjectArray();

						string[] assetFiles = Directory.GetFiles(Application.dataPath, "*.asset", SearchOption.AllDirectories);

						foreach (string assetFilePath in assetFiles)
						{
							Task task = AssetDatabase.LoadAssetAtPath<Task>(Path.Combine(PathUtility.ASSETS_PATH_NAME, assetFilePath.Substring(Application.dataPath.Length + 1)));

							if (task != null && task != this.sTask)
								subTasksProp.AddToObjectArray(task);
						}
					}
				}

				this.serializedObject.ApplyModifiedProperties();
			}
		}
	}
#endif
}