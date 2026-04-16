namespace Linn.Stores2.Facade.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    using Linn.Stores2.Domain.LinnApps.Imports;

    using Linn.Stores2.Resources.Imports;

    public static class ImportBookSearchResourceExtensions
    {
        public static Expression<Func<ImportBook, bool>> ToExpression(this ImportBookSearchResource searchResource)
        {
            Expression<Func<ImportBook, bool>> filter = ib => true;

            // build expression
            if (!string.IsNullOrEmpty(searchResource.TransportBillNumber))
            {
                filter = filter.And(ib => ib.TransportBillNumber == searchResource.TransportBillNumber);
            }

            if (!string.IsNullOrEmpty(searchResource.CustomsEntryCode))
            {
                filter = filter.And(ib => ib.CustomsEntryCode == searchResource.CustomsEntryCode);
            }

            if (searchResource.RsnNumber.HasValue)
            {
                filter = filter.And(ib => ib.OrderDetails.Any(od => od.Rsn != null && od.Rsn.RsnNumber == searchResource.RsnNumber.Value));
            }

            if (searchResource.PONumber.HasValue)
            {
                filter = filter.And(ib => ib.OrderDetails.Any(od => od.OrderNumber == searchResource.PONumber.Value));
            }

            if (searchResource.Status == "Cancelled")
            {
                filter = filter.And(ib => ib.DateCancelled != null);
            }
            else if (searchResource.Status == "Received")
            {
                filter = filter.And(ib => ib.DateReceived != null);
            }
            else if (searchResource.Status == "Cleared")
            {
                filter = filter.And(ib => ib.DateReceived == null && ib.CustomsEntryCodeDate != null);
            }
            else if (searchResource.Status == "Instruction Sent")
            {
                filter = filter.And(ib => ib.DateInstructionSent != null && ib.CustomsEntryCodeDate == null);
            }
            else if (searchResource.Status == "Raised")
            {
                filter = filter.And(ib => ib.CustomsEntryCodeDate == null && ib.DateInstructionSent == null);
            }

            if (!string.IsNullOrEmpty(searchResource.DateField))
            {
                DateTime? fromDate = string.IsNullOrEmpty(searchResource.FromDate)
                    ? null
                    : DateTime.Parse(searchResource.FromDate).Date;
                DateTime? toDate = string.IsNullOrEmpty(searchResource.ToDate)
                    ? null
                    : DateTime.Parse(searchResource.ToDate).Date.AddDays(1).AddTicks(-1);

                if (searchResource.DateField == "Date Created")
                {
                    if (fromDate != null)
                    {
                        filter = filter.And(ib => ib.DateCreated >= fromDate);
                    }

                    if (toDate != null)
                    {
                        filter = filter.And(ib => ib.DateCreated <= toDate);
                    }
                }
                else if (searchResource.DateField == "Date Received")
                {
                    if (fromDate != null)
                    {
                        filter = filter.And(ib => ib.DateReceived >= fromDate);
                    }

                    if (toDate != null)
                    {
                        filter = filter.And(ib => ib.DateReceived <= toDate);
                    }
                }
                else if (searchResource.DateField == "Customs Date")
                {
                    if (fromDate != null)
                    {
                        filter = filter.And(ib => ib.CustomsEntryCodeDate >= fromDate);
                    }

                    if (toDate != null)
                    {
                        filter = filter.And(ib => ib.CustomsEntryCodeDate <= toDate);
                    }
                }
            }

            return filter;
        }
    }
}
