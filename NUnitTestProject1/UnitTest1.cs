using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace NUnitTestProject1
{
    [TestFixture]
    public class Tests
    {
        public IClass1 FakeClass1 { get; set; }

        [SetUp]
        public void Setup()
        {
            IClass1 fakeClass1 = Substitute.For<Class1>();
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 1 && x.Count == 2)).Returns(new OrderDetail
            {
                Amount = 246,
                ProductId = 1
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 2 && x.Count == 3)).Returns(new OrderDetail
            {
                Amount = 1902,
                ProductId = 2
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 4 && x.Count == 1)).Returns(new OrderDetail
            {
                Amount = 150,
                ProductId = 4
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 1 && x.Count == 5)).Returns(new OrderDetail
            {
                Amount = 615,
                ProductId = 1
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 2 && x.Count == 4)).Returns(new OrderDetail
            {
                Amount = 2536,
                ProductId = 2
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 3 && x.Count == 7)).Returns(new OrderDetail
            {
                Amount = 1400,
                ProductId = 3
            });
            fakeClass1.ProcessOrderDetail(
                Arg.Any<string>(),
                Arg.Is<PaymentDetail>(x => x.ProdId == 4 && x.Count == 3)).Returns(new OrderDetail
            {
                Amount = 450,
                ProductId = 4
            });
            FakeClass1 = fakeClass1;
        }

        [Test, TestCaseSource(nameof(TestCase))]
        public void TestProcessOrder(PaymentInfo source, (Order, List<OrderDetail>) expectResult)
        {
            //IClass1 class1 = new Class1();
            IClass1 class1=new Class1ForTest(FakeClass1);
            var actual = class1.ProcessOrder(source);
            Assert.AreEqual(expectResult.Item1.UserId, actual.order.UserId);
            Assert.AreEqual(expectResult.Item1.OrderAmount, actual.order.OrderAmount);
            Assert.AreEqual(expectResult.Item2.Count, actual.orderDetailList.Count);
            for (var i = 0; i < actual.orderDetailList.Count; i++)
            {
                Assert.AreEqual(expectResult.Item2[i].Amount, actual.orderDetailList[i].Amount);
            }
        }

        static readonly object[] TestCase =
        {
            new object[] { new PaymentInfo
                {
                    UserId = 123,
                    Detail = new List<PaymentDetail>
                    {
                        new PaymentDetail
                        {
                            ProdId = 1,
                            Count = 2
                        },
                        new PaymentDetail
                        {
                            ProdId = 2,
                            Count = 3
                        },
                        new PaymentDetail
                        {
                            ProdId = 4,
                            Count = 1
                        }
                    }
                },(
                new Order
                {
                    UserId = 123,
                    OrderAmount = 2298M
                },
                new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        ProductId = 1,
                        Amount = 246M
                    },
                    new OrderDetail
                    {
                        ProductId = 2,
                        Amount = 1902M
                    },
                    new OrderDetail
                    {
                        ProductId = 4,
                        Amount = 150M
                    },
                })
            },
            new object[] { new PaymentInfo
                {
                    UserId = 678,
                    Detail = new List<PaymentDetail>
                    {
                        new PaymentDetail
                        {
                            ProdId = 1,
                            Count = 5
                        },
                        new PaymentDetail
                        {
                            ProdId = 2,
                            Count = 4
                        },
                        new PaymentDetail
                        {
                            ProdId = 3,
                            Count = 7
                        },
                        new PaymentDetail
                        {
                            ProdId = 4,
                            Count = 3
                        }
                    }
                },(
                    new Order
                    {
                        UserId = 678,
                        OrderAmount = 5001M
                    },
                    new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            ProductId = 1,
                            Amount = 615M
                        },
                        new OrderDetail
                        {
                            ProductId = 2,
                            Amount = 2536M
                        },
                        new OrderDetail
                        {
                            ProductId = 3,
                            Amount = 1400M
                        },
                        new OrderDetail
                        {
                            ProductId = 3,
                            Amount = 450M
                        },
                    })
            }
        };
    }

    public partial class Class1ForTest : Class1
    {
        public IClass1 FakeClass1 { get; set; }
        public Class1ForTest(IClass1 fakeClass1)
        {
            FakeClass1 = fakeClass1;
        }

        public override OrderDetail ProcessOrderDetail(string orderId, PaymentDetail paymentDetailInfo)
        {
            return FakeClass1.ProcessOrderDetail(orderId, paymentDetailInfo);
        }
    }
}