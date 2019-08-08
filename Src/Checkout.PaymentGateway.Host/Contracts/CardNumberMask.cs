using System;

namespace Checkout.PaymentGateway.Host.Contracts
{
    public class CardNumberObscurer
    {
        public  string GetFirstSixAndLastFour(string cardNumber)
        {
            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('X', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            return string.Concat(firstDigits, requiredMask, lastDigits);
        }
    }
}