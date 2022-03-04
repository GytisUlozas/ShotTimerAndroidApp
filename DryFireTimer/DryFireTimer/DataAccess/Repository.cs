using DryFireTimer.Models;
using SQLite;
using System;

namespace DryFireTimer.DataAccess
{
    public class Repository : IMyRepository
    {
        private readonly string dbPath;
        private SQLiteConnection db;

        public Repository(string _dbPath)
        {
            dbPath = _dbPath;

            using (db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<Exercise>();
                Exercise result = db.Table<Exercise>().FirstOrDefault();

                if (result == null)
                    db.Insert(Exercise.Default);
            }
        }

        public void Create(Exercise ex)
        {
            using (db = new SQLiteConnection(dbPath))
            {
                db.Insert(ex);
            }
        }

        public void Delete(Exercise ex)
        {
            using (db = new SQLiteConnection(dbPath))
            {
                db.Delete(ex);
            }
        }

        public Exercise GetFirst()
        {
            using (db = new SQLiteConnection(dbPath))
            {
                return db.Table<Exercise>().OrderBy(x => x.Id).First();
            }
        }

        public Exercise GetNext(Exercise ex)
        {
            using (db = new SQLiteConnection(dbPath))
            {
                return db.Table<Exercise>().Where(x => x.Id > ex.Id).OrderBy(x => x.Id).FirstOrDefault();
            }
        }

        public Exercise GetPrevious(Exercise ex)
        {
            using (db = new SQLiteConnection(dbPath))
            {
                return db.Table<Exercise>().Where(x => x.Id < ex.Id).OrderByDescending(x => x.Id).FirstOrDefault();
            }
        }

        public void Update(Exercise ex)
        {
            using (db = new SQLiteConnection(dbPath))
            {
                db.Update(ex);
            }
        }
    }
}
