using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MUMScrum.Model.Entity;
using MUMScrum.DataAccess;
using MUMScrum.BusinessService;

namespace MUMScrum.Web.APIControllers
{
    public class BurnDownChartController : ApiController
    {
        ISprintService service = new SprintService();
        public IEnumerable<Object> Get(int sprintId)
        {
            var list = new List<Object>();
            var cols = new Object[] { "Date", "Work Remaining" };
            list.Add(cols);
            foreach (var record in service.GetBurnDownChartForSprint(sprintId))
            {
                var newObj = new Object[]{ record.Date.ToString("MM/dd"), record.RemainingWork };
                list.Add(newObj);
            }
            return list;
        }
    }
}
