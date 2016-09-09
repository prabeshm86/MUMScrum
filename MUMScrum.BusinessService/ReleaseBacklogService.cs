using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;
using MUMScrum.DataAccess;

namespace MUMScrum.BusinessService
{
    public class ReleaseBacklogService:IReleaseBacklogService

    {
        IUnitOfWork work = new UnitOfWork();
        public void CreateRelease(Model.ReleaseBacklog release)
        {
            work.ReleaseBacklogsRepository.Insert(release);
            work.SaveChanges();

            //throw new NotImplementedException();
        }


        public IEnumerable<ReleaseBacklog> GetAll()
        {
            return work.ReleaseBacklogsRepository.Get();

           // throw new NotImplementedException();
        }

        public ReleaseBacklog GetById(int releaseId)
        {
           return  work.ReleaseBacklogsRepository.GetByID(releaseId);
            //throw new NotImplementedException();
        }

        public IEnumerable<ReleaseBacklog> GetAllReleasesByScrumMaster(int ScrumId)
        {
            return work.ReleaseBacklogsRepository.Get(r => r.ScrumMasterId == ScrumId);
        }

        public void UpdateRelease(ReleaseBacklog release)
        {
            work.ReleaseBacklogsRepository.Update(release);
            work.SaveChanges();
        }

        public void DeleteRelease(int id)
        {
            var CountStartedSprint = work.ReleaseBacklogsRepository.GetByID(id).Sprints.Where(c => c.SprintRunning == true).Count();//Get(c => c.Sprints.== true & c.Id == id).Count();
            if (CountStartedSprint > 0)
                throw new Exception("Release can't be deleted ... there is started Sprints related to it!");

            var userstories = work.UserStoryRepository.Get(i => i.ProductBackLogId == id);
            foreach (var userstory in userstories)
            {
                userstory.ReleaseId = null;
                userstory.SprintId = null;
            }

            var sprints = work.SprintRepository.Get(i => i.ReleaseBacklogId == id);
            foreach (var sprint in sprints)
            {
                work.SprintRepository.Delete(sprint);
            }
            work.ReleaseBacklogsRepository.Delete(id);
            work.SaveChanges();
        }

        public IEnumerable<ReleaseBacklog> GetAllReleasesByProductId(int ProductId)
        {
            return work.ReleaseBacklogsRepository.Get(R => R.ProductBacklogId == ProductId);
        }

       
    }
}
