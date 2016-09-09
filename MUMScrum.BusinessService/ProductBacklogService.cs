using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;
using MUMScrum.DataAccess;
namespace MUMScrum.BusinessService
{
    public class ProductBacklogService : IProductBacklogService
    {
        private IUnitOfWork unitOfWork = new UnitOfWork();
        public void CreateProduct(ProductBacklog productBacklog)
        {
            unitOfWork.ProductRepository.Insert(productBacklog);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<ProductBacklog> GetAllProductBackLogsByOwner(int OwnerId)
        {
            return unitOfWork.ProductRepository.Get(p => p.OwnerId == OwnerId);
        }

        public void DeleteProduct(int id)
        {
            var StartedSprints = unitOfWork.ProductRepository.GetByID(id).ReleaseBacklogs.Any(r => r.Sprints.Any(c => c.SprintRunning == true));
            if(StartedSprints)
                throw new Exception("Product can't be deleted ... there is started Sprints related to it!");

            var userstories = unitOfWork.UserStoryRepository.Get(i => i.ProductBackLogId == id);
            foreach (var userstory in userstories)
            {
                unitOfWork.UserStoryRepository.Delete(userstory);
            }

            var releases = unitOfWork.ReleaseBacklogsRepository.Get(i => i.ProductBacklogId == id);
            foreach(var release in releases)
            {
                var sprints = unitOfWork.SprintRepository.Get(i => i.ReleaseBacklogId == release.Id);
                foreach (var sprint in sprints)
                {                    
                    unitOfWork.SprintRepository.Delete(sprint);
                }
                unitOfWork.ReleaseBacklogsRepository.Delete(release);
            }
            unitOfWork.ProductRepository.Delete(id);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<ProductBacklog> GetAll()
        {
            return unitOfWork.ProductRepository.Get();
        }

        

        public ProductBacklog GetById(int productId)
        {
            return unitOfWork.ProductRepository.GetByID(productId);
        }

        public void UpdateProduct(ProductBacklog product)
        {
            unitOfWork.ProductRepository.Update(product);
            unitOfWork.SaveChanges();
        }
    }
}
