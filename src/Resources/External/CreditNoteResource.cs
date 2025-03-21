﻿namespace Linn.Stores2.Resources.External
{
    using System.Collections.Generic;

    public class CreditNoteResource
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public string DocumentDate { get; set; }

        public int AccountId { get; set; }

        public IEnumerable<CreditNoteDetailResource> Details { get; set; }
    }
}
