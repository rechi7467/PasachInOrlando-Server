using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Service
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public DonationService(
            IDonationRepository donationRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository)
        {
            _donationRepository = donationRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        private static DonationResponseDto ToResponseDto(Donation d) => new DonationResponseDto
        {
            Id = d.Id,
            Amount = d.Amount,
            Status = d.Status,
            CreatedAt = d.CreatedAt,
            UserId = d.UserId,
            OrderId = d.OrderId,
            GuestName = d.GuestName,
            GuestEmail = d.GuestEmail
        };

        public DonationResponseDto CreateDonation(CreateDonationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Amount <= 0)
                throw new ArgumentException("Donation amount must be greater than zero");

            bool isGuest = dto.UserId == null;

            if (isGuest && string.IsNullOrWhiteSpace(dto.GuestName))
                throw new ArgumentException("Guest name is required for anonymous donations");

            if (!isGuest)
            {
                var user = _userRepository.GetById(dto.UserId!.Value);
                if (user == null || !user.IsActive)
                    throw new ArgumentException("Invalid user");
            }

            if (dto.OrderId.HasValue)
            {
                var order = _orderRepository.GetById(dto.OrderId.Value);
                if (order == null)
                    throw new NotFoundException($"Order {dto.OrderId.Value} not found");
            }

            var donation = new Donation
            {
                Amount = dto.Amount,
                UserId = dto.UserId,
                OrderId = dto.OrderId,
                GuestName = dto.GuestName,
                GuestEmail = dto.GuestEmail,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _donationRepository.Add(donation);
            _donationRepository.SaveChanges();

            return ToResponseDto(donation);
        }

        public void UpdateDonationStatus(int donationId, PaymentStatus newStatus)
        {
            var donation = _donationRepository.GetById(donationId);
            if (donation == null)
                throw new NotFoundException($"Donation {donationId} not found");

            if (donation.Status == PaymentStatus.Success)
                throw new InvalidOperationException("Cannot change status of a completed donation");

            donation.Status = newStatus;
            _donationRepository.Update(donation);
            _donationRepository.SaveChanges();
        }

        public List<DonationResponseDto> GetDonationsByUser(int userId) =>
            _donationRepository.GetByUserId(userId).Select(ToResponseDto).ToList();

        public List<DonationResponseDto> GetDonationsByStatus(PaymentStatus status) =>
            _donationRepository.GetByStatus(status).Select(ToResponseDto).ToList();
    }
}
