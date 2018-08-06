using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Quartz;
using Quartz.Impl;

namespace MUtils.Quartz
{
    public class BaseJobScheduler<T> where T : IJob
    {
        private readonly IScheduler _scheduler;
        private readonly IEnumerable<T> _jobs;

        public BaseJobScheduler(IEnumerable<T> jobs)
        {
            _jobs = jobs;
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        }

        public void Start(Dictionary<string,Configuration> configs)
        {
            _scheduler.Start();
            foreach (var job in _jobs)
            {
                var jobDetail = JobBuilder.Create(job.GetType()).Build();
                var name = jobDetail.JobType.Name;
                if (!configs.ContainsKey(name)) continue;
                var sting = GetScheduler(configs[name]);
                if (sting.builder == null) continue;
                if (sting.configs != null && sting.configs.Any())
                {
                    foreach (var item in sting.configs)
                    {
                        jobDetail = JobBuilder.Create(job.GetType())
                            .UsingJobData(new JobDataMap((IDictionary<string, object>)item))
                            .WithIdentity(Guid.NewGuid().ToString())
                            .Build();
                        var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(sting.builder)
                            .StartNow()
                            .Build();
                        _scheduler.ScheduleJob(jobDetail, trigger);
                    }
                }
                else
                {
                    var trigger = TriggerBuilder.Create()
                        .WithSimpleSchedule(sting.builder)
                        .StartNow()
                        .Build();
                    _scheduler.ScheduleJob(jobDetail, trigger);
                }
            }
        }

        public (Action<SimpleScheduleBuilder> builder, IEnumerable<Dictionary<string, object>> configs) GetScheduler(Configuration config)
        {
            var enable = !config.Disable;
            if (!enable) return (null, null);
            var interval = config.IntervalInSeconds;
            var types = config.Types;
            var rCount = config.RepeatCount;
            Action<SimpleScheduleBuilder> b = builder =>
            {
                if (rCount.HasValue) builder.WithRepeatCount(rCount.Value);
                else
                {
                    builder.RepeatForever();
                }
                if (interval.HasValue) builder.WithInterval(TimeSpan.FromSeconds(interval.Value));
                builder.WithMisfireHandlingInstructionIgnoreMisfires();
            };

            var instances = new List<Dictionary<string, object>>();
            foreach (var type in types)
            {
                foreach (var item in config.JobInstance)
                {
                    item.Add("type", type);
                    instances.Add(item);
                }
            }
            return (b, instances.Any() ? instances : config.JobInstance);
        }

        public void ReScheduleJob(Configuration config, TriggerKey triggerKey, DateTime? startAt)
        {
            var scheduler = GetScheduler(config);
            ITrigger trigger = null;
            if (startAt.HasValue)
            {
                trigger = TriggerBuilder.Create()
                    .WithSimpleSchedule(scheduler.builder)
                    .StartAt(startAt.Value)
                    .Build();
            }
            else
            {
                TriggerBuilder.Create()
                    .WithSimpleSchedule(scheduler.builder)
                    .StartNow()
                    .Build();
            }
            _scheduler.RescheduleJob(triggerKey, trigger);
        }
    }
}
