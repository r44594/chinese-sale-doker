using server_api.Dtos;
using server_api.Models;

namespace server_api.Interfaces
{
    public interface IRandomService
    {
        Task<List<RandomDto>> DrawLottery();
        Task<List<RandomDto>> GetWinnersReportAsync();
        Task<RandomIncomeDto> GetTotalIncomeAsync();
    }

}
