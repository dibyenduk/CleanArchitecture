using MediatR;
using MES.Core.Features.Inventory;
using MES.Core.Features.ViewProcessOrder;
using MES.Core.Services.Inventory;
using MES.Core.Services.Inventory.Models;
using MES.Core.Tests.Services;
using MES.Entities;
using MES.Persistence;
using MES.SAP.Services.Inventory;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Tests
{
    [TestFixture]
    public class ProductionReporterTests
    {
        private DbConnection connection;

        public ProductionReporterTests()
        {
            connection = Effort.DbConnectionFactory.CreateTransient();            
        }

        [SetUp]
        public void Setup()
        {
            // Setup Process Orders
            using (var dbContext = new MESDbContext(connection))
            {
                var list = new List<ProcessOrder>();

                list.Add(new ProcessOrder { Id = 1, Nbr = "000101763653", CreatedBy = "XYZ", ModifiedBy = "XYZ", CreatedDateTime = DateTime.Now, ModifiedDateTime = DateTime.Now });
                list.Add(new ProcessOrder { Id = 2, Nbr = "000101763654", CreatedBy = "XYZ", ModifiedBy = "XYZ", CreatedDateTime = DateTime.Now, ModifiedDateTime = DateTime.Now });
                list.Add(new ProcessOrder { Id = 3, Nbr = "000101763655", CreatedBy = "XYZ", ModifiedBy = "XYZ", CreatedDateTime = DateTime.Now, ModifiedDateTime = DateTime.Now });

                dbContext.ProcessOrders.AddRange(list);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void SearchProcessOrders()
        {
            SearchResult result;

            // Connection to transient in memory data store
            using (var dbContext = new MESDbContext(connection))
            {  
                var container = new Container(cfg =>
                {
                    cfg.Scan(scanner =>
                    {
                        scanner.WithDefaultConventions();
                        scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                        scanner.AssemblyContainingType(typeof(SearchQuery));                        
                    });
                    
                    cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => t => ctx.GetInstance(t));
                    cfg.For<IMediator>().Use<Mediator>();

                    // In Memory Db Context
                    cfg.For<MESDbContext>().Use(dbContext);                    
                });

                var mediator = container.GetInstance<IMediator>();

                var response = mediator.Send(new SearchQuery() { Id = 2 }).Result;

                result = response.Results != null ? response.Results.FirstOrDefault() : null;
            }

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Nbr, "000101763654");
        }


        [Test]
        public void GetMaterialInventory()
        {
            // Not Actual inventory service. Replaced with test service.
            var testInventoryService = new TestInventoryService(() =>
                    new[] { new InventoryDetails { MaterialNbr = "1111", BatchNbr = "B001", Quantity = 10, QuantityUOM = "KG" } });

            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                    {
                        scanner.WithDefaultConventions();
                        scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                        scanner.AssemblyContainingType(typeof(SearchQuery));
                    });

                cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<IMediator>().Use<Mediator>();
            
                // Test Inventory Service
                cfg.For<IInventoryService>().Use(testInventoryService);
            });

            var mediator = container.GetInstance<IMediator>();

            var response = mediator.Send(new GetDetailsQuery() { MaterialNbr = "1111" }).Result;

            var result = response.Results != null ? response.Results.FirstOrDefault() : null;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MaterialNbr, "1111");
            Assert.AreEqual(result.BatchNbr, "B001");
            Assert.AreEqual(result.Quantity, 10);
            Assert.AreEqual(result.QuantityUOM, "KG");
        }
    }
}
