using pizzashop_Repository.Interface;
using pizzashop_Repository.interfaceService;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.ImplementationService;

public class KotService : IKotService
{
    private readonly IKotRespository kotRespository;
    public KotService(IKotRespository kotRespository)
    {
        this.kotRespository = kotRespository;
    }
    public List<KotDto> GetAllCards(int categoryId, string status)
    {
        return kotRespository.GetCards(categoryId,status);
    }
    public KotDto GetKotDto()
    {
        KotDto dto = new();
        dto.categories = kotRespository.GetCategories();
        return dto;
    }

    public KotDto GetKotItem(int orderId,string status)
    {
        KotDto dto = new KotDto();
        dto = kotRespository.GetKotItem(orderId,status);
        return dto;
    }

    public bool UpdateItem(List<KotItemUpdateDto> kotItem, string status)
    {
        return kotRespository.UpdateItem(kotItem,status);
    }
}
