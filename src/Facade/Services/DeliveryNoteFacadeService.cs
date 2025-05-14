namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;
    using Linn.Common.Rendering;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class DeliveryNoteFacadeService : IDeliveryNoteFacadeService
    {
        private readonly IHtmlTemplateService<DeliveryNoteDocument> htmlTemplateServiceForDeliveryNote;

        private readonly IDeliveryNoteService deliveryNoteService;

        public DeliveryNoteFacadeService(IHtmlTemplateService<DeliveryNoteDocument> htmlTemplateServiceForDeliveryNote, IDeliveryNoteService deliveryNoteService)
        {
            this.htmlTemplateServiceForDeliveryNote = htmlTemplateServiceForDeliveryNote;
            this.deliveryNoteService = deliveryNoteService;
        }

        public Task<string> GetDeliveryNoteAsHtml(int reqNumber)
        {
            var deliveryNote = this.deliveryNoteService.GetDeliveryNote(reqNumber).Result;
            return htmlTemplateServiceForDeliveryNote.GetHtml(deliveryNote);
        }
    }
}
