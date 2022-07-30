using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixLi
{
	public class SelectionController<TEntity> : MonoBehaviour
	{
		public enum ObjectReferenceType
		{
			MonoBehaviour,
			ScriptableObject
		}
		
		[SerializeField] private ObjectReferenceType _selectorType;
		public ObjectReferenceType _SelectorType => this._selectorType;
		
		[SerializeField] private SelectorMonoBehaviour<TEntity> _selectorMonoBehaviour;
		public SelectorMonoBehaviour<TEntity> _SelectorMonoBehaviour => this._selectorMonoBehaviour;
		
		[SerializeField] private SelectorScriptableObject<TEntity> _selectorScriptableObject;
		public SelectorScriptableObject<TEntity> _SelectorScriptableObject => this._selectorScriptableObject;

		[SerializeField] private Selection<TEntity> _selection;
		public Selection<TEntity> _Selection => this._selection;

		public ISelector<TEntity> GetActiveSelector()
		{
			switch (this._selectorType)
			{
				case ObjectReferenceType.MonoBehaviour:
					return this._selectorMonoBehaviour;

				case ObjectReferenceType.ScriptableObject:
					return this._selectorScriptableObject;
			}

			return null;
		}

		public bool AttemptSelection()
		{
			TEntity entity = this.GetActiveSelector().Select();

			if (entity == null)
				return false;

			this._selection.Select(entity);

			return true;
		}

		public void AttemptSelectionVoid() => this.AttemptSelection();

#if UNITY_EDITOR
		public class SelectionControllerEditor<T> : Editor
		{
			private SelectionController<T> _tSelectionController;
			
			private SerializedProperty _serializedProperty_selectorType;

			private SerializedProperty _serializedProperty_selectorMonoBehaviour;
			private SerializedProperty _serializedProperty_selectorScriptableObject;

			public virtual void OnEnable()
			{
				this._tSelectionController = (SelectionController<T>)this.target;

				this._serializedProperty_selectorType = this.serializedObject.FindProperty(propertyPath: "_selectorType");

				this._serializedProperty_selectorMonoBehaviour = this.serializedObject.FindProperty(propertyPath: "_selectorMonoBehaviour");
				this._serializedProperty_selectorScriptableObject = this.serializedObject.FindProperty(propertyPath: "_selectorScriptableObject");
			}

			public override void OnInspectorGUI()
			{
				this.serializedObject.Update();
				
				MultiSupportEditor.DrawDefaultInspector(this.serializedObject, true, "_selectorType", "_selectorMonoBehaviour", "_selectorScriptableObject");

				EditorGUILayout.Space();

				EditorGUILayout.PropertyField(this._serializedProperty_selectorType, true);

				switch (this._tSelectionController._selectorType)
				{
					case global::PixLi.SelectionController<T>.ObjectReferenceType.MonoBehaviour:

						EditorGUILayout.PropertyField(this._serializedProperty_selectorMonoBehaviour, true);

						break;
					case global::PixLi.SelectionController<T>.ObjectReferenceType.ScriptableObject:

						EditorGUILayout.PropertyField(this._serializedProperty_selectorScriptableObject, true);

						break;
				}

				this.serializedObject.ApplyModifiedProperties();
			}
		}
#endif
	}
}