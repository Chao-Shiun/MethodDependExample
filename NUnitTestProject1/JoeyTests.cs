using ExpectedObjects;
using NUnit.Framework;

namespace NUnitTestProject1
{
    [TestFixture]
    public class JoeyTests
    {
        [Test]
        public void get_order_detail_by_product_id()
        {
            var class1 = new MyFakeClass1();
            class1.ProductInfo = new ProductInfo()
            {
                Price = 100,
                ProductId = 91
            };

            var orderDetail = class1.ProcessOrderDetail("123", new PaymentDetail
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
    }

    class MyFakeClass1 : Class1
    {
        private ProductInfo _productInfo;

        public ProductInfo ProductInfo
        {
            set => _productInfo = value;
        }

        protected override ProductInfo GetProductInfo(int productId)
        {
            return _productInfo;
        }
    }
}