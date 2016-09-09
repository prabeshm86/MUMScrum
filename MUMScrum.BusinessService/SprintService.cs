using System;
using System.Collections.Generic;
using MUMScrum.DataAccess;
using MUMScrum.Model;
using System.Linq;
using MUMScrum.Model.Entity;

namespace MUMScrum.BusinessService
{
    public class SprintService : ISprintService
    {
        IUnitOfWork work = new UnitOfWork();

        public void CreateSprint(Model.Sprint sprint)
        {
            work.SprintRepository.Insert(sprint);
            work.SaveChanges();
           
            // throw new NotImplementedException();
        }

        public IEnumerable<Model.Sprint> GetAll()
        {
            return work.SprintRepository.Get();

            /// throw new NotImplementedException();
        }

        public Model.Sprint GetById(int sprintId)
        {
            return work.SprintRepository.GetByID(sprintId);

            // throw new NotImplementedException();
        }

        public void UpdateSprint(Model.Sprint sprint)
        {
            work.SprintRepository.Update(sprint);
            work.SaveChanges();
        }

        public void DeleteSprint(int id)
        {
            var userstories = work.UserStoryRepository.Get(i => i.SprintId == id);
            foreach (var userstory in userstories)
            {
                userstory.SprintId = null;
            }
            work.SprintRepository.Delete(id);
            work.SaveChanges();
            // throw new NotImplementedException();
        }

        public IEnumerable<Sprint> GetAllSprintsByRelId(int ReleaseId)
        {
            return work.SprintRepository.Get(s => s.ReleaseBacklogId == ReleaseId);
        }

        public IEnumerable<BurndownChartRecord> GetBurnDownChartForSprint(int sprintId)
        {         
            var sprint = work.SprintRepository.GetByID(sprintId);
            var records = new List<BurndownChartRecord>();           
            for (DateTime d = sprint.StartDate.Date; d < sprint.EndDate.Date.AddDays(1); d = d.AddDays(1))
            {
                var newRecord = new BurndownChartRecord
                {
                    Date = d,
                    ActualWork = 0,
                    EstimateWork = 0,
                };
                var usLogs = work.UserStoryLogRepository.Get(i => i.UserStory.SprintId == sprintId)
                   .Where(i => i.Date >= d && i.Date < d.AddDays(1));

               
                //total Actualwork               
                foreach(var log in usLogs)
                {
                    switch (log.LogType)
                    {
                        case LogType.DevActual:
                        case LogType.TestActual:
                            newRecord.ActualWork += log.NewValue;
                            break;                           
                    }
                    
                }

                //total Estimated work
                var sprintUS = work.UserStoryRepository.Get(i=>i.SprintId == sprintId);
                foreach(var us in sprintUS)
                {                    
                    newRecord.EstimateWork += (us.DevEstimate.HasValue ? us.DevEstimate.Value :0) +
                         (us.TestEstiamte.HasValue ? us.TestEstiamte.Value : 0);

                    var prevActualWorks = work.UserStoryLogRepository.Get(i => i.UserStoryId == us.Id)
                        .Where(i => i.Date.Date < d)
                        .Where(i=>i.LogType == LogType.DevActual || i.LogType == LogType.TestActual);
                    foreach(var logs in prevActualWorks)
                    {
                        newRecord.EstimateWork -= logs.NewValue;
                    }

                }
                records.Add(newRecord);
            }
            return records;
        }
    }


}


