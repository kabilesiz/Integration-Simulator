using Integration.Common;

namespace Integration.Interfaces;

public interface IItemOperationBackend
{
    public Item SaveItem(string itemContent);

    public List<Item> FindItemsWithContent(string itemContent);

    public List<Item> GetAllItems();

    public void ClearListOfItems(bool resetAutoIncrement = false);

}