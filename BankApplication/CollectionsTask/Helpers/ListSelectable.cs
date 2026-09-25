namespace CollectionsTask.Helpers;
using Microsoft.IdentityModel.Tokens;

public static class ListBuilder
{
    public static T? ListSelectable<T>(IQueryable<T> items, string itemName, Func<T, string>? displaySelector = null)
    where T : class
    {
        if (items.IsNullOrEmpty())
        {
            System.Console.WriteLine($"No {itemName} found. Press any key to return.");
            System.Console.WriteLine("- -- --- -- --- -- ----------------- --- -- --- - --- -");
            Console.ReadKey();
            return null;
        }

        T? selectedItem = default;
        int page = 0,
            pages = items.Count(),
            pageSize = 9,
            lastPage =  pages > pageSize ? 
                        pages % pageSize > 0 ?
                            pages / pageSize + 1 :
                            pages / pageSize : 
                        0;
        
        do
        {
            Console.Clear();
            System.Console.WriteLine($"Which {itemName} would you like to perform this action on?");
            System.Console.WriteLine("- -- --- -- --- -- ----------------- --- -- --- - --- -");

            int loadPages = page == lastPage ? pages % pageSize : pageSize;
            var itemsListed = items.Skip(page * pageSize).Take(loadPages);
            
            for(int i = 0; i < itemsListed.Count(); i++)
            {
                T item = itemsListed.ElementAt(i);
                string displayText = displaySelector?.Invoke(item) ?? item?.ToString() ?? string.Empty;
                Console.WriteLine($"[{i}]: {displayText}");
            }

            ConsoleKeyInfo keypress = Console.ReadKey();

            switch (keypress.Key)
            {
                case ConsoleKey.Escape:
                    return null;
                case ConsoleKey.LeftArrow:
                    page = page - 1 < 0 ? lastPage : page - 1; 
                    break;
                case ConsoleKey.RightArrow:
                    page = page + 1 > lastPage ? 0 : page + 1;
                    break;
                default:
                    try
                    {
                       selectedItem = itemsListed.ElementAtOrDefault(keypress.KeyChar - '0');
                    } 
                    catch {}
                    break;
            }
        }
        while(selectedItem == null);

        return selectedItem;
    }
}