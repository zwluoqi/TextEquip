using System;
using System.Collections.Generic;
using UnityEngine;




    public class RoundEventManager 
    {
		
        private List<RoundEnentHandler> eventList = new List<RoundEnentHandler>();
        private List<RoundEnentHandler> pauseList = new List<RoundEnentHandler>();
        private List<RoundEnentHandler> cacheList = new List<RoundEnentHandler>();
        private List<RoundEnentHandler> deathList = new List<RoundEnentHandler>();
        private List<RoundEnentHandler> tempList = new List<RoundEnentHandler>();
        private int currentRound = 0;
        private bool paused = false;

        public RoundEventManager()
        {

        }

        public void Reset()
        {
            UpdateTimeLine(0);
            UpdateCacheList();
            UpdatePauseList();
            UpdateLifeCircle();
            if (eventList.Count > 0)
            {
                XZXDDebug.LogWarning("roundEvent eventList Count:" + eventList.Count);
            }
            if (pauseList.Count > 0)
            {
                XZXDDebug.LogWarning("roundEvent pauseList Count:" + pauseList.Count);
            }
            if (cacheList.Count > 0)
            {
                XZXDDebug.LogWarning("roundEvent cacheList Count:" + cacheList.Count);
            }
            if (deathList.Count > 0)
            {
                XZXDDebug.LogWarning("roundEvent deathList Count:" + deathList.Count);
            }
            if (tempList.Count > 0)
            {
                XZXDDebug.LogWarning("roundEvent tempList Count:" + tempList.Count);
            }

            currentRound = 0;
            paused = true;
            eventList.Clear();
            pauseList.Clear();
            cacheList.Clear();
            deathList.Clear();
            tempList.Clear();
        }

        public void Start()
        {
            Reset();
            currentRound = 0;
            paused = false;
        }

        public RoundEnentHandler CreateEvent(RoundEnentHandler.EventCallback eh, int delayRound)
        {
            RoundEnentHandler teh = new RoundEnentHandler(eh, delayRound + currentRound);
            cacheList.Add(teh);
            return teh;
        }

        public RoundEnentHandler CreateEvent(RoundEnentHandler.EventCallback eh, int delayRound, int spaceRound)
        {
            RoundEnentHandler teh = new RoundEnentHandler(eh, delayRound + currentRound, spaceRound);
            cacheList.Add(teh);
            return teh;
        }

        public RoundEnentHandler CreateEvent(RoundEnentHandler.EventCallback eh, RoundEnentHandler.EventCallbackEnd eeh, int delayRound, int spaceRound, int count)
        {
            RoundEnentHandler teh = new RoundEnentHandler(eh, eeh, delayRound + currentRound, spaceRound, count);
            cacheList.Add(teh);
            return teh;
        }




        public void Update(int deltaRound)
        {
            if (paused)
            {
                return;
            }

            UpdateTimeLine(deltaRound);
            UpdateCacheList();
            UpdatePauseList();
            UpdateLifeCircle();
            UpdateDoingList();
        }


        private void UpdateTimeLine(int deltaRound)
        {
            currentRound += deltaRound;
        }

        private void UpdateCacheList()
        {
            if (cacheList.Count > 0)
            {
                foreach (var data in cacheList)
                {
                    if (data.state == RoundEnentHandler.EventLifeCircle.CREATE || data.state == RoundEnentHandler.EventLifeCircle.DOING)
                    {
                        AddEvent(data);
                    }
                    else if (data.state == RoundEnentHandler.EventLifeCircle.PAUSE)
                    {
                        pauseList.Add(data);
                    }
                }
                cacheList.Clear();
            }
        }

        private void UpdatePauseList()
        {
            if (pauseList.Count > 0)
            {
                tempList.Clear();
                foreach (RoundEnentHandler data in pauseList)
                {
                    if (data.state == RoundEnentHandler.EventLifeCircle.DOING)
                    {
                        AddEvent(data);
                        tempList.Add(data);
                    }
                }

                foreach (RoundEnentHandler data in tempList)
                {
                    pauseList.Remove(data);
                }
            }
        }

        private void UpdateLifeCircle()
        {
            foreach (RoundEnentHandler data in eventList)
            {
                switch (data.state)
                {
                    case RoundEnentHandler.EventLifeCircle.CREATE:
                        cacheList.Add(data);
                        break;
                    case RoundEnentHandler.EventLifeCircle.DOING:

                        break;
                    case RoundEnentHandler.EventLifeCircle.PAUSE:
                        pauseList.Add(data);
                        break;
                    case RoundEnentHandler.EventLifeCircle.DEATH:
                        deathList.Add(data);
                        break;
                }
            }

            if (cacheList.Count != 0)
            {
                foreach (RoundEnentHandler e in cacheList)
                {
                    eventList.Remove(e);
                }
            }


            if (pauseList.Count != 0)
            {
                foreach (RoundEnentHandler e in pauseList)
                {
                    eventList.Remove(e);
                }

            }

            if (deathList.Count != 0)
            {
                foreach (RoundEnentHandler e in deathList)
                {
                    eventList.Remove(e);
                }

                deathList.Clear();
            }
        }

        private void UpdateDoingList()
        {
            foreach (RoundEnentHandler e in eventList)
            {
                if (currentRound >= e.triggerRound)
                {
                    if (e.state != RoundEnentHandler.EventLifeCircle.DOING)
                    {
                        continue;
                    }

                    switch (e.eventType)
                    {
				case RoundEnentHandler.EventType.Once: //一次性事件
                            {
                                e.state = RoundEnentHandler.EventLifeCircle.DEATH;
                                e.handler();
                            }
                            break;
				case RoundEnentHandler.EventType.Count_Loop://有限循环事件
                            {
                                e.count--;
                                e.triggerRound += e.spaceRound;
                                if (e.count == 0)
                                {
                                    e.handler();
                                    e.endHandler();
                                    e.state = RoundEnentHandler.EventLifeCircle.DEATH;
                                }
                                else
                                {
                                    e.handler();
                                    e.state = RoundEnentHandler.EventLifeCircle.CREATE;
                                }
                            }
                            break;
				case RoundEnentHandler.EventType.Infinity_loop://无限循环事件
                            {
                                e.triggerRound += e.spaceRound;
                                e.handler();
                                e.state = RoundEnentHandler.EventLifeCircle.CREATE;
                            }
                            break;
                        default:
                            e.state = RoundEnentHandler.EventLifeCircle.DEATH;
                            break;
                    }

                }
                else //后面的事件不会触发
                {
                    break;
                }

            }
        }

        private void AddEvent(RoundEnentHandler teh)
        {
            teh.state = RoundEnentHandler.EventLifeCircle.DOING;
            for (int i = 0; i < eventList.Count; i++)
            {
                if (eventList[i].triggerRound > teh.triggerRound)
                {
                    eventList.Insert(i, teh);
                    return;
                }
            }
            eventList.Add(teh);
        }

        public void Delete(RoundEnentHandler teh)
        {
            if (teh != null)
            {
                teh.state = RoundEnentHandler.EventLifeCircle.DEATH;
            }
        }

        public void Pause(RoundEnentHandler teh)
        {
            if (teh != null && teh.state != RoundEnentHandler.EventLifeCircle.DEATH)
            {
                teh.state = RoundEnentHandler.EventLifeCircle.PAUSE;
                teh.remainderRound = teh.triggerRound - currentRound;
            }
        }

        public void Continue(RoundEnentHandler teh)
        {
            if (teh != null && teh.state == RoundEnentHandler.EventLifeCircle.PAUSE)
            {
                teh.triggerRound = currentRound + teh.remainderRound;
                teh.state = RoundEnentHandler.EventLifeCircle.DOING;
            }
        }
        public int GetEventRemainTime(RoundEnentHandler reh)
        {
            if (reh == null || reh.state == RoundEnentHandler.EventLifeCircle.DEATH)
            {
                return 0;
            }
            if (reh.state == RoundEnentHandler.EventLifeCircle.PAUSE)
            {
                return reh.remainderRound;
            }
            else
            {
                return reh.triggerRound - currentRound;
            }
        }
    }
	
