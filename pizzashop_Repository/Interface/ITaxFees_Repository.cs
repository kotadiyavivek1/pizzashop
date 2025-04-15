using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Interface;
public interface ITaxFees_Repository
{
    public List<TaxFeesDto> GetALLTaxFees(string searchString);
    public bool DeleteTaxFees(int id);
    public bool addTaxFees(TaxFeesDto taxFeesDto,string email);
    public  List<TaxFeesDto> TaxFeesFilter(int pageNumber,int PageSize,string searchString);
    public int totalItems(string searchString);
    public TaxFeesDto GetTaxFromId(int id);
    public bool isUpdate(FilterPaginationDto<TaxFeesDto> taxFeesDto);
}