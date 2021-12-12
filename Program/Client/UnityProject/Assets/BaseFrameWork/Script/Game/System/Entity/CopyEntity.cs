using System.Collections.Generic;
using Script.Game.Grow;
using TextEquip.System;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    public class CopyEntity:BaseEntity
    {
        public CopyConfig config;
        public CopyEntityImp copyEntityImp;
        public int layer = 0;
        private bool mud = false;
        public void Init(WorldEntity worldEntity,CopyConfig config)
        {
            this.world = worldEntity;
            this.config = config;
        }

        public void StartCopy(bool mud)
        {
            this.mud = mud;
            if (mud)
            {
                copyEntityImp = new MudCopyEntityImp(this);
            }
            else
            {
                copyEntityImp = new TileCopyEntityImp(this);
            }
            copyEntityImp.StartCopy(mud,layer);
        }

        public void ForceEndCopy()
        {
            if (copyEntityImp != null)
            {
                copyEntityImp.EndCopy();
                copyEntityImp = null;
            }
        }


        public void Tick()
        {
            if (copyEntityImp == null)
            {
                return;
            }
            copyEntityImp.Tick();
        }

        public void StartCopyNextLayer()
        {
            if (copyEntityImp != null)
            {
                copyEntityImp.EndCopy();
                copyEntityImp = null;
            }
            layer++;
            StartCopy(mud);
        }
    }
}