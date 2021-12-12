using System;
using System.Collections.Generic;



    public class RoundEnentHandler
    {
		//事件类型
		public enum EventType
		{
			None,
			Once,
			Count_Loop,
			Infinity_loop,
		}

		//事件状态
		public enum EventLifeCircle
		{
			CREATE,
			DOING,
			PAUSE,
			DEATH,
		}
		public delegate void EventCallback();
		public delegate void EventCallbackEnd();


        internal EventCallback handler;
        internal EventCallbackEnd endHandler;

        internal int triggerRound;
        internal int spaceRound;
        internal int count;
        internal int remainderRound;


        internal EventType eventType = EventType.None;
        internal EventLifeCircle state = EventLifeCircle.CREATE;


        internal RoundEnentHandler(EventCallback handler, int triggerRound)
        {
            this.handler = handler;
            this.triggerRound = triggerRound;

            this.eventType = EventType.Once;
        }

        internal RoundEnentHandler(EventCallback handler, EventCallbackEnd endhandler, int triggerRound, int spaceRound, int count)
        {
            this.handler = handler;
            this.endHandler = endhandler;
            this.triggerRound = triggerRound;
            this.spaceRound = spaceRound;
            this.count = count;

            this.eventType = EventType.Count_Loop;
        }

        internal RoundEnentHandler(EventCallback handler, int triggerRound, int spaceRound)
        {
            this.handler = handler;
            this.triggerRound = triggerRound;
            this.spaceRound = spaceRound;

            this.eventType = EventType.Infinity_loop;
        }

    }

