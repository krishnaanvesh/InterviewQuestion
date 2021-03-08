using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public static class OrderProcess
    {

        public static void MainProcess()
        {
            ProductDetail detail = new ProductDetail { CustomerId = 23, CustomerName = "Anvash", ProductName = "Chess", Product_ID = "Product0010" };
            PaymentInfo info = new PaymentInfo { Amount = 234.22M, CreditCard = "22233322323232", CVV = "450", ExpMonth = 9, ExpYear = 2023, NameOfTheCard = "Anvash" };

            TakeOrder(detail, info);
        }

        private static bool TakeOrder(ProductDetail detail, PaymentInfo paymentInfo)
        {
            var inventoryResult = CheckInventory(detail.Product_ID, 1);
            if (inventoryResult)
            {
                var paymentResult = ChargePayment(paymentInfo, detail.CustomerId);
                SendEmail(detail, paymentInfo.Amount, paymentResult);
                return paymentResult;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckInventory(string productId, int qty)
        {
            return _products.Any(x => x.Product_ID == productId && x.Quantity > 0);
        }

        private static bool ChargePayment(PaymentInfo paymentInfo, int customerId)
        {
            //Validate Card
            if ((string.IsNullOrEmpty(paymentInfo.CreditCard) || paymentInfo.CreditCard.Length != 14))
                return false;

            //Validate Value of CVV,ExpMonth, ExpYear
            if ((string.IsNullOrEmpty(paymentInfo.CVV) || paymentInfo.CVV.Length != 3)
                || paymentInfo.ExpMonth == 0 || paymentInfo.ExpYear == 0)
                return false;
            //
            DateTime? expiryDate = null;
            try
            {
                var lastDate = DateTime.DaysInMonth(paymentInfo.ExpYear, paymentInfo.ExpMonth);

                expiryDate = new DateTime(paymentInfo.ExpYear, paymentInfo.ExpMonth, lastDate);
            }
            catch (Exception ex)
            {
                return false;
            }

            //Validate Expiry Date
            if (expiryDate < DateTime.Today)
                return false;


            return true;
        }

        private static bool SendEmail(ProductDetail detail, decimal amount, bool paymentStatus)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("Hi {0}", detail.CustomerName));

            if (paymentStatus)
            {
                builder.AppendLine(String.Format("Your Order {0} was confirmed", detail.ProductName));
                builder.AppendLine(String.Format("Price {0}", amount));
            }
            else
            {
                builder.AppendLine(String.Format("Your Order {0} was not confirmed, please try again...", detail.ProductName));
            }
            //return SendEmailService(builder);
            return true;
        }


        private static List<Product> _products = new List<Product> {
        new Product{ Product_ID = "Product0010", ProductName ="Chess", Quantity=6 },
        new Product{ Product_ID = "Product0011", ProductName ="Jeans", Quantity=5 },
        new Product{ Product_ID = "Product0012", ProductName ="T-Shirt", Quantity=0 },
        new Product{ Product_ID = "Product0013", ProductName ="Oppo F9 Pro", Quantity=0 },
        new Product{ Product_ID = "Product0014", ProductName ="Oppo F9", Quantity=2 },
        new Product{ Product_ID = "Product0015", ProductName ="Vivo V15", Quantity=3 },
        new Product{ Product_ID = "Product0016", ProductName ="Nokia", Quantity=8 },
        new Product{ Product_ID = "Product0017", ProductName ="Samsung", Quantity=1 },
        new Product{ Product_ID = "Product0018", ProductName ="Vivo V21", Quantity=10 },
        new Product{ Product_ID = "Product0019", ProductName ="Carrom", Quantity=0 },

        };

    }


    public class PaymentInfo
    {
        public string CreditCard { get; set; }
        public string CVV { get; set; }

        public string NameOfTheCard { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProductDetail
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Product_ID { get; set; }
        public string ProductName { get; set; }

    }



    public class Product
    {
        public string Product_ID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

    }
}
