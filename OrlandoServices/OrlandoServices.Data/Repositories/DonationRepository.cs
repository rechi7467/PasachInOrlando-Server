using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Data.Repositories
{
    public class DonationRepository : IDonationRepository
    {
        // Implementation will go here
        private readonly DBContext _context;
        public DonationRepository(DBContext context)
        {
            _context = context;
        }
        public Donation CreateDonation(Donation donation)
        {
            _context.Donation.Add(donation);
            _context.SaveChanges();
            return donation;
        }
        public void DeleteDonation(int id)
        {
            var donation = _context.Donation.Find(id);
            if (donation == null)
                throw new KeyNotFoundException($"Donation with id {id} not found");
            _context.Donation.Remove(donation);
            _context.SaveChanges();
        }
        public List<Donation> GetAllDonations()
        {
            return _context.Donation.ToList();
        }
        public Donation GetById(int id)
        {
            var donation = _context.Donation.Find(id);
            if (donation == null)
                throw new KeyNotFoundException($"Donation with id {id} not found");
            return donation;
        }
        public Donation UpdateDonationStatus(PaymentStatus status, int id)
        {
            var existingDonation = _context.Donation.Find(id);
            if (existingDonation == null)
                throw new KeyNotFoundException($"Donation with id {id} not found");
           existingDonation.Status = status;
            _context.SaveChanges();
            return existingDonation;
        }
        public List<Donation> GetDonationsByStatus(PaymentStatus status)
        {
            return _context.Donation.Where(d => d.Status == status).ToList();
        }
        public List<Donation> GetDonationsByUserId(int userId)
        {
            return _context.Donation.Where(d => d.UserId == userId).ToList();
        }
    }
}
