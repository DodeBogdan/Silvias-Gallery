using System;
using System.IO;
using SQLite;

namespace SilviasGallery.Utilities
{
    public static class Constants
    {
        #region Static Fields and Constants
        public const string DatabaseFilename = "DataBase.db3";

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;
        #endregion

        #region Properties and Indexers
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
        #endregion
    }
}
