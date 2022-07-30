using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixLi
{
	public class SegmentSelector : SelectorMonoBehaviour<Segment>
	{
		[SerializeField] private RaycastEmitter _raycastEmitter;
		public RaycastEmitter _RaycastEmitter => this._raycastEmitter;

		[SerializeField] private PolytopialSegmentsStructure _polytopialSegmentsStructure;
		public PolytopialSegmentsStructure _PolytopialSegmentsStructure => this._polytopialSegmentsStructure;

		public override Segment Select()
		{
			if (this._raycastEmitter.Raycast(Camera.main, out RaycastHit raycastHit))
			{
				Segment segment = this._polytopialSegmentsStructure.GetSegment(position: raycastHit.point);
				
				return segment;
			}

			return null;
		}
	}
}