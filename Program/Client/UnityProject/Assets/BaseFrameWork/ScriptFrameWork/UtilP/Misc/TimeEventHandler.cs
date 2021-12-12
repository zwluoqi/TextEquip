using System;
using System.Collections.Generic;

//namespace Battle
//{

    public class TimeEventHandler
    {
		public delegate void TickEventCallback(float deltaTime);
		public delegate void TickEventCallbackEnd(float deltaTime);

		public delegate void EventCallback();
		public delegate void EventCallbackEnd();
		public enum EventType{
			None,
			Once,
			Count_Loop,
			Infinity_loop,
		}

		public enum EventLifeCircle{
			CREATE,
			DOING,
			DEATH,
			PAUSE,
		}

        internal EventCallback handler;
        internal EventCallbackEnd endHandler;

        internal double triggerTime;
        internal double spaceTime;
        internal int count;
        internal double remainderTime;


        internal EventType eventType = EventType.None;
        internal EventLifeCircle state = EventLifeCircle.CREATE;


        internal TimeEventHandler(EventCallback handler, double triggerTime)
        {
            this.handler = handler;
            this.triggerTime = triggerTime;

            this.eventType = EventType.Once;
        }

        internal TimeEventHandler(EventCallback handler, EventCallbackEnd endhandler, double triggerTime, double spaceTime, int count)
        {
            this.handler = handler;
            this.endHandler = endhandler;
            this.triggerTime = triggerTime;
            this.spaceTime = spaceTime;
            this.count = count;

            this.eventType = EventType.Count_Loop;
        }

        internal TimeEventHandler(EventCallback handler, double triggerTime, double spaceTime)
        {
            this.handler = handler;
            this.triggerTime = triggerTime;
            this.spaceTime = spaceTime;

            this.eventType = EventType.Infinity_loop;
        }

    }
//}
