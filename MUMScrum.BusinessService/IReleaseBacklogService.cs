using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;

namespace MUMScrum.BusinessService
{
    public interface IReleaseBacklogService
    {
        void CreateRelease(ReleaseBacklog release);
        IEnumerable<ReleaseBacklog> GetAll();
        ReleaseBacklog GetById(int releaseId);
        void UpdateRelease(ReleaseBacklog release);
        void DeleteRelease(int id);
        IEnumerable<ReleaseBacklog> GetAllReleasesByProductId(int ProductId);
        IEnumerable<ReleaseBacklog> GetAllReleasesByScrumMaster(int ScrumId);
    }
}

