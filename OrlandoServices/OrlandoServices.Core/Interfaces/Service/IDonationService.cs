using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Core.Interfaces.Service
{
    public interface IDonationService
    {
        DonationResponseDto CreateDonation(CreateDonationDto dto);
        void UpdateDonationStatus(int donationId, PaymentStatus newStatus);
        List<DonationResponseDto> GetDonationsByUser(int userId);
        List<DonationResponseDto> GetDonationsByStatus(PaymentStatus status);
        List<DonationResponseDto> GetAll();
    }
}
