﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Quartz;
using Quartz.Impl;

namespace MUtils.Quartz
{
    public class BaseJobScheduler<T> where T : IJob
    {
        public IScheduler Scheduler { get; private set; }
        private readonly IEnumerable<T> _jobs;

        public BaseJobScheduler(IEnumerable<T> jobs)
        {
            _jobs = jobs;
            Scheduler = new StdSchedulerFactory().GetScheduler().Result;
        }

        public void Start(Dictionary<string,Configuration> configs)
        {
            Scheduler.Start();
            foreach (var job in _jobs)
            {
                var jobDetail = JobBuilder.Create(job.GetType()).Build();
                var name = jobDetail.JobType.Name;
                if (!configs.ContainsKey(name)) continue;
                var sting = GetScheduler(configs[name]);
                var startDelay = configs[name].StartDelayInSeconds ?? 0;

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
                            .StartAt(startDelay > 0 ? DateTimeOffset.UtcNow.AddSeconds(startDelay) : DateTimeOffset.UtcNow)
                            .Build();
                            
                        Scheduler.ScheduleJob(jobDetail, trigger);
                    }
                }
                else
                {
                    var trigger = TriggerBuilder.Create()
                        .WithSimpleSchedule(sting.builder)
                        .StartAt(startDelay > 0 ? DateTimeOffset.UtcNow.AddSeconds(startDelay) : DateTimeOffset.UtcNow)
                        .Build();
                    Scheduler.ScheduleJob(jobDetail, trigger);
                }
            }
        }

        public void AddJobToScheduler(string jobName, Configuration config)
        {
            var job = _jobs.FirstOrDefault(i => i.GetType().Name.Equals(jobName, StringComparison.CurrentCultureIgnoreCase));
            if (job == null) throw new KeyNotFoundException($"no job with name {jobName} found in job directory");
            var jobDetail = JobBuilder.Create(job.GetType()).Build();
            var name = jobDetail.JobType.Name;
            var sting = GetScheduler(config);
            var startDelay = config.StartDelayInSeconds ?? 0;

            if (sting.builder == null) throw new KeyNotFoundException("job builder not found");
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
                        .StartAt(startDelay > 0 ? DateTimeOffset.UtcNow.AddSeconds(startDelay) : DateTimeOffset.UtcNow)
                        .Build();

                    Scheduler.ScheduleJob(jobDetail, trigger);
                }
            }
            else
            {
                var trigger = TriggerBuilder.Create()
                    .WithSimpleSchedule(sting.builder)
                    .StartAt(startDelay > 0 ? DateTimeOffset.UtcNow.AddSeconds(startDelay) : DateTimeOffset.UtcNow)
                    .Build();
                Scheduler.ScheduleJob(jobDetail, trigger);
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
            if(types!=null)
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
            Scheduler.RescheduleJob(triggerKey, trigger);
        }

        public void StartJob(JobKey key)
        {
                
        }
    }
}
