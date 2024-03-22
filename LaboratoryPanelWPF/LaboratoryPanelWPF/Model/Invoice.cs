using DiagnosticServicesModel.Request;

namespace LaboratoryPanelWPF.Model
{
    public class Invoice
    {
        public AppointmentRequest AppointmentRequest { get; }

        public float TotalPrice { get; }

        public float Discount { get; }

        public float PriceAfterDiscount { get; }

        public Invoice(AppointmentRequest appointmentRequest, float totalPrice, float discount, float priceAfterDiscount)
        {
            AppointmentRequest = appointmentRequest;
            TotalPrice = totalPrice;
            Discount = discount;
            PriceAfterDiscount = priceAfterDiscount;
        }

    }
}