using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;
using System.Data.Entity;

namespace MUMScrum.DataAccess
{
    public class MUMScrumDbContext : DbContext
    {
        //public DbSet<Employee> Employees { get; set; }
        public MUMScrumDbContext() : base("DefaultConnection")
        {
        }

        public static MUMScrumDbContext Create()
        {
            return new MUMScrumDbContext();
        }

        public DbSet<ProductBacklog> ProductBacklogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ReleaseBacklog> ReleaseBacklogs { get; set; }
        //public DbSet<ProductBacklog> ProductBacklogs { get; set; }
        public DbSet<Sprint> Sprints { get; set; }

        public System.Data.Entity.DbSet<MUMScrum.Model.UserStory> UserStories { get; set; }
        public System.Data.Entity.DbSet<MUMScrum.Model.UserStoryLog> UserStoryLogs { get; set; }

    }
}
