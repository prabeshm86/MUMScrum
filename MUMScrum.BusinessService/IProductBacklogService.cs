using MUMScrum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.BusinessService
{
    public interface IProductBacklogService
    {
        void CreateProduct(ProductBacklog product);
        IEnumerable<ProductBacklog> GetAll();
        ProductBacklog GetById(int productId);
        void UpdateProduct(ProductBacklog product);
        void DeleteProduct(int id);
        IEnumerable<ProductBacklog> GetAllProductBackLogsByOwner(int OwnerId);
    }
}
