using RouteDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RouteDelivery.Data.Implementations
{
    public class DeliveryScheduleRepository: IRepository<DeliverySchedule>
    {
        private EDM.RouteDelivery _dbContext;

        public DeliveryScheduleRepository(EDM.RouteDelivery dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.DeliverySchedules.Count() == 0)
            {
                AddRange(GetMockDeliveryScheduleData());
                _dbContext.SaveChanges();
            }
        }

        private List<DeliverySchedule> GetMockDeliveryScheduleData()
        {
            return new List<DeliverySchedule>() {
                new DeliverySchedule() { OptimizationRequestID = 1, CustomerID = 12001, DriverName = "Rag", PackageID = 1, TransportType = Models.Type.TransportType.Heavy, EstimatedTime = new DateTime(2016,01,01,10,0,0)}
            };
        }

        public void Add(DeliverySchedule newEntity)
        {
            _dbContext.DeliverySchedules.Add(newEntity);
        }

        public List<DeliverySchedule> Find(Func<DeliverySchedule, bool> match)
        {
            return _dbContext.DeliverySchedules.Where(match).ToList();
        }

        public List<DeliverySchedule> FindAll()
        {
            return _dbContext.DeliverySchedules.ToList();
        }

        public void Remove(DeliverySchedule entity)
        {
            _dbContext.DeliverySchedules.Remove(entity);
        }

        public void Update(DeliverySchedule entity)
        {
        }

        public DeliverySchedule FindByID(int id)
        {
            var results = _dbContext.DeliverySchedules.Where(d => d.ID == id);

            return results.FirstOrDefault();
        }

        public void AddRange(List<DeliverySchedule> scheduledDeliveries)
        {
            _dbContext.DeliverySchedules.AddRange(scheduledDeliveries);        
        }
    }
}