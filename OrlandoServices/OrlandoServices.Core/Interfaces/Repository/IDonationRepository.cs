using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IDonationRepository
    {
        public List<Donation> GetAllDonations();
        public List<Donation> GetDonationsByUserId(int userId);
        public List<Donation> GetDonationsByStatus(PaymentStatus status);
        public Donation GetById(int id);
        public Donation CreateDonation(Donation donation);
        public Donation UpdateDonationStatus(PaymentStatus status,int id);
        public void DeleteDonation(int id);
    }
}
