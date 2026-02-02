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
        public List<Donation> GetByUserId(int userId);
        public List<Donation> GetByStatus(PaymentStatus status);
        public Donation? GetById(int id);
        public void Add(Donation donation);
        public void Update(Donation donation);
        public void Remove(Donation donation);
    }
}
