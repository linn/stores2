namespace Linn.Stores2.Domain.LinnApps.External
{
    public interface IStoragePlaceAuditPack
    {
        string CreateAuditReq(string auditLocation, int createdBy, string department);
    }
}
