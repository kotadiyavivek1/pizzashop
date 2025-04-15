using System.Reflection.Metadata.Ecma335;
using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.interfaceService;
public interface IKotService{
    
    public KotDto GetKotDto();
    public List<KotDto> GetAllCards(int categoryId,string status);
    public KotDto GetKotItem(int orderId,string status);
    public bool UpdateItem(List<KotItemUpdateDto> kotItem, string status);

}