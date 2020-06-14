using System;
using System.Collections.Generic;
using ExpectedObjects;
using NUnit.Framework;

namespace NUnitTestProject1
{
    [TestFixture]
    public class JoeyTests
    {
        private MyFakeClass1 _class1;

        [SetUp]
        public void SetUp()
        {
            _class1 = new MyFakeClass1();
        }

        [Test]
        public void get_order_detail_by_product_id()
        {
            _class1.AddProduct(new ProductInfo()
            {
                Price = 100,
                ProductId = 91
            });

            var orderDetail = _class1.ProcessOrderDetail("123", new PaymentDetail
            {
                Count = 10,
                ProdId = 91
            });

            var expected = new OrderDetail
            {
                Amount = 1000,
                OrderId = "123",
                ProductId = 91
            };

            expected.ToExpectedObject().ShouldMatch(orderDetail);
        }

        [Test]
        public void get_order_with_amount_and_order_details()
        {
            var class1 = new MyFakeClass1();
            class1.OrderId = "5487";

            class1.AddProduct(new ProductInfo()
            {
                Price = 100,
                ProductId = 91
            });

            class1.AddProduct(new ProductInfo()
            {
                Price = 200,
                ProductId = 92
            });

            var (order, orderDetailList) = class1.ProcessOrder(new PaymentInfo
            {
                UserId = 999,
                Detail = new List<PaymentDetail>
                {
                    new PaymentDetail {Count = 10, ProdId = 91},
                    new PaymentDetail {Count = 20, ProdId = 92},
                }
            });

            var expectedOrder = new Order() {OrderAmount = 5000, OrderId = "5487", UserId = 999};

            expectedOrder.ToExpectedObject().ShouldMatch(order);

            var expectedOrderDetails = new List<OrderDetail>
            {
                new OrderDetail() {Amount = 1000, OrderId = "5487", ProductId = 91},
                new OrderDetail() {Amount = 4000, OrderId = "5487", ProductId = 92},
            };
            expectedOrderDetails.ToExpectedObject().ShouldMatch(orderDetailList);
        }
    }

    class MyFakeClass1 : Class1
    {
        private readonly List<ProductInfo> _products = new List<ProductInfo>();
        private int _counter;
        private string _orderId;

        public string OrderId
        {
            set => _orderId = value;
        }

        public void AddProduct(ProductInfo productInfo)
        {
            _products.Add(productInfo);
        }

        protected override string CreateOrderId()
        {
            return _orderId;
        }

        protected override ProductInfo GetProductInfo(int productId)
        {
            var product = _products[_counter];
            Console.WriteLine($"product index:{_counter}");
            _counter++;
            return product;
        }
    }
}