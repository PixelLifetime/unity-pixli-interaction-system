using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixLi
{
	public class Selection<TEntity> : MonoBehaviour
	{
		public bool Active_ { get; private set; } = true;

		public void Activate()	 => this.Active_ = true;
		public void Deactivate() => this.Active_ = false;

		[SerializeField] private UnityEvent<TEntity> _onSelectedEntityChanged;
		public UnityEvent<TEntity> _OnSelectedEntityChanged => this._onSelectedEntityChanged;

		public TEntity SelectedEntity_ { get; private set; }

		[SerializeField] private UnityEvent<TEntity> _onPreviouslySelectedEntityChanged;
		public UnityEvent<TEntity> _OnPreviouslySelectedEntityChanged => this._onPreviouslySelectedEntityChanged;

		public TEntity PreviouslySelectedEntity_ { get; private set; }

		public bool Select(TEntity entity)
		{
			if (this.Active_)
			{
				this.PreviouslySelectedEntity_ = this.SelectedEntity_;
				this.SelectedEntity_ = entity;

				//!? If this becomes too frequent - move this bit into `SelectedEntity_` setter.
				this._onPreviouslySelectedEntityChanged.Invoke(this.PreviouslySelectedEntity_);
				this._onSelectedEntityChanged.Invoke(this.SelectedEntity_);

				return true;
			}

			return false;
		}

		public void SelectVoid(TEntity entity) => this.Select(entity: entity);

		public void Deselect() => this.Select(default);
	}
}