using server_api.Dtos;
using server_api.Models;

namespace server_api.Interfaces
{
    public interface IDonorServices
    {
        Task<GetDonor> GetByDonorName(string donorName);
        Task<List<GetDonor>> GetByDonorEmail(string email);
        Task<List<GetDonor>> GetByDonorGift(string giftName);

        Task<GetDonor> GetDonorById(int id);
        Task AddDonor(DonorDto.CreatUpdateDonor dto);
        Task<List<GetDonor>> GetAllDonors();
        Task DeleteDonor(int id);
        Task<Donor> UpdateDonor(int id, DonorDto.CreatUpdateDonor dto);

    }
}
