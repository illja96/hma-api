using AutoMapper;
using HMA.DTO.Models.Transactions;
using HMA.DTO.ViewModels.Transactions;

namespace HMA.AutoMapper.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionCreationRequestViewModel, TransactionCreationRequest>();

            CreateMap<TransactionCreationRequest, TransactionInfo>();

            CreateMap<TransactionInfo, TransactionInfoViewModel>();
        }
    }
}
