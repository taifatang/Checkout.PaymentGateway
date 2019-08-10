using System;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class CardDetailsMasker : ICardDetailsMasker
    {
        public CardDetails Mask(CardDetails cardDetails)
        {
            cardDetails.CardNumber = GetFirstSixAndLastFour(cardDetails.CardNumber);
            cardDetails.SecurityCode = "***";

            return cardDetails;
        }

        public string GetFirstSixAndLastFour(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return cardNumber;
            }

            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            return string.Concat(firstDigits, requiredMask, lastDigits);
        }
    }
}