using System;
using System.Collections.Generic;


//namespace Battle
//{
using UnityEngine;

    public class TimeEventManager 
    {

        private List<TimeEventHandler> eventList = new List<TimeEventHandler>();
        private List<TimeEventHandler> pauseList = new List<TimeEventHandler>();
        private List<TimeEventHandler> cacheList = new List<TimeEventHandler>();
        private List<TimeEventHandler> deathList = new List<TimeEventHandler>();
        private List<TimeEventHandler> tempList = new List<TimeEventHandler>();
        private double timeLine = 0;
        private bool paused = false;

        public bool needReSort = false;

        public TimeEventManager()
        {

        }

        public void Stop()
        {
            Clear();
            timeLine = 0;
            paused = true;
        }

        private void Clear()
        {
            UpdateTimeLine(0);
            UpdateCacheList();
            UpdatePauseList();
            UpdateLifeCircle();
            if (eventList.Count > 0)
            {
                #if DEBUG 
			Debug.LogError("timeEvent eventList Count:" + eventList.Count);  
 #endif
                foreach(var data in eventList){
                    #if DEBUG 
					Debug.LogError("timeEvent  eventList detail: methond" + data.handler.Method.ToString() + " " + data.handler.Target.ToString() + " timer:" + data.triggerTime + " timeLine:" + timeLine);  
 #endif
                }
            }
            if (pauseList.Count > 0)
            {
                #if DEBUG 
				Debug.LogError("timeEvent  pauseList Count:" + pauseList.Count);  
 #endif
            }
            if (cacheList.Count > 0)
            {
                #if DEBUG 
				Debug.LogError("timeEvent  cacheList Count:" + cacheList.Count);  
 #endif
            }
            if (deathList.Count > 0)
            {
                #if DEBUG 
				Debug.LogError("timeEvent  deathList Count:" + deathList.Count);  
 #endif
            }
            if (tempList.Count > 0)
            {
                #if DEBUG 
				Debug.LogError("timeEvent  tempList Count:" + tempList.Count);  
 #endif
            }

            eventList.Clear();
            pauseList.Clear();
            cacheList.Clear();
            deathList.Clear();
            tempList.Clear();
        }

        public void Start()
        {
            Clear();
            timeLine = 0;
            paused = false;
        }

        public void StartDoNotClearEventList()
        {
            UpdateTimeLine(0);
            UpdateCacheList();
            UpdatePauseList();
            UpdateLifeCircle();
            pauseList.Clear();
            cacheList.Clear();
            deathList.Clear();
            tempList.Clear();
            timeLine = 0;
            paused = false;
        }

        public TimeEventHandler CreateEvent(TimeEventHandler.EventCallback eh, double delay)
        {
            TimeEventHandler teh = new TimeEventHandler(eh, delay + timeLine);
            cacheList.Add(teh);
            return teh;
        }

        public TimeEventHandler CreateEvent(TimeEventHandler.EventCallback eh, double delay, double spaceTime)
        {
            TimeEventHandler teh = new TimeEventHandler(eh, delay + timeLine, spaceTime);
            cacheList.Add(teh);
            return teh;
        }

        public TimeEventHandler CreateEvent(TimeEventHandler.EventCallback eh,TimeEventHandler.EventCallbackEnd eeh, double delay, double spaceTime,int count)
        {
            TimeEventHandler teh = new TimeEventHandler(eh, eeh, delay + timeLine, spaceTime, count);
            cacheList.Add(teh);
            return teh;
        }




        public void OrderUpdate(double deltaTime)
        {
            if (paused)
            {
                return;
            }

            UpdateTimeLine(deltaTime);
            UpdateCacheList();
            UpdatePauseList();
            UpdateLifeCircle();
            UpdateDoingList();
        }


        private void UpdateTimeLine(double deltaTime)
        {
            timeLine += deltaTime;
        }

        private void UpdateCacheList()
        {
            if (cacheList.Count > 0)
            {
                foreach (var data in cacheList)
                {
                    if (data.state == TimeEventHandler.EventLifeCircle.CREATE || data.state == TimeEventHandler.EventLifeCircle.DOING)
                    {
                        AddEvent(data);
                    }
                    else if (data.state == TimeEventHandler.EventLifeCircle.PAUSE)
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
                for (int i = 0; i < pauseList.Count;i++ )
                {
                    if (pauseList[i].state == TimeEventHandler.EventLifeCircle.DOING)
                    {
                        AddEvent(pauseList[i]);
                        tempList.Add(pauseList[i]);
                    }
                }
                for (int i = 0; i < tempList.Count; i++)
                {
                    pauseList.Remove(tempList[i]);
                }
            }
        }

        private void UpdateLifeCircle()
        {
            for (int i=0;i< eventList.Count;i++)
            {
                switch (eventList[i].state)
                {
                    case TimeEventHandler.EventLifeCircle.CREATE:
                        cacheList.Add(eventList[i]);
                        break;
                    case TimeEventHandler.EventLifeCircle.DOING:

                        break;
                    case TimeEventHandler.EventLifeCircle.PAUSE:
                        pauseList.Add(eventList[i]);
                        break;
                    case TimeEventHandler.EventLifeCircle.DEATH:
                        deathList.Add(eventList[i]);
                        break;
                }
            }

            if (cacheList.Count != 0)
            {
                for (int i = 0; i < cacheList.Count;i++ )
                {
                    eventList.Remove(cacheList[i]);
                }
            }


            if (pauseList.Count != 0)
            {
                for (int i = 0; i < pauseList.Count; i++)
                {
                    eventList.Remove(pauseList[i]);
                }

            }

            if (deathList.Count != 0)
            {
                for (int i = 0; i < deathList.Count; i++)
                {

                    eventList.Remove(deathList[i]);
                }

                deathList.Clear();
            }
        }

        private static int Compare(TimeEventHandler a,TimeEventHandler b)
        {
            if( a.triggerTime == b.triggerTime)
            {
                return 0;
            }
            else
            {
                return a.triggerTime > b.triggerTime ? 1 : -1;
            }
        }

        private void UpdateDoingList()
        {
            if(needReSort)
            {
                eventList.Sort(Compare);
                needReSort = false;
            }


            for (int i = 0; i < eventList.Count; i++)
            {
                TimeEventHandler e = eventList[i];

                // 特殊处理
                if(double.IsNaN(e.triggerTime))
                {
					Debug.LogError("fuck！发现 NaN 触发时间！");
                    e.state = TimeEventHandler.EventLifeCircle.DEATH;
                    e.handler();
                    continue;
                }

                if (timeLine >= e.triggerTime)
                {
                    if (e.state != TimeEventHandler.EventLifeCircle.DOING)
                    {
                        continue;
                    }

                    switch (e.eventType)
                    {
                        case TimeEventHandler.EventType.Once: //一次性事件
                            {
                                e.state = TimeEventHandler.EventLifeCircle.DEATH;
                                e.handler();
                            }
                            break;
                        case TimeEventHandler.EventType.Count_Loop://有限循环事件
                            {
                                e.count--;
                                e.triggerTime += e.spaceTime;
                                if (e.count == 0)
                                {
                                    e.state = TimeEventHandler.EventLifeCircle.DEATH;
                                    e.handler();
                                    e.endHandler();
                                }
                                else
                                {
                                    e.state = TimeEventHandler.EventLifeCircle.CREATE;
                                    e.handler();
                                }
                            }
                            break;
                        case TimeEventHandler.EventType.Infinity_loop://无限循环事件
                            {
                                e.triggerTime += e.spaceTime;
                                e.state = TimeEventHandler.EventLifeCircle.CREATE;
                                e.handler();
                            }
                            break;
                        default:
                            e.state = TimeEventHandler.EventLifeCircle.DEATH;
                            break;
                    }

                }
                else if (e.triggerTime > DMaxValue)
                {
                    UnityEngine.Debug.LogError(@"严重逻辑事件错误事件"+ ": methond" + e.handler.Method.ToString() + " " + e.handler.Target.ToString() + " timer:" + e.triggerTime + " timeLine:" + timeLine);
                    e.triggerTime = timeLine;
                    break;
                }
                else //后面的事件不会触发
                {
                    break;
                }

            }
        }

        public const double DMaxValue = double.MaxValue - 1;

        private void AddEvent(TimeEventHandler teh)
        {
            teh.state = TimeEventHandler.EventLifeCircle.DOING;
            for (int i = 0; i < eventList.Count; i++)
            {
                if (eventList[i].triggerTime > teh.triggerTime)
                {
                    eventList.Insert(i, teh);
                    return;
                }
            }
            eventList.Add(teh);
        }

        /// <summary>
        /// 如果使用cache的话就用ref
        /// </summary>
        /// <param name="teh"></param>
		public static void Delete(ref TimeEventHandler teh)
        {
            if (teh != null)
            {
                teh.state = TimeEventHandler.EventLifeCircle.DEATH;
                teh = null;
            }            
        }

        public void Pause(TimeEventHandler teh)
        {
            if (teh != null && teh.state != TimeEventHandler.EventLifeCircle.DEATH)
            {
                teh.state = TimeEventHandler.EventLifeCircle.PAUSE;
                teh.remainderTime = teh.triggerTime - timeLine;
            }
        }

        public void Continue(TimeEventHandler teh)
        {
            if (teh != null && teh.state == TimeEventHandler.EventLifeCircle.PAUSE)
            {
                teh.triggerTime = timeLine + teh.remainderTime;
                teh.state = TimeEventHandler.EventLifeCircle.DOING;
            }
        }

        public double GetEventRemainTime(TimeEventHandler teh)
        {
            if (teh == null || teh.state == TimeEventHandler.EventLifeCircle.DEATH)
            {
                return double.MaxValue;
            }
            if (teh.state == TimeEventHandler.EventLifeCircle.PAUSE)
            {
                return teh.remainderTime;
            }
            else
            {
                return teh.triggerTime - timeLine;
            }
        }

        /// <summary>
        /// 改变事件的剩余时间
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="factor">缩放系数</param>
        public void ChangeEventRemainFactor(TimeEventHandler ed, double factor)
        {
            if (ed == null)
            {
                return;
            }
            if (ed.state == TimeEventHandler.EventLifeCircle.DOING || ed.state == TimeEventHandler.EventLifeCircle.CREATE)
            {
                ed.state = TimeEventHandler.EventLifeCircle.CREATE;
                ed.triggerTime = (ed.triggerTime - timeLine) * factor + timeLine;
            }
            else if (ed.state == TimeEventHandler.EventLifeCircle.DEATH)
            {
                return;
            }
            else if (ed.state == TimeEventHandler.EventLifeCircle.PAUSE)
            {
                ed.remainderTime *= factor;
            }

            if (ed.eventType != TimeEventHandler.EventType.Once)
            {
                ed.spaceTime *= factor;
            }
            needReSort = true;
        }

        /// <summary>
        /// 改变事件的剩余时间
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="factor">缩放系数</param>
        public void AddEventRemainTime(TimeEventHandler ed, double timer)
        {
            if (ed == null)
            {
                return;
            }
            if (ed.state == TimeEventHandler.EventLifeCircle.DOING || ed.state == TimeEventHandler.EventLifeCircle.CREATE)
            {
                ed.state = TimeEventHandler.EventLifeCircle.CREATE;
                ed.triggerTime += timer;
            }
            else if (ed.state == TimeEventHandler.EventLifeCircle.DEATH)
            {
                return;
            }
            else if (ed.state == TimeEventHandler.EventLifeCircle.PAUSE)
            {
                ed.remainderTime += timer;
            }

            if (ed.eventType != TimeEventHandler.EventType.Once)
            {
                ed.spaceTime += timer;
            }
            needReSort = true;
        }

        public int GetCurrentEventCount()
        {
            return eventList.Count;
        }

        public void DebugEventLog()
        {
            foreach (var data in eventList)
            {
                #if DEBUG 
				XZXDDebug.LogWarning("roundEvent eventList detail: methond" + data.handler.Method.ToString() + " " + data.handler.Target.ToString() + " timer:" + data.triggerTime + " timeLine:" + timeLine);  
 #endif
            }
        }
    }




//}
