namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    public class RequisitionSerialNumber
    {
        public RequisitionSerialNumber()
        {
        }

        public RequisitionSerialNumber(
            int reqNumber,
            int lineNumber,
            int sequence,
            int serialNumber)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Sequence = sequence;
            this.SerialNumber = serialNumber;
        }

        public int ReqNumber { get; protected set; }

        public int LineNumber { get; protected set; }

        public int Sequence { get; protected set; }

        public int SerialNumber { get; protected set; }
    }
}
