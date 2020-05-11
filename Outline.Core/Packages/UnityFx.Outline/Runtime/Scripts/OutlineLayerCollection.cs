﻿// Copyright (C) 2019-2020 Alexander Bogarsukov. All rights reserved.
// See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFx.Outline
{
	/// <summary>
	/// A serializable collection of outline layers.
	/// </summary>
	/// <seealso cref="OutlineLayer"/>
	/// <seealso cref="OutlineEffect"/>
	/// <seealso cref="OutlineSettings"/>
	[CreateAssetMenu(fileName = "OutlineLayerCollection", menuName = "UnityFx/Outline/Outline Layer Collection")]
	public sealed class OutlineLayerCollection : ScriptableObject, IList<OutlineLayer>, IReadOnlyList<OutlineLayer>
	{
		#region data

		[SerializeField, HideInInspector]
		private List<OutlineLayer> _layers = new List<OutlineLayer>();

		#endregion

		#region interface

		/// <summary>
		/// Gets the objects for rendering.
		/// </summary>
		public void GetRenderObjects(IList<OutlineObject> renderObjects)
		{
			foreach (var layer in _layers)
			{
				layer.GetRenderObjects(renderObjects);
			}
		}

		#endregion

		#region internals

		internal void Reset()
		{
			foreach (var layer in _layers)
			{
				layer.Reset();
			}
		}

		#endregion

		#region ScriptableObject

		private void OnEnable()
		{
			foreach (var layer in _layers)
			{
				layer.SetCollection(this);
			}
		}

		#endregion

		#region IList

		/// <inheritdoc/>
		public OutlineLayer this[int layerIndex]
		{
			get
			{
				return _layers[layerIndex];
			}
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException("layer");
				}

				if (layerIndex < 0 || layerIndex >= _layers.Count)
				{
					throw new ArgumentOutOfRangeException(nameof(layerIndex));
				}

				if (_layers[layerIndex] != value)
				{
					value.SetCollection(this);

					_layers[layerIndex].SetCollection(null);
					_layers[layerIndex] = value;
				}
			}
		}

		/// <inheritdoc/>
		public int IndexOf(OutlineLayer layer)
		{
			if (layer != null)
			{
				return _layers.IndexOf(layer);
			}

			return -1;
		}

		/// <inheritdoc/>
		public void Insert(int index, OutlineLayer layer)
		{
			if (layer is null)
			{
				throw new ArgumentNullException(nameof(layer));
			}

			if (layer.ParentCollection != this)
			{
				layer.SetCollection(this);
				_layers.Insert(index, layer);
			}
		}

		/// <inheritdoc/>
		public void RemoveAt(int index)
		{
			if (index >= 0 && index < _layers.Count)
			{
				_layers[index].SetCollection(null);
				_layers.RemoveAt(index);
			}
		}

		#endregion

		#region ICollection

		/// <inheritdoc/>
		public int Count => _layers.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => false;

		/// <inheritdoc/>
		public void Add(OutlineLayer layer)
		{
			if (layer is null)
			{
				throw new ArgumentNullException(nameof(layer));
			}

			if (layer.ParentCollection != this)
			{
				layer.SetCollection(this);
				_layers.Add(layer);
			}
		}

		/// <inheritdoc/>
		public bool Remove(OutlineLayer layer)
		{
			if (_layers.Remove(layer))
			{
				layer.SetCollection(null);
				return true;
			}

			return false;
		}

		/// <inheritdoc/>
		public void Clear()
		{
			if (_layers.Count > 0)
			{
				foreach (var layer in _layers)
				{
					layer.SetCollection(null);
				}

				_layers.Clear();
			}
		}

		/// <inheritdoc/>
		public bool Contains(OutlineLayer layer)
		{
			if (layer is null)
			{
				return false;
			}

			return _layers.Contains(layer);
		}

		/// <inheritdoc/>
		public void CopyTo(OutlineLayer[] array, int arrayIndex)
		{
			_layers.CopyTo(array, arrayIndex);
		}

		#endregion

		#region IEnumerable

		/// <inheritdoc/>
		public IEnumerator<OutlineLayer> GetEnumerator()
		{
			return _layers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _layers.GetEnumerator();
		}

		#endregion

		#region implementation
		#endregion
	}
}
