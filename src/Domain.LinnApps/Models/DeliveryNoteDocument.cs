namespace Linn.Stores2.Domain.LinnApps.Models
{
    using System;
    using System.Collections.Generic;

    public class DeliveryNoteDocument
    {
        public int DocumentNumber { get; set; }

        public DateTime DocumentDate { get; set; }

        public string AccountReference { get; set; }

        public string AddressOfIssuer { get; set; }

        public string RegisteredOffice { get; set; }

        public int DeliveryAddressId { get; set; }

        public string DeliveryAddress { get; set; }

        public string TransReference { get; set; }

        public string Comments { get; set; }

        public IList<DeliveryNoteLine> Lines { get; set; }
    }
}
