namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    public interface ICalcLabourHoursProxy
    {
        Task CalcLabourTimes(bool newOnly = false);
    }
}
