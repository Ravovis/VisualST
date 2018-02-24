using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.DB
{
    class SQLActionRepository:IRepository<Models.Action>
    {
        private Models.ActionContext db;
        public SQLActionRepository()
        {
            this.db = new Models.ActionContext();
        }
        public IEnumerable<Models.Action> GetList() // получение всех объектов
        {
            return db.Actions;
        }
        public Models.Action Get(int id) // получение одного объекта по id
        {
            return db.Actions.Find(id);
        }
        public void Create(Models.Action item) // создание объекта
        {
            db.Actions.Add(item);
        }
        public void Update(Models.Action item) // обновление объекта
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public void Delete(int id) // удаление объекта по id
        {
            Models.Action action = db.Actions.Find(id);
            if (action != null)
                db.Actions.Remove(action);
        }
        public void Save()  // сохранение изменений
        {
            db.SaveChanges();
        }




        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
