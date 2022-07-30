using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixLi
{
	public class Indicator : MonoBehaviour
	{
		public void SetScaleX(float value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetX(value: value);

			this.transform.localScale = localScale;
		}

		public void SetScaleY(float value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetY(value: value);

			this.transform.localScale = localScale;
		}

		public void SetScaleZ(float value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetZ(value: value);

			this.transform.localScale = localScale;
		}

		public void SetScaleX(int value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetX(value: value);

			this.transform.localScale = localScale;
		}

		public void SetScaleY(int value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetY(value: value);

			this.transform.localScale = localScale;
		}

		public void SetScaleZ(int value)
		{
			Vector3 localScale = this.transform.localScale;
			localScale.SetZ(value: value);

			this.transform.localScale = localScale;
		}
	}

	public class Indicator<TWorldObject> : Indicator
		where TWorldObject : IWorldObject
	{
		public void MatchPosition(TWorldObject worldObject)
		{
			this.transform.position = worldObject.WorldPosition;
		}
	}
}