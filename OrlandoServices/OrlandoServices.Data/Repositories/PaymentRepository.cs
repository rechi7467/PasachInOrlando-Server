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
    public class PaymentRepository:IPaymentRepository
    {
        private readonly DBContext _context;
        public PaymentRepository(DBContext context)
        {
            _context = context;
        }
        public Payment CreatePayment(Payment payment)
        {
            _context.Payment.Add(payment);
            _context.SaveChanges();
            return payment;
        }
        public List<Payment> GetPaymentsByOrderId(int orderId)
        {
            return _context.Payment.Where(p => p.OrderId == orderId).ToList();
        }
        public List<Payment> GetPaymentsByStatus(PaymentStatus status)
        {
            return _context.Payment.Where(p => p.Status == status).ToList();
        }
        public Payment GetById(int id)
        {
            var payment = _context.Payment.Find(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with id {id} not found");
            return payment;
        }
        public Payment UpdatePaymentStatus(int paymentId, PaymentStatus status)
        {
            var existingPayment = _context.Payment.Find(paymentId);
            if (existingPayment == null)
                throw new KeyNotFoundException($"Payment with id {paymentId} not found");
            existingPayment.Status = status;
            _context.SaveChanges();
            return existingPayment;
        }
    }
}
