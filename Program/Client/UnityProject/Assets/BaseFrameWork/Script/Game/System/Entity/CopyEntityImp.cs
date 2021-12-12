using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    public abstract class CopyEntityImp
    {
        protected CopyEntity copyEntity;
        
        public bool getKey = false;


        protected List<CopyEventEntity> eventEntities = new List<CopyEventEntity>();

        public abstract void StartCopy(bool repeated,int layer);

        public abstract void EndCopy();

        public abstract void Tick();

        public virtual void StartIndexAction(int index)
        {
            
        }
        
        public virtual void OpenIndexAction(int index)
        {
            
        }

        public List<CopyEventEntity> GetEventEntity()
        {
            return eventEntities;
        }

        public virtual bool IsMudMode()
        {
            return false;
        }
    }
}