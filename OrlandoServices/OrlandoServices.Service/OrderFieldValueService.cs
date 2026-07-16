using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;

namespace OrlandoServices.Service
{
    public class OrderFieldValueService : IOrderFieldValueService
    {
        private readonly IOrderFieldValueRepository _orderFieldValueRepository;
        private readonly IServiceFieldRepository _serviceFieldRepository;

        public OrderFieldValueService(IOrderFieldValueRepository orderFieldValueRepository, IServiceFieldRepository serviceFieldRepository)
        {
            _orderFieldValueRepository = orderFieldValueRepository;
            _serviceFieldRepository = serviceFieldRepository;
        }

        // שומר את ערכי השדות הדינמיים לפריט הזמנה — עם snapshot של שם וסוג השדה
        // - בודק שאין שדות כפולים (אותו ServiceFieldId פעמיים)
        // - מוודא שכל שדות החובה מולאו לפי הגדרת השירות ב-DB
        // - שומר FieldNameAtOrderTime + FieldTypeAtOrderTime: גם אם השדה ישתנה בעתיד — הנתון ישמר כפי שהיה בזמן ההזמנה
        // - שדה לא חובה שנשאר ריק — נדלג עליו ולא נשמר כלל
        public void CreateOrderFieldValues(int orderItemId, int serviceId, List<CreateOrderFieldValueDto> fieldValues)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("Invalid order item id");

            if (fieldValues == null || fieldValues.Count == 0)
                throw new ArgumentException("Field values are required");

            if (fieldValues.Select(v => v.ServiceFieldId).Distinct().Count() != fieldValues.Count)
                throw new ArgumentException("Duplicate field values are not allowed");

            var serviceFields = _serviceFieldRepository.GetByServiceId(serviceId)
                .Where(f => f.IsActive)
                .ToDictionary(f => f.Id);

            var requiredFieldIds = serviceFields.Values.Where(f => f.IsRequired).Select(f => f.Id).ToList();
            var sentFieldIds = fieldValues.Select(v => v.ServiceFieldId).ToList();

            if (requiredFieldIds.Except(sentFieldIds).Any())
                throw new ArgumentException("Missing required fields");

            var entities = new List<OrderFieldValue>(fieldValues.Count);

            foreach (var fieldValue in fieldValues)
            {
                if (!serviceFields.TryGetValue(fieldValue.ServiceFieldId, out var serviceField))
                    throw new InvalidOperationException("Field does not belong to this service");

                if (string.IsNullOrWhiteSpace(fieldValue.Value))
                {
                    if (serviceField.IsRequired)
                        throw new ArgumentException($"Value for field '{serviceField.FieldName}' is required");
                    continue;
                }

                entities.Add(new OrderFieldValue
                {
                    OrderItemId = orderItemId,
                    ServiceFieldId = fieldValue.ServiceFieldId,
                    Value = fieldValue.Value,
                    FieldTypeAtOrderTime = serviceField.FieldType,
                    FieldNameAtOrderTime = serviceField.FieldName
                });
            }

            _orderFieldValueRepository.AddRange(entities);
            _orderFieldValueRepository.SaveChanges();
        }

        public void UpdateOrderFieldValues(int orderItemId, int serviceId, Dictionary<int, string> fieldValues)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("Invalid order item id");

            if (fieldValues == null || fieldValues.Count == 0)
                throw new ArgumentException("Field values are required");

            var serviceFields = _serviceFieldRepository.GetByServiceId(serviceId).ToDictionary(f => f.Id);
            var existingValues = _orderFieldValueRepository.GetByOrderItemId(orderItemId).ToDictionary(v => v.ServiceFieldId);

            foreach (var kvp in fieldValues)
            {
                if (!serviceFields.TryGetValue(kvp.Key, out var serviceField))
                    throw new InvalidOperationException("Field does not belong to this service");

                if (string.IsNullOrWhiteSpace(kvp.Value))
                    throw new ArgumentException($"Value for field {kvp.Key} is required");

                if (existingValues.TryGetValue(kvp.Key, out var existing))
                {
                    existing.Value = kvp.Value;
                }
                else
                {
                    _orderFieldValueRepository.Add(new OrderFieldValue
                    {
                        OrderItemId = orderItemId,
                        ServiceFieldId = kvp.Key,
                        Value = kvp.Value,
                        FieldTypeAtOrderTime = serviceField.FieldType,
                        FieldNameAtOrderTime = serviceField.FieldName
                    });
                }
            }

            _orderFieldValueRepository.SaveChanges();
        }

        public List<OrderFieldValue> GetByOrderItemId(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentException("Invalid order item id");

            return _orderFieldValueRepository.GetByOrderItemId(orderItemId);
        }
    }
}
