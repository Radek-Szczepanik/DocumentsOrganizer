using AutoMapper;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;

namespace DocumentsOrganizer
{
    public class DocumentMappingsProfile : Profile
    {
        public DocumentMappingsProfile()
        {
            CreateMap<Document, DocumentDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Informations, y => y.MapFrom(z => z.DocumentInformations));

            CreateMap<DocumentInformation, DocumentInformationDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Description, y => y.MapFrom(z => z.Description));

            CreateMap<CreateDocumentDto, Document>();

            CreateMap<CreateInformationDto, DocumentInformation>();
        }
    }
}
