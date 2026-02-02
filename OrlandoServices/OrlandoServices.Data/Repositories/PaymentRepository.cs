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
        public void Add(Payment payment)
        {
            _context.Payment.Add(payment);
        }
        public List<Payment> GetByOrderId(int orderId)
        {
            return _context.Payment.Where(p => p.OrderId == orderId).ToList();
        }
        public List<Payment> GetByStatus(PaymentStatus status)
        {
            return _context.Payment.Where(p => p.Status == status).ToList();
        }
        public Payment? GetById(int id)
        {
            return _context.Payment.Find(id);
        }
        public void Update(Payment payment)
        {
            _context.Payment.Update(payment);
        }
    }
}
