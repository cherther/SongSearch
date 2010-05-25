using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace SongSearch.Web {
	
	public class DataSessionReadOnlyEF : IDataSessionReadOnly {
		ObjectContext _context;
		public DataSessionReadOnlyEF(ObjectContext context) {
			_context = context;
			_context.ContextOptions.LazyLoadingEnabled = false;
		}
		string GetSetName<T>() {
			var entitySetProperty =
			_context.GetType().GetProperties()
			   .Single(p => p.PropertyType.IsGenericType && typeof(IQueryable<>)
			   .MakeGenericType(typeof(T)).IsAssignableFrom(p.PropertyType));

			return entitySetProperty.Name;
		}
		public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new() {

			return new ObjectQuery<T>(GetSetName<T>(), _context, MergeOption.NoTracking).SingleOrDefault();
		}

		public IQueryable<T> All<T>() where T : class, new() {
			return new ObjectQuery<T>(GetSetName<T>(), _context, MergeOption.NoTracking);
		}
		public ObjectQuery<T> GetObjectQuery<T>() where T : class, new() {
			return new ObjectQuery<T>(GetSetName<T>(), _context, MergeOption.NoTracking);
		}
		#region IDispose Members
		// ----------------------------------------------------------------------------
		// Dispose
		// ----------------------------------------------------------------------------
		private bool _disposed;

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing) {
			if (!_disposed) {
				// If disposing equals true, dispose all managed
				// and unmanaged resources.if (disposing)
				{
					if (_context != null) { _context.Dispose(); _context = null; }
				}

				// Call the appropriate methods to clean up
				// unmanaged resources here.
				//CloseHandle(handle);
				//handle = IntPtr.Zero;

				_disposed = true;

			}
		}

		#endregion

	}
}