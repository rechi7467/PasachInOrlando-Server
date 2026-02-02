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
        public void Add(Donation donation)
        {
            _context.Donation.Add(donation);
        }
        public void Remove(Donation donation)
        {
            _context.Donation.Remove(donation);
        }
        public Donation? GetById(int id)
        {
            return _context.Donation.FirstOrDefault(o => o.Id == id);
        }
        public void Update(Donation donation)
        {
            _context.Donation.Update(donation);
        }
        public List<Donation> GetByStatus(PaymentStatus status)
        {
            return _context.Donation.Where(d => d.Status == status).ToList();
        }
        public List<Donation> GetByUserId(int userId)
        {
            return _context.Donation.Where(d => d.UserId == userId).ToList();
        }
    }
}
