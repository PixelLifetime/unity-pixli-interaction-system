using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixLi
{
	public class SegmentSelection : Selection<Segment>
	{
		private void Awake()
		{
			this._OnSelectedEntityChanged.AddListener(
				(segment) =>
				{
					// Maybe update the UI?
				}
			);
		}
	}
}