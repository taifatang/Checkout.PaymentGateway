# Checkout.PaymentGateway

Checkout.PaymentGateway is responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from the acquiring bank

## Main Deliverable

1. CPG-1 - Process Payment
2. CPG-2 - Get Payment
3. CPG-3 - Application Log
4. CPG-4 - Monitor Payment


## Future

### Next

1. CPG-5 - Encryption - Decorate IRepository<T> with EncryptionRepository<T> and consume a physical key living on the VM to encrypt and decrypt data saved onto the machine
2. CPG-6 - Authentication - I probably would assert MerchantAccountId against an ApiKey 
3. E2E acceptance tests

### Improvement

1. Idempotency
2. Duplicate requests
