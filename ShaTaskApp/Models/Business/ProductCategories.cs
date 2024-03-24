namespace ShaTaskApp.Models.Business
{
    public enum ProductCategories
    {
        //we reassign the first value as 1 not 0 by default, because we want 0 set to the please select option
        None = 1,

        Electronics,
        Appliances,
        Furniture,
        Clothing,
        Accessories,
        Beauty,
        HealthAndWellness,
        HomeAndGarden,
        ToysAndGames,
        BabyAndKids,
        Pets,
        Groceries,
        Beverages,
        SportsAndOutdoors,
        BooksAndMusic,
        MoviesAndTvShows,
        ArtsAndCrafts,
        OfficeSupplies,
        ToolsAndHardware
    }
}