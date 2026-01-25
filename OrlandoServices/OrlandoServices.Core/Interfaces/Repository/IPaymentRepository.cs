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
        public Payment GetById(int id);
        public List<Payment> GetPaymentsByOrderId(int orderId);
        public List<Payment> GetPaymentsByStatus(PaymentStatus status);
        public Payment CreatePayment(Payment payment);
        public Payment UpdatePaymentStatus(int paymentId, PaymentStatus status);
    }
}
