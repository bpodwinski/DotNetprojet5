using System.Collections.Generic;
using System.Threading.Tasks;
using Express_Voitures.Models.Entities;

namespace Express_Voitures.Repositories
{
    public interface IPurchaseRepository
    {
        Task AddAsync(Purchase purchase);
    }
}