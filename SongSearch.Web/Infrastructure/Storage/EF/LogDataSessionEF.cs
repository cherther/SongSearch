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
	public class LogDataSessionEF : ILogDataSession {
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

		public LogDataSessionEF(ObjectContext context) {
			_context = context;
			_context.ContextOptions.LazyLoadingEnabled = false;
			
		}


		// ----------------------------------------------------------------------------
		// (Public)
		// ----------------------------------------------------------------------------

		#region ILogDataSession Members

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

		public void Add<T>(T item) where T : class, new() {
			_context.AddObject(GetSetName<T>(), item);
		}
		public void Add<T>(IEnumerable<T> items) where T : class, new() {
			foreach (var item in items) {
				Add(item);
			}
		}
		public void QuickAdd<T>(T item) where T : class, new() {
			if (_context.Connection.State != System.Data.ConnectionState.Open) { _context.Connection.Open(); }
				_context.AddObject(GetSetName<T>(), item);
//				_context.SaveChanges();
				_context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
			
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
