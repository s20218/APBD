using System;
using System.Threading.Tasks;
using Tutorial5.Models;

namespace Tutorial5.Services
{
    public interface IDatabaseService
    {
        Task<int> AddWarehouseAsync(Purchase newPurchase, int idOrder);

        Task<bool> ProductAndWarehouseExist(int idProduct, int idWarehouse);

        Task<int> GetOrderId(int idProduct, DateTime createdAt, int amount);

        Task<bool> ProductPurchaseExist(int idOrder);

        Task ExecuteProcudere(Purchase newPurchase);
    }

}
