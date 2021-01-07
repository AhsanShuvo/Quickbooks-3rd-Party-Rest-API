namespace QuickbooksDAL.Interfaces
{
    public interface IPaymentRepository
    {
        void SavePaymentInfo(PaymentInfo model);
        PaymentInfo GetPaymentInfo(string id);
        void DeletePayment(string id);
    }
}
