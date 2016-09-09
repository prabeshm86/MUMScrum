using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;
using MUMScrum.Model.Entity;

namespace MUMScrum.BusinessService
{
    public interface ISprintService
    {
        void CreateSprint(Sprint sprint);
        IEnumerable<Sprint> GetAll();
        Sprint GetById(int sprintId);
        void UpdateSprint(Sprint sprint);
        void DeleteSprint(int id);
        IEnumerable<Sprint> GetAllSprintsByRelId(int ReleaseId);
        IEnumerable<BurndownChartRecord> GetBurnDownChartForSprint(int sprintId);

    }
}
