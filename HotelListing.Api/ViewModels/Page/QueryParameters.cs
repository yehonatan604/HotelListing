namespace HotelListing.Api.ViewModels.Page
{
    public class QueryParameters
    {
        private int _PageSize = 15;
        
        public int PageSize { get => _PageSize; set => _PageSize = value; }
        public int StartIndex { get; set; }

    }
}
