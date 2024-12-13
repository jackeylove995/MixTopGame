using System.Collections;

namespace MTG
{
    public interface IAppInitTask 
    {
        public IEnumerator DOTask();
        public void OnAllTasksInitSuccessfully();
    }
}

