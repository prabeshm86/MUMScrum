using MUMScrum.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MUMScrum.DataAccess
{
    public class ScrumDBInitializer : DropCreateDatabaseIfModelChanges<MUMScrumDbContext>
    {
        protected override void Seed(MUMScrumDbContext context)
        {
            context.Roles.Add(new Model.Role() { RoleName = "HR" });
            context.Roles.Add(new Model.Role() { RoleName = "ScrumMaster" });
            context.Roles.Add(new Model.Role() { RoleName = "ProductOwner" });
            context.Roles.Add(new Model.Role() { RoleName = "Developer" });
            context.Roles.Add(new Model.Role() { RoleName = "Tester" });
            context.SaveChanges();

            context.Employees.Add(new Model.Employee
            {
                FirstName = "Super",
                LastName = "Admin",
                UserName = "admin",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.HR
            });

            var po = new Model.Employee
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "productowner",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.ProductOwner,
            };
            context.Employees.Add(po);

            var po1 = new Model.Employee
            {
                FirstName = "Rabin",
                LastName = "Doe",
                UserName = "productowner1",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.ProductOwner,
            };
            context.Employees.Add(po1);


            context.SaveChanges();

            context.Employees.Add(new Model.Employee
            {
                FirstName = "Grace",
                LastName = "Monroe",
                UserName = "dev",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.Developer,
            });

            context.Employees.Add(new Model.Employee
            {
                FirstName = "David",
                LastName = "Fake",
                UserName = "tester",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.Tester,
            });

            context.Employees.Add(new Model.Employee
            {
                FirstName = "Jane",
                LastName = "Roe",
                UserName = "smaster",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.ScrumMaster,
            });

            context.Employees.Add(new Model.Employee
            {
                FirstName = "Sanjeev",
                LastName = "Rajbanshi",
                UserName = "smaster",
                Password = "1234",
                IsDeactivated = false,
                RoleId = (int)RoleEnum.ScrumMaster,
            });

            context.SaveChanges();


            for (int i = 1; i <= 1; i++)
            {
                var newProduct = new Model.ProductBacklog()
                {
                    Name = "MUMScrum ",
                    Description = "Our MUMScrum - " + i + " Backlog",
                    OwnerId = po.Id
                };
                context.ProductBacklogs.Add(newProduct);
            }
            context.SaveChanges();
            for (int i = 1; i <= 1; i++)
            {
                //var skip = (i - 1) % 5;
                var product = context.ProductBacklogs.FirstOrDefault();
                var newRelease = new ReleaseBacklog()
                {
                    ReleaseDate = DateTime.Now.Date.AddDays(20),
                    Description = "this release is for the final project presentation of Software Engineering block. Hope it goes all well",
                    ReleaseName = "Release V" + i
                };
                //newRelease.ReleaseDate = DateTime.Now;                
                newRelease.ProductBacklogId = product.Id;
                newRelease.ScrumMasterId = 6;
                context.ReleaseBacklogs.Add(newRelease);
            }
            context.SaveChanges();

            for (int i = 1; i <= 2; i++)
            {
                var release = context.ReleaseBacklogs.FirstOrDefault();
                var sprint = new Sprint();
                sprint.ReleaseBacklogId = release.Id;
                sprint.Description = "This is new Sprint for this release - " + release.ReleaseName;
                sprint.SprintRunning = false;
                if (i == 1)
                {
                    sprint.StartDate = DateTime.Now.AddDays(-5);
                    sprint.EndDate = DateTime.Now;
                }
                else
                {
                    sprint.StartDate = DateTime.Now;
                    sprint.EndDate = DateTime.Now.Date.AddDays(5);
                }

                sprint.SprintName = "Sprint - " + i;
                context.Sprints.Add(sprint);
            }
            context.SaveChanges();

            for (int i = 1; i <= 3; i++)
            {
                var product = context.ProductBacklogs.FirstOrDefault();
                //var pcount = (i - 1) % product.ReleaseBacklogs.Count();
                var release = product.ReleaseBacklogs.OrderBy(j => j.Id).FirstOrDefault();
                //var rcount = (i - 1) % context.Sprints.Where(j => j.ReleaseBacklogId == release.Id).Count();
                var sprint = context.Sprints.Where(j => j.ReleaseBacklogId == release.Id).OrderBy(j => j.Id).FirstOrDefault();
                var US = new UserStory()
                {
                    Title = "UStory - " + i,
                    Description = "This is a " + i + " description",
                    SprintId = sprint.Id,
                    ReleaseId = release.Id,
                    DevelopedId = context.Employees.First(d => d.RoleId == (int)RoleEnum.Developer).Id,
                    TesterId = context.Employees.First(d => d.RoleId == (int)RoleEnum.Tester).Id,
                    ProductBackLogId = product.Id,
                    DevActual = 0,
                    TestActual = 0,
                    DevEstimate = 30,
                    TestEstiamte = 0

                };
                context.UserStories.Add(US);
            }
            context.SaveChanges();

            var sprint1 = context.Sprints.FirstOrDefault();
            foreach (var us in sprint1.UserStories.ToList())
            {
                var currentdevActual = us.DevActual.Value;
                var currentdevEstimate = us.DevEstimate.Value;
                for (int i = 1; i < 5; i++)
                {

                    var usLog = new UserStoryLog()
                    {
                        ChangedByUserId = us.DevelopedId.Value,
                        Date = sprint1.StartDate.AddDays(i),
                        LogType = LogType.DevActual,
                        NewValue = 5,
                        OldValue = currentdevActual,
                        UserStoryId = us.Id
                    };
                    us.DevActual += 5;
                    context.UserStoryLogs.Add(usLog);
                }                
            }
            context.SaveChanges();
        }

    }
}
