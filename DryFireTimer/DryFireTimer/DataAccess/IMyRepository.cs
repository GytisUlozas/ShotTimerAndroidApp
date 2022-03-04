using DryFireTimer.Models;

namespace DryFireTimer.DataAccess
{
    public  interface IMyRepository
    {
        Exercise GetFirst();
        Exercise GetPrevious(Exercise ex);
        Exercise GetNext(Exercise ex);
        void Create(Exercise ex);
        void Update(Exercise ex);
        void Delete(Exercise ex);
    }
}
