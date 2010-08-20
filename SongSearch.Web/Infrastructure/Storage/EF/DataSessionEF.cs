using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using SongSearch.Web.Data;

namespace SongSearch.Web {


	// **************************************
	// SqlSession
	// **************************************    
	public class DataSessionEF : IDataSession {
		// ----------------------------------------------------------------------------
		// (Properties)
		// ----------------------------------------------------------------------------
		private ObjectContext _context;

		// ----------------------------------------------------------------------------
		// (Constructor)
		// ----------------------------------------------------------------------------
		//public DataSessionEF()// : this(new _context()) { }
		//{
		//    _context = new SongSearchContext(Connections.ConnectionString(ConnectionStrings.SongSearchContext));
		//}

		public DataSessionEF(ObjectContext context) {
			_context = context;
		}


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		#region IDataSession Members

		public void CommitChanges() {
			_context.SaveChanges();
		}

		string GetSetName<T>() {

			//If you get an error here it's because your namespace
			//for your EDM doesn't match the partial contentModel class
			//to change - open the properties for the EDM FILE and change "Custom Tool Namespace"
			//Not - this IS NOT the Namespace setting in the EDM designer - that's for something
			//else entirely. This is for the EDMX file itself (rm-click, properties)

			var entitySetProperty =
			_context.GetType().GetProperties()
			   .Single(p => p.PropertyType.IsGenericType && typeof(IQueryable<>)
			   .MakeGenericType(typeof(T)).IsAssignableFrom(p.PropertyType));

			return entitySetProperty.Name;
		}

		// ----------------------------------------------------------------------------
		//  Generic SqlSession methods
		// ----------------------------------------------------------------------------
		private ObjectSet<T> GetTable<T>() where T : class, new() {
			return _context.CreateObjectSet<T>();
		}

		public IQueryable<T> All<T>() where T : class, new() {
				return GetTable<T>().AsQueryable();
		}
		//public IQueryable<T> All<T>(string loadWith = null) where T : class, new() {
		//    if (loadWith == null) {
		//        return GetTable<T>().AsQueryable();
		//    }

		//    var set = GetTable<T>();
		//    var query = !String.IsNullOrWhiteSpace(loadWith) ? set.Include(loadWith) : set;
		//    return set.AsQueryable();
		//}

		public IQueryable<T> All<T>(string commandText, params object[] parameters) where T : class, new() {

			return _context.ExecuteStoreQuery<T>(commandText, parameters).AsQueryable();
		}

		public ObjectQuery<T> GetObjectQuery<T>() where T : class, new() {
			return new ObjectQuery<T>(GetSetName<T>(), _context);
		}

		public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new() {
			
			return GetTable<T>().SingleOrDefault(expression);
		}

		public void Add<T>(T item) where T : class, new() {
			_context.AddObject(GetSetName<T>(), item);
		}
		public void Add<T>(IEnumerable<T> items) where T : class, new() {
			foreach (var item in items) {
				Add(item);
			}
		}
		public void QuickAdd<T>(T item) where T : class, new() {
			_context.AddObject(GetSetName<T>(), item);
			_context.SaveChanges();
		}
		public void Update<T>(T item) where T : class, new() {
			//nothing needed here
		}

		public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, new() {
			var query = All<T>().Where(expression);
			_context.DeleteObject(query);
		}

		public void Delete<T>(T item) where T : class, new() {
			GetTable<T>().DeleteObject(item);
		}

		public void DeleteMany<T>(IEnumerable<T> items) where T : class, new() {
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
