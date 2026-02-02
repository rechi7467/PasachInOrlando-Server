using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Interfaces.Repository
{
    public interface IPaymentRepository
    {
        public Payment? GetById(int id);
        public List<Payment> GetByOrderId(int orderId);
        public List<Payment> GetByStatus(PaymentStatus status);
        public void Add(Payment payment);
        public void Update(Payment payment);
    }
}
