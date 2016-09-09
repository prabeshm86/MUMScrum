using System;
using MUMScrum.Model;
using MUMScrum.DataAccess.Repository;

namespace MUMScrum.DataAccess
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private MUMScrumDbContext context = new MUMScrumDbContext();
        private GenericRepository<Employee> employeeRepository;
        private GenericRepository<ProductBacklog> productRepository;

        private GenericRepository<Role> roleRepository;
        private GenericRepository<UserStory> userStoryRepository;
        //Sanjeev changes
        private GenericRepository<ReleaseBacklog> releaseBacklogRepository;
        private GenericRepository<Sprint> sprintRepository;
        private GenericRepository<UserStoryLog> userStoryLogRepository;


        

        public GenericRepository<UserStory> UserStoryRepository
        {
            get
            {
                if (userStoryRepository == null)
                {
                    userStoryRepository = new GenericRepository<UserStory>(context);
                }
                return userStoryRepository;
            }

            set
            {
                userStoryRepository = value;
            }
        }




        //Sanjeev
        

        public GenericRepository<Employee> EmployeeRepository
        {
            get
            {

                if (this.employeeRepository == null)
                {
                    this.employeeRepository = new GenericRepository<Employee>(context);
                }
                return employeeRepository;
            }
        }
        public GenericRepository<ProductBacklog> ProductRepository
        {
            get
            {

                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<ProductBacklog>(context);
                }
                return productRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {

                if (this.roleRepository == null)
                {
                    this.roleRepository = new GenericRepository<Role>(context);
                }
                return roleRepository;
            }
        }

        GenericRepository<ReleaseBacklog> IUnitOfWork.ReleaseBacklogsRepository
        {
            get
            {
                if (releaseBacklogRepository == null)
                    this.releaseBacklogRepository = new GenericRepository<ReleaseBacklog>(context);
                return this.releaseBacklogRepository;
            }
        }

        GenericRepository<Sprint> IUnitOfWork.SprintRepository
        {
            get
            {
                if (sprintRepository == null)
                    sprintRepository = new GenericRepository<Sprint>(context);
                return sprintRepository;
            }
        }

        public GenericRepository<UserStoryLog> UserStoryLogRepository
        {
            get
            {
                if (userStoryLogRepository == null)
                    userStoryLogRepository = new GenericRepository<UserStoryLog>(context);
                return userStoryLogRepository;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                int effected = context.SaveChanges();
                return effected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
