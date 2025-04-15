using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using pizzashop_Repository.Models;
using pizzashop_Repository.ViewModel;
namespace pizzashop_Repository.Interface;

public class KotRepository : IKotRespository
{
    private readonly PizzashopContext context;
    public KotRepository(PizzashopContext context)
    {
        this.context = context;
    }
    public List<Category> GetCategories()
    {
        return context.Menucategories.Select(u => new Category()
        {
            Categoryid = u.Id,
            CategoryName = u.Name,
            Description = u.Description
        }).ToList();
    }

    public bool UpdateItem(List<KotItemUpdateDto> kotItem, string status)
    {
        if (kotItem == null || !kotItem.Any())
            return false;

        try
        {
            foreach (var item in kotItem)
            {
                var existingItem = context.Ordereditems.FirstOrDefault(u => u.Id == item.OrderedItemId);
                if (existingItem == null)
                    continue;
                int orderedQty = existingItem.Quantity;
                int readyQty = existingItem.Noofreadyitems ?? 0;
                int updateQty = item.Quantity;

                if (status == "InProgress")
                {
                    int remainingQty = orderedQty - readyQty;
                    int quantityToAdd = Math.Min(updateQty, remainingQty);
                    existingItem.Noofreadyitems += quantityToAdd;
                }
                else 
                {
                    int quantityToSubtract = Math.Min(updateQty, readyQty);
                    existingItem.Noofreadyitems -= quantityToSubtract;
                }

                existingItem.ReadyItem = existingItem.Noofreadyitems == orderedQty;
            }
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            // Log error here
            return false;
        }
    }


    public KotDto GetKotItem(int orderId, string status)
    {
        var order = context.Orders
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Menuitem)
                    .ThenInclude(mi => mi.Category)
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Ordereditemmodifiermappings)
                    .ThenInclude(mod => mod.Modifier)
            .Include(o => o.Tableordermappings)
                .ThenInclude(tom => tom.Table)
                    .ThenInclude(t => t.Section)
            .FirstOrDefault(o => o.Id == orderId);

        if (order == null)
            return null;

        return new KotDto
        {
            OrderId = order.Id,
            CreatedAt = order.Createdat,
            SectionName = order.Tableordermappings
                .Select(tom => tom.Table.Section.Name)
                .FirstOrDefault(),
            TableName = string.Join(", ", order.Tableordermappings
                .Select(tom => tom.Table.Name)),
            // OrderInstruction = order,
            CategoryName = "All", // or derive based on context
            KotItem = order.Ordereditems
                .Where((oi =>
                    status == "Ready" ||
                    oi.Noofreadyitems != oi.Quantity) 
                )
                .Where(oi=>status=="InProgress" || oi.Noofreadyitems > 0)
                .Select(oi => new KotItemDTO
                {
                    OrderedItemId = oi.Id,
                    KotItemName = oi.Menuitem.Name,
                    Quantity = oi.Quantity,
                    ReadyItemQuantity = oi.Noofreadyitems ?? 0,
                    ItemInstruction = oi.Instruction,
                    Modifiers = oi.Ordereditemmodifiermappings
                        .Select(mod => new KotItemModifierDTO
                        {
                            KotItemModifierName = mod.Modifier.Name
                        }).ToList()
                }).ToList()
        };
    }

    public List<KotDto> GetCards(int? categoryId, string status)
    {
        bool isReady = status == "Ready";

        var ordersQuery = context.Orders
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Menuitem)
                    .ThenInclude(mi => mi.Category)
            .Include(o => o.Ordereditems)
                .ThenInclude(oi => oi.Ordereditemmodifiermappings)
                    .ThenInclude(mod => mod.Modifier)
            .Include(o => o.Tableordermappings)
                .ThenInclude(tom => tom.Table)
                    .ThenInclude(t => t.Section)
            .Where(o => o.Ordereditems.Any(oi =>
                isReady
                    ? (oi.Noofreadyitems ?? 0) > 0
                    : (oi.Noofreadyitems ?? 0) < oi.Quantity
            ));

        if (categoryId.HasValue && categoryId.Value != 0)
        {
            ordersQuery = ordersQuery.Where(o =>
                o.Ordereditems.Any(oi =>
                    (isReady
                        ? (oi.Noofreadyitems ?? 0) > 0
                        : (oi.Noofreadyitems ?? 0) < oi.Quantity) &&
                    oi.Menuitem.Categoryid == categoryId));
        }

        return ordersQuery
            .Select(o => new KotDto
            {
                CategoryName = (categoryId == null || categoryId == 0)
                    ? "All"
                    : o.Ordereditems
                        .Where(oi => oi.Menuitem.Categoryid == categoryId)
                        .Select(oi => oi.Menuitem.Category.Name)
                        .FirstOrDefault(),
                Status = status,
                OrderId = o.Id,
                CreatedAt = o.Createdat,
                SectionName = o.Tableordermappings
                    .Select(tom => tom.Table.Section.Name)
                    .FirstOrDefault(),
                TableName = string.Join(", ", o.Tableordermappings
                    .Select(tom => tom.Table.Name)),
                // OrderInstruction = o.Instruction,
                KotItem = o.Ordereditems
                    .Where(oi =>
                        (isReady
                            ? (oi.Noofreadyitems ?? 0) > 0
                            : (oi.Noofreadyitems ?? 0) < oi.Quantity) &&
                        (categoryId == null || categoryId == 0 || oi.Menuitem.Categoryid == categoryId))
                    .Select(oi => new KotItemDTO
                    {
                        KotItemName = oi.Menuitem.Name,
                        Quantity = isReady
                            ? (oi.Noofreadyitems ?? 0)
                            : (oi.Quantity - (oi.Noofreadyitems ?? 0)),
                        ItemInstruction = oi.Instruction,
                        Modifiers = oi.Ordereditemmodifiermappings
                            .Select(mod => new KotItemModifierDTO
                            {
                                KotItemModifierName = mod.Modifier.Name
                            }).ToList()
                    }).ToList()
            }).ToList();
    }
}



