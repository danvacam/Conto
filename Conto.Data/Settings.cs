namespace Conto.Data
{
    public class Settings
    {
        public string InvoiceOwnerName
        {
            get;
            set;
        }

        public string InvoiceOwnerAddress
        {
            get;
            set;
        }

        public string InvoiceOwnerCity
        {
            get;
            set;
        }

        public string InvoiceOwnerPostalCode
        {
            get;
            set;
        }

        public string InvoiceOwnerFiscalCode
        {
            get;
            set;
        }

        public string InvoiceOwnerVatCode
        {
            get;
            set;
        }

        public decimal? MaxInvoiceValue
        {
            get;
            set;
        }
    }
}