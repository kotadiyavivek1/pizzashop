using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Services.interfaceService;
public interface ITaxFees_Service
{

    public List<TaxFeesDto> GetALLTaxFees(string searchString);
    public bool DeleteTaxFees(int id);
    public bool addTaxFees(TaxFeesDto taxFeesDto,string email);
    FilterPaginationDto<TaxFeesDto> filterPaginationDto(int pageNumber,int pageSize,string searchString);
    public TaxFeesDto taxFees(int id);
    public bool updateTax(FilterPaginationDto<TaxFeesDto> taxFeesDto);
}