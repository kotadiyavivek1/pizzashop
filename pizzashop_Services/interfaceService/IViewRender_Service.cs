namespace pizzashop_Services.interfaceService;

public interface IViewRender_Service
{
         Task<string> RenderToStringAsync(string viewName, object model);
}
