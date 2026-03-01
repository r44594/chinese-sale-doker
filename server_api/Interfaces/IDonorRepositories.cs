using server_api.Models;

namespace server_api.Interfaces
{
    public interface IDonorRepositories
    {
        Task<List<Donor>> GetByDonorEmail(string email);
        Task<List<Donor>> GetByDonorGift(string giftName);
        Task<List<Donor>> GetAllDonors();
        Task<Donor?> GetByDonorName(string name);
        Task<Donor?> GetDonorById(int id);
        Task AddDonor(Donor donor);

        Task DeleteDonor(Donor donor);
        Task<Donor> UpdateDonor(Donor donor);
    }
}
