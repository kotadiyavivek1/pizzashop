using pizzashop_Repository.Interface;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
using pizzashop_Services.interfaceService;

namespace pizzashop_Services.ImplementationService;
public class TaxFeesService : ITaxFees_Service
{
    private readonly ITaxFees_Repository taxFeesRepository;
    public TaxFeesService(ITaxFees_Repository _taxFeesRepository)
    {
        taxFeesRepository = _taxFeesRepository;
    }

    public bool addTaxFees(TaxFeesDto taxFeesDto,string email)
    {
            if(taxFeesRepository.addTaxFees(taxFeesDto,email)){
                return true;
            }
            return false;
    }

    public bool DeleteTaxFees(int id)
    {
        return taxFeesRepository.DeleteTaxFees(id);
    }

    public List<TaxFeesDto> GetALLTaxFees(string searchString)
    {
        return taxFeesRepository.GetALLTaxFees(searchString);
    }

    public FilterPaginationDto<TaxFeesDto> filterPaginationDto(int pageNumber,int pageSize,string searchString)
    {
        FilterPaginationDto<TaxFeesDto> paginationDto = new FilterPaginationDto<TaxFeesDto>();
        List<TaxFeesDto> taxes = taxFeesRepository.TaxFeesFilter(pageNumber,pageSize,searchString);
        int totalItems =  taxFeesRepository.totalItems(searchString);
        paginationDto.SetPaginationData(taxes,totalItems,pageNumber,pageSize,searchString);
        return paginationDto;
    }

    public TaxFeesDto taxFees(int id)
    {
        return taxFeesRepository.GetTaxFromId(id);;
    }

    public bool updateTax(FilterPaginationDto<TaxFeesDto> taxFeesDto)
    {
        return taxFeesRepository.isUpdate(taxFeesDto);
    }

}