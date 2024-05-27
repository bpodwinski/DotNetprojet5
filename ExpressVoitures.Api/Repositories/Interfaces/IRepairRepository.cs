using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Repositories.Interfaces
{
    public interface IRepairRepository
    {
        Task AddAsync(Repair repair);
    }
}