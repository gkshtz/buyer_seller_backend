namespace BuyerSeller.Application.DTO
{
    public class CustomResponse<TResponseType>
    {
        public int StatusCode { get; set; }
        public string? ResponseMessage { get; set; }
        public TResponseType? ResponseData { get; set; }
        public string? ErrorMessage {  get; set; }
    }
}
