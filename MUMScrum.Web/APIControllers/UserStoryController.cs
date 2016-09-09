using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MUMScrum.Model;
using MUMScrum.BusinessService;

namespace MUMScrum.Web.APIControllers
{
    public class UserStoryController : ApiController
    {
        // private MUMScrumDbContext db = new MUMScrumDbContext();
        private IUserStoryService service = new UserStoryService();
        private IReleaseBacklogService ReleaseService = new ReleaseBacklogService();
        private ISprintService SprintService = new SprintService();

        // GET: api/UserStory
        [HttpGet]
        public IEnumerable<UserStory> Get()
        {
            return service.GetAll();
        }

        [HttpGet]
        public IEnumerable<ReleaseBacklog> GetProductReleases(int ProductId)
        {
            return ReleaseService.GetAllReleasesByProductId(ProductId);
        }

        [HttpGet]
        public IEnumerable<Sprint> GetSprintByReleaseId(int ReleaseId)
        {
            return SprintService.GetAllSprintsByRelId(ReleaseId);
        }

        // GET: api/UserStory/5
        [ResponseType(typeof(UserStory))]
        public IHttpActionResult GetUserStory(int id)
        {
            UserStory userStory = service.GetUserStoryById(id);
            if (userStory == null)
            {
                return NotFound();
            }

            return Ok(userStory);
        }

        // PUT: api/UserStory/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutUserStory(int id, UserStory userStory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != userStory.Id)
        //    {
        //        return BadRequest();
        //    }


        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserStoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/UserStory
        //[ResponseType(typeof(UserStory))]
        //public IHttpActionResult PostUserStory(UserStory userStory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.UserStories.Add(userStory);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = userStory.Id }, userStory);
        //}

        //// DELETE: api/UserStory/5
        //[ResponseType(typeof(UserStory))]
        //public IHttpActionResult DeleteUserStory(int id)
        //{
        //    UserStory userStory = db.UserStories.Find(id);
        //    if (userStory == null)
        //    {
        //        return NotFound();
        //    }

        //    db.UserStories.Remove(userStory);
        //    db.SaveChanges();

        //    return Ok(userStory);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool UserStoryExists(int id)
        {
            return service.GetAll().Count(e => e.Id == id) > 0;
        }
    }
}