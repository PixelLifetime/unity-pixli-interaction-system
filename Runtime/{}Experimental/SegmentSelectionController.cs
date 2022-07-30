using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixLi
{
	public class SegmentSelectionController : SelectionController<Segment>
	{
#if UNITY_EDITOR
		[
			CustomEditor(
				inspectedType: typeof(SegmentSelectionController),
					editorForChildClasses: true
			),
			CanEditMultipleObjects
		]
		public class SegmentSelectionControllerEditor : SelectionControllerEditor<Segment> { }
#endif
	}
}