using pizzashop_Repository.ViewModel;

namespace pizzashop_Repository.Interface;
public interface IKotRespository
{
    public  List<Category> GetCategories();
    public KotDto GetKotItem(int orderId,string status);
    public List<KotDto> GetCards(int? categoryId, string status);
    public bool UpdateItem(List<KotItemUpdateDto> kotItem, string status);
}