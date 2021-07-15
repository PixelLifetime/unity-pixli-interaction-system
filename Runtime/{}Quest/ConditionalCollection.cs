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
	public class ConditionalCollection : ICondition
	{
		[SerializeField] private TaskCondition[] _taskConditions;
		[SerializeField] private MonoBehaviourCondition[] _monoBehaviourConditions;
		[SerializeField] private ScriptableObjectCondition[] _scriptableObjectConditions;

		/// <summary>
		/// Returns true if all of the conditions in collection are satisfied.
		/// </summary>
		public bool _Satisfied
		{
			get
			{
				for (int i = 0; i < this._taskConditions.Length; i++)
				{
					if (this._taskConditions[i]._Satisfied)
						continue;

					return false;
				}

				for (int i = 0; i < this._monoBehaviourConditions.Length; i++)
				{
					if (this._monoBehaviourConditions[i]._Satisfied)
						continue;

					return false;
				}

				for (int i = 0; i < this._scriptableObjectConditions.Length; i++)
				{
					if (this._scriptableObjectConditions[i]._Satisfied)
						continue;

					return false;
				}

				return true;
			}
		}

#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(ConditionalCollection))]
		[CanEditMultipleObjects]
		public class ConditionalCollectionEditor : Editor
		{
#pragma warning disable 0219, 414
			private ConditionalCollection _sConditionalCollection;
#pragma warning restore 0219, 414

			private void OnEnable()
			{
				//this._sConditionalCollection = this.target as ConditionalCollection;
			}

			public override void OnInspectorGUI()
			{
				this.DrawDefaultInspector();
			}
		}
#endif
	}
}