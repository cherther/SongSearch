using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace SongSearch.Web.Data {
	

	// **************************************
	// SqlSession
	// **************************************    
	public class EFSession : ISession  {
		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private SongSearchContext _ctx;

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		public EFSession()// : this(new _ctx()) { }
		{
			_ctx = new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext));
		}

		public EFSession(SongSearchContext ctx) {
			_ctx = ctx;
		}


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		#region ISession Members

		public void CommitChanges()
		{
			_ctx.SaveChanges();
		}


		// ----------------------------------------------------------------------------
		//  Generic SqlSession methods
		// ----------------------------------------------------------------------------
		private ObjectSet<T> GetTable<T>() where T : class
		{
			return _ctx.CreateObjectSet<T>();
		}

		public IQueryable<T> All<T>() where T : class {
			return GetTable<T>().AsQueryable();
		}

		public IQueryable<T> All<T>(string loadWith) where T : class {
			var set = GetTable<T>();
			var query = !String.IsNullOrWhiteSpace(loadWith) ? set.Include(loadWith) : set;
			return set.AsQueryable();
		}

		public IQueryable<T> All<T>(string commandText, params object[] parameters) where T : class {
			return _ctx.ExecuteStoreQuery<T>(commandText, parameters).AsQueryable();
		}

		public T Single<T>(Expression<Func<T, bool>> expression) where T : class {
			return Single(expression, "");// GetTable<T>().SingleOrDefault(expression);
		}

		public T Single<T>(Expression<Func<T, bool>> expression, string loadWith) where T : class {

			var set = GetTable<T>();
			var query = !String.IsNullOrWhiteSpace(loadWith) ? set.Include(loadWith) : set;
			return set.SingleOrDefault(expression);
		}
		
		public void Add<T>(T item) where T : class {
			GetTable<T>().AddObject(item);
		}
		
		public void Add<T>(IEnumerable<T> items) where T : class {
			throw new NotImplementedException();
		}
		
		public void Update<T>(T item) where T : class {
			//nothing needed here
		}
		
		public void Delete<T>(Expression<Func<T, bool>> expression) where T : class
		{
			var query = All<T>().Where(expression);
			_ctx.DeleteObject(query);
		}

		public void Delete<T>(T item) where T : class
		{
			GetTable<T>().DeleteObject(item);
		}

		public void DeleteMany<T>(IEnumerable<T> items) where T : class {
			// is this the only way to do many?
			throw new NotImplementedException();
//			items.ForEach(i => GetTable<T>().DeleteObject(i));
		}

		

		#endregion


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
					if (_ctx != null) { _ctx.Dispose(); _ctx = null; }
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
