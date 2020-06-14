using System;
using System.Collections.Generic;
using ExpectedObjects;
using NUnit.Framework;

namespace NUnitTestProject1
{
    [TestFixture]
    public class JoeyTests
    {
        private const string DefaultOrderId = "5487";
        private const int DefaultUserId = 999;
        private MyFakeClass1 _class1;

        [SetUp]
        public void SetUp()
        {
            _class1 = new MyFakeClass1();
            _class1.OrderId = DefaultOrderId;
        }

        [Test]
        public void get_order_detail_by_product_id()
        {
            GivenProductInfos(new ProductInfo()
            {
                Price = 100,
                ProductId = 91
            });

            var orderDetail = _class1.ProcessOrderDetail(DefaultOrderId, new PaymentDetail
            {
                Count = 10,
                ProdId = 91
            });

            OrderDetailShouldBe(new OrderDetail
            {
                Amount = 1000,
                OrderId = DefaultOrderId,
                ProductId = 91
            }, orderDetail);
        }

        [Test]
        public void get_order_with_amount_and_order_details()
        {
            GivenProductInfos(
                CreateProductInfo(100, 91),
                CreateProductInfo(200, 92));

            var (order, orderDetailList) = _class1.ProcessOrder(new PaymentInfo
            {
                UserId = DefaultUserId,
                Detail = new List<PaymentDetail>
                {
                    new PaymentDetail {Count = 10, ProdId = 91},
                    new PaymentDetail {Count = 20, ProdId = 92},
                }
            });

            OrderShouldBe(new Order
            {
                OrderAmount = 5000,
                OrderId = DefaultOrderId,
                UserId = DefaultUserId
            }, order);

            OrderDetailsShouldBe(new List<OrderDetail>
            {
                new OrderDetail() {Amount = 1000, OrderId = DefaultOrderId, ProductId = 91},
                new OrderDetail() {Amount = 4000, OrderId = DefaultOrderId, ProductId = 92},
            }, orderDetailList);
        }

        private static ProductInfo CreateProductInfo(int price, int productId)
        {
            return new ProductInfo()
            {
                Price = price,
                ProductId = productId
            };
        }

        private static void OrderDetailShouldBe(OrderDetail expected, OrderDetail orderDetail)
        {
            expected.ToExpectedObject().ShouldMatch(orderDetail);
        }

        private static void OrderDetailsShouldBe(List<OrderDetail> expected, List<OrderDetail> orderDetailList)
        {
            expected.ToExpectedObject().ShouldMatch(orderDetailList);
        }

        private static void OrderShouldBe(Order expectedOrder, Order order)
        {
            expectedOrder.ToExpectedObject().ShouldMatch(order);
        }

        private void GivenProductInfos(params ProductInfo[] productInfos)
        {
            foreach (var productInfo in productInfos)
            {
                _class1.AddProduct(productInfo);
            }
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